using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.UserServices;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void AddUserIfNoneExists()
    {
        if (_userRepository.GetAllUsers().Count == 0)
        {
            var newUser = new UserModel
            {
                UserId = Guid.NewGuid(),
                Username = "System",
                Email = "System",
                PasswordHash = "0",
                Status = UserStatus.Online,
                CreatedAt = DateTime.Now,
                LastLogin = DateTime.Now
            };
            
            _userRepository.AddUser(newUser);
        }
    }

    public UserDTO GetSystemUser()
    {
        return _userRepository.GetUserByUsername("System");
    }
}