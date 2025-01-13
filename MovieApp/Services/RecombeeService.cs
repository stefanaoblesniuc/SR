using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;
using Recombee.ApiClient.Bindings;
using Recombee.ApiClient.Util;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using MovieApp.DataBase;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using MovieApp.DTO.Movie;
using System.Security.Cryptography.Xml;
using static System.Net.WebRequestMethods;


namespace MovieApp.Services
{
    public class RecombeeService
    {
        private readonly RecombeeClient _recombeeClient;
        private readonly MovieAppDbContext _dbContext;
        private readonly PreferenceFormService _formService;
        private readonly FavoriteMovieService _favoriteMovieService;
        private readonly MovieService _movieService;

        public RecombeeService(string database, string token, MovieAppDbContext dbContext, PreferenceFormService formService,
            FavoriteMovieService favoriteMovieService)
        {
            _formService = formService;
            _favoriteMovieService = favoriteMovieService;
           // _movieService = movieService;
            _dbContext = dbContext;
            _recombeeClient = new RecombeeClient(database, token);

        }

       

        public async Task<List<RecommMovieDTO>> GetRecommendationsAsync(string username)
        {
            // Retrieve the user by username
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Retrieve the user's preferences
            var preferences = await _formService.GetPreferencesAsync(user.Id);
            if (preferences == null)
            {
                throw new Exception("No preferences found for the user.");
            }
            string sentence;
            if (preferences.IMDBScore == "yes")
            {
                sentence = "and 5 > 'IMDB-Score'";
            } else
            {
                sentence = "";
            }

            RecommendationResponse result = _recombeeClient.Send(new RecommendItemsToUser(user.Id.ToString(), 2,
            returnProperties: true,
            filter: $"\"{preferences.Genres}\" in 'Genre' and \"{preferences.Language}\" in 'Language' {sentence}"
              )
            );

            // Mapping function to create Movie objects
            List<RecommMovieDTO> recommendedMovies = result.Recomms.Select(r =>
            {
                string titleProperty = r.Values.GetValueOrDefault("Title").ToString();
                string genreProperty = r.Values.GetValueOrDefault("Genre").ToString();
                var languageProperty = r.Values.GetValueOrDefault("Language").ToString();
                var imdbScoreProperty = r.Values.GetValueOrDefault("IMDB-Score").ToString();

                return new RecommMovieDTO
                {
                    Title = titleProperty,
                    Genre = genreProperty,
                    Language = languageProperty,
                    IMDBScore = imdbScoreProperty
                };
            }).ToList();

            return recommendedMovies;

            // Select recommendations and map to Movie objects
            //  List<Movie> recommendedMovies = 

            List<Movie> recommendedMovies1 = result.Recomms.Select(r => new Movie
            {
                Title = (string)r.Values["Title"],
                Genre = (string)r.Values["Genre"],
                Language = (string)r.Values["Language"],
                IMDBScore = (string)r.Values["IMDB-Score"]
            }).ToList();

            //return result.Recomms.Select(r => r.Id).ToList();
        }

        // Metodă pentru configurarea bazei Recombee
         public void ConfigureDatabase()
         {
             try
             {
                // Adaugă proprietăți pentru item
                _recombeeClient.Send(new AddItemProperty("Title", "string"));
                _recombeeClient.Send(new AddItemProperty("Genre", "string"));
                _recombeeClient.Send(new AddItemProperty("Premiere", "string"));
                _recombeeClient.Send(new AddItemProperty("Runtime", "int"));
                _recombeeClient.Send(new AddItemProperty("IMDB-Score", "double"));
                _recombeeClient.Send(new AddItemProperty("Language", "string"));
                 Console.WriteLine("Baza de date a fost configurată cu succes!");
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Eroare la configurarea bazei: {ex.Message}");
             }
         }

        // Metodă pentru încărcarea datelor dintr-un fișier CSV
        // public void LoadDataFromCsv(string filePath, int maxLines = 585)
        // {
        //     try
        //     {
        //         var requests = new List<Request>();
        //         int lineCount = 0;
        //         int itemId = 0;

        //         using (var reader = new StreamReader(filePath))
        //         {
        //             var headerLine = reader.ReadLine();
        //             if (headerLine == null)
        //                 throw new Exception("Fișierul CSV este gol!");

        //             var headers = headerLine.Split(',');

        //             Console.WriteLine("Header extras din fișierul CSV:");
        //             Console.WriteLine(string.Join(", ", headers));

        //             while (!reader.EndOfStream)
        //             {
        //                 var line = reader.ReadLine();
        //                 var values = line?.Split(',');

        //                 if (values == null) continue;

        //                 // Selectăm primele 4 coloane (sau câte sunt disponibile)
        //                 var selectedColumns = new Dictionary<string, object>
        //                 {
        //                     { headers[0], values[0] }, // Title
        //                     { headers[1], values[1] }, // Genre
        //                     { headers[2], values[2] }, // Premiere
        //                     { headers[3], values[3] },  // Runtime
        //                     { headers[4], values[4] },
        //                     { headers[5], values[5] }
        //                 };

