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

        public List<List<Showtime>> GetTodaysShowtimesByMovies()
        {
            var showtimes = _context.Showtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Screen)
                .Where(showtime => showtime.Time.Date == DateTime.Today)
                .OrderBy(showtime => showtime.Movie.Title)
                .ToList();

            var showtimesOrderedByMovieTitleAndTime = new List<List<Showtime>>();
            var showtimesOfMovie = new List<Showtime>() { showtimes[0] };
            for (int i = 1; i < showtimes.Count; i++)
            {
                if (showtimes[i].Movie.Title == showtimes[i - 1].Movie.Title && i != showtimes.Count - 1)
                {
                    showtimesOfMovie.Add(showtimes[i]);
                }
                else
                {
                    if (i == showtimes.Count - 1)
                    {
                        showtimesOfMovie.Add(showtimes[i]);
                    }
                    showtimesOfMovie = showtimesOfMovie.OrderBy(showtime => showtime.Time).ToList();
                    showtimesOrderedByMovieTitleAndTime.Add(showtimesOfMovie);
                    showtimesOfMovie = new List<Showtime>() { showtimes[i] };
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
