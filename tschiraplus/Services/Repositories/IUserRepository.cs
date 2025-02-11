using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface IUserRepository
{
    void AddUser(UserModel user);
    List<UserModel> GetAllUsers();
    UserDto GetUserByUsername(string username);
    Task<UserModel> GetUserByIdAsync(Guid id);
    bool UserExists(Guid userId);
    Task<bool> SendInvitationAsync(ProjectInvitationModel invitation);
    UserModel? GetUserById(Guid userId);
}