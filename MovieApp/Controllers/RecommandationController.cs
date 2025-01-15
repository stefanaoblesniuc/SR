using Microsoft.AspNetCore.Mvc;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommandationController : Controller
    {
        private readonly RecombeeService _recommendationService;
        private readonly MovieService _movieService;

        public RecommandationController(RecombeeService recommendationService, MovieService movieService)
        {
            _recommendationService = recommendationService;
            _movieService = movieService;
        }

        [HttpGet("coldStartRecc")]
        public async Task<IActionResult> GetRecommendations([FromQuery] string username)
        {
            Console.WriteLine($"Received request for recommendations with username: {username}");
            var recommendations = await _recommendationService.GetRecommendationsAsync(username);

            if (recommendations.Count == 0)
            {
                return NotFound("No recommendations found based on the given preferences.");
            }

            return Ok(new { recommendations });
        }

        [HttpGet("normalReccLike")]
        public async Task<IActionResult> GetRecommLike(string username)
        {
            var recommendations = await _movieService.GetGoodRecommendationsAsync(username);

            if (recommendations.Count == 0)
            {
                return NotFound("No recommendations found based on the given preferences.");
            } 

            return Ok(new { recommendations });
        }


        [HttpGet("normalReccDislike")]
        public async Task<IActionResult> GetRecommDislike(string username)
        {
            var recommendations = await _movieService.GetDislikeRecommendationsAsync(username);

            if (recommendations.Count == 0)
            {
                return NotFound("No recommendations found based on the given preferences.");
            }

            return Ok(new { recommendations });
        }


        [HttpGet("randomRecc")]
        public async Task<IActionResult> GetRandomRecomm(string username)
        {
            var recommendations = await _movieService.GetRandomRecommendationsAsync(username);

            if (recommendations.Count == 0)
            {
                return NotFound("No recommendations found based on the given preferences.");
            }

            return Ok(new { recommendations });
        }
    }
}
