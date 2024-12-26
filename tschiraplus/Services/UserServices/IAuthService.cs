using Services.DTOs;

namespace Services.UserServices;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserDto registerUserDto);
    Task<string> LoginAsync(LoginUserDto loginUserDto);
    Task LogoutAsync();
}