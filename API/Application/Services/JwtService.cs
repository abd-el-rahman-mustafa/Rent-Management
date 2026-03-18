namespace API.Application.Services;

using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using Microsoft.Extensions.Options;

public class JwtService : IJwtService
{
    private readonly JwtSettings _tokenSettings;

    private readonly int AccessTokenExpirationMinutes = 60; //Access Token valid for 60 minutes
    public JwtService(IOptions<JwtSettings> tokenSettings)
    {
        _tokenSettings = tokenSettings.Value;
    }
    public async Task<ServiceResult<TokenDto>> GenerateTokenAsync(AppUser user)
    {
        
        
    }
}
