using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Areas.Shared.Models.ViewModels;

namespace MjondalenInstallasjon.Web.Areas.Cms.ViewComponents
{
    public class AdminHeadViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Areas/Cms/Views/Shared/_Head.cshtml", new HeadView());
        }
    }
}