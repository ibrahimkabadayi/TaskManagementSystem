using System.Linq.Expressions;
using Application.DTOs;
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

    public async Task<UserDto> GiveAccessToUser(string name, string email, string password)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            List<int> projectUserIdList = new List<int>();

            
            if(user.ProjectUsers != null)
            {
                foreach (var projectUser in user.ProjectUsers)
                {
                    projectUserIdList.Add(projectUser.Id);
                }
            }
            
            var userDto = new UserDto{Id = user.Id, Name =  name, Email = email, Password = password,  ProjectUserIds = projectUserIdList};
            
            return userDto;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<bool> CheckUserExists(string name, string email)
    {
        try
        {
            await _userRepository.GetByEmailAsync(email);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
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