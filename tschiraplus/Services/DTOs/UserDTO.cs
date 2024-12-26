namespace Services.DTOs;

public class UserDto
{
    public Guid UserId { get; init; }
    public string Username { get; set; } = string.Empty;
}