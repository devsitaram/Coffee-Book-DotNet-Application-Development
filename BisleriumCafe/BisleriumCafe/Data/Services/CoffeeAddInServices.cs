using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services
{
    internal class CoffeeAddInServices
    {
        public static List<CoffeeAddIn> GetAllAddIn()
        {
            string addInFilePath = Utils.GetAddInFilePath();
            if (File.Exists(addInFilePath))
            {
                var json = File.ReadAllText(addInFilePath);
                return JsonSerializer.Deserialize<List<CoffeeAddIn>>(json);
            }

            return new List<CoffeeAddIn>();
        }

        public static void SaveAllAddIn(List<CoffeeAddIn> addIns)
        {
            string addInFilePath = Utils.GetAddInFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
            {
                Directory.CreateDirectory(appDirectoryFilePath);
            }

            var json = JsonSerializer.Serialize(addIns);
            File.WriteAllText(addInFilePath, json);
        }

        public static List<CoffeeAddIn> CreateAddIn(string name, double price)
        {
            List<CoffeeAddIn> addIns = GetAllAddIn();
            bool addInExists = addIns.Any(x => x.AddName == name);

            if (addInExists)
            {
                throw new Exception("Username already exists.");
            }

            addIns.Add(
                new CoffeeAddIn
                {
                    AddName = name,
                    AddPrice = price
                });
            SaveAllAddIn(addIns);
            return addIns;
        }

        public static void SeedAddIns()
        {
            var coffeeList = GetAllAddIn();
            if (coffeeList == null)
            {
                CreateAddIn("Cinnamon", 25.00);
                CreateAddIn("Honey", 30.00);
                CreateAddIn("Ginger", 45.00);
                CreateAddIn("Chocolate", 20.00);
                CreateAddIn("Ice Cream", 35.00);
            }
        }


        // flavor add 
        //public Task<List<CoffeeAddIn>> GetAllAddIns()
        //{
        //    new CoffeeAddIn { AddName = "Cinnamon", AddPrice = 10 },
        //    new CoffeeAddIn { AddName = "Honey", AddPrice = 20 },
        //    new CoffeeAddIn { AddName = "Ginger", AddPrice = 15 },
        //    new CoffeeAddIn { AddName = "Chocolate", AddPrice = 25 },
        //    new CoffeeAddIn { AddName = "Ice Cream", AddPrice = 5 }
        //    // Add more add-ins as needed
        //    return new List<CoffeeAddIn>
        //}
    }
}