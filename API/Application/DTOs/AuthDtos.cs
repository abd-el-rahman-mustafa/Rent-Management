using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs;

public class RegisterDto
{
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Email OTP must be exactly 6 digits.")]
    public required string EmailOtpCode { get; set; }

    // [Required]
    // [Phone]
    // public required string Phone { get; set; }

    // [Required]
    // [StringLength(6, MinimumLength = 6, ErrorMessage = "Phone OTP must be exactly 6 digits.")]
    // public required string PhoneOtpCode { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public required string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Confirm password does not match.")]
    public required string ConfirmPassword { get; set; }
}

public class AuthResponseDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
}

/// <summary>Used to request sending an OTP to the user's email.</summary>
public class SendEmailOtpDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}

/// <summary>Used to request sending an OTP to the user's phone.</summary>
public class SendPhoneOtpDto
{
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
}

/// <summary>Used to submit the OTP code received via email for confirmation.</summary>
public class VerifyEmailOtpDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits.")]
    public required string OtpCode { get; set; }
}

/// <summary>Used to submit the OTP code received via SMS for confirmation.</summary>
public class VerifyPhoneOtpDto
{
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits.")]
    public required string OtpCode { get; set; }
}
