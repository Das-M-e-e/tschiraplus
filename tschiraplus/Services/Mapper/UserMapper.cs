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


    public UserModel toModel(UserDto dto)
    {
        var userModel = _userRepository.GetUserById(dto.UserId);
        if (userModel == null)
        {
            throw new NullReferenceException($"User {dto.UserId} not found");
        }
        
        userModel.Username = dto.Username;
        userModel.Email = dto.Email;
        userModel.Bio = dto.Bio;

        return userModel;
    }
    
    
    
}