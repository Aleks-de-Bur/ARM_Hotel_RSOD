using ARM_Hotel.Models;
using OfficeOpenXml;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace ARM_Hotel.Classes
{
    public class ReportSender : IJob
    {
        private HotelDBEntities db = new HotelDBEntities();
        string file_path_template;
        private void PrepareReport()
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
        }
        public async Task Execute(IJobExecutionContext context)
        {
            file_path_template = HostingEnvironment.MapPath("~/Content/Reports/BookingsReport.xlsx");
            try
            {
                if (File.Exists(file_path_template))
                    File.Delete(file_path_template);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            PrepareReport();
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("aleksdeburov@gmail.com", "Aleks");
            // кому отправляем
            MailAddress to = new MailAddress("Aleks-de-Bur@yandex.ru");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Отчёт о бронированиях за месяц";
            // текст письма
            m.Body = "<h2>Это письмо сгенерировано автоматически, не надо на него отвечать.</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("aleksdeburov@gmail.com",
           "Cfyz2012");
            smtp.EnableSsl = true;
            // вкладываем файл в письмо
            m.Attachments.Add(new Attachment(file_path_template));
            // отправляем асинхронно
            await smtp.SendMailAsync(m);
            m.Dispose();
        }
    }
}