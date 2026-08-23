using ExpenseTracker.Application.DTOs.Authentication.Requests;
using ExpenseTracker.Application.DTOs.Authentication.Responses;
using ExpenseTracker.Application.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _authenticationService.RegisterAsync(request);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authenticationService.LoginAsync(request);

        return StatusCode(response.StatusCode, response);
    }
}