namespace MovieApp.DTO.FavoriteMovie
{
    public class FavoriteMovieDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
