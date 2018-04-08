using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Areas.Shared.Models.ViewModels;

namespace MjondalenInstallasjon.Web.Areas.Web.ViewComponents
{
    public class NavigationViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Areas/Web/Views/Shared/_Navigation.cshtml", new NavigationView());
        }
    }
}