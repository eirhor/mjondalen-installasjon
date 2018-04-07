using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Models.ViewModels;

namespace MjondalenInstallasjon.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("Index", new HomeView());
        }

        [Authorize]
        public IActionResult Test()
        {
            return View("Index", new HomeView());
        }
    }
}