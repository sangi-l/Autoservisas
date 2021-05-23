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
            db.SymptomData = db.GetSymptoms();
            return View(db);
        }
        [HttpPost]
        public ActionResult BreakFormView(Reservation model)
        {
            Reservation db = new Reservation();
            //List<Mechanic> mechList = db.GetMechanicsFromCategory(model.Categoryy);
            model.mechanikai = db.GetMechanicsFromCategory(model.Categoryy);
            return RedirectToAction("BreakFormViewMechanic", model);
        }

        public ActionResult BreakFormViewMechanic(Reservation model)
        {
            Reservation db = new Reservation();
            model.mechanikai = db.GetMechanicsFromCategory(model.Categoryy);
            return View(model);
        }

        [HttpPost]
        public ActionResult BreakFormViewMechanic(Reservation model, string kazkas = "")
        {

            return RedirectToAction("BreakFormViewTime", (model.mID, model));
        }

        public ActionResult BreakFormViewTime(int id, Reservation model)
        {
            Reservation db = new Reservation();
            List<Reservation.Timee> timeList = db.GetTime(id);
            Debug.WriteLine(model.CarNumber);
            Debug.WriteLine(model.mechanikai.Count());
            return View(timeList);
        }
    }
}