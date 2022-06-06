using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARM_Hotel.Controllers
{
    public class GetLivStatsController : Controller
    {
        // GET: GetLivStats
        public ActionResult Index()
        {
            
            //Создание нового объекта ExcelPackage - создание нового файла
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Установка свойств документа (сервисная функция - используется для "красоты")
                excelPackage.Workbook.Properties.Author = "Буров А.М.";
                excelPackage.Workbook.Properties.Title = "Первый Excel файл, созданный программно";
                excelPackage.Workbook.Properties.Subject = "Тестовый документ";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //Создание рабочего листа в книге
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                //Занесение информации в ячейку A3
                worksheet.Cells["A3"].Value = "Какие-то данные";
                //Для внесения данных - для адресации ячеек можно использовать формат [line, column]:
                worksheet.Cells[4, 1].Value = "Сново какие-то данные!";
                //Сохраняем файл
                FileInfo fil = new FileInfo(@"E:\Саня\Лаб\ARM_Hotel\Content\Reports");
                excelPackage.SaveAs(fil);
            }
            //Открытие существующего файла
            FileInfo fi = new FileInfo(@"E:\Саня\Лаб\ARM_Hotel\Content\Reports\LivigsReport.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //Получение листа книги по индексу: ВНИМАНИЕ - нумерация листов начинается 1, не с 0!!!
                ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[1];
                //Получение листа по имени. Если лист с таким именем не существует - будет сгенерировано исключение
                ExcelWorksheet namedWorksheet =
               excelPackage.Workbook.Worksheets["SomeWorksheet"];
                //получение листа также возможно с импользованием LINQ
                ExcelWorksheet anotherWorksheet =
               excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "SomeWorksheet");
                //Получение данных из ячеек листа. Также возможно два варианта: по имени
                //области(А3) и по номеру строки и столбца
                string valA1 = firstWorksheet.Cells["A3"].Value.ToString();
                string valA4 = firstWorksheet.Cells[4, 1].Value.ToString();
                //Сохраняем файл - здесь уже не указываем имя файла
                excelPackage.Save();
            }
            return View();

        }


    }
}