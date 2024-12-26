using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    // Todo: Temporary, will remove when user profiles are implemented @Das_M_e_e_
    public void AddUserIfNoneExists()
    {
        if (_userRepository.GetAllUsers().Count == 0)
        {
            var newUser = new UserModel
            {
                UserId = Guid.Empty,
                Username = "System",
                Email = "System",
                Status = UserStatus.Online,
                CreatedAt = DateTime.Now,
                LastLogin = DateTime.Now
            };
            
            _userRepository.AddUser(newUser);
        }
    }

    // Todo: Temporary, will remove when user profiles are implemented @Das_M_e_e_
    public UserDto GetSystemUser()
    {
        return _userRepository.GetUserByUsername("System");
    }

    public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        await _authService.RegisterAsync(registerUserDto);
    }
}