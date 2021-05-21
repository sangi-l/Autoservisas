using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Autoservisas.Controllers
{
    public class WorkHoursController : Controller
    {
        // GET: WorkHours
        public ActionResult Index()
        {
            WorkHours db = new WorkHours();
            int id = Auth.GetUserId();
            return View(db.GetWorkHours(id));
        }

        // GET: AddTime
        public ActionResult AddTime()
        {
            return View();
        }

        // POST: AddTime
        [HttpPost]
        public ActionResult AddNewTime(WorkHours time)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    WorkHours db = new WorkHours();
                    if (db.AddTime(time))
                    {
                        return RedirectToAction("Index", "WorkHours");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}