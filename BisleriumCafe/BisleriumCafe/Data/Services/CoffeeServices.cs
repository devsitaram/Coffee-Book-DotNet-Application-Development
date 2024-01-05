using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services
{
    public class CoffeeServices
    {
        public static List<Coffee> CreateCoffee(string coffeeName, double coffeePrice)
        {
            List<Coffee> listOfCoffee = GetAllCoffee();
            bool coffeeExists = listOfCoffee.Any(x => x.coffeeName == coffeeName);

            if (coffeeExists)
            {
                throw new Exception("Coffee already exists.");
            }

            listOfCoffee.Add(
                new Coffee
                {
                    coffeeName = coffeeName,
                    coffeePrice = coffeePrice
                }
            );
            SaveAllCoffee(listOfCoffee);
            return listOfCoffee;
        }

        private static void SaveAllCoffee(List<Coffee> coffee)
        {
            string coffeeFilePath = Utils.GetCoffeeFilePath();
            string appCoffeeFilePath = Utils.GetAppDirectoryPath();

            try
            {
                if (!Directory.Exists(appCoffeeFilePath))
                {
                    Directory.CreateDirectory(appCoffeeFilePath);
                }

                var json = JsonSerializer.Serialize(coffee);

                // Write the JSON content to the file
                using (StreamWriter streamWriter = File.CreateText(coffeeFilePath))
                {
                    streamWriter.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving coffee data: {ex.Message}");
                // Handle the exception as needed (log, rethrow, etc.)
            }
        }

        public static List<Coffee> GetAllCoffee()
        {
            string coffeeFilePath = Utils.GetCoffeeFilePath();
            if (!File.Exists(coffeeFilePath))
            {
                return new List<Coffee>();
            }

            var json = File.ReadAllText(coffeeFilePath);
            return JsonSerializer.Deserialize<List<Coffee>>(json);
        }

        public static void SeedCoffee()
        {
            CreateCoffee("Espresso", 2.99);
            CreateCoffee("Latte", 3.49);
            CreateCoffee("Cappuccino", 3.99);
        }

        //public static User GetByName(string coffeeName)
        //{
        //    List<User> listOfCoffee = GetAllCoffee();
        //    return listOfCoffee.FirstOrDefault(x => x.coffeeName == coffeeName);
        //}

        //public static List<User> DeleteCoffee(string coffeeName)
        //{
        //    List<User> listOfCoffee = GetAllCoffee();
        //    Coffee coffee = listOfCoffee.FirstOrDefault(x => x.coffeeName == coffeeName);

        //    if (coffeeName == null)
        //    {
        //        throw new Exception("coffee not available.");
        //    }

        //    //TodosService.DeleteKByUserId(id);
        //    coffee.Remove(user);
        //    SaveAll(coffee);
        //    return coffee;
        //}

    }
}
