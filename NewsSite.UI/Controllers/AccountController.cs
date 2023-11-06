using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Core.DataTransferObjects.IdentityObjects;
using NewsSite.Core.Domain.Models.IdentityModels;
using NewsSite.Core.Enums.Identity;

namespace NewsSite.UI.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Policy = "NotAuthorized")]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "NotAuthorized")]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterObject registerObject)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = registerObject.FirstName,
                    LastName = registerObject.LastName,
                    UserName = registerObject.Username,
                    Email = registerObject.Email,
                    PhoneNumber = registerObject.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, registerObject.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (await _roleManager.FindByNameAsync(UserRoles.Reader.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Reader.ToString() });
                    }

                    await _userManager.AddToRoleAsync(user, UserRoles.Reader.ToString());

                    return RedirectToAction("Index", "Articles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegisterError", error.Description);
                }
            }

            return View(registerObject);
        }

        [HttpGet]
        [Authorize(Policy = "NotAuthorized")]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "NotAuthorized")]
        [Route("login")]
        public async Task<IActionResult> Login(LoginObject loginObject)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginObject.Username, loginObject.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Articles");
                }

                ModelState.AddModelError("LoginError", "Invalid login attempt");
            }

            return View(loginObject);
        }

        [HttpGet]
        [Route("new-author")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> BecomeAuthor()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Articles");
            }

            if (await _roleManager.FindByNameAsync(UserRoles.Author.ToString()) is null)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Author.ToString() });
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Author.ToString());

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "Articles");
        }

        [HttpGet]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Articles");
        }

        [AllowAnonymous]
        [Route("is-username-free")]
        public IActionResult IsUsernameFree(string username)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [AllowAnonymous]
        [Route("is-email-free")]
        public IActionResult IsEmailFree(string email)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [AllowAnonymous]
        [Route("is-phone-number-free")]
        public IActionResult IsPhoneNumberFree(string phoneNumber)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
