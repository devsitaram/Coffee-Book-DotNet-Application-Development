﻿using System;
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
                    // Coffee with the same name already exists, update its price
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
            //CreateCoffee("Espresso", 2.99);
            //CreateCoffee("Latte", 3.49);
            //CreateCoffee("Cappuccino", 3.99);
        }

        public static Coffee GetByName(string coffeeName)
        {
            List<Coffee> listOfCoffee = GetAllCoffee();
            return listOfCoffee.FirstOrDefault(x => x.CoffeeName == coffeeName);
        }

        public static List<Coffee> DeleteCoffee(string coffeeName)
        {
            List<Coffee> listOfCoffee = GetAllCoffee();
            Coffee coffee = listOfCoffee.FirstOrDefault(x => x.CoffeeName == coffeeName);

            if (coffee == null)
            {
                throw new Exception("Coffee not available.");
            }

            listOfCoffee.Remove(coffee); // Remove the coffee from the list
            SaveAllCoffee(listOfCoffee); // Save the updated list of coffees
            return listOfCoffee; // Return the updated list of coffees
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
            }
        }

    }
}