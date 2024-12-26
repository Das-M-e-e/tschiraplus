using Services.DTOs;

namespace Services.UserServices;

public interface IUserService
{
    public void AddUserIfNoneExists();
    public UserDTO GetSystemUser();
}