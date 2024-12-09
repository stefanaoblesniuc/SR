using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.DataBase;
using MovieApp.Entities;

namespace MovieApp.Services
{
    public class FavoriteMovieService
    {
        private readonly MovieAppDbContext _dbContext;

        public FavoriteMovieService(MovieAppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Movie>> GetFavoriteMoviesAsync(Guid userId)
        {
            // Fetch the favorite movies for the user
            var favoriteMovies = await _dbContext.FavoriteMovies
                .Where(fm => fm.UserId == userId)
                .Select(fm => fm.Movie)
                .ToListAsync();

            return favoriteMovies;
        }

        public async Task<bool> AddFavoriteAsync(Guid userId, Guid movieId)
        {
            // Validate if the user exists
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Validate if the movie exists
            var movie = await _dbContext.Movies.FindAsync(movieId);
            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found.");
            }

            // Check if the favorite movie already exists for the user
            bool isAlreadyFavorite = await _dbContext.FavoriteMovies
                .AnyAsync(fm => fm.UserId == userId && fm.MovieId == movieId);

            if (isAlreadyFavorite)
            {
                return false; // Indicate that the movie is already in favorites
            }

            // Add the favorite movie
            var favoriteMovie = new FavoriteMovie
            {
                UserId = userId,
                MovieId = movieId,
                AddedDate = DateTime.UtcNow
            };

            _dbContext.FavoriteMovies.Add(favoriteMovie);
            await _dbContext.SaveChangesAsync();

            return true; // Indicate success
        }
    }
}
