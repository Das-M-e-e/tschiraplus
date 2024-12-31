using Core.Models;
using Newtonsoft.Json;
using PetaPoco;
using Services.DatabaseServices;
using Services.DTOs;

namespace Services.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Database _db;
    private readonly RemoteDatabaseService _remoteDb;

    public UserRepository(Database db, RemoteDatabaseService remoteDb)
    {
        _db = db;
        _remoteDb = remoteDb;
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

    /// <summary>
    /// Gets a user by id from the local database
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private UserModel? GetUserById(Guid userId)
    {
        return _db.SingleOrDefault<UserModel>($"SELECT * FROM Users WHERE UserId = @0", userId);
    }
    
    //****** REMOTE DB ******//
    /// <summary>
    /// Gets a user by id from the remote database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The UserModel of the wanted user</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<UserModel> GetUserByIdAsync(Guid id)
    {
        var jsonString = await _remoteDb.GetByIdAsync("Users", id);

        var user = JsonConvert.DeserializeObject<UserModel>(jsonString);

        if (user == null)
        {
            throw new InvalidOperationException("Failed to deserialize json string to UserModel");
        }

        return user;
    }
    
    //****** HELPER ******//
    /// <summary>
    /// Checks if a user exists in the local database by id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>true or false</returns>
    public bool UserExists(Guid userId)
    {
        var user = GetUserById(userId);

        return user != null;
    }
}