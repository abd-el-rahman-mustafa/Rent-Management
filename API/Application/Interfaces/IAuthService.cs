using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided details.
    /// Returns the created user's info on success, or throws on failure.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
}
