using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MjondalenInstallasjon.Identity.Models;
using MjondalenInstallasjon.Identity.Services;

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
                return RedirectToAction("CreateInitialUser", new { returnUrl });
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
                    return Redirect(returnUrl);
                }

                if (result.Result.IsLockedOut)
                {
                    return RedirectToAction("AccessDenied", new { errorMessage = "Brukerkontoen er låst." });
                }

                if (result.Result.IsNotAllowed)
                {
                    return RedirectToAction("AccessDenied", new { errorMessage = "Brukerkontoen har ikke tilgang." });
                }

                if (result.Result.RequiresTwoFactor)
                {
                    return RedirectToAction("AccessDenied", new { errorMessage = "Konfigurasjonsfeil: Tofaktorautentisering." });
                }
                
                ModelState.AddModelError(string.Empty, "Ugyldig innlogging");
            }
            
            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreateInitialUser(string returnUrl = null)
        {
            var users = _identityService.GetUsersInRole(Constants.Roles.Administrator);
            if (users.Result.Any())
            {
                return RedirectToActionPermanent("Index", new { returnUrl });
            }
            
            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateInitialUser(RegisterViewModel model, string returnUrl = null)
        {
            
            
            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied(string errorMessage = null)
        {
            return View();
        }
    }
}