using Core.Models;
using PetaPoco;
using Services.DTOs;

namespace Services.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Database _db;

    public UserRepository(Database db)
    {
        _db = db;
    }

    public void AddUser(UserModel user)
    {
        _db.Insert("Users", "UserId", user);
    }

    public List<UserModel> GetAllUsers()
    {
        return _db.Fetch<UserModel>("SELECT * FROM Users");
    }

    public UserDTO GetUserByUsername(string username)
    {
        var user = _db.SingleOrDefault<UserModel>("SELECT * FROM Users WHERE Username = @0", username);

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username
        };
    }
}