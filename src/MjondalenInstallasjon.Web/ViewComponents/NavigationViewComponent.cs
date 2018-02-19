using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Models.ViewModels;

namespace MjondalenInstallasjon.Web.ViewComponents
{
    public class NavigationViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/_Navigation.cshtml", new NavigationView());
        }
    }
}