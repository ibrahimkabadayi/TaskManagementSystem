using System.Linq.Expressions;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService
{
    ApplicationDbContext context;
    UserRepository _userRepository;

    public UserService()
    {
        context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        _userRepository = new UserRepository(context);
    }

    public async void AddUser(User user)
    {
        try
        {
            if (user.Name.Length < 5)
            {
                throw new ArgumentException("User name is too short.");
            }
            
            if (user.Password.Length < 6)
            {
                throw new ArgumentException("Password is too short.");
            }

            if (user.Password.Contains(' '))
            {
                throw new ArgumentException("Password must not contain spaces.");
            }

            if (LengthOfLongestSubstring(user.Password) < 3)
            {
                throw new ArgumentException("Password must contain more than 3 non repeat substrings.");
            }
            
            if (user.Password.Length < 11)
            {
                throw new ArgumentException("Email is too short.");
            }

            if (!user.Password.Contains("@gmail.com"))
            {
                throw new ArgumentException("Email must contain \"@gmail.com\".");
            }
            
            await _userRepository.AddAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException?.Message);
        }
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAllAsync();
    }

    public async void DeleteUser(User user)
    {
        try
        {
            await _userRepository.DeleteAsync(user.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async void UpdateUser(User user)
    {
        try
        {
            await _userRepository.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<List<User>> GetUser(Expression<Func<User, bool>> predicate)
    {
       return await _userRepository.FindAsync(predicate);
    }

    public async Task<bool> CheckUserExists(Expression<Func<User, bool>> predicate)
    {
        return await _userRepository.ExistsAsync(predicate);
    }
    
    private static int LengthOfLongestSubstring(string s)
    {
        var lastIndex = new int[256];
        for (var i = 0; i < 256; i++) lastIndex[i] = -1;

        var maxLen = 0;
        var left = 0;

        for (var right = 0; right < s.Length; right++)
        {
            var ch = s[right];
            if (lastIndex[ch] >= left)
            {
                left = lastIndex[ch] + 1;
            }

            lastIndex[ch] = right;
            var windowLen = right - left + 1;
            if (windowLen > maxLen)
            {
                maxLen = windowLen;
            }
        }

        return maxLen;
    }
}