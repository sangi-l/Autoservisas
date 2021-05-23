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
        [HttpPost]
        public ActionResult BreakFormView(Reservation model)
        {
            Reservation db = new Reservation();
            List<Mechanic> mechList = db.GetMechanicsFromCategory(model.Categoryy);
            return View("BreakFormViewMechanic", mechList);
        }

        public ActionResult BreakFormViewMechanic(List<Mechanic> mech)
        {
            return View(mech);
        }

        public ActionResult BreakFormViewTime(int id)
        {
            Reservation db = new Reservation();
            List<Reservation.Timee> timeList = db.GetTime(id);
            return View(timeList);
        }
    }
}