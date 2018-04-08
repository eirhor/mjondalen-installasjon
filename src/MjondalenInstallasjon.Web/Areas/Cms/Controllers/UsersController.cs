using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MjondalenInstallasjon.Identity.Models;
using MjondalenInstallasjon.Identity.Services;
using MjondalenInstallasjon.Web.Areas.Shared.Extensions;

namespace MjondalenInstallasjon.Web.Areas.Cms.Controllers
{
    [Authorize]
    [Area("Cms")]
    public class UsersController : Controller
    {
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreateInitialUser(string returnUrl = null)
        {
            var users = _identityService.GetUsersInRole(Constants.Roles.Administrator);
            if (users.Result.Any())
            {
                return RedirectToActionPermanent(nameof(LoginController.Index), "Login", new { returnUrl });
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
                return RedirectToActionPermanent(nameof(LoginController.Index), "Login", new { returnUrl });
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
                            if (string.IsNullOrEmpty(returnUrl))
                            {
                                return RedirectToAction(nameof(CmsController.Index), "Cms");
                            }

                            return Redirect(returnUrl);
                        }
                       
                        return RedirectToAction(nameof(LoginController.Index), "Login", new {returnUrl});
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
    }
}