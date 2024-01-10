using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services.Export
{
    public class CSVServices
    {
        public static string CSVFileGenerate()
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
                        var csvOutputPath = Path.Combine(Utils.GetAppDirectoryPath(), "sale_transactions.csv");

                        using (var writer = new StreamWriter(csvOutputPath))
                        {
                            writer.WriteLine("Coffee Name, Coffee Price, Add In Flavor, Add In Price, Customer Number, Total Price, Order Date");

                            foreach (var item in deserializedData)
                            {
                                writer.WriteLine($"{Escape(item.CoffeeName)}, {item.CoffeePrice}, {Escape(item.AddFlavorName)}, {item.AddFlavorPrice}, {item.CustomerNumber}, {item.TotalPrice}, {item.OrderDate}");
                            }
                        }
                        return csvOutputPath;
                    }
                }
            }
            return null;
        }
        private static string Escape(string value)
        {
            return "\"" + value?.Replace("\"", "\"\"") + "\"";
        }
    }
}
