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
    public class ApartmentsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: Apartments
        public ActionResult Index()
        {
            ViewBag.Error = "";
            return View(db.Apartments.ToList());
        }

        //Photos
        [Authorize(Roles = "Admin")]
        public ActionResult AddPhoto(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
                return HttpNotFound();
            return View(apartment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddPhoto(int id, HttpPostedFileBase upload, string description)
        {
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                Photo photo = new Photo();
                photo.ApartmentId = id;
                photo.Filename = fileName;
                photo.Description = description;
                db.Photos.Add(photo);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }



        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Include(a => a.Photos).FirstOrDefault(a => a.Id == id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            List<byte[]> photos = new List<byte[]>();
            foreach (Photo photo in apartment.Photos)
            {
                photos.Add(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/" + photo.Filename)));
            }
            ViewBag.Photos = photos;
            return View(apartment);
        }

        // GET: Apartments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Apartments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Number,Type,Price,MaxGuests")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {

                if (db.Apartments.Any(cl => cl.Number == apartment.Number))
                {
                    if (db.Apartments.Any(cl => cl.Type == apartment.Type))
                    {
                        ModelState.AddModelError("Number", "Такой номер уже существует");
                        return View();
                    }
                }

                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(apartment);
        }

        // GET: Apartments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Number,Type,Price,MaxGuests")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = "";
            if (db.Livings.Any(cl => cl.Number == apartment.Number))
            {
                DateTime dt = DateTime.Now;
                List<Living> living = db.Livings.Where(cl => cl.Number == apartment.Number).Where(cl => cl.Eviction < dt).ToList();
                if ((living.Count != 0)&&(db.Bookings.Any(cl => cl.Number == apartment.Number)))
                {
                    ViewBag.Error = "Нельзя удалить номер, пока в нём живут люди";
                }
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);


            db.Apartments.Remove(apartment);
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
