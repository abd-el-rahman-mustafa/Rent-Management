namespace API.Domain.Entities;

public class OtpRecord
{
    public int Id { get; set; }

    /// <summary>The user this OTP belongs to.</summary>
    public required string Identifier { get; set; }

    /// <summary>The 6-digit OTP code.</summary>
    public required string Code { get; set; }

    /// <summary>Indicates whether this OTP is for email or phone verification.</summary>
    public required OtpType Type { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>OTP expires after a short window (e.g. 10 minutes).</summary>
    public required DateTimeOffset ExpiresAt { get; set; }

    /// <summary>True once the user has successfully verified with this code.</summary>
    public bool IsConsumed { get; set; } = false;
}

public enum OtpType
{
    RegisterEmail,
    RegisterPhone,
    ResetPasswordEmail,
    ResetPasswordPhone,
    LoginEmail,
    LoginPhone,

    // add more types as needed (e.g. 2FA, etc.)
}
