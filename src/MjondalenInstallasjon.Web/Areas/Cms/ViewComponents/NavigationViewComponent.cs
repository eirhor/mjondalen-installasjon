﻿using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Web.Areas.Shared.Models.ViewModels;

namespace MjondalenInstallasjon.Web.Areas.Cms.ViewComponents
{
    public class NavigationViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Areas/Cms/Views/Shared/_Navigation.cshtml", new NavigationView());
        }
    }
}