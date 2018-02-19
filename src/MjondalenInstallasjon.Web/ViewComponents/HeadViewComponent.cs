using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Models.ViewModels;

namespace MjondalenInstallasjon.Web.ViewComponents
{
    public class HeadViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/_Head.cshtml", new HeadView());
        }
    }
}