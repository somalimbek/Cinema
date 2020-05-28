using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Persistence.Services
{
    public class CinemaService
    {
        private readonly CinemaContext _context;

        public CinemaService(CinemaContext context)
        {
            _context = context;
        }

        #region Create

        public Movie CreateMovie(Movie movie)
        {
            try
            {
                _context.Add(movie);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return movie;
        }

        public List<Seat> CreateSeatsForShowtime(int id)
        {
            var seats = new List<Seat>();

            var screen = GetShowtime(id).Screen;
            for (int row = 0; row < screen.NumberOfRows; row++)
            {
                for (int seat = 0; seat < screen.SeatsPerRow; seat++)
                {
                    seats.Add(new Seat
                    {
                        ShowtimeId = id,
                        RowNumber = row,
                        SeatNumber = seat,
                        Status = SeatStatus.Free
                    });
                }
            }

            try
            {
                _context.Seats.AddRange(seats);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return seats;
        }

        public Showtime CreateShowtime(Showtime newShowtime)
        {
            var movie = GetMovie(newShowtime.MovieId);
            var previousShowtimes = _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Where(showtime => showtime.ScreenId == newShowtime.ScreenId)
                .Where(showtime => showtime.Time < newShowtime.Time)
                .OrderByDescending(showtime => showtime.Time)
                .ToList();

            Showtime prevShowtime;
            if (previousShowtimes.Count > 0)
                prevShowtime = previousShowtimes.First();
            else
                prevShowtime = null;

            if (prevShowtime != null && prevShowtime.Time.AddMinutes(prevShowtime.Movie.Runtime + 15) > newShowtime.Time)
            {
                return null;
            }

            var followingShowtimes = _context.Showtimes
                .Where(showtime => showtime.ScreenId == newShowtime.ScreenId)
                .Where(showtime => showtime.Time > newShowtime.Time)
                .OrderBy(showtime => showtime.Time)
                .ToList();

            Showtime nextShowtime;
            if (followingShowtimes.Count > 0)
                nextShowtime = followingShowtimes.First();
            else
                nextShowtime = null;

            if (nextShowtime != null && newShowtime.Time.AddMinutes(movie.Runtime + 15) > nextShowtime.Time)
            {
                return null;
            }

            try
            {
                _context.Add(newShowtime);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return newShowtime;
        }

        #endregion

        #region Read

        public List<Showtime> GetFutureShowtimesForMovie(int movieId)
        {
            return _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Screen)
                .Where(showtime => showtime.Movie.Id == movieId)
                .Where(showtime => showtime.Time > DateTime.Now)
                .OrderBy(showtime => showtime.Time)
                .ToList();
        }

        public List<Movie> GetLatestMovies(int count = 5)
        {
            return _context.Movies
                .OrderByDescending(movie => movie.Added)
                .Take(count)
                .ToList();
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies
                .FirstOrDefault(movie => movie.Id == id);
        }

        public List<Movie> GetMovies(string title = null)
        {
            return _context.Movies
                .Where(movie => movie.Title.Contains(title ?? ""))
                .OrderBy(movie => movie.Title)
                .ToList();
        }

        public List<Screen> GetScreens(string name = null)
        {
            return _context.Screens
                .Where(screen => screen.Name.Contains(name ?? ""))
                .OrderBy(screen => screen.Name)
                .ToList();
        }

        public Seat GetSeat(int id)
        {
            var seat = _context.Seats
                .FirstOrDefault(seat => seat.Id == id);
            return seat;
        }

        public List<Seat> GetSeatsListForShowtime(int showtimeId)
        {
            var seatsForShowtime = _context.Seats
                .Include(seat => seat.Showtime)
                .Where(seat => seat.Showtime.Id == showtimeId)
                .OrderBy(seat => seat.RowNumber)
                .ToList();

            if (seatsForShowtime.Count == 0)
            {
                seatsForShowtime = CreateSeatsForShowtime(showtimeId);
            }

            var seatsOrderedByRowAndSeatNumber = new List<Seat>();
            var seatsOfRow = new List<Seat>() { seatsForShowtime.First() };
            for (int i = 1; i < seatsForShowtime.Count; i++)
            {
                if (seatsForShowtime[i].RowNumber == seatsForShowtime[i - 1].RowNumber && i != seatsForShowtime.Count - 1)
                {
                    seatsOfRow.Add(seatsForShowtime[i]);
                }
                else
                {
                    if (i == seatsForShowtime.Count - 1)
                    {
                        seatsOfRow.Add(seatsForShowtime[i]);
                    }
                    seatsOfRow = seatsOfRow.OrderBy(seat => seat.SeatNumber).ToList();
                    seatsOrderedByRowAndSeatNumber.AddRange(seatsOfRow);
                    seatsOfRow = new List<Seat>() { seatsForShowtime[i] };
                }
            }

            return seatsOrderedByRowAndSeatNumber;
        }

        public List<List<Seat>> GetSeatsForShowtime(int showtimeId)
        {
            var seatsForShowtime = _context.Seats
                .Include(seat => seat.Showtime)
                .Where(seat => seat.Showtime.Id == showtimeId)
                .OrderBy(seat => seat.RowNumber)
                .ToList();

            var seatsOrderedByRowAndSeatNumber = new List<List<Seat>>();
            var seatsOfRow = new List<Seat>() { seatsForShowtime[0] };
            for (int i = 1; i < seatsForShowtime.Count; i++)
            {
                if (seatsForShowtime[i].RowNumber == seatsForShowtime[i - 1].RowNumber && i != seatsForShowtime.Count - 1)
                {
                    seatsOfRow.Add(seatsForShowtime[i]);
                }
                else
                {
                    if (i == seatsForShowtime.Count - 1)
                    {
                        seatsOfRow.Add(seatsForShowtime[i]);
                    }
                    seatsOfRow = seatsOfRow.OrderBy(seat => seat.SeatNumber).ToList();
                    seatsOrderedByRowAndSeatNumber.Add(seatsOfRow);
                    seatsOfRow = new List<Seat>() { seatsForShowtime[i] };
                }
            }

            return seatsOrderedByRowAndSeatNumber;
        }

        public Showtime GetShowtime(int id)
        {
            return _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Screen)
                .FirstOrDefault(showtime => showtime.Id == id);
        }

        public List<List<Showtime>> GetShowtimesForMovie(int movieId)
        {
            var showtimesForMovie = _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Screen)
                .Where(showtime => showtime.Movie.Id == movieId)
                .OrderBy(showtime => showtime.Time)
                .ToList();

            var showtimesOrderedByDaysAndTime = new List<List<Showtime>>();
            var showtimesOfDay = new List<Showtime>() { showtimesForMovie[0] };
            for (int i = 1; i < showtimesForMovie.Count; i++)
            {
                if (showtimesForMovie[i].Time.Day == showtimesForMovie[i - 1].Time.Day && i != showtimesForMovie.Count - 1)
                {
                    showtimesOfDay.Add(showtimesForMovie[i]);
                }
                else
                {
                    if (i == showtimesForMovie.Count - 1)
                    {
                        showtimesOfDay.Add(showtimesForMovie[i]);
                    }
                    showtimesOfDay = showtimesOfDay.OrderBy(showtime => showtime.Time).ToList();
                    showtimesOrderedByDaysAndTime.Add(showtimesOfDay);
                    showtimesOfDay = new List<Showtime>() { showtimesForMovie[i] };
                }
            }

            return showtimesOrderedByDaysAndTime;
        }

        public List<List<Showtime>> GetTodaysShowtimesByMovies()
        {
            var todaysShowtimes = _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Screen)
                .Where(showtime => showtime.Time.Date == DateTime.Today)
                .OrderBy(showtime => showtime.Movie.Title)
                .ToList();

            var showtimesOrderedByMovieTitleAndTime = new List<List<Showtime>>();
            var showtimesOfMovie = new List<Showtime>() { todaysShowtimes[0] };
            for (int i = 1; i < todaysShowtimes.Count; i++)
            {
                if (todaysShowtimes[i].Movie.Title == todaysShowtimes[i - 1].Movie.Title && i != todaysShowtimes.Count - 1)
                {
                    showtimesOfMovie.Add(todaysShowtimes[i]);
                }
                else
                {
                    if (i == todaysShowtimes.Count - 1)
                    {
                        showtimesOfMovie.Add(todaysShowtimes[i]);
                    }
                    showtimesOfMovie = showtimesOfMovie.OrderBy(showtime => showtime.Time).ToList();
                    showtimesOrderedByMovieTitleAndTime.Add(showtimesOfMovie);
                    showtimesOfMovie = new List<Showtime>() { todaysShowtimes[i] };
                }
            }

            return showtimesOrderedByMovieTitleAndTime;
        }

        #endregion

        #region Update

        public bool SaveBooking(List<Seat> seatsToBook)
        {
            var successful = true;
            foreach (var seatToBook in seatsToBook)
            {
                var seatInDatabase = GetSeat(seatToBook.Id);
                if (successful && seatInDatabase.Status == SeatStatus.Free)
                {
                    _context.Entry(seatInDatabase).CurrentValues.SetValues(seatToBook);
                    successful = true;
                }
                else
                {
                    successful = false;
                }
            }
            if (successful)
            {
                _context.SaveChanges();
            }

            return successful;
        }

        #endregion

        #region Delete
        #endregion
    }
}
