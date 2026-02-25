using API.Application.DTOs;
using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface IOtpService
{
    /// <summary>
    /// Generates a new 6-digit OTP code for the given user and type,
    /// persists it to the database, and returns it so the caller can dispatch it.
    /// </summary>
    Task<string> GenerateOtpAsync(int userId, OtpType type);

    /// <summary>
    /// Validates the supplied OTP code against the latest unconsumed record
    /// for this user + type. Marks the record consumed if it matches and has
    /// not expired.
    /// </summary>
    Task<bool> ValidateOtpAsync(int userId, OtpType type, string code);
}
