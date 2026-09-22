using ExpenseTracker.Application.Common.Helpers;
using ExpenseTracker.Application.Common.Response;
using ExpenseTracker.Application.DTOs.Authentication.Requests;
using ExpenseTracker.Application.DTOs.Authentication.Responses;
using ExpenseTracker.Application.Interfaces.Services.Authentication;
using ExpenseTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtProvider _jwtProvider;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return ResponseHelper.Unauthorized<AuthResponse>("Invalid email or password.");

        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return ResponseHelper.Unauthorized<AuthResponse>("Invalid email or password.");
        }
        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtProvider.GenerateToken(user.Id, user.UserName!, user.Email!, roles);

        var response = new AuthResponse
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Token = token
        };

        return ResponseHelper.Success(
            response,
            "Login successful.");
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return ResponseHelper.Failure<AuthResponse>(
                "Email is already registered.");
        }
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return ResponseHelper.ValidationFailure<AuthResponse>(
                result.Errors
                      .Select(x => x.Description)
                      .ToList());
        }
        var response = new AuthResponse
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!
        };
        return ResponseHelper.Created(
    response,
    "Account created successfully.");
    }

  
}