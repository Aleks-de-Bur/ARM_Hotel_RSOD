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
    public class GuestsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: Guests
        public ActionResult Index()
        {
            return View(db.Guests.ToList());
        }

        // GET: Guests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // GET: Guests/Create
        public ActionResult Create(int? livingId, int? bookingId)
        {
            
            ViewBag.LivingId = livingId == null ? 0 : livingId;
            ViewBag.BookingId = bookingId == null ? 0 : bookingId;
            return View();
        }

        // POST: Guests/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Patronymic,SeriaPas,NumberPas")] Guest guest, int livingId, int bookingId)
        {
            if (ModelState.IsValid)
            {
                if (!db.Guests.Any(l => l.SeriaPas == guest.SeriaPas && l.NumberPas == guest.NumberPas))
                {
                    db.Guests.Add(guest);
                    db.SaveChanges();
                }
                else
                {
                    guest = db.Guests.First(l => l.SeriaPas == guest.SeriaPas && l.NumberPas == guest.NumberPas);
                }
                if (livingId != 0)
                {
                    Living liv = db.Livings.Include(m => m.Guests).First(l => l.Id == livingId);
                    liv.Guests.Add(guest);
                    db.SaveChanges();

                    
                    if (liv.ValueOfGuests > liv.Guests.Count)
                    {
                        return RedirectToAction("Create", "Guests", new { livingId = livingId, bookingId = 0 });
                    }

                }
                if (bookingId != 0)
                {
                    Booking boo = db.Bookings.Include(m => m.Guests).First(l => l.Id == bookingId);
                    boo.Guests.Add(guest);
                    db.SaveChanges();

                    
                    if (boo.ValueOfGuests > boo.Guests.Count)
                    {
                        return RedirectToAction("Create", "Guests", new { livingId = 0, bookingId = bookingId });
                    }
                }
                return RedirectToAction("Index");
            }

            return View(guest);
        }

        // GET: Guests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Patronymic,SeriaPas,NumberPas")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guest guest = db.Guests.Find(id);
            db.Guests.Remove(guest);
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
