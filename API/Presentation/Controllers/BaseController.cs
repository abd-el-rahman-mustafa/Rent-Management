namespace API.Presentation.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {

        protected string Language
        {
            get
            {
                var lang = Request.Headers["Accept-Language"].ToString();
                return !string.IsNullOrEmpty(lang) && lang.StartsWith("en") ? "en" : "ar"; // Default to Arabic if not English
            }
        }
        protected string Platform
        {
            get
            {
                var platform = Request.Headers["X-Platform"].ToString();
                return string.IsNullOrEmpty(platform) ? "Not Specified" : platform;
            }
        }

    }
}