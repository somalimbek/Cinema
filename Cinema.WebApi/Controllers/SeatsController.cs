using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Persistence;
using Cinema.Persistence.Services;
using Cinema.Persistence.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly CinemaService _service;

        public SeatsController(CinemaService service)
        {
            _service = service;
        }

        // GET: api/Seats
        [HttpGet]
        public ActionResult<IEnumerable<SeatDto>> GetSeats(int showtimeId)
        {
            return _service.GetSeatsListForShowtime(showtimeId).Select(seat => (SeatDto)seat).ToList();
        }

        // PUT: api/Seats/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut]
        public IActionResult PutSeats(IEnumerable<SeatDto> seats)
        {
            if (seats.Count() == 0)
            {
                return BadRequest();
            }

            if (_service.SellSeats(seats.Select(dto => (Seat)dto).ToList()))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
