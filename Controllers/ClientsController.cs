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
using OfficeOpenXml;

namespace ARM_Hotel.Controllers
{
    public class ClientsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: Clients
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Clients/Details/5
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {

                if (db.Clients.Any(cl => cl.SeriaPas == client.SeriaPas))
                {
                    if (db.Clients.Any(cl => cl.NumberPas == client.NumberPas))
                    {
                        ModelState.AddModelError("SeriaPas", "Клиент с таким паспортом уже существует");
                        ModelState.AddModelError("NumberPas", "Клиент с таким паспортом уже существует");
                        return View();
                    }
                }


                db.Clients.Add(client);
                db.SaveChanges();

                if (User.IsInRole("Client"))
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Patronymic,Birthday,SeriaPas,NumberPas,TelNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [Authorize(Roles = "Admin, Operator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            if (!db.Livings.Any(cl => cl.ClientId == id) && !db.Bookings.Any(cl => cl.ClientId == id))
            {
                db.Clients.Remove(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                @ViewBag.Message = "Клиент имеет проживания и/или брони. Удаление невозможно.";
                return View(client);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ClientsStat()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //Открытие существующего файла
            FileInfo fi = new FileInfo(@"E:\Саня\Лаб\ARM_Hotel\Content\Reports\ClientsReport.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //Получение листа по имени. Если лист с таким именем не существует - будет сгенерировано исключение
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Лист1"];
                //Занесение информации
                List<Client> clients = db.Clients.ToList();
                int i = 3;
                foreach (var client in clients)
                {
                    worksheet.Cells[i, 1].Value = i - 2;
                    worksheet.Cells[i, 2].Value = client.LastName;
                    worksheet.Cells[i, 3].Value = client.FirstName;
                    worksheet.Cells[i, 4].Value = client.Patronymic;
                    worksheet.Cells[i, 5].Value = client.Birthday.Date.ToString();
                    worksheet.Cells[i, 6].Value = client.SeriaPas;
                    worksheet.Cells[i, 7].Value = client.NumberPas;
                    worksheet.Cells[i, 8].Value = client.TelNumber;
                    i++;
                }
                //Сохраняем файл
                excelPackage.Save();
            }
            return RedirectToAction("Index", "Clients");
        }
    }
}
