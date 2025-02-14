using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.Mapper;

public class UserMapper
{
    private readonly IUserRepository _userRepository;

    public UserMapper(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    /// <summary>
    /// Creates a UserDto from a UserModel
    /// </summary>
    /// <param name="model"></param>
    /// <returns>The UserModel</returns>
    public UserDto toDto(UserModel model)
    {
        var userDto = new UserDto()
        {
            UserId = model.UserId,
            Username = model.Username,
            Email = model.Email,
            Bio = model.Bio
        };
        
        return userDto;
    }

    /// <summary>
    /// Creates a UserModel from a UserDto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>The UserModel</returns>
    public UserModel toModel(UserDto dto)
    {
        var userModel = _userRepository.GetUserById(dto.UserId);
        if (userModel == null)
        {
            userModel = new UserModel
            {
                UserId = dto.UserId,
                Username = dto.Username,
                Email = dto.Email,
                Bio = dto.Bio
            };
        }
        else
        {
            userModel.Username = dto.Username;
            userModel.Email = dto.Email;
            userModel.Bio = dto.Bio;
        }

        return userModel;
    }
}