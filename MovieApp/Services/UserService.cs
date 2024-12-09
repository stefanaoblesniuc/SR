using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using MovieApp.Entities;
using MovieApp.DataBase;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.Services;

public class UserService
{
    private readonly MovieAppDbContext _dbContext;

    public UserService(MovieAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUser(string username, string password)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public async Task<User> AddUser(string username, string password)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Username == username))
        {
            throw new Exception("User already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Password = password
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
}