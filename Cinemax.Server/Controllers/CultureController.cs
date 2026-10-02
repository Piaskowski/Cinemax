using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Cinemax.Server.Controllers
{

    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        [HttpGet]
        public IActionResult Set(string culture, string redirectUri)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                });

            return LocalRedirect(redirectUri);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Content(CultureInfo.CurrentUICulture.Name);
        }
    }
}
