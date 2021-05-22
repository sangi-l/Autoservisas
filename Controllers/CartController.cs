using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Prototipas.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["cart"] == null)
            {
                List<Part> cart = new List<Part>();
                Session["cart"] = cart;
            }
            return View();
        }
        public ActionResult Remove(int id)
        {
            List<Part> cart = (List<Part>)Session["cart"];
            Part part = cart.Find(item => item.PartID == id);
            cart.Remove(part);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
        public ActionResult Pay()
        {
            PayInfo info = new PayInfo();
            return View(info);
        }
        public ActionResult Send()
        {
            SendEmail();
            return RedirectToAction("Index");
        }
        public void SendEmail()
        {
            var username = "ktuautoproject@gmail.com";
            var password = "autoproject";

            string toEmail = "igansbiz@gmail.com";

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
                message.Subject = "Mokėjimas";
                message.Body = "Mokėjimas atliktas";
                message.IsBodyHtml = false;
                client.Send(message);
            }
        }
    }
}