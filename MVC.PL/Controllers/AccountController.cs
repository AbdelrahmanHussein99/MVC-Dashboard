using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.DAL.Models;
using MVC.PL.Helper;
using MVC.PL.ViewModels;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByNameAsync(model.Email);
                    if (user is null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsAgree = model.IsAgree

                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(SignIn));

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }

                }
                ModelState.AddModelError(string.Empty, "User is Already Exits ");


            }
            return View(model);
        }
        #endregion



        #region SignIn
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                        {

                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login ");

            }
            return View(model);
        }
        #endregion

        #region Signout
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        public  IActionResult ForgetPassword()
        {

            return View();
        }
		public  async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var token =_userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token },Request.Scheme);

                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Recipents = model.Email,
                        Body = url,
                    };
                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }

				ModelState.AddModelError(string.Empty, "Invalid Email ");

			}

			return View(nameof(ForgetPassword),model);
		}
		public IActionResult CheckYourInbox()
		{

			return View();
		}


		public IActionResult ResetPassword(string email,string token)
		{
            TempData["email"]=email;
            TempData["token"]=token;
			return View();
		}

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
                    var result= await _userManager.ResetPasswordAsync(user,token,model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid reset password");

					}
                }
            }
			return View();
		}
        #endregion

        #region MyRegion

        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
