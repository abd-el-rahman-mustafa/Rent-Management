using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided details.
    /// Returns the created user's info on success, or throws on failure.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Generates an OTP code and sends it to the user's email address.
    /// TODO: implement actual email delivery.
    /// </summary>
    Task SendEmailOtpAsync(SendEmailOtpDto dto);

    /// <summary>
    /// Validates the OTP code the user received via email.
    /// Marks EmailConfirmed = true on the Identity user if valid.
    /// </summary>
    Task ConfirmEmailOtpAsync(VerifyEmailOtpDto dto);

    /// <summary>
    /// Generates an OTP code and sends it to the user's phone number via SMS.
    /// TODO: implement actual SMS delivery.
    /// </summary>
    Task SendPhoneOtpAsync(SendPhoneOtpDto dto);

    /// <summary>
    /// Validates the OTP code the user received via SMS.
    /// Marks PhoneNumberConfirmed = true on the Identity user if valid.
    /// </summary>
    Task ConfirmPhoneOtpAsync(VerifyPhoneOtpDto dto);
}
