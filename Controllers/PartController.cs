using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Autoservisas.Controllers
{
    public class PartController : Controller
    {
        // GET: Part
        public ActionResult Index()
        {
            Part db = new Part();
            ModelState.Clear();
            return View(db.GetPart());
        }

        // GET: Part/Edit/5
        public ActionResult Edit(int id)
        {
            Part db = new Part();
            return View(db.GetPart().Find(model => model.PartID == id));
        }

        // POST: Part/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Part model)
        {
            try
            {
                Part db = new Part();
                db.UpdateAmmount(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ManualChoose
        public ActionResult ManualChooseIndex()
        {
            Part db = new Part();
            return View("ManualChooseIndex", db.GetCategories());
        }

        public ActionResult ManualChooseParts(string category)
        {
            Part db = new Part();
            return View("ManualChooseParts", db.GetPartsFromCategory(category));
        }
    }
}