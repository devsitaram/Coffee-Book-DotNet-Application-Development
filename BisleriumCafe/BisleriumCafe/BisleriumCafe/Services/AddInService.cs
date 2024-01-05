using BisleriumCafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Services
{
    public class AddInService
    {
        public static List<AddIn> GetAllAddIn()
        {
            string addInFilePath = Utils.GetAddInFilePath();
            if (File.Exists(addInFilePath))
            {
                var json = File.ReadAllText(addInFilePath);
                return JsonSerializer.Deserialize<List<AddIn>>(json);
            }

            return new List<AddIn>();
        }

        public static void SaveAllAddIn(List<AddIn> addIns)
        {
            string addInFilePath = Utils.GetAddInFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
                Directory.CreateDirectory(appDirectoryFilePath);

            var json = JsonSerializer.Serialize(addIns);
            File.WriteAllText(addInFilePath, json);
        }

        public static List<AddIn> CreateAddIn(string name, double price)
        {
            List<AddIn> addIns = GetAllAddIn();
            bool addInExists = addIns.Any(x => x.Name == name);

            if (addInExists)
                throw new Exception("Username already exists.");

            addIns.Add(
                new AddIn
                {
                    Name = name,
                    Price = price
                });
            SaveAllAddIn(addIns);
            return addIns;
        }
        public static void SeedAddIns()
        {
            var coffeeList = GetAllAddIn();
            if (coffeeList == null)
            {
                CreateAddIn("Honey", 20.00);
                CreateAddIn("Cinnamon", 30.00);
                CreateAddIn("Mint", 50.00);
            }
        }
    }
}
