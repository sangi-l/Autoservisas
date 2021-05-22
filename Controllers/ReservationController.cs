using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;
using System.Diagnostics;

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

        public ActionResult BreakFormView(Reservation category)
        {
            return View("BreakFormViewMechanic", category.Categoryy);
        }

        public ActionResult BreakFormViewMechanic(Reservation MV)
        {
            Reservation db = new Reservation();
            string selectedvalue = MV.Categoryy;
            Debug.WriteLine(selectedvalue);
            return View("BreakFormViewMechanic", db.GetMechanicsFromCategory(selectedvalue));
        }
    }
}