using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class Showtime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Movie")]
        public int MovieId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [DisplayName("Screen")]
        public int ScreenId { get; set; }


        public virtual Movie Movie { get; set; }
        public virtual Screen Screen { get; set; }
    }
}
