using Nimap.BAL;
using Nimap.Filters;
using Nimap.Models;
using Nimap.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimap.Controllers
{
    public class AuthController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel userRequest)
        {
            UserModel authenticatedUser = new UserRepository().GetUser(userRequest.UserName);

            if (authenticatedUser == null)
            {
                ModelState.AddModelError("", "The user was not found.");
                return View(userRequest);
            }

            bool credentials = authenticatedUser.Password.Equals(userRequest.Password);

            if (!credentials)
            {
                ModelState.AddModelError("", "The username/password combination was wrong.");
                return View(userRequest);
            }
            
            AuthBAL.GenerateToken(authenticatedUser);

            return RedirectToAction("Index","Home");
        }

    }
}
