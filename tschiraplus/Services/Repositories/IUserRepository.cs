using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface IUserRepository
{
    public void AddUser(UserModel user);
    public List<UserModel> GetAllUsers();
    public UserDto GetUserByUsername(string username);
    Task<UserModel> GetUserByIdAsync(Guid id);
    bool UserExists(Guid userId);
}