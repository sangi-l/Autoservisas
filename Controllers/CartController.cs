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
    }
}