using Cinema.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Services
{
    public class CinemaService
    {
        private readonly CinemaContext context;

        public CinemaService(CinemaContext context)
        {
            this.context = context;
        }

        #region Create
        #endregion

        #region Read

        public List<Movie> GetMovies(string title = null)
        {
            return context.Movies
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
