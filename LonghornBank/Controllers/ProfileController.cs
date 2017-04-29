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
using LonghornBank.Models;
using LonghornBank.Utility;

namespace LonghornBank.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private AppUserManager _userManager;

        public ProfileController()
        {
        }

        public ProfileController(AppUserManager userManager, ApplicationSignInManager signInManager)
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

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Profile/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated) //user has been redirected here from a page they're not authorized to see
            {
                return View("Error", new string[] { "Access Denied" });
            }
            AuthenticationManager.SignOut(); //this removes any old cookies hanging around
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Profile/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Portal","Home");
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        //
        // GET: /Profile/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Profile/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: Add fields to user here so they will be saved to do the database
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    City = model.City,
                    DOB = model.DOB,
                    MiddleInitial = model.MiddleInitial,
                    State = model.State,
                    StreetAddress = model.StreetAddress,
                    Zip = model.Zip,
                    PhoneNumber = model.PhoneNumber
                    
                };
                var result = await UserManager.CreateAsync(user, model.Password);

                //TODO:  Once you get roles working, you may want to add users to roles upon creation
                // await UserManager.AddToRoleAsync(user.Id, "User");
                // --OR--
                // await UserManager.AddToRoleAsync(user.Id, "Employee");

                // Add ever new register as a member!
                await UserManager.AddToRoleAsync(user.Id, "Customer");


                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                   

                    return RedirectToAction("Create", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Profile/Details
        // Returns the customer's profile detials
        public async Task<ActionResult> Details()
        {
            // Get the customers
            var Customer = await UserManager.FindByNameAsync(User.Identity.Name);

            // Build out the view
            CustomerProfileDetails CustomerProfile = new CustomerProfileDetails
            {
                City = Customer.City,
                DOB = Customer.DOB,
                Email = Customer.Email,
                FName = Customer.FName,
                LName = Customer.LName,
                MiddleInitial = Customer.MiddleInitial,
                PhoneNumber = Customer.PhoneNumber,
                State = Customer.State,
                StreetAddress = Customer.StreetAddress,
                Zip = Customer.Zip
            };

            // return the strongly typed view 
            return View("Details", CustomerProfile);
        }

        // GET: /Profile/Edit
        // Edit the customer's profile 
        public async Task<ActionResult> Edit()
        {
            // Get the customers
            var Customer = await UserManager.FindByNameAsync(User.Identity.Name);

            // Build out the view
            CustomerProfileEdit CustomerProfile = new CustomerProfileEdit
            {
                City = Customer.City,
                FName = Customer.FName,
                LName = Customer.LName,
                MiddleInitial = Customer.MiddleInitial,
                PhoneNumber = Customer.PhoneNumber,
                State = Customer.State,
                StreetAddress = Customer.StreetAddress,
                Zip = Customer.Zip
            };

            return View("Edit", CustomerProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerProfileEdit CustomerProfile)
        {
            // Check the model state 
            if (!ModelState.IsValid)
            {
                return View(CustomerProfile);
            }

            // Get the customers
            var Customer = await UserManager.FindByNameAsync(User.Identity.Name);

            // update the customer 
            Customer.City = CustomerProfile.City;
            Customer.FName = CustomerProfile.FName;
            Customer.LName = CustomerProfile.LName;
            Customer.MiddleInitial = CustomerProfile.MiddleInitial;
            Customer.PhoneNumber = CustomerProfile.PhoneNumber;
            Customer.State = CustomerProfile.State;
            Customer.StreetAddress = CustomerProfile.StreetAddress;
            Customer.Zip = CustomerProfile.Zip;

            // Update the customer 
            var result = await UserManager.UpdateAsync(Customer);

            return RedirectToAction("Portal","Home");
        }

        //
        // GET: /Profile/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Profile/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("Index", "Home");
                }

                // check the birthday 
               if (user.DOB.Year.ToString() != model.Birthday.Trim())
                {
                    // The Birday years do not match up
                    return RedirectToAction("Portal", "Home");
                }
                  
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Profile", new { userId = user.Id}, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
               
                Email.PasswordEmail(user.Email, "Reset Password", "Please reset your password by clicking " + callbackUrl);
                return RedirectToAction("ForgotPasswordConfirmation", "Profile");
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
        // GET: /Profile/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Profile/ResetPassword
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
            // remove the password
            var PasswordRemove = await UserManager.RemovePasswordAsync(user.Id);

            var PasswordUpdate = await UserManager.AddPasswordAsync(user.Id, model.Password);
            if (PasswordUpdate.Succeeded)
            {
                
                return RedirectToAction("ResetPasswordConfirmation", "Profile");
            }
            AddErrors(PasswordUpdate);
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
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
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