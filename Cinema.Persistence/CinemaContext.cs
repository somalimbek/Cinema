using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Persistence
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Screen> Screens { get; set; }

        public DbSet<Showtime> Showtimes { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
