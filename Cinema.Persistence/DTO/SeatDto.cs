using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class SeatDto
    {
        public int Id { get; set; }

        public int ShowtimeId { get; set; }

        public int RowNumber { get; set; }

        public int SeatNumber { get; set; }

        public SeatStatus Status { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public static explicit operator Seat(SeatDto dto) => new Seat
        {
            Id = dto.Id,
            ShowtimeId = dto.ShowtimeId,
            RowNumber = dto.RowNumber,
            SeatNumber = dto.SeatNumber,
            Status = dto.Status,
            CustomerName = dto.CustomerName,
            CustomerPhoneNumber = dto.CustomerPhoneNumber
        };

        public static explicit operator SeatDto(Seat seat) => new SeatDto
        {
            Id = seat.Id,
            ShowtimeId = seat.ShowtimeId,
            RowNumber = seat.RowNumber,
            SeatNumber = seat.SeatNumber,
            Status = seat.Status,
            CustomerName = seat.CustomerName,
            CustomerPhoneNumber = seat.CustomerPhoneNumber
        };
    }
}
