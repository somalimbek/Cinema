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
    public class MoviesController : ControllerBase
    {
        private readonly CinemaService _service;

        public MoviesController(CinemaService service)
        {
            _service = service;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<MovieDto>> GetMovies()
        {
            return _service.GetMovies().Select(movie => (MovieDto)movie).ToList();
        }
        
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovie(int id)
        {
            try
            {
                return (MovieDto)_service.GetMovie(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        /*
        // PUT: api/Movies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        */
        // POST: api/Movies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<MovieDto> PostMovie(MovieDto movieDto)
        {
            var movie = _service.CreateMovie((Movie)movieDto);
            if (movie is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, (MovieDto)movie);
            }
        }
        /*
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
        */
    }
}
