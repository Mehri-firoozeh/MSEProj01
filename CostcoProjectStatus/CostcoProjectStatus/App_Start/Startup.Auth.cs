using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using CostcoProjectStatus.Models;
using System.Threading.Tasks;
using Owin.Security.Providers;



namespace CostcoProjectStatus
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                // LoginPath = new PathString("/Account/Login"),
                LoginPath = new PathString("/index.html"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
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

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");











            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "17455599033-ba8qcod8m4m3v8pbt7dj09v5trcmelah.apps.googleusercontent.com",
                ClientSecret = "uEq1Hhb_JeD9rwM56Iwm2c-W",
                //  AccessType = "offline",

                //Provider = new GoogleOAuth2AuthenticationProvider
                //{

                //    OnAuthenticated = async context =>
                //    {
                //        // Retrieve the OAuth access token to store for subsequent API calls
                //        string accessToken = context.AccessToken;

                //        // Retrieve the name of the user in Google
                //        string googleName = context.Name;

                //        // Retrieve the user's email address
                //        string googleEmailAddress = context.Email;

                //        // You can even retrieve the full JSON-serialized user
                //        var serializedUser = context.User;
                //    }
                //}








                //Provider = new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationProvider
                //{
                //    OnAuthenticated = context =>
                //    {
                //        if (!String.IsNullOrEmpty(context.RefreshToken))
                //        {
                //            context.Identity.AddClaim(new  Claim("RefreshToken", context.RefreshToken));
                //        }
                //        return Task.FromResult<object>(null);


                //Provider = new GoogleOAuth2AuthenticationProvider(),
                //CallbackPath = new PathString("/Account/ExternalLoginCallback")
                //}
                // }
            });







        }
    }
}