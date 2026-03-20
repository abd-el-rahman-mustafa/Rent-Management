namespace API.Application.Services;

using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly JwtSettings _tokenSettings;

    public JwtService(IOptions<JwtSettings> tokenSettings)
    {
        _tokenSettings = tokenSettings.Value;
    }
    public async Task<ServiceResult<TokenResponseDto>> GenerateTokenAsync(AppUser user)
    {
        try
        {


            // Steps 
            // 1. Create claims based on user information
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            };
            // Add role claims if the user has any roles
            var userRoles = user.UserRoles.Select(ur => ur.Role.Name).ToArray();

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // 2. Create signing credentials using the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 3. Create the JWT token with claims, signing credentials, and expiration

            var expires = DateTimeOffset.UtcNow.AddMinutes(_tokenSettings.ExpiryMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires.UtcDateTime,
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new TokenResponseDto
            {
                AccessToken = tokenString,
                AccessTokenExpires = expires
            };
            // 4. Return the token and its expiration time

            return ServiceResult<TokenResponseDto>.Success(
                response,
                "Token Generated",
                "JWT token generated successfully."
            );
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return ServiceResult<TokenResponseDto>.Failure(
                "Token Generation Failed",
                $"An error occurred while generating the token: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }

    }
}
