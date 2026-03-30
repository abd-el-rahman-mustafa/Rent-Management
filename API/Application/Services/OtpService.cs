using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class OtpService : IOtpService
{
    private static readonly TimeSpan OtpValidity = TimeSpan.FromSeconds(60);
    private static readonly TimeSpan SignupOtpValidity = TimeSpan.FromMinutes(3);

    private static readonly TimeSpan OtpTimeoutPeriod = TimeSpan.FromSeconds(60);



    private readonly DataContext _context;

    public OtpService(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<string>> GenerateOtpAsync(string identifier, OtpType type)
    {
        // Invalidate any existing unconsumed OTPs of the same type for this user
        var existing = await _context.OtpRecords
            .Where(o => o.Identifier == identifier && o.Type == type && !o.IsConsumed)
            .ToListAsync();

        foreach (var old in existing)
            old.IsConsumed = true;

        // check if the user can request a new OTP (e.g. not too soon after the last one)
        if (!await CanRequestOtpAsync(identifier, type))
            return ServiceResult<string>.Failure("Too Many Requests", "Please wait before requesting a new OTP.", StatusCodes.Status429TooManyRequests);


        // Generate a cryptographically random 6-digit code
        var code = GenerateCode();

        var record = new OtpRecord
        {
            Identifier = identifier,
            Type = type,
            Code = code,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.Add(type == OtpType.RegisterEmail ? SignupOtpValidity : OtpValidity),
        };

        _context.OtpRecords.Add(record);
        await _context.SaveChangesAsync();

        return ServiceResult<string>.Success(code, "OTP Generated", "A new OTP code has been generated and is valid for a limited time.");
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<bool>> ValidateOtpAsync(string identifier, OtpType type, string code)
    {
        var record = await _context.OtpRecords
            .Where(o => o.Identifier == identifier
                     && o.Type == type
                     && !o.IsConsumed
                     && o.Code == code)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (record is null)
            return ServiceResult<bool>.Failure("Invalid OTP", "The provided OTP code is invalid or has expired.", StatusCodes.Status400BadRequest);

        if (record.ExpiresAt < DateTimeOffset.UtcNow)
            return ServiceResult<bool>.Failure("Expired OTP", "The provided OTP code has expired.", StatusCodes.Status400BadRequest);

        record.IsConsumed = true;
        await _context.SaveChangesAsync();

        return ServiceResult<bool>.Success(true, "OTP Valid", "The provided OTP code is valid.");
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static string GenerateCode()
    {
        // Random.Shared is thread-safe and cryptographically adequate for OTPs
        int code = Random.Shared.Next(100_000, 999_999);
        return code.ToString();
    }

    // check if the user can request a new OTP (e.g. not too soon after the last one)
    public async Task<bool> CanRequestOtpAsync(string identifier, OtpType type)
    {
        var lastOtp = await _context.OtpRecords
            .Where(o => o.Identifier == identifier && o.Type == type)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastOtp is null)
            return true;

        return lastOtp.CreatedAt.Add(OtpTimeoutPeriod) <= DateTimeOffset.UtcNow;
    }
}
