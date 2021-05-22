using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ActionResult Edit(Work model)
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

        // GET: Automatisation
        public ActionResult Automatisation(int id)
        {
            Work db = new Work();
            db.ReservationID = id;
            return View(db);
        }

        // POST: Automatisation
        [HttpPost]
        public ActionResult Automatisation(Work model)
        {
            try
            {
                Work db = new Work();
                string symptom = db.FindSymptoms(model.ReservationID);
                List<Part> allParts = db.FilterBySymptom(symptom);
                double avgPrice = db.AveragePrice(allParts);
                List<Part> changeableParts = new List<Part>();

                while(allParts.Count > 0)
                {
                    foreach(var item in allParts)
                    {
                        if(item.Ammount > 0)
                        {
                            if(CheckByCriteria(model.OriginalParts, model.QualityParts, model.AvgPrice, avgPrice, item))
                            {
                                changeableParts.Add(item);
                                AddToPartList(item);
                                RemovePartFromList(allParts, item);
                                break;
                            }
                            else
                            {

                                RemovePartFromList(allParts, item);
                                break;
                            }

                        }
                        else
                        {
                            RemovePartFromList(allParts, item);
                            break;
                        }
                    }
                }

                foreach(var item in changeableParts)
                {
                    Debug.WriteLine(item.Name + " " + item.Code);
                }

                Debug.WriteLine(model.ReservationID);
                Debug.WriteLine(model.OriginalParts);
                Debug.WriteLine(model.QualityParts);
                Debug.WriteLine(model.AvgPrice);

                /*Work db = new Work();
                db.UpdateDetails(model);
                return RedirectToAction("Index");*/
                return View();
            }
            catch
            {
                return View();
            }
        }

        private void RemovePartFromList(List<Part> list, Part item)
        {
            list.Remove(item);
        }

        private bool CheckByCriteria(bool OriginalParts, bool QualityParts, bool AvgPrice, double avgPrice, Part item)
        {
            bool passed = false;

            bool passed1 = false;
            bool passed2 = false;
            bool passed3 = false;

            if (OriginalParts)
            {
                if (item.Originallity)
                    passed1 = true;
            }

            if (QualityParts)
            {
                if (item.Quality > 3)
                    passed2 = true;
            }

            if (AvgPrice)
            {
                if (item.Price < avgPrice)
                    passed3 = true;
            }

            if (OriginalParts == passed1 && QualityParts == passed2 &&
                AvgPrice == passed3)
            {
                passed = true;
            }

                return passed;
        }

        private void AddToPartList(Part item)
        {

        }
    }
}