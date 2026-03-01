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

        // verify email otp
        var emailOtpValid = await _otpService.ValidateOtpAsync(
            identifier: registerDto.Email,
            otpType: OtpType.RegisterEmail,
            otpCode: registerDto.EmailOtpCode
        );

        if (!emailOtpValid)
            throw new InvalidOperationException("Invalid or expired email OTP code.");
        // // verify phone otp
        // var phoneOtpValid = await _otpService.ValidateOtpAsync(
        //     identifier: registerDto.Phone,
        //     otpType: OtpType.RegisterPhone,
        //     otpCode: registerDto.PhoneOtpCode
        // );
        // if (!phoneOtpValid)
        //     throw new InvalidOperationException("Invalid or expired phone OTP code.");

        // verify that email is unique (not already taken by another user)
        if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            throw new InvalidOperationException("An account with that email address already exists.");

        // // verify that phone number is unique (not already taken by another user)
        // if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerDto.Phone))
        //     throw new InvalidOperationException("An account with that phone number already exists.");

        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            // PhoneNumber = registerDto.Phone,
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

    public async Task SendEmailOtpAsync(SendEmailOtpDto dto, OtpType otpType)
    {
        // if otp for registration, don't verify that email exists because user might be in the process of registering and hasn't received the OTP yet
        if (otpType != OtpType.RegisterEmail)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new InvalidOperationException("No account found with that email address.");
        }

        var code = await _otpService.GenerateOtpAsync(dto.Email, otpType);

        // TODO: send the OTP code via email (e.g. SendGrid, SMTP, etc.)
        // Example:
        //   await _emailSender.SendAsync(dto.Email, "Your verification code", $"Your OTP is: {code}");
        // _ = code; // suppress unused-variable warning until email sending is implemented
    }

    public async Task ConfirmEmailOtpAsync(VerifyEmailOtpDto dto, OtpType otpType)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new InvalidOperationException("No account found with that email address.");

        var valid = await _otpService.ValidateOtpAsync(dto.Email, otpType, dto.OtpCode);
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

    public async Task SendPhoneOtpAsync(SendPhoneOtpDto dto, OtpType otpType)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber)
                   ?? throw new InvalidOperationException("No account found with that phone number.");

        var code = await _otpService.GenerateOtpAsync(dto.PhoneNumber, otpType);

        // TODO: send the OTP code via SMS (e.g. Twilio, AWS SNS, etc.)
        // Example:
        //   await _smsSender.SendAsync(dto.PhoneNumber, $"Your verification code is: {code}");
        // _ = code; // suppress unused-variable warning until SMS sending is implemented
    }

    public async Task ConfirmPhoneOtpAsync(VerifyPhoneOtpDto dto, OtpType otpType)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber)
                   ?? throw new InvalidOperationException("No account found with that phone number.");

        var valid = await _otpService.ValidateOtpAsync(dto.PhoneNumber, otpType, dto.OtpCode);
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
