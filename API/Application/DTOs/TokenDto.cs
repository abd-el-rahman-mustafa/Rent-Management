namespace API.Application.DTOs;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpires { get; set; }
}
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
}