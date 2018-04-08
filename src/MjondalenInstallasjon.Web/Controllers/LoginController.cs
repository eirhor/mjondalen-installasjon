using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Identity.Models;
using MjondalenInstallasjon.Identity.Services;
using MjondalenInstallasjon.Web.Extensions;

namespace MjondalenInstallasjon.Web.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly IIdentityService _identityService;
        
        public LoginController(IIdentityService identityService)
        {
            _identityService = identityService;

            if (!identityService.RoleExists(Constants.Roles.Administrator).Result)
            {
                identityService.CreateRole(Constants.Roles.Administrator);
            }
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            var users = _identityService.GetUsersInRole(Constants.Roles.Administrator);
            if (!users.Result.Any())
            {
                return RedirectToAction(nameof(UsersController.CreateInitialUser), "Users", new { returnUrl });
            }

            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(SignInViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = _identityService.SignInUser(model);
                if (result.Result.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction(nameof(AdminController.Index), "Admin");
                    }

                    return Redirect(returnUrl);
                }

                if (result.Result.IsLockedOut)
                {
                    return RedirectToAction(nameof(AccessDenied), new { errorMessage = "Brukerkontoen er låst." });
                }

                if (result.Result.IsNotAllowed)
                {
                    return RedirectToAction(nameof(AccessDenied), new { errorMessage = "Brukerkontoen har ikke tilgang." });
                }

                if (result.Result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(AccessDenied), new { errorMessage = "Konfigurasjonsfeil: Tofaktorautentisering." });
                }
                
                ModelState.AddModelError(string.Empty, "Ugyldig innlogging");
            }
            
            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _identityService.SignOutUser();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied(string errorMessage = null)
        {
            return View("AccessDenied", errorMessage);
        }
    }
}