
namespace API.Application.Interfaces;

using API.Application.Common;
using API.Application.DTOs;
using API.Domain.Entities;

public interface IJwtService
{
    /// <summary>
    /// Validates user credentials and generates a JWT token if valid.
    /// Returns the token on success, or an error result on failure.
    /// </summary>
    Task<ServiceResult<TokenResponseDto>> GenerateTokenAsync(AppUser user);
}