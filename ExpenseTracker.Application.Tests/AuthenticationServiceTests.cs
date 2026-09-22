using ExpenseTracker.Application.Common.Constants;
using ExpenseTracker.Application.DTOs.Authentication.Requests;
using ExpenseTracker.Application.Interfaces.Services.Authentication;
using ExpenseTracker.Infrastructure.Authentication;
using ExpenseTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ExpenseTracker.Application.Tests.Authentication;

public class AuthenticationServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;

    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        _jwtProviderMock = new Mock<IJwtProvider>();

        _authenticationService = new AuthenticationService(
            _userManagerMock.Object,
            _jwtProviderMock.Object);
    }
    //Login Tests
    private LoginRequest CreateLoginRequest()
    {
        return new LoginRequest
        {
            Email = "test@test.com",
            Password = "Password123!"
        };
    }
    private ApplicationUser CreateUser()
    {
        return new ApplicationUser
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@test.com"
        };
    }
    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsSuccess()
    {
        // Arrange

        var request = CreateLoginRequest();

        var user = CreateUser();
        var roles = new List<string> { "User" };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(true);

        _userManagerMock
       .Setup(x => x.GetRolesAsync(user))
       .ReturnsAsync(roles);

        _jwtProviderMock
            .Setup(x => x.GenerateToken(
                user.Id,
                user.UserName!,
                user.Email!,
                It.IsAny<IEnumerable<string>>()))
            .Returns("fake-jwt-token");


        // Act

        var result = await _authenticationService.LoginAsync(request);


        // Assert

        Assert.NotNull(result);
        Assert.Equal(ApiStatusCodes.Success, result.StatusCode);
        Assert.Equal("Login successful.", result.Message);

        Assert.NotNull(result.Data);
        Assert.Equal(user.Id, result.Data.UserId);
        Assert.Equal(user.UserName, result.Data.UserName);
        Assert.Equal(user.Email, result.Data.Email);
        Assert.Equal("fake-jwt-token", result.Data.Token);
       
        _jwtProviderMock.Verify(
    x => x.GenerateToken(
        user.Id,
        user.UserName!,
        user.Email!,
        It.Is<IEnumerable<string>>(roles =>
            roles.SequenceEqual(new[] { "User" }))),
    Times.Once);
    }
    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ReturnsUnauthorized()
    {
        // Arrange

        var request = CreateLoginRequest();
        request.Email = "notfound@test.com";

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser?)null);


        // Act

        var result = await _authenticationService.LoginAsync(request);


        // Assert

        Assert.NotNull(result);
        Assert.Equal(ApiStatusCodes.Unauthorized, result.StatusCode);
        Assert.Equal("Invalid email or password.", result.Message);
        Assert.Null(result.Data);
    }
    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ReturnsUnauthorized()
    {
        // Arrange

        var request = new LoginRequest
        {
            Email = "test@test.com",
            Password = "WrongPassword!"
        };

        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@test.com"
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(false);


        // Act

        var result = await _authenticationService.LoginAsync(request);


        // Assert

        Assert.NotNull(result);
        Assert.Equal(ApiStatusCodes.Unauthorized, result.StatusCode);
        Assert.Equal("Invalid email or password.", result.Message);
        Assert.Null(result.Data);
    }

    private RegisterRequest CreateRegisterRequest()
    {
        return new RegisterRequest
        {
            FirstName = "Afnan",
            LastName = "Yasser",
            UserName = "afnan",
            Email = "afnan@test.com",
            Password = "Password123!"
        };
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailIsNotRegistered_ReturnsCreated()
    {
        // Arrange

        var request = CreateRegisterRequest();

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser?)null);

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(),
                request.Password))
            .ReturnsAsync(IdentityResult.Success);


        // Act

        var result = await _authenticationService.RegisterAsync(request);


        // Assert

        Assert.NotNull(result);
        Assert.Equal(ApiStatusCodes.Created, result.StatusCode);
        Assert.Equal("Account created successfully.", result.Message);

        Assert.NotNull(result.Data);
        Assert.Equal(request.UserName, result.Data.UserName);
        Assert.Equal(request.Email, result.Data.Email);

        _userManagerMock.Verify(
    x => x.CreateAsync(
        It.IsAny<ApplicationUser>(),
        request.Password),
    Times.Once);
    }
    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ReturnsFailure()
    {
        // Arrange

        var request = CreateRegisterRequest();

        var existingUser = new ApplicationUser
        {
            Id = 1,
            UserName = "existinguser",
            Email = request.Email
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);


        // Act

        var result = await _authenticationService.RegisterAsync(request);


        // Assert

        Assert.NotNull(result);
        Assert.Equal(ApiStatusCodes.BadRequest, result.StatusCode);
        Assert.Equal("Email is already registered.", result.Message);
        Assert.Null(result.Data);

        _userManagerMock.Verify(
    x => x.CreateAsync(
        It.IsAny<ApplicationUser>(),
        request.Password),
    Times.Never);
    }
    [Fact]
    public async Task RegisterAsync_WhenCreateUserFails_ReturnsValidationFailure()
    {
        // Arrange

        var request = CreateRegisterRequest();

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser?)null);

        var identityErrors = new[]
        {
        new IdentityError
        {
            Code = "PasswordTooShort",
            Description = "Password is too short."
        }
    };

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(),
                request.Password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));


        // Act

        var result = await _authenticationService.RegisterAsync(request);


        // Assert

        Assert.NotNull(result);
        Assert.Equal(ApiStatusCodes.ValidationError, result.StatusCode);
        Assert.Equal("Validation failed.", result.Message);

        Assert.NotNull(result.Errors);
        Assert.Contains("Password is too short.", result.Errors);

        Assert.Null(result.Data);
    }

}