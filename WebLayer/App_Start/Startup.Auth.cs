﻿using Autofac;
using Autofac.Integration.Mvc;
using BLL.Services;
using BLL.Services.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(WebLayer.App_Start.Startup))]

namespace WebLayer.App_Start
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new DataModule("FinalDBConn", app));

            // REGISTER CONTROLLERS SO DEPENDENCIES ARE CONSTRUCTOR INJECTED

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // BUILD THE CONTAINER
            var container = builder.Build();

            // REPLACE THE MVC DEPENDENCY RESOLVER WITH AUTOFAC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // REGISTER WITH OWIN
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            //Configuration(app);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(MyOptions.OptionCookies());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");


            app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
            {
                AppId = "1991172914492732",
                AppSecret = "00f559d9093d1a7c106b828d9b9715c9",
                BackchannelHttpHandler = new HttpClientHandler(),
                UserInformationEndpoint = "https://graph.facebook.com/v2.8/me?fields=id,name,email,first_name,last_name,birthday",
                //Scope = { "email" },
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                  {
                      context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                      foreach (var claim in context.User)
                      {
                          var claimType = string.Format("urn:facebook:{0}", claim.Key);
                          string claimValue = claim.Value.ToString();
                          if (!context.Identity.HasClaim(claimType, claimValue))
                              context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                      }
                  }
                }
            });

            //var facebookOptions = new FacebookAuthenticationOptions()
            //{
            //    AppId = "1991172914492732",
            //    AppSecret = "00f559d9093d1a7c106b828d9b9715c9"
            //};

            //facebookOptions.Scope.Add("email");

            //facebookOptions.Fields.Add("name");
            //facebookOptions.Fields.Add("email");
            //facebookOptions.Fields.Add("first_name");
            //facebookOptions.Fields.Add("last_name");
            //facebookOptions.Fields.Add("birthday");

            //app.UseFacebookAuthentication(facebookOptions);



            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}