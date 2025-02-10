namespace Services.DTOs;

public class UserDto
{
    public Guid UserId { get; init; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}