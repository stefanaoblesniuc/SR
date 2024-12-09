namespace MovieApp.DTO.User;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
