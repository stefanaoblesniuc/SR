namespace MovieApp.DTO.PreferenceForm;

public class PreferenceFormAddDTO
{
    public List<string> Genres { get; set; } // List of selected genres
    public string IMDBScore { get; set; } // IMDB score preference
    public string Language { get; set; } // Language preference
    public string Username { get; set; }
}
