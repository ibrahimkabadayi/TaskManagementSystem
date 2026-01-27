using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Interfaces;

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

    public async Task<UserDto?> GetUserWithProjectUsersAsync(int id)
    {
        var user = await _userRepository.GetUserWithProjectUsersAsync(id);
        return _mapper.Map<User, UserDto>(user);
    }

    public async Task<UserDto?> RegisterUserAsync(UserDto user)
    {
        try
        {
            var newUser = _mapper.Map<UserDto, User>(user);
            await _userRepository.AddAsync(newUser);
            var savedUserDto = _mapper.Map<User, UserDto>(newUser);
            return savedUserDto;  
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null; 
        }
    }

    public async Task<List<UserDto?>> GetAllUsersAsync()
    {
        var users =  await _userRepository.GetAllAsync();
        return (users == null ? null : _mapper.Map<List<User>, List<UserDto>>(users))!;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : _mapper.Map<User, UserDto>(user);
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
    
    public async Task<bool> UpdateUserProfileAsync(int userId, string fullName, string profileColor)
    {
        var user = await _userRepository.GetByAsyncId(userId);
        if (user == null) return false;

        user.Name = fullName;
        user.ProfileColor = profileColor;
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (parts.Length)
            {
                case >= 2:
                    user.ProfileLetters = $"{parts[0][0]}{parts[^1][0]}".ToUpper();
                    break;
                case 1:
                {
                    var singleName = parts[0];
                    user.ProfileLetters = (singleName.Length > 1 ? singleName.Substring(0, 2) : singleName).ToUpper();
                    break;
                }
            }
        }
    
        await _userRepository.UpdateAsync(user);
        return true;
    }
}