using Cinema.Persistence;
using Cinema.Persistence.DTO;
using Cinema.Persistence.Services;
using Cinema.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Cinema.WebApi.Tests
{
    public class CinemaWebApiTest : IDisposable
    {
        private readonly CinemaContext _context;
        private readonly CinemaService _service;
        private readonly MoviesController _moviesController;
        private readonly ScreensController _screensController;
        private readonly SeatsController _seatsController;
        private readonly ShowtimesController _showtimesController;

        public CinemaWebApiTest()
        {
            var options = new DbContextOptionsBuilder<CinemaContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new CinemaContext(options);
            TestDbInitializer.Initialize(_context);
            _service = new CinemaService(_context);

            _moviesController = new MoviesController(_service);
            _screensController = new ScreensController(_service);
            _seatsController = new SeatsController(_service);
            _showtimesController = new ShowtimesController(_service);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetMoviesTest()
        {
            var result = _moviesController.GetMovies();

            var content = Assert.IsAssignableFrom<IEnumerable<MovieDto>>(result.Value);
            Assert.Equal(3, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetMovieTest(int id)
        {
            var result = _moviesController.GetMovie(id);

            var content = Assert.IsAssignableFrom<MovieDto>(result.Value);
            Assert.Equal(id, content.Id);
        }

        [Fact]
        public void GetInvalidMovieTest()
        {
            var id = 4;

            var result = _moviesController.GetMovie(id);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void PostMovieTest()
        {
            var newMovie = new MovieDto
            {
                Title = "New Movie",
                Director = "Mr. Dir",
                Cast = "A B C",
                Storyline = "ASDF",
                Runtime = 10
            };
            var count = _context.Movies.Count();

            var result = _moviesController.PostMovie(newMovie);

            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<MovieDto>(objectResult.Value);
            Assert.Equal(count + 1, _context.Movies.Count());
        }

        [Fact]
        public void GetScreensTest()
        {
            var result = _screensController.GetScreens();

            var content = Assert.IsAssignableFrom<IEnumerable<ScreenDto>>(result.Value);
            Assert.Equal(3, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetScreenTest(int showtimeId)
        {
            var result = _screensController.GetScreen(showtimeId);

            var content = Assert.IsAssignableFrom<ScreenDto>(result.Value);
            Assert.Equal(1, content.Id);
        }

        [Fact]
        public void GetInvalidScreenTest()
        {
            var showtimeId = 64;

            var result = _screensController.GetScreen(showtimeId);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetSeatsTest()
        {
            var showtimeId = 1;

            var result = _seatsController.GetSeats(showtimeId);

            var content = Assert.IsAssignableFrom<IEnumerable<SeatDto>>(result.Value);
            Assert.Equal(120, content.Count());
        }

        [Fact]
        public void GetShowtimesTest()
        {
            var movieId = 1;

            var result = _showtimesController.GetShowtimes(movieId);

            var content = Assert.IsAssignableFrom<IEnumerable<ShowtimeDto>>(result.Value);
            Assert.Equal(21, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetShowtimeTest(int id)
        {
            var result = _showtimesController.GetShowtime(id);

            var content = Assert.IsAssignableFrom<ShowtimeDto>(result.Value);
            Assert.Equal(id, content.Id);
        }

        [Fact]
        public void GetInvalidShowtimeTest()
        {
            var id = 64;

            var result = _showtimesController.GetShowtime(id);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }
    }
}
