using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Models.ViewModels;

namespace MjondalenInstallasjon.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("Index", new HomeView());
        }
    }
}