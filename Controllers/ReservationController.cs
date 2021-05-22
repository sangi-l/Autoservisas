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
            return View(db);
        }

        // GET: Work/Edit
        public ActionResult Edit(int id)
        {
            Reservation db = new Reservation();
            return View(db.GetReservation(id));
        }

    }
}