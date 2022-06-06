using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ARM_Hotel.Models;

namespace ARM_Hotel.Controllers
{
    public class AdditionalServicesController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: AdditionalServices
        public ActionResult Index()
        {
            var additionalServices = db.AdditionalServices.Include(a => a.Living);
            return View(additionalServices.ToList());
        }

        // GET: AdditionalServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdditionalService additionalService = db.AdditionalServices.Find(id);
            if (additionalService == null)
            {
                return HttpNotFound();
            }
            return View(additionalService);
        }

        // GET: AdditionalServices/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.LivingId = new SelectList(db.Livings, "Id", "Number");
            return View();
        }

        // POST: AdditionalServices/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,LivingId")] AdditionalService additionalService)
        {
            if (ModelState.IsValid)
            {
                db.AdditionalServices.Add(additionalService);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LivingId = new SelectList(db.Livings, "Id", "Number", additionalService.LivingId);
            return View(additionalService);
        }

        // GET: AdditionalServices/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdditionalService additionalService = db.AdditionalServices.Find(id);
            if (additionalService == null)
            {
                return HttpNotFound();
            }
            ViewBag.LivingId = new SelectList(db.Livings, "Id", "Number", additionalService.LivingId);
            return View(additionalService);
        }

        // POST: AdditionalServices/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,LivingId")] AdditionalService additionalService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(additionalService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LivingId = new SelectList(db.Livings, "Id", "Number", additionalService.LivingId);
            return View(additionalService);
        }

        // GET: AdditionalServices/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdditionalService additionalService = db.AdditionalServices.Find(id);
            if (additionalService == null)
            {
                return HttpNotFound();
            }
            return View(additionalService);
        }

        // POST: AdditionalServices/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdditionalService additionalService = db.AdditionalServices.Find(id);
            db.AdditionalServices.Remove(additionalService);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
