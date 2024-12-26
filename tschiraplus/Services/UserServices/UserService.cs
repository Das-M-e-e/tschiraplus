using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
                PasswordHash = "0",
                Status = UserStatus.Online,
                CreatedAt = DateTime.Now,
                LastLogin = DateTime.Now
            };
            
            _userRepository.AddUser(newUser);
        }
    }

    // Todo: Temporary, will remove when user profiles are implemented @Das_M_e_e_
    public UserDTO GetSystemUser()
    {
        return _userRepository.GetUserByUsername("System");
    }
}