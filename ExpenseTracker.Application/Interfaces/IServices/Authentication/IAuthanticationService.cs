using ExpenseTracker.Application.Common.Response;
using ExpenseTracker.Application.DTOs.Authentication.Requests;
using ExpenseTracker.Application.DTOs.Authentication.Responses;

public interface IAuthenticationService
{
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);

    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
}