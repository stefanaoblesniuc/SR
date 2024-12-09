using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.DTO;
using MovieApp.Entities;
using MovieApp.Services;
using MovieApp.Utils;
using System.Globalization;
using System.Reflection.Metadata;
using MovieApp.DTO.Movie;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly MovieService _movieService;
        private readonly CSVService _csvService;

        public MovieController(MovieService movieService, CSVService csvService)
        {
            _movieService = movieService;
            _csvService = csvService;
        }

        [HttpGet("getMovie")]
        public async Task<IActionResult> GetMovie([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Movie name is required.");
            }

            var movie = await _movieService.GetMovie(name);
            if (movie == null)
            {
                return NotFound(new { message = "No movie with this name found." });
            }

            return Ok(new { message = "Movie found", movie });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMovies()
        {
            string file = Constants.CONST_CSVPath; // Path to your CSV file

            if (!System.IO.File.Exists(file))
            {
                return BadRequest("CSV file not found.");
            }

            try
            {
                var movies = new List<Movie>();

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null, // Ignore missing fields
                    BadDataFound = null,   // Ignore malformed rows
                    HeaderValidated = null
                };

                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<MovieMap>();

                    // Read and map movies
                    movies = csv.GetRecords<Movie>().ToList();
                }

                foreach (var movie in movies)
                {
                    // Optional: Validate movie data (e.g., check for required fields)
                    if (string.IsNullOrWhiteSpace(movie.Title) || string.IsNullOrWhiteSpace(movie.Genre))
                    {
                        continue; // Skip invalid rows
                    }

                    // Save each movie to the database
                    await _movieService.AddMovie(movie);
                }

                return Ok("Movies uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
