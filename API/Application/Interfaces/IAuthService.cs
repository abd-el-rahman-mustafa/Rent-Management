using API.Application.Common;
using API.Application.DTOs;
using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided detail.
    /// Returns the created user's info on success, or throws on failure.
    /// </summary>
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Authenticates a user with email and password.
    /// Send otp to email if credentials are valid.
    /// </summary>
    Task<ServiceResult<string>> loginRequestAsync(LoginRequestDto loginDto);

        /// <summary>
        /// Authenticates a user with email and otp.
        /// Returns a JWT token if successful, or throws on failure.
    Task<ServiceResult<TokenResponseDto>> EmailOtpLoginAsync(EmailOtpLoginDto loginDto);

    /// <summary>
    /// Generates an OTP code and sends it to the user's email address.
    /// TODO: implement actual email delivery.
    /// </summary>
    Task<ServiceResult<bool>> SendEmailOtpAsync(SendEmailOtpDto dto, OtpType otpType);

    /// <summary>
    /// Validates the OTP code the user received via email.
    /// Marks EmailConfirmed = true on the Identity user if valid.
    /// </summary>
    Task<ServiceResult<bool>> ConfirmEmailOtpAsync(VerifyEmailOtpDto dto, OtpType otpType);

    /// <summary>
    /// Generates an OTP code and sends it to the user's phone number via SMS.
    /// TODO: implement actual SMS delivery.
    /// </summary>
    Task<ServiceResult<bool>> SendPhoneOtpAsync(SendPhoneOtpDto dto, OtpType otpType);

    /// <summary>
    /// Validates the OTP code the user received via SMS.
    /// Marks PhoneNumberConfirmed = true on the Identity user if valid.
    /// </summary>
    Task<ServiceResult<bool>> ConfirmPhoneOtpAsync(VerifyPhoneOtpDto dto, OtpType otpType);
}
