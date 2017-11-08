using BLL.Services.Identity;
using BLL.ViewModels.Identity;
using DAL.Abstract;
using DAL.Entities;
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
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AccountProvider(AppUserManager userManager,
            AppSignInManager signInManager,
            IAuthenticationManager authManager,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authManager = authManager;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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

        public bool Register(RegisterViewModel model)
        {
            bool res = false;
            try
            {
                using (var uof = _unitOfWork)
                {
                    uof.StartTransaction();
                    var user = new AppUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = UserManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        Guid guidImage = Guid.NewGuid();
                        UserProfile userProfile = new UserProfile
                        {
                            Id = user.Id,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Image = guidImage.ToString()
                        };
                        _userRepository.Add(userProfile);
                        _userRepository.SaveChanges();
                        //throw new Exception();
                        uof.CommitTransaction();
                        
                    }
                }
            }
            catch {
            }
            
            return res;
        }
        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            return await Task.Run(() => this.Register(model));
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
