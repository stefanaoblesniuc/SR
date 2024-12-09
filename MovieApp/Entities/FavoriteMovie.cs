namespace MovieApp.Entities;

public class FavoriteMovie
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MovieId { get; set; }
    public DateTime AddedDate { get; set; }

    public User User { get; set; }
    public Movie Movie { get; set; }
}
