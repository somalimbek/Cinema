using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class HomeViewModel
    {
        public List<Movie> LatestMovies { get; set; }

        public List<List<Showtime>> TodaysShowtimesByMovies { get; set; }
    }
}
