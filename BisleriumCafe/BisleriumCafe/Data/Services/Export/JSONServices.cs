using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services.Export
{
    public class JSONServices
    {
        public static string JSONFileGenerate(List<CoffeeOrder> orders)
        {
            try
            {
                string jsonFilePath = Utils.GetRevenueFilePath();
                string appDirectoryFilePath = Utils.GetAppDirectoryPath();

                if (!Directory.Exists(appDirectoryFilePath))
                {
                    Directory.CreateDirectory(appDirectoryFilePath);
                }
                var json = JsonSerializer.Serialize(orders);
                File.WriteAllText(jsonFilePath, json);
                return jsonFilePath;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
