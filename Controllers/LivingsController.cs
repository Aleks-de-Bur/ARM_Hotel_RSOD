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
    public class LivingsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();
        private UserManager<IdentityClient> userManager;
        private RoleManager<ClientRole> roleManager;

        public LivingsController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<IdentityClient> userStore = new UserStore<IdentityClient>(db);
            userManager = new UserManager<IdentityClient>(userStore);
            RoleStore<ClientRole> roleStore = new RoleStore<ClientRole>(db);
            roleManager = new RoleManager<ClientRole>(roleStore);

        }

        // GET: Livings
        [Authorize(Roles = "Admin, Operator, Client")]
        public ActionResult Index()
        {
            var livings = db.Livings.Include(l => l.Apartment).Include(l => l.Client);
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
                livings = db.Livings.Include(l => l.Apartment).Include(l => l.Client).Where(l => l.ClientId == client.Id);
            }

            return View(livings.ToList());
        }

        // GET: Livings/Details/5
        [Authorize(Roles = "Admin, Operator, Client")]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Living living = db.Livings.Include(l => l.Apartment).Include(g => g.Guests).Include(l => l.Client).FirstOrDefault(l => l.Id == id);
            if (living == null)
            {
                return HttpNotFound();
            }

            LivingDetailsModel model = new LivingDetailsModel();
            model.Living = living;
            model.Guests = db.Guests.Where(g => g.LivingId == id).ToList();
            model.Services = db.AdditionalServices
                .Where(s => s.LivingId == id)
                .Include(s => s.Service)
                .ToList();


            ViewBag.AllPrice = sum(id);

            return View(model);
        }

        [Authorize(Roles = "Admin, Operator")]
        public ActionResult AddService(int? id)
        {
            Living living = db.Livings.Find(id);
            ViewBag.AddServices = id;

            return View(living);
        }

        // GET: Livings/Edit/5
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Living living = db.Livings.Find(id);
            if (living == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApartmentId = new SelectList(db.Apartments, "Id", "Number", living.ApartmentId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", living.ClientId);
            return View(living);
        }

        // POST: Livings/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Edit([Bind(Include = "Id,Settling,Eviction,Number,MaxGuests,ClientId,GuestId,Price,Type,ApartmentId")] Living living)
        {
            if (ModelState.IsValid)
            {
                db.Entry(living).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApartmentId = new SelectList(db.Apartments, "Id", "Number", living.ApartmentId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", living.ClientId);
            return View(living);
        }

        // GET: Livings/Delete/5
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Living living = db.Livings.Find(id);
            if (living == null)
            {
                return HttpNotFound();
            }
            return View(living);
        }

        // POST: Livings/Delete/5
        [Authorize(Roles = "Admin, Operator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Living living = db.Livings.Find(id);
            db.Livings.Remove(living);
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

        [Authorize(Roles = "Admin, Operator")]
        public ActionResult LivingStat()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //Открытие существующего файла
            FileInfo fi = new FileInfo(@"E:\Саня\Лаб\ARM_Hotel\Content\Reports\LivingsReport.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //Получение листа по имени. Если лист с таким именем не существует - будет сгенерировано исключение
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Лист1"];
                //Занесение информации
                TimeSpan ts = new TimeSpan(-30, 0, 0, 0, 0);
                DateTime settling = DateTime.Now.Date + ts;
                List<Living> livings = db.Livings
                                    .Include(m => m.Client)
                                    .Where(l => l.Settling > settling).ToList();
                int i = 3;
                foreach (var living in livings)
                {
                    worksheet.Cells[i, 1].Value = i - 2;
                    worksheet.Cells[i, 2].Value = living.Client.LastName + " " + living.Client.FirstName;
                    worksheet.Cells[i, 3].Value = living.Settling;
                    worksheet.Cells[i, 4].Value = living.Eviction;
                    worksheet.Cells[i, 5].Value = living.Type;
                    worksheet.Cells[i, 6].Value = Convert.ToInt32(living.Number);
                    worksheet.Cells[i, 7].Value = living.ValueOfGuests;
                    worksheet.Cells[i, 8].Value = living.Price;
                    i++;
                }
                //Сохраняем файл
                excelPackage.Save();
            }
            return RedirectToAction("Index", "Livings");
        }
        public decimal sum(int id)//id livings
        {
            decimal p = 0;
            var apartments = db.Apartments.Find(id);
            p = apartments.Price;
            var livings = db.Livings.Find(id);
            p = p * Convert.ToInt32((livings.Eviction.Date - livings.Settling.Date).TotalDays);
            //здесь приплюсовать доп услуги
            var AS = db.AdditionalServices.Where(a => a.LivingId == id).ToList();
            foreach (var a in AS)
            {
                p += a.Price;
            }
            return p;
        }

    }
}
