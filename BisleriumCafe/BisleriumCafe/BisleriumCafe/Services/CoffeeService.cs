using BisleriumCafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Services
{
    public class CoffeeService
    {
        public static List<Coffee> GetAllCoffee()
        {
            string coffeeFilePath = Utils.GetCoffeeFilePath();
            if (File.Exists(coffeeFilePath))
            {
                var json = File.ReadAllText(coffeeFilePath);
                return JsonSerializer.Deserialize<List<Coffee>>(json);
            }

            return new List<Coffee>();
        }

        public static void SaveAllCoffee(List<Coffee> coffeeList)
        {
            string coffeeFilePath = Utils.GetCoffeeFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
                Directory.CreateDirectory(appDirectoryFilePath);

            var json = JsonSerializer.Serialize(coffeeList);
            File.WriteAllText(coffeeFilePath, json);
        }

        public static List<Coffee> CreateCoffee(string name, double price)
        {
            List<Coffee> coffeeList = GetAllCoffee();
            bool coffeeExists = coffeeList.Any(x => x.Name == name);

            if (coffeeExists)
                throw new Exception("Username already exists.");
            
            coffeeList.Add(
                new Coffee
                {
                    Name = name,
                    Price = price
                });
            SaveAllCoffee(coffeeList);
            return coffeeList;
        }

        public static void UpdateCoffee(string name, double price)
        {
            List<Coffee> coffeeList = GetAllCoffee();
            Coffee coffeeToUpdate = coffeeList.FirstOrDefault(x => x.Name == name);

            if (coffeeToUpdate == null)
            {
                throw new Exception("Coffee not found.");
            }

            coffeeToUpdate.Name = name;
            coffeeToUpdate.Price = price;
            SaveAllCoffee(coffeeList);
        }

        public static void SeedCoffee()
        {
            var coffeeList = GetAllCoffee();
            if (coffeeList == null)
            {
                CreateCoffee("Espresso", 80.00);
                CreateCoffee("Latte", 100.00);
                CreateCoffee("Americano", 120.00);
            }
        }
    }
}
