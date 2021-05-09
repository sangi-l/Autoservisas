using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoservisas.Models;

namespace Autoservisas.Controllers
{
    public class SymptomController : Controller
    {
        // GET: Symptom
        public ActionResult Index()
        {
            Symptom db= new Symptom();
            ModelState.Clear();
            return View(db.GetSymptom());
        }
        // GET: Symptom/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Symptom/Create
        [HttpPost]
        public ActionResult Create(Symptom model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Symptom db = new Symptom();
                    if (db.AddSymptom(model))
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Symptom/Edit/5
        public ActionResult Edit(int id)
        {
            Symptom db = new Symptom();
            return View(db.GetSymptom().Find(model => model.SymptomID == id));
        }

        // POST: Symptom/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Symptom model)
        {
            try
            {
                Symptom db = new Symptom();
                db.UpdateDetails(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Symptom/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Symptom db = new Symptom();
                if (db.DeleteSymptom(id))
                {
                    ViewBag.AlertMsg = "Student Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
