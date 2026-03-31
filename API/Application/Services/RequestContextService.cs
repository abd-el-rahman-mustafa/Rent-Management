using API.Application.Interfaces;

namespace API.Application.Services;


public class RequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Language
    {
        get
        {
            var lang = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();
            return !string.IsNullOrEmpty(lang) && lang.StartsWith("en") ? "en" : "ar";
        }
    }

    public string Platform
    {
        get
        {
            var platform = _httpContextAccessor.HttpContext?.Request.Headers["X-Platform"].ToString();
            return string.IsNullOrEmpty(platform) ? "Not Specified" : platform;
        }
    }
}