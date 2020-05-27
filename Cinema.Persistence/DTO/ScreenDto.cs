using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class ScreenDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfRows { get; set; }

        public int SeatsPerRow { get; set; }

        public static explicit operator Screen(ScreenDto dto) => new Screen
        {
            Id = dto.Id,
            Name = dto.Name,
            NumberOfRows = dto.NumberOfRows,
            SeatsPerRow = dto.SeatsPerRow
        };

        public static explicit operator ScreenDto(Screen screen) => new ScreenDto
        {
            Id = screen.Id,
            Name = screen.Name,
            NumberOfRows = screen.NumberOfRows,
            SeatsPerRow = screen.SeatsPerRow
        };
    }
}
