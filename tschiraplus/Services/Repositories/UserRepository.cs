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

    //****** LOCAL DB ******//
    /// <summary>
    /// Saves a user to the local database
    /// </summary>
    /// <param name="user"></param>
    public void AddUser(UserModel user)
    {
        _db.Insert("Users", "UserId", user);
    }

    /// <summary>
    /// Gets all users saved in the local database
    /// </summary>
    /// <returns>A List of all users as UserModel</returns>
    public List<UserModel> GetAllUsers()
    {
        return _db.Fetch<UserModel>("SELECT * FROM Users");
    }

    /// <summary>
    /// Gets a user by username from the local database
    /// </summary>
    /// <param name="username"></param>
    /// <returns>The wanted user as UserDto</returns>
    public UserDto GetUserByUsername(string username)
    {
        var user = _db.SingleOrDefault<UserModel>("SELECT * FROM Users WHERE Username = @0", username);

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username
        };
    }
}