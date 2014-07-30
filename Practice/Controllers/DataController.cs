using epiPGSInter.Tmigma.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreLinq;

namespace Practice.Controllers
{
    public class DataController : Controller
    {
        DataContext dc = new DataContext();
        //
        // GET: /Data/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DistinctUserIds()
        {
            return Json(new { value = dc.Events.Select(e=>e.User).Distinct() }, JsonRequestBehavior.AllowGet);   
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                dc.Dispose();
            }
        }
	}
}