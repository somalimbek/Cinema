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
    public class ShowtimesController : ControllerBase
    {
        private readonly CinemaService _service;

        public ShowtimesController(CinemaService service)
        {
            _service = service;
        }

        // GET: api/Showtimes
        [HttpGet]
        public ActionResult<IEnumerable<ShowtimeDto>> GetShowtimes(int movieId)
        {
            return _service.GetFutureShowtimesForMovie(movieId).Select(showtime => (ShowtimeDto)showtime).ToList();
        }
        
        // GET: api/Showtimes/5
        [HttpGet("{id}")]
        public ActionResult<ShowtimeDto> GetShowtime(int id)
        {
            try
            {
                return (ShowtimeDto)_service.GetShowtime(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        // POST: api/Showtimes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public ActionResult<ShowtimeDto> PostShowtime(ShowtimeDto showtimeDto)
        {
            var showtime = _service.CreateShowtime((Showtime)showtimeDto);
            if (showtime is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction(nameof(GetShowtime), new { id = showtime.Id }, (ShowtimeDto)showtime);
            }
        }
    }
}
