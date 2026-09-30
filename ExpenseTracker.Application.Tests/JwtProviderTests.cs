using ExpenseTracker.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ExpenseTracker.Application.Tests.Authentication;

public class JwtProviderTests
{
    private static JwtSettings CreateJwtSettings()
    {
        return new JwtSettings
        {
            Key = "this-is-a-secret-key-for-testing-jwt",
            Issuer = "ExpenseTracker",
            Audience = "ExpenseTrackerClient",
            DurationInMinutes = 60
        };
    }
    private static JwtProvider CreateJwtProvider(JwtSettings settings)
    {
        return new JwtProvider(Options.Create(settings));
    }

    [Fact]
    public void GenerateToken_ReturnsToken()
    {
        // Arrange
        var settings = CreateJwtSettings();
        var jwtProvider = CreateJwtProvider(settings);

       

        var roles = new List<string>
    {
        "User"
    };

        // Act
        var token = jwtProvider.GenerateToken(
            1,
            "testuser",
            "test@test.com",
            roles);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }


    [Fact]
    public void GenerateToken_ContainsExpectedClaims()
    {
        // Arrange
        var settings = CreateJwtSettings();
        var jwtProvider = CreateJwtProvider(settings);

        var userId = 1;
        var userName = "testuser";
        var email = "test@test.com";
        var roles = new List<string>
    {
        "User",
        "Admin"
    };

        // Act
        var token = jwtProvider.GenerateToken(
            userId,
            userName,
            email,
            roles);

        // Read the generated JWT
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.Equal(
            userId.ToString(),
            jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);

        Assert.Equal(
            userName,
            jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value);

        Assert.Equal(
            email,
            jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value);

        var tokenRoles = jwtToken.Claims
            .Where(x => x.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();

        Assert.Equal(roles, tokenRoles);
    }

    [Fact]
    public void GenerateToken_UsesExpectedJwtSettings()
    {
        // Arrange
        var settings = CreateJwtSettings();
        var jwtProvider = CreateJwtProvider(settings);

        var beforeGeneration = DateTime.UtcNow;

        // Act
        var token = jwtProvider.GenerateToken(
            1,
            "testuser",
            "test@test.com",
            new List<string> { "User" });

        var afterGeneration = DateTime.UtcNow;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        Assert.Equal(settings.Issuer, jwtToken.Issuer);
        Assert.Equal(settings.Audience, jwtToken.Audiences.Single());

        Assert.True(
            jwtToken.ValidTo >= beforeGeneration.AddMinutes(settings.DurationInMinutes).AddSeconds(-1));

        Assert.True(
            jwtToken.ValidTo <= afterGeneration.AddMinutes(settings.DurationInMinutes).AddSeconds(1));
    }

    [Fact]
    public void GenerateToken_WithCorrectKey_CanBeValidated()
    {
        // Arrange
        var settings = CreateJwtSettings();
        var jwtProvider = CreateJwtProvider(settings);

        // Act
        var token = jwtProvider.GenerateToken(
            1,
            "testuser",
            "test@test.com",
            new List<string> { "User" });

        var handler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,

            ValidateAudience = true,
            ValidAudience = settings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(settings.Key)),

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };

        // Assert
        var principal = handler.ValidateToken(
            token,
            validationParameters,
            out var validatedToken);

        Assert.NotNull(principal);
        Assert.NotNull(validatedToken);

        var jwtToken = Assert.IsType<JwtSecurityToken>(validatedToken);

        Assert.Equal(
            SecurityAlgorithms.HmacSha256,
            jwtToken.Header.Alg);
    }
}