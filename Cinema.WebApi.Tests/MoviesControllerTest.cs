using Cinema.Persistence;
using Cinema.Persistence.DTO;
using Cinema.Persistence.Services;
using Cinema.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Cinema.WebApi.Tests
{
    public class MoviesControllerTest : IDisposable
    {
        private readonly CinemaContext _context;
        private readonly CinemaService _service;
        private readonly MoviesController _controller;

        public MoviesControllerTest()
        {
            var options = new DbContextOptionsBuilder<CinemaContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new CinemaContext(options);
            TestDbInitializer.Initialize(_context);
            _service = new CinemaService(_context);
            _controller = new MoviesController(_service);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetMoviesTest()
        {
            var result = _controller.GetMovies();

            var content = Assert.IsAssignableFrom<IEnumerable<MovieDto>>(result.Value);
            Assert.Equal(3, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetMovieTest(int id)
        {
            var result = _controller.GetMovie(id);

            var content = Assert.IsAssignableFrom<MovieDto>(result.Value);
            Assert.Equal(id, content.Id);
        }

        [Fact]
        public void GetInvalidMovieTest()
        {
            var id = 4;

            var result = _controller.GetMovie(id);

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

            var result = _controller.PostMovie(newMovie);

            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<MovieDto>(objectResult.Value);
            Assert.Equal(count + 1, _context.Movies.Count());
        }
    }
}
