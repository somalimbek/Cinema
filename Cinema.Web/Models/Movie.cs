using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Director { get; set; }

        [Required]
        public string Cast { get; set; }

        [Required]
        public string Storyline { get; set; }

        [Required]
        public int Runtime { get; set; }

        public byte[] Poster { get; set; }

        public DateTime Added { get; set; }
    }
}
