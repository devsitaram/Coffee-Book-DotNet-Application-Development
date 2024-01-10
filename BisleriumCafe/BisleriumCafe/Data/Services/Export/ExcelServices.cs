using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;
using OfficeOpenXml;

namespace BisleriumCafe.Data.Services.Export
{
    public class ExcelServices
    {
        public static string ExcelFileGenerate()
        {
            var filePath = Utils.GetOrderFilePath();
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (json.Trim().Length > 0)
                {
                    var deserializedData = JsonSerializer.Deserialize<List<CoffeeOrder>>(json);
                    if (deserializedData != null)
                    {
                        var excelFilePath = Path.Combine(Utils.GetAppDirectoryPath(), "sale_transactions.xlsx");

                        using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Orders");

                            // Headers
                            string[] headers = { "Coffee Name", "Coffee Price", "Add In Flavor", "Add In Price", "Customer Number", "Total Price", "Order Date" };
                            for (int i = 0; i < headers.Length; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = headers[i];
                            }

                            // Data
                            int row = 2;
                            foreach (var item in deserializedData)
                            {
                                worksheet.Cells[row, 1].Value = item.CoffeeName;
                                worksheet.Cells[row, 2].Value = item.CoffeePrice;
                                worksheet.Cells[row, 3].Value = item.AddFlavorName;
                                worksheet.Cells[row, 4].Value = item.AddFlavorPrice;
                                worksheet.Cells[row, 5].Value = item.CustomerNumber;
                                worksheet.Cells[row, 6].Value = item.TotalPrice;
                                worksheet.Cells[row, 7].Value = item.OrderDate;

                                row++;
                            }
                            package.Save();
                            return filePath;
                        }
                    }
                }
            }
            return null;
        }
    }
}