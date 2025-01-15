using Microsoft.AspNetCore.Mvc;
using MovieApp.DTO;
using MovieApp.DTO.FavoriteMovie;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DislikeMovieController : Controller
    {
        public readonly DislikeMovieService _dislikeMovieService;

        public DislikeMovieController(DislikeMovieService dislikeMovieService)
        {
            _dislikeMovieService = dislikeMovieService;
        }

        [HttpPost("adddislike")]
        public async Task<IActionResult> AddDislike([FromBody] DislikeRequestDTO request)
        {
            try
            {
                var isAdded = await _dislikeMovieService.AddDislikeAsync(request.Username, request.MovieTitle);
                if (!isAdded)
                {
                    return BadRequest(new { message = "Movie is already in dislike." });
                }

                return Ok(new { message = "Movie added to dislike!" });
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
        public async Task<IActionResult> GetDislikeMovies(Guid userId)
        {
            try
            {
                var dislikeMovies = await _dislikeMovieService.GetDislikedMoviesAsync(userId);

                if (dislikeMovies == null || !dislikeMovies.Any())
                {
                    return NotFound(new { message = "No Disliked movies found for this user." });
                }

                return Ok(new { message = "Disliked movies retrieved successfully.", movies = dislikeMovies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Disliked movies.", details = ex.Message });
            }
        }
    }
}
