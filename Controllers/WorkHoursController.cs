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
        public ActionResult AddTime(WorkHours time)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (time.TimeTo - time.TimeFrom <= 9)
                    {
                        time.MechanicID = Auth.GetUserId();
                        WorkHours db = new WorkHours();
                        if (db.AddTime(time))
                        {
                            return RedirectToAction("Index", "WorkHours");
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Negalima dirbti daugiau nei 9h per dieną!";
                        return View();
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