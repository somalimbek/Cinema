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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly CinemaService _service;

        public ScreensController(CinemaService service)
        {
            _service = service;
        }

        // GET: api/Screens/GetScreens
        [HttpGet]
        public ActionResult<IEnumerable<ScreenDto>> GetScreens()
        {
            return _service.GetScreens().Select(screen => (ScreenDto)screen).ToList();
        }

        // GET: api/Screens/GetScreen
        [HttpGet]
        public ActionResult<ScreenDto> GetScreen(int showtimeId)
        {
            Showtime showtime;
            try
            {
                showtime = _service.GetShowtime(showtimeId);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return (ScreenDto)showtime.Screen;
        }
    }
}
