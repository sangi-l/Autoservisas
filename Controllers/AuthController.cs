using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Autoservisas.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Auth.SetRole((int)Auth.Roles.NotLoggedIn);
            Auth.SetUserId(0);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            Registered_User db = new Registered_User();
            Mechanic mechanic = new Mechanic();
            Administrator administrator = new Administrator();
            int userid = db.GetUserId(email, password);
            if(userid != 0)
            {
                Auth.SetUserId(userid);
            }
            if (mechanic.IsMechanic(userid))
            {
                Auth.SetRole((int)Auth.Roles.Mechanic);
                return RedirectToAction("Index", "Home");
            }
            if (administrator.IsAdministrator(userid))
            {
                Auth.SetRole((int)Auth.Roles.Administrator);
                return RedirectToAction("Index", "Home");
            }
            if(userid != 0)
            {
                Auth.SetRole((int)Auth.Roles.User);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Registered_User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Registered_User db = new Registered_User();
                    if (db.AddUser(user))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        
        private void SendEmail(string toEmail)
        {
            var username = "..ktu";
            var password = "..ktu";

            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            using (var message = new MailMessage(username, toEmail))
            {
                message.Subject = " ";
                message.Body = " ";
                message.IsBodyHtml = false;
                client.Send(message);
            }
        }
    }

    public static class Auth
    {
        public static void SetUserId(int id)
        {
            HttpContext.Current.Session["LoggedUserId"] = id;
        }

        public static int GetUserId()
        {
            object value = HttpContext.Current.Session["LoggedUserId"];
            return Convert.ToInt32(value);
        }

        public static int GetRole()
        {
            object value = HttpContext.Current.Session["Role"];
            return Convert.ToInt32(value);
        }

        public static void SetRole(int role)
        {
            HttpContext.Current.Session["Role"] = role;
        }

        public enum Roles
        {
            NotLoggedIn = 0,
            Mechanic = 1,
            User = 2,
            Administrator = 3
        }
    }
}