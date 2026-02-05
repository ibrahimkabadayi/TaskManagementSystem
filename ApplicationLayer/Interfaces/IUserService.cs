using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> RegisterUserAsync(UserDto user);
    Task<List<UserDto?>> GetAllUsersAsync();
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto?> AuthenticateUserAsync(string email, string password);
    Task<bool> CheckUserExistsAsync(string email);
    Task<UserDto?> GetUserWithProjectUsersAsync(int id);
    Task<bool> UpdateUserProfileAsync(int userId, string fullName, string profileColor);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}