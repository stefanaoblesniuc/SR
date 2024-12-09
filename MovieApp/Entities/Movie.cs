namespace MovieApp.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Premiere { get; set; }
        public string IMDBScore { get; set; }
        public string Language { get; set; }
    }
}
