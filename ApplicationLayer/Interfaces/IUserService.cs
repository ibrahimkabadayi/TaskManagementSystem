using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<bool> RegisterUserAsync(UserDto user);
    Task<List<UserDto?>> GetAllUsersAsync(string email);
    Task<UserDto?> AuthenticateUserAsync(string email, string password);
    Task<bool> CheckUserExistsAsync(string email);
}