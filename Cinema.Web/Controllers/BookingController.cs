using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Web.Models;
using Cinema.Web.Services;

namespace Cinema.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly CinemaService _service;

        public BookingController(CinemaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index(int showtimeId, string errorMessage = "")
        {
            var viewModel = new BookingViewModel();
            viewModel.ErrorMessage = errorMessage;
            viewModel.Seats = _service.GetSeatsForShowtime(showtimeId);
            viewModel.Showtime = _service.GetShowtime(showtimeId);
            viewModel.Movie = viewModel.Showtime.Movie;
            viewModel.Screen = viewModel.Showtime.Screen;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(int showtimeId, BookingViewModel booking)
        {
            var seatsToBook = new List<Seat>();
            var seatsOfShowtime = new List<List<Seat>>(_service.GetSeatsForShowtime(showtimeId));

            var seatStrings = booking.SeatsToBook.Split(" ");
            seatStrings = seatStrings.Take(seatStrings.Count() - 1).ToArray();

            foreach (var seatString in seatStrings)
            {
                var rowNumber = int.Parse(seatString.Split("-").First()) - 1;
                var seatNumber = int.Parse(seatString.Split("-").Last()) - 1;
                var seatInDatabase = seatsOfShowtime[rowNumber][seatNumber];
                var seat = new Seat()
                {
                    Id = seatInDatabase.Id,
                    ShowtimeId = seatInDatabase.ShowtimeId,
                    RowNumber = seatInDatabase.RowNumber,
                    SeatNumber = seatInDatabase.SeatNumber,
                    Status = SeatStatus.Booked,
                    CustomerName = booking.CustomerName,
                    CustomerPhoneNumber = booking.CustomerPhoneNumber
                };
                seatsToBook.Add(seat);
            }
            if (!_service.SaveBooking(seatsToBook))
            {
                var em = "Foglalás feldolgozása sikertelen. Kérem próbálkozzon újra.";
                return RedirectToAction("Index", "Booking", new { showtimeId = showtimeId, errorMessage = em });
            }

            var showtime = _service.GetShowtime(showtimeId);

            return View("Result", showtime);
        }
    }
}
