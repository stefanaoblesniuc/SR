using Microsoft.EntityFrameworkCore;
using MovieApp.DataBase;
using MovieApp.DTO;
using MovieApp.Entities;
using Recombee.ApiClient.ApiRequests;
using Recombee.ApiClient;
using System;
using MovieApp.DTO.Movie;

namespace MovieApp.Services
{
    public class MovieService
    {
        private readonly MovieAppDbContext _context;
        private readonly RecombeeService _recombeeService;
        private readonly FavoriteMovieService _favoriteMovieService;
        private readonly DislikeMovieService _dislikeMovieService;

        public MovieService(MovieAppDbContext context, RecombeeService recombeeService, FavoriteMovieService favoriteMovieService,
            DislikeMovieService dislikeMovieService)
        {
            _context = context;
            _favoriteMovieService = favoriteMovieService;
            _dislikeMovieService = dislikeMovieService;
            _recombeeService = recombeeService;
        }

        public async Task<List<RecommMovieDTO>> GetGoodRecommendationsAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var favorites = await _favoriteMovieService.GetFavoriteMoviesAsync(user.Id);

            if (favorites == null)
            {
                throw new Exception("No favorite movies found for the user.");
            }
            var recommendations = new List<string>();

            foreach (var movie in favorites)
            {

                var response = await _recombeeService.SendRecomm(movie, user);

                recommendations.AddRange(response.Recomms.Select(r => r.Id));
            }

            // Remove duplicates from recommendations
            recommendations = recommendations.Distinct().ToList();
            var movieReccomDTOs = new List<RecommMovieDTO>();

            foreach (var movieId in recommendations)
            {
                Guid movieGuid;
                if (Guid.TryParse(movieId, out movieGuid))
                {
                    var movie = await GetMovieById(movieGuid); // assuming GetMovieById returns a Movie object
                    if (movie != null)
                    {
                        var movieDTO = new RecommMovieDTO
                        {
                            Title = movie.Title,
                            Genre = movie.Genre,
                            IMDBScore = movie.IMDBScore,
                            Language = movie.Language,
                            Runtime = movie.Runtime // add any other properties you need here
                        };

                        movieReccomDTOs.Add(movieDTO);
                    }
                }
            }
            return movieReccomDTOs;
        }

        public async Task<List<RecommMovieDTO>> GetDislikeRecommendationsAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var dislikes = await _dislikeMovieService.GetDislikedMoviesAsync(user.Id);

            if (dislikes == null)
            {
                throw new Exception("No favorite movies found for the user.");
            }
            var recommendations = new List<string>();

            foreach (var movie in dislikes)
            {

                var response = await _recombeeService.SendRecommDislike(movie, user);

                recommendations.AddRange(response.Recomms.Select(r => r.Id));
            }

            // Remove duplicates from recommendations
            recommendations = recommendations.Distinct().ToList();
            var movieReccomDTOs = new List<RecommMovieDTO>();

            foreach (var movieId in recommendations)
            {
                Guid movieGuid;
                if (Guid.TryParse(movieId, out movieGuid))
                {
                    var movie = await GetMovieById(movieGuid); // assuming GetMovieById returns a Movie object
                    if (movie != null)
                    {
                        var movieDTO = new RecommMovieDTO
                        {
                            Title = movie.Title,
                            Genre = movie.Genre,
                            IMDBScore = movie.IMDBScore,
                            Language = movie.Language,
                            Runtime = movie.Runtime // add any other properties you need here
                        };

                        movieReccomDTOs.Add(movieDTO);
                    }
                }
            }
            return movieReccomDTOs;
        }

        public async Task<List<RecommMovieDTO>> GetRandomRecommendationsAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var response = await _recombeeService.SendRandomRecomm(user);

            // Remove duplicates from recommendations
            var recommendations = response.Recomms
                                .Select(recommendation => recommendation.Id) // Extract the `Id` property (or another property you need)
                                .Distinct() // Remove duplicates
                                .ToList();
            var movieReccomDTOs = new List<RecommMovieDTO>();

            foreach (var movieId in recommendations)
            {
                Guid movieGuid;
                if (Guid.TryParse(movieId, out movieGuid))
                {
                    var movie = await GetMovieById(movieGuid); // assuming GetMovieById returns a Movie object
                    if (movie != null)
                    {
                        var movieDTO = new RecommMovieDTO
                        {
                            Title = movie.Title,
                            Genre = movie.Genre,
                            IMDBScore = movie.IMDBScore,
                            Language = movie.Language,
                            Runtime = movie.Runtime // add any other properties you need here
                        };

                        movieReccomDTOs.Add(movieDTO);
                    }
                }
            }
            return movieReccomDTOs;
        }


        public async Task<Movie?> GetMovieById(Guid id)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
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
               // Runtime = movieDto.Runtime,
                Language = movieDto.Language
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> UpdateMovieRuntimeAsync()
        {
            Random random = new Random();
            int randomNumber = random.Next(50, 201);
            // Find the movie by ID
            // Fetch all movies from the database
            var movies = await _context.Movies.ToListAsync();

            // Apply custom logic to update each movie's runtime
            foreach (var movie in movies)
            {
                randomNumber = random.Next(50, 201);
                // Example: Add additional runtime to each movie
                movie.Runtime = randomNumber.ToString();
            }

            // Save all changes to the database
            var updatedCount = await _context.SaveChangesAsync();

            return true; // Returns the number of rows updated
        }
    }
}
