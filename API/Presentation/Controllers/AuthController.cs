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
            return Ok(authResponse);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // Email OTP
    [HttpPost("send-email-otp")]
    public async Task<IActionResult> SendEmailOtp([FromBody] SendEmailOtpDto dto)
    {
        try
        {
            await _authService.SendEmailOtpAsync(dto, OtpType.RegisterEmail);
            return Ok(new { message = "OTP code sent to email." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // Phone OTP
    [HttpPost("send-phone-otp")]
    public async Task<IActionResult> SendPhoneOtp([FromBody] SendPhoneOtpDto dto)
    {
        try
        {
            await _authService.SendPhoneOtpAsync(dto, OtpType.RegisterPhone);
            return Ok(new { message = "OTP code sent to phone." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}
