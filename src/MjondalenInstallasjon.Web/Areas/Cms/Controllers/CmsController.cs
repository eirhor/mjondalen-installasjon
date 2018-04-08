using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MjondalenInstallasjon.Web.Areas.Cms.Controllers
{
    [Authorize]
    [Area("Cms")]
    [Route("cms")]
    public class CmsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}