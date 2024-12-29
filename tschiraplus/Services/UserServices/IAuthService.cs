using Services.DTOs;

namespace Services.UserServices;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserDto registerUserDto);
    Task<LoginResponse> LoginAsync(LoginUserDto loginUserDto);
    Task LogoutAsync();
    void SaveToken(string token);
    string? LoadToken();
    bool IsTokenValid(string? token);
}