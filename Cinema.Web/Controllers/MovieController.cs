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
    public class MovieController : Controller
    {
        private readonly CinemaService _service;

        public MovieController(CinemaService service)
        {
            _service = service;
        }

        // GET: Movie
        public IActionResult Index(int id)
        {
            var viewModel = new MovieViewModel();
            viewModel.Movie = _service.GetMovie(id);
            viewModel.ShowtimesForMovie = _service.GetShowtimesForMovie(id);
            return View(viewModel);
        }

        public IActionResult DisplayImage(int id)
        {
            var movie = _service.GetMovie(id);
            return File(movie.Poster, "image/jpg");
        }

        public IActionResult Book(int showtimeId)
        {
            return RedirectToAction("Index", "Booking", new { showtimeId = showtimeId });
        }
    }
}
