using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using IAuthenticationService = Application.Interfaces.IAuthenticationService;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<AuthResult> RegisterAndLoginAsync(string name, string email, string password, HttpContext context)
    {
        var checkExists = await _userService.CheckUserExistsAsync(email);

        if (checkExists)
        {
            return new AuthResult{Success = false, Message = "Email already exists"};
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var profileLetters = "U";

        if (!string.IsNullOrWhiteSpace(name))
        {
            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (parts.Length)
            {
                case >= 2:
                    profileLetters = $"{parts[0][0]}{parts[^1][0]}";
                    break;
                case 1:
                {
                    var singleName = parts[0];
                    profileLetters = singleName.Length > 1 ? singleName[..2] : singleName;
                    break;
                }
            }
        }

        profileLetters = profileLetters.ToUpper();
        
        var user = new UserDto
        {
            Name = name,
            Email = email,
            Password = hashedPassword,
            ProfileColor = "#0079bf",
            ProfileLetters = profileLetters
        };

        var savedUser = await _userService.RegisterUserAsync(user);
        
        if (savedUser == null)
            return new AuthResult{Success = false, Message = "User not found"};
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, savedUser.Name!),
            new (ClaimTypes.Email, savedUser.Email!),
            new (ClaimTypes.NameIdentifier, savedUser.Id.ToString()),
            new (ClaimTypes.Role, "User")
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, 
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = false,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)   
        };
        
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        
        return new AuthResult
        {
            Success = true, 
            Message = "User created and logged in", 
            RedirectUrl = "/Home/Home",
            User = new UserInfo
            {
                Id = savedUser.Id,
                Name = savedUser.Name!,
                Email = savedUser.Email!,
                Role = "User"
            }
        };
    }

    public async Task<AuthResult> LoginAsync(string email, string password, HttpContext context)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return new AuthResult{Success = false, Message = "User not found"};
            
            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
            
            if (!isPasswordCorrect)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Email or password is incorrect"
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Role, "User")
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, 
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)   
            };
        
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        
            return new AuthResult
            {
                Success = true, 
                Message = "User created and logged in", 
                RedirectUrl = "/Home/Home",
                User = new UserInfo
                {
                    Id = user.Id,
                    Name = user.Name!,
                    Email = user.Email!,
                    Role = "User"
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new AuthResult
            {
                Success = false,
                Message = "There was an error logging in"
            };
        }
    }

    public async Task RefreshUserSessionAsync(string userId, string email, string newName, string role, string newColor)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, newName), 
            new Claim(ClaimTypes.Role, role),
            
            new Claim("ProfileColor", newColor) 
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddDays(7)
        };

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }

    public async Task LogoutAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}