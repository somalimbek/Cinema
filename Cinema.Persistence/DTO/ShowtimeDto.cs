using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class ShowtimeDto
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public DateTime Time { get; set; }

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
