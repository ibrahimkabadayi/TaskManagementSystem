using Application.DTOs;
using Application.Services;
using AutoMapper;
using DataAccessLayer.Context;
using DataAccessLayer.Implementations;
using DomainLayer.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace TaskManegementSystem.Tests.ServicesTests;

public class UserServiceTests : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly Mock<IMapper> _mockMapper;
    
    public UserServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;
        
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
        
        var userRepository = new UserRepository(_context);

        _mockMapper = new Mock<IMapper>();

        _userService = new UserService(userRepository, _mockMapper.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldAddUserToDatabase_WhenValid()
    {
        var userDto = new UserDto { Name = "Ibrahim", Email = "test@example.com", Password = "123456"};
        var userEntity = new User { Name = "Ibrahim", Email = "test@example.com", Password = "123456"};

        _mockMapper.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
            .Returns(userEntity);
        
        _mockMapper.Setup(m => m.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var result = await _userService.RegisterUserAsync(userDto);

        Assert.NotNull(result);

        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        
        Assert.NotNull(userInDb);
        Assert.Equal("Ibrahim", userInDb.Name);
    }

    [Fact]
    public async Task GettingByEmailAsync_ShouldReturnUser_WhenValid()
    {
        var userDto = new UserDto { Name = "Ibrahim", Email = "test@example.com", Password = "123456"};
        var userEntity = new User { Name = "Ibrahim", Email = "test@example.com", Password = "123456"};

        _mockMapper.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
            .Returns(userEntity);
        
        _mockMapper.Setup(m => m.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);

        var addedUserDto = await _userService.RegisterUserAsync(userDto);
        
        var result = await _userService.GetUserByEmailAsync(userDto.Email);
        
        Assert.NotNull(result);
        Assert.Equal(addedUserDto!.Email, result.Email);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers_WhenValid()
    {
        var userDto = new UserDto { Name = "Ibrahim", Email = "test@example.com", Password = "123456"};
        
        var userEntity = new User { Name = "Ibrahim", Email = "test@example.com", Password = "123456"};
        

        _mockMapper.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
            .Returns(userEntity);
        
        _mockMapper.Setup(m => m.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto);
        
        var result = await _userService.RegisterUserAsync(userDto);
        
        var userDto2 = new UserDto {Name = "Ali", Email = "test2@example.com", Password = "1234567"};
        var userEntity2 = new User {Name = "Ali", Email = "test2@example.com", Password = "1234567"};
        
        _mockMapper.Setup(m => m.Map<UserDto, User>(It.IsAny<UserDto>()))
            .Returns(userEntity2);
        
        _mockMapper.Setup(m => m.Map<User, UserDto>(It.IsAny<User>()))
            .Returns(userDto2);
        
        var userDtoList = new List<UserDto> { userDto, userDto2 };
        
        _mockMapper
            .Setup(m => m.Map<List<User>, List<UserDto>>(It.IsAny<List<User>>()))
            .Returns(userDtoList);
        
        var result2 = await _userService.RegisterUserAsync(userDto2);

        var result3 = await _userService.GetAllUsersAsync();
        
        Assert.Equal(2, result3.Count);
        Assert.Equal(result3.First(), result);
        Assert.Equal(result3.Last(), result2);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnTrue_WhenCurrentPasswordIsCorrect()
    {
        // Arrange
        var userEntity = new User { Id = 1, Name = "Test User", Email = "test@example.com", Password = "OldPassword" };
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.ChangePasswordAsync(1, "OldPassword", "NewPassword");

        // Assert
        Assert.True(result);
        var updatedUser = await _context.Users.FindAsync(1);
        Assert.Equal("NewPassword", updatedUser!.Password);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnFalse_WhenCurrentPasswordIsIncorrect()
    {
        // Arrange
        var userEntity = new User { Id = 2, Name = "Test User 2", Email = "test2@example.com", Password = "OldPassword" };
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.ChangePasswordAsync(2, "WrongPassword", "NewPassword");

        // Assert
        Assert.False(result);
        var updatedUser = await _context.Users.FindAsync(2);
        Assert.Equal("OldPassword", updatedUser!.Password);
    }
    
    public void Dispose()
    {
        _connection.Dispose();
        _context.Dispose();
    }
}