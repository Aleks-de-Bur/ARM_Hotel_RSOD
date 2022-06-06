using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ARM_Hotel.Models;
using ARM_Hotel.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;

namespace ARM_Hotel.Controllers
{
    public class BookingsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();
        private UserManager<IdentityClient> userManager;
        private RoleManager<ClientRole> roleManager;

        public BookingsController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<IdentityClient> userStore = new UserStore<IdentityClient>(db);
            userManager = new UserManager<IdentityClient>(userStore);
            RoleStore<ClientRole> roleStore = new RoleStore<ClientRole>(db);
            roleManager = new RoleManager<ClientRole>(roleStore);

        }

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Apartment).Include(b => b.Client);
            if (User.IsInRole("Client"))
            {
                var curuser = userManager.FindById(User.Identity.GetUserId());
                Client client = db.Clients.FirstOrDefault(l => l.Email == curuser.Email);
                if (client == null)
                {
                    var user = userManager.FindByName(User.Identity.Name);
                    Client newclient = new Client();
                    newclient.Email = user.Email;
                    newclient.FirstName = user.LastName;

                    return View("~/Views/Clients/Create.cshtml", newclient);
                }
                ViewBag.ClientId = client.Id;
                bookings = db.Bookings.Include(b => b.Apartment).Include(b => b.Client).Where(b => b.ClientId == client.Id);
            }

            
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Include(l => l.Apartment).Include(g => g.Guests).Include(l => l.Client).FirstOrDefault(l => l.Id == id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            BookingDetailsModel model = new BookingDetailsModel();
            model.Booking = booking;
            model.Guests = db.Guests.Where(g => g.BookingId == id).ToList();

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApartmentId = new SelectList(db.Apartments, "Id", "Number", booking.ApartmentId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", booking.ClientId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Settling,Eviction,Number,MaxGuests,ClientId,GuestId,Price,Type,ApartmentId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApartmentId = new SelectList(db.Apartments, "Id", "Number", booking.ApartmentId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", booking.ClientId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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

        public ActionResult BookingToLiving(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            Living living = new Living()
            {
                ClientId = booking.ClientId,
                Settling = booking.Settling,
                Eviction = booking.Eviction,
                Price = booking.Price,
                ValueOfGuests = booking.ValueOfGuests,
                Active = true,
                ApartmentId = booking.ApartmentId,
                Type = booking.Apartment.Type,
                Number = booking.Apartment.Number

            };
            db.Livings.Add(living);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index", "Livings");
        }

        public ActionResult BookingStat()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //Открытие существующего файла
            FileInfo fi = new FileInfo(@"E:\Саня\Лаб\ARM_Hotel\Content\Reports\BookingsReport.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //Получение листа по имени. Если лист с таким именем не существует - будет сгенерировано исключение
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Лист1"];
                //Занесение информации
                TimeSpan ts = new TimeSpan(-30, 0, 0, 0, 0);
                DateTime settling = DateTime.Now.Date + ts;
                List<Booking> bookings = db.Bookings
                                    .Include(m => m.Client)
                                    .Where(l => l.Settling > settling).ToList();
                int i = 3;
                foreach (var booking in bookings)
                {
                    worksheet.Cells[i, 1].Value = i - 2;
                    worksheet.Cells[i, 2].Value = booking.Client.LastName + " " + booking.Client.FirstName;
                    worksheet.Cells[i, 3].Value = booking.Settling;
                    worksheet.Cells[i, 4].Value = booking.Eviction;
                    worksheet.Cells[i, 5].Value = booking.Type;
                    worksheet.Cells[i, 6].Value = Convert.ToInt32(booking.Number);
                    worksheet.Cells[i, 7].Value = booking.ValueOfGuests;
                    worksheet.Cells[i, 8].Value = booking.Price;
                    i++;
                }
                //Сохраняем файл
                excelPackage.Save();
            }
            return RedirectToAction("Index", "Bookings");
        }

    }
}
