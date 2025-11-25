using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<UserDto?>> GetAllUsersAsync(string email)
    {
        var users =  await _userRepository.GetAllAsync();
        return users == null ? null : _mapper.Map<List<User>, List<UserDto>>(users);
    }

    public async Task<UserDto> AuthenticateUserAsync(string name, string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckUserExistsAsync(string email)
    {
        throw new NotImplementedException();
    }
}