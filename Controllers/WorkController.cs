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
        public ActionResult Index(int count = -1)
        {
            Debug.WriteLine(count);
            if(count > 0)
            {
                ViewData["Error"] = "Detalės sėkmingai parinktos!";
            }

            if(count == 0)
            {
                ViewData["Error"] = "Automatinė paieška detalių nerado!";
            }

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
                int count = 0;

                while (allParts.Count > 0)
                {
                    foreach (var item in allParts)
                    {
                        if (CheckIfRemains(item.Ammount))
                        {
                            if (CheckByCriteria(model.OriginalParts, model.QualityParts, model.AvgPrice, avgPrice, item))
                            {
                                changeableParts.Add(item);
                                AddToPartList(item, model.ReservationID);
                                RemovePartFromList(allParts, item);
                                count++;
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
                Debug.WriteLine(count);
                return RedirectToAction("Index", new { count = count});
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

        private bool CheckIfRemains(int amount)
        {
            if (amount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private void AddToPartList(Part item, int reservationID)
        {
            Part db = new Part();
            db.AddToReservationPartList(item, reservationID);
        }
    }
}