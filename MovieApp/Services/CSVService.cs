using CsvHelper;
using MovieApp.Entities;
using System.Globalization;

namespace MovieApp.Services
{
    public class CSVService
    {
        public IEnumerable<Movie> ParseMovies(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Movie>().ToList();
        }
    }
}
