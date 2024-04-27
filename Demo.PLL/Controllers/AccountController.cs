using Demo.DAL.Models;
using Demo.PLL.Helpers;
using Demo.PLL.Services.EmailSender;
using Demo.PLL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Demo.PLL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailsender;
        private readonly IConfiguration _configration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signIn;

        public AccountController(IEmailSender emailsender,IConfiguration configration,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInManager)
        {
           _emailsender = emailsender;
            _configration = configration;
            _userManager = userManager;
            _signIn = SignInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                    FName = model.Fname,
                    LName = model.Lname
                };
                var res = await _userManager.CreateAsync(User, model.Password);
                if (res.Succeeded)
                    return RedirectToAction(nameof(Login));
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {
                        var loginresult = await _signIn.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (loginresult.Succeeded)
                            return RedirectToAction("Index", "Home");

                    }
                    else
                        ModelState.AddModelError(string.Empty, "Password is not Correct");

                }
                else

                    ModelState.AddModelError(string.Empty, "Email is not Existed");

            }
            return View();
        }
        public new async Task< IActionResult> SignOut()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, Request.Scheme );
                    await _emailsender.SendAsync(
                        from: _configration["EmailSetting:SenderEmail"],
                        recipients: model.Email,
                            subject: "Reset Your Password",
                            body: resetPasswordUrl);
                    return RedirectToAction(nameof(CheckYourEmail));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There is No Account with this Email!");
                }
            }
            

                return View("ForgetPassword", model);
            
		}
		public IActionResult CheckYourEmail()
		{
			return View();
		}
        [HttpGet ]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var Token = TempData["Token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    await _userManager.ResetPasswordAsync(user, Token, model.NewPassword);
                    return RedirectToAction(nameof(Login));
                }
                ModelState.AddModelError(string.Empty, "url is not valid");
            }
            return View(model);
        }
    }
}
