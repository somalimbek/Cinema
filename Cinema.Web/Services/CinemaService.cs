using Cinema.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Services
{
    public class CinemaService
    {
        private readonly CinemaContext _context;

        public CinemaService(CinemaContext context)
        {
            _context = context;
        }

        #region Create
        #endregion

        #region Read

        public List<Movie> GetLatestMovies(int count = 5)
        {
            return _context.Movies
                .OrderByDescending(movie => movie.Added)
                .Take(count)
                .ToList();
        }

        public List<List<Showtime>> GetShowtimesForMovie(int movieId)
        {
            var showtimesForMovie = _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Screen)
                .Where(showtime => showtime.Movie.Id == movieId)
                .OrderBy(showtime => showtime.Time)
                .ToList();

            var showtimesOrderedByDays = new List<List<Showtime>>();
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
                    showtimesOrderedByDays.Add(showtimesOfDay);
                    showtimesOfDay = new List<Showtime>() { showtimesForMovie[i] };
                }
            }

            return showtimesOrderedByDays;
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

        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion
    }
}
