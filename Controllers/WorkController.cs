using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Autoservisas.Controllers
{
    public class WorkController : Controller
    {
        // GET: Work
        public ActionResult Index()
        {
            Work db = new Work();
            int id = Auth.GetUserId();
            return View(db.GetWork(id));
        }

        // GET: Work/Edit
        public ActionResult Edit(int id)
        {
            Work db = new Work();
            return View(db.GetReservation(id));
        }

        // POST: Work/Edit
        [HttpPost]
        public ActionResult Edit(int id, Work model)
        {
            try
            {
                Work db = new Work();
                db.UpdateDetails(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}