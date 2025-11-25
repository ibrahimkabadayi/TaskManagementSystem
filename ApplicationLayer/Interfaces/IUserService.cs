using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<List<UserDto?>> GetAllUsersAsync(string email);
    Task<UserDto?> AuthenticateUserAsync(string name, string email, string password);
    Task<bool> CheckUserExistsAsync(string email);
}