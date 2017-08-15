using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Distributor.Models;
using Distributor.Helpers;
using static Distributor.Enums.UserEnums;
using System.Collections.Generic;
using static Distributor.Enums.EntityEnums;
using System.Data.Entity.Core.Metadata.Edm;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    ApplicationUser user = UserManager.FindByEmail(model.Email);
                    //validate the user is not on-hold
                    if (!AppUserHelpers.IsAppUserActive(user))
                    {
                        EntityStatusEnum appUserStatus = AppUserHelpers.GetAppUserEntityStatus(user);
                        switch (appUserStatus)
                        {
                            case EntityStatusEnum.Inactive:
                                ModelState.AddModelError("", "This user is currently inactive.  You will need to re-register or contact your account administrator");
                                break;
                            case EntityStatusEnum.OnHold:
                                ModelState.AddModelError("", "This user is currently on hold.  You will need to contact your account administrator to active your account");
                                break;
                        }
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
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
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //DropDown
            ViewBag.CompanyList = ControlHelpers.AllCompaniesListDropDown();
            ViewBag.BranchList = new SelectList(Enumerable.Empty<SelectListItem>(), "BranchId", "BranchName");
            ViewBag.BusinessTypeList = ControlHelpers.BusinessTypeEnumListDropDown();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //If this is a new user and company then set to ACTIVE and an ADMIN role, else set to ON-HOLD and USER role and await activating by admin for branch/company of user and/or new branch details.
                EntityStatusEnum statusForUser = EntityStatusEnum.Active;
                UserRoleEnum userRoleForUser = UserRoleEnum.Admin;

                //initialise the task creation flags
                bool createUserOnHoldTask = false;
                bool createBranchOnHoldTask = false;
                
                if (model.SelectedCompanyId.HasValue)
                {
                    statusForUser = EntityStatusEnum.OnHold;
                    userRoleForUser = UserRoleEnum.User;
                }

                //Create a new AppUser then write here
                AppUser appUser = AppUserHelpers.CreateAppUser(model.FirstName, model.LastName, Guid.Empty, statusForUser);

                Company company = null;
                Branch branch = null;
                BranchUser branchUser = null;

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, AppUserId = appUser.AppUserId, FullName = model.FirstName + " " + model.LastName, CurrentUserRole = userRoleForUser };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //only log in if this user is not set to on-hold
                    if (!model.SelectedCompanyId.HasValue)
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    else  //we will need to create a task for the branch
                        createUserOnHoldTask = true;

                    //Now Update related entities
                    //Company                 
                    bool createCompany = true;

                    if (model.SelectedCompanyId.HasValue)
                        if (model.SelectedCompanyId.Value != Guid.Empty)
                            createCompany = false;

                    if (createCompany)
                        company = CompanyHelpers.CreateCompany(Guid.Empty, model.CompanyName, model.CompanyRegistrationDetails, model.CharityRegistrationDetails, model.VATRegistrationDetails, statusForUser);
                    else
                        company = CompanyHelpers.GetCompany(model.SelectedCompanyId.Value);
                    

                    //Branch
                    bool createBranch = true;

                    if (model.SelectedBranchId.HasValue)
                        if (model.SelectedBranchId.Value != Guid.Empty)
                            createBranch = false;

                    if (createBranch)
                    {
                        string branchName = model.BranchName;
                        if (!model.SelectedCompanyId.HasValue)
                            branchName = "Head Office";

                        if (createCompany) //use details stored against company part of model
                            branch = BranchHelpers.CreateBranch(company.CompanyId, model.CompanyBusinessType.Value, branchName, model.CompanyAddressLine1, model.CompanyAddressLine2, model.CompanyAddressLine3, model.CompanyAddressTownCity, model.CompanyAddressCounty, model.CompanyAddressPostcode, model.CompanyTelephoneNumber, model.CompanyEmail, model.CompanyContactName, statusForUser);
                        else
                            branch = BranchHelpers.CreateBranch(company.CompanyId, model.BranchBusinessType.Value, branchName, model.BranchAddressLine1, model.BranchAddressLine2, model.BranchAddressLine3, model.BranchAddressTownCity, model.BranchAddressCounty, model.BranchAddressPostcode, model.BranchTelephoneNumber, model.BranchEmail, model.BranchContactName, statusForUser);

                        createBranchOnHoldTask = true;

                        //Company - set head office branch as the newly created branch for this new company (defaults to 'Head Office')
                        if (!model.SelectedCompanyId.HasValue)
                            company = CompanyHelpers.UpdateCompanyHeadOffice(company.CompanyId, branch.BranchId);
                    }
                    else
                        branch = BranchHelpers.GetBranch(model.SelectedBranchId.Value);
                    
                    //BranchUser
                    branchUser = BranchUserHelpers.CreateBranchUser(appUser.AppUserId, branch.BranchId, company.CompanyId, userRoleForUser, statusForUser);

                    //Update AppUser with the branch we are adding/using to set as current branch for new user
                    appUser = AppUserHelpers.UpdateCurrentBranchId(appUser.AppUserId, branch.BranchId);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //Task creation
                    if (createUserOnHoldTask)
                        UserTaskHelpers.CreateUserTask(TaskTypeEnum.UserOnHold, "New user on hold, awaiting administrator/manager activation", appUser.AppUserId, appUser.AppUserId, EntityStatusEnum.Active);

                    if (createBranchOnHoldTask)
                        UserTaskHelpers.CreateUserTask(TaskTypeEnum.BranchOnHold, "New branch on hold, awaiting administrator activation", branch.BranchId, appUser.AppUserId, EntityStatusEnum.Active);


                    if (model.SelectedCompanyId.HasValue)
                        return RedirectToAction("Confirmation");
                    else
                        return RedirectToAction("Dashboard", "Home");
                }

                //Delete the appUser account as this has not gone through
                AppUserHelpers.DeleteAppUser(appUser.AppUserId);
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form - set up the drop downs dependant on what was there originally from the model
            if (model.SelectedCompanyId.HasValue)
            {
                ViewBag.CompanyList = ControlHelpers.AllCompaniesListDropDown(model.SelectedCompanyId.Value);
                if (model.SelectedBranchId.HasValue)
                    ViewBag.BranchList = ControlHelpers.AllBranchesForCompanyListDropDown(model.SelectedCompanyId.Value, model.SelectedBranchId.Value);
                else
                    ViewBag.BranchList = new SelectList(Enumerable.Empty<SelectListItem>(), "BranchId", "BranchName");
            }
            else
            {
                ViewBag.CompanyList = ControlHelpers.AllCompaniesListDropDown();
                ViewBag.BranchList = new SelectList(Enumerable.Empty<SelectListItem>(), "BranchId", "BranchName");
            }            

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}