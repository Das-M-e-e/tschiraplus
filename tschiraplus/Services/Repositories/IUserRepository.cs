using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface IUserRepository
{
    public void AddUser(UserModel user);
    public List<UserModel> GetAllUsers();
    public UserDTO GetUserByUsername(string username);
}