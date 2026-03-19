using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var authResponse = await _authService.RegisterAsync(registerDto);
            return authResponse.IsSuccess
                    ? Ok(authResponse)
                    : Problem(detail: authResponse.Detail, statusCode: authResponse.StatusCode);

        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: 400);
        }
    }

    [HttpPost("login-request")]
    public async Task<IActionResult> loginRequest([FromBody] LoginRequestDto loginDto)
    {
        try
        {
            var authResponse = await _authService.loginRequestAsync(loginDto);
            return authResponse.IsSuccess
                    ? Ok(authResponse)
                    : Problem(detail: authResponse.Detail, statusCode: authResponse.StatusCode);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: 400);
        }
    }

    [HttpPost("email-otp-login")]
    public async Task<IActionResult> EmailOtpLogin([FromBody] EmailOtpLoginDto dto)
    {
        try
        {
            var authResponse = await _authService.EmailOtpLoginAsync(dto);
            return authResponse.IsSuccess
                    ? Ok(authResponse)
                    : Problem(detail: authResponse.Detail, statusCode: authResponse.StatusCode);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: 400);
        }
    }

    // Email OTP For Registration 
    [HttpPost("send-email-otp")]
    public async Task<IActionResult> SendEmailOtp([FromBody] SendEmailOtpDto dto)
    {
        try
        {
            var result = await _authService.SendEmailOtpAsync(dto, OtpType.RegisterEmail);
            return result.IsSuccess
                    ? Ok(result)
                    : Problem(detail: result.Detail, statusCode: result.StatusCode);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    // Phone OTP
    // TODO: re-enable phone OTP endpoints once phone verification is implemented in the registration flow or as a separate feature.
    // [HttpPost("send-phone-otp")]
    // public async Task<IActionResult> SendPhoneOtp([FromBody] SendPhoneOtpDto dto)
    // {
    //     try
    //     {
    //         await _authService.SendPhoneOtpAsync(dto, OtpType.RegisterPhone // or OtpType.ResetPasswordPhone, depending on the use case);
    //         return Ok(new { message = "OTP code sent to phone." });
    //     }
    //     catch (InvalidOperationException ex)
    //     {
    //         return BadRequest(new { error = ex.Message });
    //     }
    // }

}
