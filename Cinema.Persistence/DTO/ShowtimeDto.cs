using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class ShowtimeDto
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


        public static explicit operator Showtime(ShowtimeDto dto) => new Showtime
        {
            Id = dto.Id,
            MovieId = dto.MovieId,
            Time = dto.Time,
            ScreenId = dto.ScreenId
        };

        public static explicit operator ShowtimeDto(Showtime showtime) => new ShowtimeDto
        {
            Id = showtime.Id,
            MovieId = showtime.MovieId,
            Time = showtime.Time,
            ScreenId = showtime.ScreenId
        };
    }
}
