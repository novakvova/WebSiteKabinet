using BLL.Services.Identity;
using BLL.ViewModels.Identity;
using DAL.Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Providers
{
    public class AccountProvider
    {
        private readonly AppUserManager _userManager;
        private readonly AppSignInManager _signInManager;
        private readonly IAuthenticationManager _authManager;

        public AccountProvider(AppUserManager userManager,
            AppSignInManager signInManager,
            IAuthenticationManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authManager = authManager;
        }

        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager;
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authManager;
            }
        }

        public async Task<SignInStatus> Login(LoginViewModel model)
        {
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            return result;
        }

        public void LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return result;
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _authManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo)
        {
            return await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        }
    }
}
