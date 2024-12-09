using Microsoft.EntityFrameworkCore;
using MovieApp.DataBase;
using MovieApp.DTO;
using MovieApp.Entities;
using System;

namespace MovieApp.Services
{
    public class MovieService
    {
        private readonly MovieAppDbContext _context;

        public MovieService(MovieAppDbContext context)
        {
            _context = context;
        }

        public async Task<Movie?> GetMovie(string name)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(m => m.Title == name);
        }

        public async Task<List<Movie>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> AddMovie(Movie movieDto)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = movieDto.Title,
                Genre = movieDto.Genre,
                Premiere = movieDto.Premiere,
                IMDBScore = movieDto.IMDBScore,
                Language = movieDto.Language
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }
    }
}
