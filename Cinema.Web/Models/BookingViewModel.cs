using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class BookingViewModel
    {
        public List<List<Seat>> Seats { get; set; }

        public Showtime Showtime { get; set; }

        public Movie Movie { get; set; }

        public Screen Screen { get; set; }
    }
}
