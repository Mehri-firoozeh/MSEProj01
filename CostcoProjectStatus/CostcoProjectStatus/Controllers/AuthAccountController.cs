using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CostcoProjectStatus.Controllers
{
    public class AuthAccountController : Controller
    {
        // GET: AuthAccount
        public ActionResult Index()
        {
            return View();
        }
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        public ActionResult ExternalLogin()
        {
            // Request a redirect to the external login provider
            return new AccountController.ChallengeResult("google", Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = "http://www.google.com" }));
        }
    }
}