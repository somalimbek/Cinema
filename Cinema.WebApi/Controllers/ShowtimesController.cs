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
        /*
        // GET: api/Showtimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Showtime>> GetShowtime(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);

            if (showtime == null)
            {
                return NotFound();
            }

            return showtime;
        }

        // PUT: api/Showtimes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShowtime(int id, Showtime showtime)
        {
            if (id != showtime.Id)
            {
                return BadRequest();
            }

            _context.Entry(showtime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowtimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Showtimes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Showtime>> PostShowtime(Showtime showtime)
        {
            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShowtime", new { id = showtime.Id }, showtime);
        }

        // DELETE: api/Showtimes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Showtime>> DeleteShowtime(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null)
            {
                return NotFound();
            }

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();

            return showtime;
        }

        private bool ShowtimeExists(int id)
        {
            return _context.Showtimes.Any(e => e.Id == id);
        }
        */
    }
}
