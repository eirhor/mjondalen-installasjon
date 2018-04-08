using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                return RedirectToAction(nameof(LoginController.CreateInitialUser), new { returnUrl });
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
                    return RedirectToAction(nameof(LoginController.AccessDenied), new { errorMessage = "Brukerkontoen er låst." });
                }

                if (result.Result.IsNotAllowed)
                {
                    return RedirectToAction(nameof(LoginController.AccessDenied), new { errorMessage = "Brukerkontoen har ikke tilgang." });
                }

                if (result.Result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginController.AccessDenied), new { errorMessage = "Konfigurasjonsfeil: Tofaktorautentisering." });
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
        public IActionResult CreateInitialUser(string returnUrl = null)
        {
            var users = _identityService.GetUsersInRole(Constants.Roles.Administrator);
            if (users.Result.Any())
            {
                return RedirectToActionPermanent(nameof(LoginController.Index), new { returnUrl });
            }
            
            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateInitialUser(RegisterViewModel model, string returnUrl = null)
        {
            var users = _identityService.GetUsersInRole(Constants.Roles.Administrator);
            if (users.Result.Any())
            {
                return RedirectToActionPermanent(nameof(LoginController.Index), new { returnUrl });
            }
            
            if (ModelState.IsValid)
            {
                var result = _identityService.CreateUser(model);

                if (result.Result.Result.Succeeded)
                {
                    var roleResult = _identityService.AddUserToRole(result.Result.CreatedUser, Constants.Roles.Administrator);

                    if (roleResult.Result.Succeeded)
                    {
                        var signInResult = _identityService.SignInUser(new SignInViewModel
                        {
                            Email = model.Email,
                            Password = model.Password
                        });

                        if (signInResult.Result.Succeeded)
                        {
                            return Redirect(returnUrl);
                        }
                        
                        ModelState.AddModelError(string.Empty, "Klarte ikke å logge inn brukeren etter opprettelse.");
                        return RedirectToAction(nameof(LoginController.Index), new {returnUrl});
                    }
                    
                    ModelState.AddErrors(roleResult.Result);
                }
                else
                {
                    ModelState.AddErrors(result.Result.Result);
                }
            }
            
            ViewData[Constants.ViewData.ReturnUrl] = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied(string errorMessage = null)
        {
            return View("AccessDenied", errorMessage);
        }
    }
}