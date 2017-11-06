using BLL.Services;
using BLL.Services.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppUserManager _userManager;
        private readonly AppSignInManager _signInManager;
        private readonly IAuthenticationManager _authManager;

        public HomeController(AppUserManager userManager, AppSignInManager signInManager, IAuthenticationManager authManager)
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}