using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Autoservisas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var role = Auth.GetRole();
            if (role != (int)Auth.Roles.User && role != (int)Auth.Roles.Mechanic && role != (int)Auth.Roles.Administrator)
                Auth.SetRole((int)Auth.Roles.NotLoggedIn);
            return View();
        }
    }
}