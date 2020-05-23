using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cinema.Web.Models;
using Cinema.Persistence.Services;

namespace Cinema.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CinemaService _service;

        public HomeController(CinemaService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();
            viewModel.LatestMovies = _service.GetLatestMovies();
            viewModel.TodaysShowtimesByMovies = _service.GetTodaysShowtimesByMovies();
            return View("Index", viewModel);
        }

        public IActionResult Details(int movieId)
        {
            return RedirectToAction("Index", "Movie", new { id = movieId });
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

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
