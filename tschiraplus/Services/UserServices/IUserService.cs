using Services.DTOs;

namespace Services.UserServices;

public interface IUserService
{
    void AddUserIfNoneExists();
    UserDto GetSystemUser();
    Task RegisterUserAsync(RegisterUserDto registerUserDto);
    Task LoginUserAsync(LoginUserDto loginUserDto);
    Task<bool> AuthenticateWithTokenAsync(string token);
}