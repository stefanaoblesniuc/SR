using Microsoft.EntityFrameworkCore;
using MovieApp.DataBase;
using MovieApp.Entities;

namespace MovieApp;

public class DislikeMovieService
{
    private readonly MovieAppDbContext _dbContext;

    public DislikeMovieService(MovieAppDbContext context)
    {
        _dbContext = context;
    }


    public async Task<List<Movie>> GetDislikedMoviesAsync(Guid userId)
    {
        // Fetch the favorite movies for the user
        var dislikedMovies = await _dbContext.DislikeMovies
            .Where(fm => fm.UserId == userId)
            .Select(fm => fm.Movie)
            .ToListAsync();

        return dislikedMovies;
    }

    public async Task<bool> AddDislikeAsync(string userName, string movieTitle)
    {
        // Validate if the user exists
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
        // var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // Validate if the movie exists
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == movieTitle);
        if (movie == null)
        {
            throw new KeyNotFoundException("Movie not found.");
        }

        // Check if the dislikeMovie movie already exists for the user
        bool isAlreadyDisliked = await _dbContext.DislikeMovies
            .AnyAsync(fm => fm.UserId == user.Id && fm.MovieId == movie.Id);

        if (isAlreadyDisliked)
        {
            return false; // Indicate that the movie is already in dislike
        }

        // Add the dislikeMovie movie
        var dislikeMovie = new DislikeMovie
        {
            UserId = user.Id,
            MovieId = movie.Id,
            AddedDate = DateTime.UtcNow
        };

        _dbContext.DislikeMovies.Add(dislikeMovie);
        await _dbContext.SaveChangesAsync();

        return true; // Indicate success
    }
}
