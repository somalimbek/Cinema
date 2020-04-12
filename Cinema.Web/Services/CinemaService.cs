using Cinema.Web.Models;
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

        public List<Movie> GetMovies(string title = null)
        {
            return _context.Movies
                .Where(movie => movie.Title.Contains(title ?? ""))
                .OrderBy(movie => movie.Title)
                .ToList();
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies
                .FirstOrDefault(movie => movie.Id == id);
        }

        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion
    }
}
