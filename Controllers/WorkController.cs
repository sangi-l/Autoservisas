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
    }
}