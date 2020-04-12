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

        public IActionResult Index(int showtimeId)
        {
            return View();
        }
    }
}
