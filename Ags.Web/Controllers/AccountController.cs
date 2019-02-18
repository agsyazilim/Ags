using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Data.Core.Http;
using Ags.Data.Domain;
using Ags.Data.Domain.Logging;
using Ags.Services.Logging;
using Ags.Services.Message;
using Ags.Web.Framework.Authorization;
using Ags.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Controllers
{
    [Authorize]
    public class AccountController : BasePublicController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHelper _webHelper;
        private ApplicationUser _applicationUser;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            RoleManager<IdentityRole> roleManager,
            ILogger logger, IHttpContextAccessor httpContextAccessor, IWebHelper webHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            this._roleManager = roleManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _webHelper = webHelper;
        }
        protected virtual string GetCustomerCookie()
        {
            var cookieName = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.CustomerCookie}";
            if (User.Identity.IsAuthenticated)
            {
                if (_httpContextAccessor.HttpContext?.Request?.Cookies[cookieName] !=null)
                {
                    return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
                }
            }
            else
            {
                if (_httpContextAccessor.HttpContext?.Request?.Cookies[cookieName] !=null)
                {
                     _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);
                     return null;

                }
            }
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }
        protected virtual void SetCustomerCookie(string userId)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
            {
                return;
            }

            //delete current cookie value
            var cookieName = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.CustomerCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (string.IsNullOrEmpty(userId))
            {
                cookieExpiresDate = DateTime.Now.AddMonths(-1);
            }

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, userId, options);
            _applicationUser = _userManager.FindByIdAsync(userId).Result;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            var userCookei = GetCustomerCookie();
            if (!string.IsNullOrEmpty(userCookei))
            {
                return RedirectToLocal(returnUrl);
            }

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                //    lockoutOnFailure: false).ConfigureAwait(true);
               var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false).ConfigureAwait(true);
                if (result.Succeeded)
                {
                    _logger.InsertLog(LogLevel.Information, "Giriş Yapıldı", model.Email);
                    var user = _userManager.FindByEmailAsync(model.Email).Result;
                    SetCustomerCookie(user.Id);
                    _applicationUser = user;
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.InsertLog(LogLevel.Warning, "Hesap Kilitli", model.Email);
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                    return View(model);
                }
            }

            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    //extended properties
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateRegistered = DateTime.UtcNow.ToString("d"),
                    AvatarURL = model.AvatarId.ToString(),
                    Position = "",
                    NickName = "",

                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //$"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    //await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);

                    _logger.InsertLog(LogLevel.Information, "Yeni Hesap Oluşturuldu", model.Email);
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Name, user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Surname, user.LastName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.PictureId, model.AvatarId.ToString()?? ""));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.ReadOperationName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CreateOperationName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.DeleteOperationName));
                    await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.UpdateOperationName));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, CustomerRole.Constants.CustomerMemberRole));
                    IdentityRole adminRole = await _roleManager.FindByNameAsync(CustomerRole.Constants.CustomerAdministratorsRole);
                    IdentityRole editorRole = await _roleManager.FindByNameAsync(CustomerRole.Constants.CustomerManagersRole);
                    IdentityRole memberRole = await _roleManager.FindByNameAsync(CustomerRole.Constants.CustomerMemberRole);
                    if (adminRole == null && editorRole == null && memberRole == null)
                    {
                        adminRole = new IdentityRole(CustomerRole.Constants.CustomerAdministratorsRole);
                        await _roleManager.CreateAsync(adminRole);
                        editorRole = new IdentityRole(CustomerRole.Constants.CustomerManagersRole);
                        await _roleManager.CreateAsync(editorRole);
                        memberRole = new IdentityRole(CustomerRole.Constants.CustomerMemberRole);
                        await _roleManager.CreateAsync(memberRole);

                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.ReadOperationName));
                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.DeleteOperationName));
                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.UpdateOperationName));
                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.ApproveOperationName));
                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.RejectOperationName));
                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CreateOperationName));
                        await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.AccessAdminPanel));
                        await _roleManager.AddClaimAsync(editorRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.AccessAdminPanel));
                        await _roleManager.AddClaimAsync(editorRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.ReadOperationName));
                        await _roleManager.AddClaimAsync(editorRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.UpdateOperationName));
                        await _roleManager.AddClaimAsync(editorRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.CreateOperationName));
                        await _roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, CustomerRole.Constants.PublicAccessSite));

                    }
                    if (!await _userManager.IsInRoleAsync(user, CustomerRole.Constants.CustomerMemberRole))
                    {
                        await _userManager.AddToRoleAsync(user, CustomerRole.Constants.CustomerMemberRole);

                    }

                    return RedirectToAction("Login");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LogOff()
        {
            _logger.InsertLog(LogLevel.Information, "Çıkış Yapıldı", GetCurrentUserAsync().Result.Email);
             await _signInManager.SignOutAsync();
            var cookieName = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.CustomerCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            _applicationUser = null;


            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            string redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            Microsoft.AspNetCore.Authentication.AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.InsertLog(LogLevel.Information, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.InsertLog(LogLevel.Information, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "E-postanızı Onaylayın" : "hata");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                //return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            System.Collections.Generic.IList<string> userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            System.Collections.Generic.List<SelectListItem> factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            string code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            string message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                //await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.InsertLog(LogLevel.Warning, "Kullanıcı hesabı kilitlendi.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kod.");
                return View(model);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
