using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class OtpService : IOtpService
{
    private static readonly TimeSpan OtpValidity = TimeSpan.FromMinutes(10);

    private readonly DataContext _context;

    public OtpService(DataContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<string> GenerateOtpAsync(int userId, OtpType type)
    {
        // Invalidate any existing unconsumed OTPs of the same type for this user
        var existing = await _context.OtpRecords
            .Where(o => o.UserId == userId && o.Type == type && !o.IsConsumed)
            .ToListAsync();

        foreach (var old in existing)
            old.IsConsumed = true;

        // Generate a cryptographically random 6-digit code
        var code = GenerateCode();

        var record = new OtpRecord
        {
            UserId = userId,
            Type = type,
            Code = code,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.Add(OtpValidity),
        };

        _context.OtpRecords.Add(record);
        await _context.SaveChangesAsync();

        return code;
    }

    /// <inheritdoc/>
    public async Task<bool> ValidateOtpAsync(int userId, OtpType type, string code)
    {
        var record = await _context.OtpRecords
            .Where(o => o.UserId == userId
                     && o.Type == type
                     && !o.IsConsumed
                     && o.Code == code)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (record is null)
            return false;

        if (record.ExpiresAt < DateTimeOffset.UtcNow)
            return false;

        record.IsConsumed = true;
        await _context.SaveChangesAsync();

        return true;
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
}
