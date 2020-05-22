using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }

        public List<List<Showtime>> ShowtimesForMovie { get; set; }
    }
}
