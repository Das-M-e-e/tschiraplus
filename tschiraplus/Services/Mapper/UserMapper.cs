using Core.Models;
using Services.DTOs;

namespace Services.Mapper;

public class UserMapper
{
    public static UserDto toDto(UserModel model)
    {
        var ProjectDto = new UserDto()
        {
            UserId = model.UserId,
            Username = model.Username
        };
        
        return ProjectDto;
    }


    public static UserModel toModel(UserDto dto)
    {
        var UserModel = new UserModel()
        {
            UserId = dto.UserId,
            Username = dto.Username,
            
        };
        
        return UserModel;
    }
    
    
    
}