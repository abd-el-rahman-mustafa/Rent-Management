namespace API.Application.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public double AccessTokenExpirationMinutes { get; set; }
}

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
}