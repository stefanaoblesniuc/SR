using CsvHelper.Configuration;
using MovieApp.Entities;
using MovieApp.Utils;

namespace MovieApp.DTO.Movie;

public class MovieMap : ClassMap<MovieApp.Entities.Movie>
{
    public MovieMap()
    {
        Map(m => m.Title).Name("Title");
        Map(m => m.Genre).Name("Genre");
        Map(m => m.Premiere).Name("Premiere");
        Map(m => m.IMDBScore).Name("IMDB Score"); // Match the header in the CSV
        Map(m => m.Language).Name("Language");
        Map(m => m.Runtime).Name("Runtime");
        Map(m => m.Id).Ignore(); // Ignore the 'Id' property since it's not in the CSV
    }
}
