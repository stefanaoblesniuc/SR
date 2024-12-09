namespace MovieApp.Entities
{
    public class PreferenceForm
    {
        public Guid Id { get; set; } // Unique ID for the preference form
        public Guid UserId { get; set; } // UserId (foreign key)
        public List<string> Genres { get; set; } // List of selected genres
        public string IMDBScore { get; set; } // IMDB score preference
        public string Language { get; set; } // Language preference
        public DateTime CreatedDate { get; set; } // Date the preference was created

        public User User { get; set; } // Navigation property to the User
    }
}
