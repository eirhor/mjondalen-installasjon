using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Areas.Shared.Models.ViewModels;

namespace MjondalenInstallasjon.Web.Areas.Web.ViewComponents
{
    public class HeadViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Areas/Web/Views/Shared/_Head.cshtml", new HeadView());
        }
    }
}