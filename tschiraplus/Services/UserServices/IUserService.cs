using Services.DTOs;

namespace Services.UserServices;

public interface IUserService
{
    void AddUserIfNoneExists();
    UserDto GetSystemUser();
    Task RegisterUserAsync(RegisterUserDto registerUserDto);
}