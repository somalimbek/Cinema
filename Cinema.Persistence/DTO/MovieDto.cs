using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Persistence.DTO
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public string Cast { get; set; }

        public string Storyline { get; set; }

        public int Runtime { get; set; }

        public byte[] Poster { get; set; }

        public DateTime Added { get; set; }

        public static explicit operator Movie(MovieDto dto) => new Movie
        {
            Id = dto.Id,
            Title = dto.Title,
            Director = dto.Director,
            Cast = dto.Cast,
            Storyline = dto.Storyline,
            Runtime = dto.Runtime,
            Poster = dto.Poster,
            Added = dto.Added
        };

        public static explicit operator MovieDto(Movie movie) => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Director = movie.Director,
            Cast = movie.Cast,
            Storyline = movie.Storyline,
            Runtime = movie.Runtime,
            Poster = movie.Poster,
            Added = movie.Added
        };
    }
}
