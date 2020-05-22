using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class BookingViewModel
    {
        public string ErrorMessage { get; set; }

        public List<List<Seat>> Seats { get; set; }

        public Showtime Showtime { get; set; }

        public Movie Movie { get; set; }

        public Screen Screen { get; set; }

        [Required(ErrorMessage = "Név megadása kötelező.")]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Telefonszám megadása kötelező.")]
        [MaxLength(15, ErrorMessage = "Túl hosszú telefonszám.")]
        public string CustomerPhoneNumber { get; set; }

        [Required(ErrorMessage = "Nem választott helyeket.\nA foglalni kívánt székeket a fenti táblázatban, kattintással tudja kiválasztani.")]
        [MaxLength(36, ErrorMessage = "Legfeljebb 6 hely foglalható.")]
        public string SeatsToBook { get; set; }
    }
}
