using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public enum SeatStatus
    {
        Free,
        Booked,
        Sold
    }
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Showtime")]
        public int ShowtimeId { get; set; }

        [Required]
        [DisplayName("Screen")]
        public int ScreenId { get; set; }

        [Required]
        public int RowNumber { get; set; }

        [Required]
        public int SeatNumber { get; set; }

        [Required]
        public SeatStatus Status { get; set; }

        [MaxLength(30)]
        public string CustomerName { get; set; }

        [MaxLength(15)]
        public string CustomerPhoneNumber { get; set; }


        public virtual Showtime Showtime { get; set; }
        public virtual Screen Screen { get; set; }
    }
}
