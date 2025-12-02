using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByAsyncId(id);
        
        return user == null ? null : _mapper.Map<User, UserDto>(user);
    }

    public async Task<bool> RegisterUserAsync(UserDto user)
    {
        try
        {
            var newUser = _mapper.Map<UserDto, User>(user);
            await _userRepository.AddAsync(newUser);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<List<UserDto?>> GetAllUsersAsync(string email)
    {
        var users =  await _userRepository.GetAllAsync();
        return users == null ? null : _mapper.Map<List<User>, List<UserDto>>(users);
    }
    
    public async Task<UserDto?> AuthenticateUserAsync(string email, string password)
    {
        var isUserReal = await CheckUserExistsAsync(email);

        if (!isUserReal)
        {
            return null;
        }

        var user = await _userRepository.GetByEmailAsync(email);

        return user!.Password != password ? throw new Exception("Wrong password") : _mapper.Map<User, UserDto>(user);
    }

    public async Task<bool> CheckUserExistsAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user != null;
    }
}