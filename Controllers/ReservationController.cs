using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Prototipas.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult BreakFormView()
        {
            Reservation db = new Reservation();
            db.Category = db.GetCategories();
            db.Types = db.GetTypes();
            db.FuelTypes = db.GetFuelTypes();
            return View(db);
        }

        public ActionResult BreakFormViewMechanic(string category)
        {
            Reservation db = new Reservation();
            return View("BreakFormViewMechanic", db.GetMechanicsFromCategory(category));
        }
    }
}