        //                 // Adaugă cererea pentru setarea valorilor item-ului
        //                 var request = new SetItemValues(itemId.ToString(), selectedColumns, cascadeCreate: true);
        //                 requests.Add(request);

        //                 itemId++;
        //                 lineCount++;

        //                 if (lineCount >= maxLines)
        //                     break;
        //             }
        //         }

        //         // Trimite cererile în Batch
        //         var response = client.Send(new Batch(requests));
        //         Console.WriteLine($"S-au încărcat cu succes {lineCount} elemente din CSV.");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Eroare la încărcarea datelor din CSV: {ex.Message}");
        //     }
        // }

        public async Task AddUserAsync(string userId)
        {
            try
            {
                await _recombeeClient.SendAsync(new AddUser(userId));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user to Recombee: {ex.Message}");
                throw;
            }
        }

        public async Task<RecommendationResponse> SendRecomm(Movie movie,MovieApp.Entities.User user)
        {
            try
            {
                var response = await _recombeeClient.SendAsync(new RecommendItemsToItem(
                    movie.Id.ToString(),
                    user.Id.ToString(),// ID of the favorite movie
                    3                  // Number of recommendations
                ));
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user to Recombee: {ex.Message}");
                throw;
            }
        }

        public async Task AddMovieAsync(Movie movie)
        {

            try
            {
                // await _recombeeClient.SendAsync(new AddItem(
                //     movie.Id.ToString()));

                await _recombeeClient.SendAsync(new SetItemValues(
                    movie.Id.ToString(), new Dictionary<string, object>
                            {
                            //{ "Title", movie.Title },
                            //{ "Genre", movie.Genre},
                            //{ "Premiere", movie.Premiere },
                            //{ "Language", movie.Language },
                            {"Runtime", movie.Runtime }
                            //{ "IMDB-Score", movie.IMDBScore },
                            }));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user to Recombee: {ex.Message}");
                throw;
            }


        }
         // Use Movie.Id as the Recombee item ID
                 /*   new Dictionary<string, object>
                            {
                            { "Title", movie.Title
    },
                            { "Genre", movie.Genre
},
                            { "Premiere", movie.Premiere },
                            { "Language", movie.Language },
                            { "IMDB-Score", movie.IMDBScore },
                            })*/

        // Metodă pentru a testa conexiunea (opțional)

       /* public async Task LoadDataFromCsv(string filePath, int maxLines = 585)
        {
            
            try
            {
                var requests = new List<Request>();
                
                int lineCount = 0;
                int itemId = 0;

                // Configurația pentru CSV (ignora diferențele majuscule/minuscule în header)
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null, // Ignorăm câmpurile lipsă
                    BadDataFound = context => Console.WriteLine($"Linie invalidă: {context.RawRecord}")
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    // Citim fișierul CSV
                    var records = csv.GetRecords<dynamic>().Take(maxLines);

                    // Maparea dintre coloanele CSV și proprietățile Recombee
                    var propertyMapping = new Dictionary<string, string>
                    {
                        { "Title", "Title" },
                        { "Genre", "Genre" },
                        { "Premiere", "Premiere" },
                        { "Runtime", "Runtime" },
                        { "IMDB Score", "IMDB-Score" },
                        { "Language", "Language" }
                    };

                   /* foreach (var movie in records)
                    {
                         _recombeeClient.SendAsync(new SetItemValues(
                                        movie.Id.ToString(), // Use Movie.Id as the Recombee item ID
                                        new Dictionary<string, object>
                                        {
                                        { "Title", movie.Title },
                                        { "Genre", movie.Genre },
                                        { "Premiere", movie.Premiere },
                                        { "Language", movie.Language },
                                        { "IMDB-Score", movie.IMDBScore },

                                        }
                                    ));
                    }*/

                 /*   foreach (var record in records)
                    {
                        var recordDict = (IDictionary<string, object>)record;

                        var selectedColumns = new Dictionary<string, object>();

                        foreach (var column in propertyMapping)
                        {
                            if (recordDict.TryGetValue(column.Key, out var value))
                            {
                                selectedColumns[column.Value] = value;
                            }
                        }

                        // Console.WriteLine("Selected columns pentru item:");
                        // foreach (var kvp in selectedColumns)
                        // {
                        //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                        // }

                        // Adaugă cererea pentru setarea valorilor item-ului
                        var request = new SetItemValues(itemId.ToString(), selectedColumns, cascadeCreate: true);
                        requests.Add(request);

                        itemId++;
                        lineCount++;
                    }
                }

                // Trimite cererile în Batch
                Console.WriteLine($"Număr total de cereri: {requests.Count}");
                var response = _recombeeClient.Send(new Batch(requests));
                Console.WriteLine($"Răspuns de la Recombee: {response.Responses}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încărcarea datelor din CSV: {ex.Message}");
            }
        }*/
    }
}
