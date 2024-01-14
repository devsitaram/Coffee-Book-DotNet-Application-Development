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
        public static string CreateCoffee(string coffeeName, double coffeePrice)
        {
            try
            {
                List<Coffee> listOfCoffee = GetAllCoffee();
                bool existingCoffee = listOfCoffee.Any(x => x.CoffeeName == coffeeName);

                if (existingCoffee)
                {
                    // Coffee with the same name already exists
                    return "Coffee is already exists.";
                }
                else
                {
                    // Coffee doesn't exist, add a new one
                    listOfCoffee.Add(new Coffee
                    {
                        CoffeeName = coffeeName,
                        CoffeePrice = coffeePrice
                    });
                    SaveAllCoffee(listOfCoffee);
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return $"Error in CreateOrUpdateCoffee: {ex.Message}"; 
            }
        }

        public static string UpdateCoffee(string coffeeName, double coffeePrice)
        {
            try
            {
                List<Coffee> listOfCoffee = GetAllCoffee();
                Coffee existingCoffee = listOfCoffee.FirstOrDefault(x => x.CoffeeName == coffeeName);

                if (existingCoffee != null)
                {
                    existingCoffee.CoffeePrice = coffeePrice;
                    SaveAllCoffee(listOfCoffee);
                    return "success";
                }
                else
                {
                    return "The coffee is not exits.";
                }
            }
            catch (Exception ex)
            {
                return $"Error in CreateOrUpdateCoffee: {ex.Message}";
            }
        }


        public static List<Coffee> GetAllCoffee()
        {
            try
            {
                string coffeeFilePath = Utils.GetCoffeeFilePath();
                if (!File.Exists(coffeeFilePath))
                {
                    return new List<Coffee>();
                }
                var json = File.ReadAllText(coffeeFilePath);
                return JsonSerializer.Deserialize<List<Coffee>>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Coffee>();
            }
            
        }

        public static Coffee CoffeeGetByName(string coffeeName)
        {
            List<Coffee> listOfCoffee = GetAllCoffee();
            return listOfCoffee.FirstOrDefault(x => x.CoffeeName == coffeeName);
        }

        // this function can be delete the coffee name
        public static List<Coffee> DeleteCoffee(string coffeeName)
        {
            try
            {
                List<Coffee> listOfCoffee = GetAllCoffee();
                Coffee coffee = listOfCoffee.FirstOrDefault(x => x.CoffeeName == coffeeName);

                if (coffee == null)
                {
                    throw new Exception("Coffee not available.");
                }

                listOfCoffee.Remove(coffee);
                SaveAllCoffee(listOfCoffee);
                return listOfCoffee;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Coffee>();
            }
        }

        // save the customer data in app file directory
        private static void SaveAllCoffee(List<Coffee> coffee)
        {
            try
            {
                string coffeeFilePath = Utils.GetCoffeeFilePath();
                string appCoffeeFilePath = Utils.GetAppDirectoryPath();

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
            }
        }

    }
}
