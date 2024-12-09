using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoriteMovieController : Controller
    {
        public readonly FavoriteMovieService _favoriteMovieService;

        public FavoriteMovieController(FavoriteMovieService favoriteMovieService)
        {
            _favoriteMovieService = favoriteMovieService;
        }

        [HttpPost("add-favorite")]
        public async Task<IActionResult> AddFavorite(Guid userId, Guid movieId)
        {
            try
            {
                var isAdded = await _favoriteMovieService.AddFavoriteAsync(userId, movieId);
                if (!isAdded)
                {
                    return BadRequest(new { message = "Movie is already in favorites." });
                }

                return Ok(new { message = "Movie added to favorites!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavoriteMovies(Guid userId)
        {
            try
            {
                var favoriteMovies = await _favoriteMovieService.GetFavoriteMoviesAsync(userId);

                if (favoriteMovies == null || !favoriteMovies.Any())
                {
                    return NotFound(new { message = "No favorite movies found for this user." });
                }

                return Ok(new { message = "Favorite movies retrieved successfully.", movies = favoriteMovies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching favorite movies.", details = ex.Message });
            }
        }
    }
}
