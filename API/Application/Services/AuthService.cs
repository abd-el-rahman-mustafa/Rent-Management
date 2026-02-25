using API.Application.DTOs;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Application.Interfaces;

namespace API.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IOtpService _otpService;

    public AuthService(UserManager<AppUser> userManager, IOtpService otpService)
    {
        _userManager = userManager;
        _otpService = otpService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            PhoneNumber = registerDto.Phone,
            Gender = Gender.NotSpecified,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }

        return new AuthResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Phone = user.PhoneNumber!,
        };
    }

    // -------------------------------------------------------------------------
    // Email OTP
    // -------------------------------------------------------------------------

    public async Task SendEmailOtpAsync(SendEmailOtpDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new InvalidOperationException("No account found with that email address.");

        var code = await _otpService.GenerateOtpAsync(user.Id, OtpType.Email);

        // TODO: send the OTP code via email (e.g. SendGrid, SMTP, etc.)
        // Example:
        //   await _emailSender.SendAsync(dto.Email, "Your verification code", $"Your OTP is: {code}");
        _ = code; // suppress unused-variable warning until email sending is implemented
    }

    public async Task ConfirmEmailOtpAsync(VerifyEmailOtpDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new InvalidOperationException("No account found with that email address.");

        var valid = await _otpService.ValidateOtpAsync(user.Id, OtpType.Email, dto.OtpCode);
        if (!valid)
            throw new InvalidOperationException("Invalid or expired OTP code.");

        // Mark the email as confirmed in ASP.NET Identity
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Email confirmation failed: {errors}");
        }
    }

    // -------------------------------------------------------------------------
    // Phone OTP
    // -------------------------------------------------------------------------

    public async Task SendPhoneOtpAsync(SendPhoneOtpDto dto)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber)
                   ?? throw new InvalidOperationException("No account found with that phone number.");

        var code = await _otpService.GenerateOtpAsync(user.Id, OtpType.Phone);

        // TODO: send the OTP code via SMS (e.g. Twilio, AWS SNS, etc.)
        // Example:
        //   await _smsSender.SendAsync(dto.PhoneNumber, $"Your verification code is: {code}");
        _ = code; // suppress unused-variable warning until SMS sending is implemented
    }

    public async Task ConfirmPhoneOtpAsync(VerifyPhoneOtpDto dto)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber)
                   ?? throw new InvalidOperationException("No account found with that phone number.");

        var valid = await _otpService.ValidateOtpAsync(user.Id, OtpType.Phone, dto.OtpCode);
        if (!valid)
            throw new InvalidOperationException("Invalid or expired OTP code.");

        // Mark the phone number as confirmed in ASP.NET Identity
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, dto.PhoneNumber);
        var result = await _userManager.ChangePhoneNumberAsync(user, dto.PhoneNumber, token);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Phone confirmation failed: {errors}");
        }
    }
}
