using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using ARM_Hotel.Models;

namespace ARM_Hotel.Controllers
{
    public class TakeLivingsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: TakeLivings
        [Authorize(Roles = "Admin, Operator, Client")]
        public ActionResult Index(int? clientId, bool? isLiving)
        {
            ViewBag.IsLiving = isLiving;

            if (clientId != null)
            {
                Client client = db.Clients.Find(clientId);
                if (client == null)
                    return HttpNotFound();

                ViewBag.ClientId = clientId;
                ViewBag.Client = client;

                return View("ChooseDate");

            }

            return View(db.Clients.ToList());
        }
        [Authorize(Roles = "Admin, Operator, Client")]
        [HttpPost]
        public ActionResult Index(int clientId, bool isLiving)
        {
            Client client = db.Clients.Find(clientId);
            if (client == null)
                return HttpNotFound();

            ViewBag.ClientId = clientId;
            ViewBag.Client = client;
            ViewBag.settling = DateTime.Now.Date;
            ViewBag.eviction = DateTime.Now.Date.AddDays(1);
            ViewBag.IsLiving = isLiving;
            return View("ChooseDate");
        }

        [Authorize(Roles = "Admin, Operator, Client")]
        [HttpPost]
        public ActionResult ChooseDate(int clientId, DateTime? settling, DateTime? eviction, int guestCount, bool isLiving)
        {

            Client client = db.Clients.Find(clientId);
            if (settling < DateTime.Now.Date || settling == null)
            {
                ModelState.AddModelError("Settling", "Нельзя оформить проживание на прошедшие даты");
                ViewBag.ClientId = clientId;
                ViewBag.settling = settling;
                ViewBag.eviction = eviction;
                ViewBag.guestCount = guestCount;
                ViewBag.IsLiving = isLiving;
                ViewBag.Client = client;
                return View();
            }
            else if (eviction <= settling || eviction == null)
            {
                ModelState.AddModelError("Eviction", "Дата выселения должна быть больше даты выселения");
                ViewBag.ClientId = clientId;
                ViewBag.settling = settling;
                ViewBag.eviction = eviction;
                ViewBag.guestCount = guestCount;
                ViewBag.IsLiving = isLiving;
                ViewBag.Client = client;
                return View();
            }
            if (guestCount < 0 || guestCount > 5)
            {
                ModelState.AddModelError("GuestCount", "Число гостей должно быть от 0 до 5");
                ViewBag.ClientId = clientId;
                ViewBag.settling = settling;
                ViewBag.eviction = eviction;
                ViewBag.guestCount = guestCount;
                ViewBag.IsLiving = isLiving;
                ViewBag.Client = client;
                return View();
            }

            List<Apartment> Apartments = db.Apartments
                                    .Include(m => m.Living)
                                    .Include(n => n.Booking)
                                    .ToList();
            List<Apartment> avalibleApartment = new List<Apartment>();
            foreach (var apartment in Apartments)
            {
                if (apartment.MaxGuests >= guestCount)
                    if (!(apartment.Living.Any(l => l.Settling >= settling && l.Settling <= eviction)) &&
                        !(apartment.Living.Any(l => l.Eviction >= settling && l.Eviction <= eviction)) &&
                        !(apartment.Living.Any(l => l.Settling <= settling && l.Eviction >= eviction)) &&
                        !(apartment.Booking.Any(l => l.Settling >= settling && l.Settling <= eviction)) &&
                        !(apartment.Booking.Any(l => l.Eviction >= settling && l.Eviction <= eviction)) &&
                        !(apartment.Booking.Any(l => l.Settling <= settling && l.Eviction >= eviction)))
                    {
                        avalibleApartment.Add(apartment);
                    }
            }

            ViewBag.ClientId = clientId;
            ViewBag.settling = settling;
            ViewBag.eviction = eviction;
            ViewBag.Client = client;
            ViewBag.guestCount = guestCount;
            ViewBag.IsLiving = isLiving;
            return View("ChooseApartment", avalibleApartment);
        }

        [Authorize(Roles = "Admin, Operator, Client")]
        [HttpPost]
        public ActionResult ChooseApartment(int clientId, DateTime settling, DateTime eviction, int apartmentId, int guestCount, bool isLiving)
        {
            Client client = db.Clients.Find(clientId);
            Apartment apartment = db.Apartments.Find(apartmentId);

            decimal price = apartment.Price * Convert.ToDecimal((eviction - settling).TotalDays);

            ViewBag.ClientId = clientId;
            ViewBag.settling = settling.ToLongDateString();
            ViewBag.eviction = eviction.ToLongDateString();
            ViewBag.Client = client;
            ViewBag.Apartment = apartment;
            ViewBag.Price = price;
            ViewBag.guestCount = guestCount;
            ViewBag.IsLiving = isLiving;
            return View("ConfirmLiving");
        }

        //Services
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult AddAdditionalService(int id)
        {
            Living living = db.Livings.Find(id);
            if (living == null)
                return HttpNotFound();
            return View(living);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddAdditionalService(int id, HttpPostedFileBase upload, string description)
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

        [Authorize(Roles = "Admin, Operator, Client")]
        [HttpPost]
        public ActionResult ConfirmLiving(int clientId, DateTime settling, DateTime eviction, int apartmentId, int guestCount, bool isLiving, decimal price)
        {
            Apartment apartment = db.Apartments.Find(apartmentId);

            if (isLiving)
            {
                Living living = new Living()
                {
                    ClientId = clientId,
                    Settling = settling,
                    Eviction = eviction,
                    Price = price,
                    ValueOfGuests = guestCount,
                    Active = true,
                    ApartmentId = apartmentId,
                    Type = apartment.Type,
                    Number = apartment.Number

                };
                db.Livings.Add(living);

                db.SaveChanges();

                if (guestCount > 0)
                {
                    return RedirectToAction("Create", "Guests", new { livingId = living.Id });
                }


                return RedirectToAction("Index", "Livings");
            }
            else
            {
                Booking booking = new Booking()
                {
                    ClientId = clientId,
                    Settling = settling,
                    Eviction = eviction,
                    Price = price,
                    ValueOfGuests = guestCount,
                    ApartmentId = apartmentId,
                    Type = apartment.Type,
                    Number = apartment.Number
                };
                db.Bookings.Add(booking);

                db.SaveChanges();

                if (guestCount > 0)
                {
                    return RedirectToAction("Create", "Guests", new { bookingId = booking.Id });
                }

                return RedirectToAction("Index", "Bookings");
            }

        }
    }
}