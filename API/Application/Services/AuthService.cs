using API.Application.DTOs;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Application.Interfaces;
using API.Application.Common;
using API.Infrastructure.Data;

namespace API.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly DataContext _dbContext;

    public AuthService(UserManager<AppUser> userManager, IOtpService otpService, IEmailService emailService, IJwtService jwtService, DataContext dbContext)
    {
        _userManager = userManager;
        _otpService = otpService;
        _jwtService = jwtService;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    // -------------------------------------------------------------------------
    // Registration
    // -------------------------------------------------------------------------
    public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {

        // verify email otp
        var validateResult = await _otpService.ValidateOtpAsync(
            identifier: registerDto.Email,
            otpType: OtpType.RegisterEmail,
            otpCode: registerDto.EmailOtpCode
        );

        if (!validateResult.IsSuccess)
            return ServiceResult<AuthResponseDto>.Failure(
                title: validateResult.Title,
                detail: validateResult.Detail,
                statusCode: validateResult.StatusCode
            );

        // verify that email is unique (not already taken by another user)
        if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            return ServiceResult<AuthResponseDto>.Failure(
                title: "Email Already In Use",
                detail: "An account with that email address already exists.",
                statusCode: StatusCodes.Status409Conflict
            );

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


        var createUserResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }

        var result = new AuthResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };

        return ServiceResult<AuthResponseDto>.Success(
            data: result,
            title: "Registration successful",
            detail: "Your account has been created successfully."
        );
    }

    // -------------------------------------------------------------------------
    // Login
    // -------------------------------------------------------------------------

    public async Task<ServiceResult<string>> loginRequestAsync(LoginRequestDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return ServiceResult<string>.Failure(
                title: "Invalid Credentials",
                detail: "No account found with that email address.",
                statusCode: StatusCodes.Status404NotFound
            );

        var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!passwordValid)
            return ServiceResult<string>.Failure(
                title: "Invalid Credentials",
                detail: "Incorrect password.",
                statusCode: StatusCodes.Status400BadRequest
            );

        // send OTP to email for 2FA
        var generateResult = await _otpService.GenerateOtpAsync(user.Email, OtpType.LoginEmail);

        if (!generateResult.IsSuccess)
            return ServiceResult<string>.Failure(
                title: generateResult.Title,
                detail: generateResult.Detail,
                statusCode: generateResult.StatusCode
            );

        await _emailService.SendAsync(user.Email, "Your login verification code", $"Your OTP is: {generateResult.Data}");
        return ServiceResult<string>.Success(
            data: "OTP Sent",
            title: "Success",
#if DEBUG
            detail: $"Login OTP Sent , Your OTP is: {generateResult.Data}"
#else
            detail: "Login OTP Sent to your email address."
#endif
        );
    }

    public async Task<ServiceResult<TokenResponseDto>> EmailOtpLoginAsync(EmailOtpLoginDto loginDto)
    {
        var user = await _dbContext.Users
                        .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
            return ServiceResult<TokenResponseDto>.Failure(
                title: "Invalid Credentials",
                detail: "No account found with that email address.",
                statusCode: StatusCodes.Status404NotFound
            );
        var validateResult = await _otpService.ValidateOtpAsync(loginDto.Email, OtpType.LoginEmail, loginDto.Otp);
        if (!validateResult.IsSuccess)
            return ServiceResult<TokenResponseDto>.Failure(
                title: validateResult.Title,
                detail: validateResult.Detail,
                statusCode: validateResult.StatusCode
            );
        // For now, we'll assume the OTP is valid and proceed with login

        // Generate a JWT token for the user
        var result = await _jwtService.GenerateTokenAsync(user);
        if (!result.IsSuccess)
            return ServiceResult<TokenResponseDto>.Failure(
                "Token Generation Failed",
                 result.Detail,
                result.StatusCode
            );

        return ServiceResult<TokenResponseDto>.Success(
            data: result.Data!,
            title: "Login successful",
            detail: "You have been logged in successfully."
        );
    }

    // -------------------------------------------------------------------------
    // Email OTP
    // -------------------------------------------------------------------------

    public async Task<ServiceResult<bool>> SendEmailOtpAsync(SendEmailOtpDto dto, OtpType otpType)
    {
        // if otp for registration, don't verify that email exists because user might be in the process of registering and hasn't received the OTP yet
        if (otpType != OtpType.RegisterEmail)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return ServiceResult<bool>.Failure(
                     title: "Email Not Found",
                     detail: "No account found with that email address.",
                     statusCode: StatusCodes.Status404NotFound
                 );

        }

        var generateResult = await _otpService.GenerateOtpAsync(dto.Email, otpType);

        if (!generateResult.IsSuccess)
            return ServiceResult<bool>.Failure(
                title: generateResult.Title,
                detail: generateResult.Detail,
                statusCode: generateResult.StatusCode
            );

        await _emailService.SendAsync(dto.Email, "Your verification code", $"Your OTP is: {generateResult.Data}");

        return ServiceResult<bool>.Success(
            data: true,
            title: "OTP Sent",
            detail: "OTP code has been sent to the provided email address."
        );
    }

    public async Task<ServiceResult<bool>> ConfirmEmailOtpAsync(VerifyEmailOtpDto dto, OtpType otpType)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return ServiceResult<bool>.Failure(
                title: "Email Not Found",
                detail: "No account found with that email address.",
                statusCode: StatusCodes.Status404NotFound
            );


        var validateResult = await _otpService.ValidateOtpAsync(dto.Email, otpType, dto.Otp);
        if (!validateResult.IsSuccess)
            return ServiceResult<bool>.Failure(
                title: validateResult.Title,
                detail: validateResult.Detail,
                statusCode: validateResult.StatusCode
            );

        // Mark the email as confirmed in ASP.NET Identity
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return ServiceResult<bool>.Failure(
                title: "Email Confirmation Failed",
                detail: $"Failed to confirm email: {errors}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return ServiceResult<bool>.Success(
            data: true,
            title: "Email Confirmed",
            detail: "Your email has been confirmed successfully."
        );
    }

    // -------------------------------------------------------------------------
    // Phone OTP
    // -------------------------------------------------------------------------

    public async Task<ServiceResult<bool>> SendPhoneOtpAsync(SendPhoneOtpDto dto, OtpType otpType)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

        if (user == null)
            return ServiceResult<bool>.Failure(
                 title: "Phone Number Not Found",
                 detail: "No account found with that phone number.",
                 statusCode: StatusCodes.Status404NotFound
             );


        var code = await _otpService.GenerateOtpAsync(dto.PhoneNumber, otpType);

        return ServiceResult<bool>.Success(
            data: true,
            title: "OTP Sent",
            detail: "OTP code has been sent to the provided phone number."
        );
    }

    public async Task<ServiceResult<bool>> ConfirmPhoneOtpAsync(VerifyPhoneOtpDto dto, OtpType otpType)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

        if (user == null)
            return ServiceResult<bool>.Failure(
                title: "Phone Number Not Found",
                detail: "No account found with that phone number.",
                statusCode: StatusCodes.Status404NotFound
            );


        var validateResult = await _otpService.ValidateOtpAsync(dto.PhoneNumber, otpType, dto.Otp);
        if (!validateResult.IsSuccess)
            return ServiceResult<bool>.Failure(
                title: validateResult.Title,
                detail: validateResult.Detail,
                statusCode: validateResult.StatusCode
            );

        // Mark the phone number as confirmed in ASP.NET Identity
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, dto.PhoneNumber);
        var result = await _userManager.ChangePhoneNumberAsync(user, dto.PhoneNumber, token);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ServiceResult<bool>.Failure(
                title: "Phone Number Confirmation Failed",
                detail: $"Failed to confirm phone number: {errors}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return ServiceResult<bool>.Success(
            data: true,
            title: "Phone Number Confirmed",
            detail: "Your phone number has been confirmed successfully."
        );
    }
}
