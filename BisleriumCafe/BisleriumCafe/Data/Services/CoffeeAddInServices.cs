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
        // get all the add-in flavors from file directory
        public static List<CoffeeAddIn> GetAllAddIn()
        {
            try
            {
                string addInFilePath = Utils.GetAddInFilePath();
                if (File.Exists(addInFilePath))
                {
                    var json = File.ReadAllText(addInFilePath);
                    return JsonSerializer.Deserialize<List<CoffeeAddIn>>(json);
                }

                return new List<CoffeeAddIn>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<CoffeeAddIn>();
            }
        }

        // insert or add the new add in flavor
        public static string CoffeeAddInFlavor(string name, double price)
        {
            try
            {
                List<CoffeeAddIn> addIns = GetAllAddIn();
                bool addInExists = addIns.Any(x => x.AddName == name);

                if (addInExists)
                {
                    return "Add in flavor is already exists.";
                }
                else
                {
                    addIns.Add(
                    new CoffeeAddIn
                    {
                        AddName = name,
                        AddPrice = price
                    });
                    SaveAllAddIn(addIns);
                    return "success";
                }
            } catch(Exception ex)
            {
                return $"Enter the valid data {ex}";
            }
        }

        // update the add in flavor
        public static string UpdateAddInFlavor(string addName, double addPrice)
        {
            try
            {
                List<CoffeeAddIn> listOfAddIn = GetAllAddIn();
                CoffeeAddIn existingAddInCoffee = listOfAddIn.FirstOrDefault(x => x.AddName == addName);

                if (existingAddInCoffee != null)
                {
                    // Coffee flavor exists, update its name and price
                    existingAddInCoffee.AddPrice = addPrice;
                    SaveAllAddIn(listOfAddIn); 
                    return "Add-In coffee flavor updated successfully.";
                }
                else
                {
                    return "The flavor does not exist.";
                }
            }
            catch (Exception ex)
            {
                return $"Enter valid data: {ex.Message}"; 
            }
        }

        // save the app path directory
        public static void SaveAllAddIn(List<CoffeeAddIn> addIns)
        {
            try
            {
                string addInFilePath = Utils.GetAddInFilePath();
                string appDirectoryFilePath = Utils.GetAppDirectoryPath();

                if (!Directory.Exists(appDirectoryFilePath))
                {
                    Directory.CreateDirectory(appDirectoryFilePath);
                }

                var json = JsonSerializer.Serialize(addIns);

                // Write the JSON content to the file
                using (StreamWriter streamWriter = File.CreateText(addInFilePath))
                {
                    streamWriter.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving coffee data: {ex.Message}");
            }
        }

        // delete the add in coffee flavor
        public static List<CoffeeAddIn> DeleteAddIn(string addName)
        {
            try
            {
                List<CoffeeAddIn> listOfAddIn = GetAllAddIn();
                CoffeeAddIn coffee = listOfAddIn.FirstOrDefault(x => x.AddName == addName);

                if (coffee == null)
                {
                    throw new Exception("Coffee not available.");
                }

                listOfAddIn.Remove(coffee); // Remove the coffee from the list
                SaveAllAddIn(listOfAddIn); // Save the updated list of coffees
                return listOfAddIn; // Return the updated list of coffees
            } 
            catch (Exception ex)
            {
                Console.WriteLine(Console.Error);
                return new List<CoffeeAddIn>();
            }
            
        }

        public static void SeedAddIns()
        {
            var addins = GetAllAddIn();
            if (addins == null)
            {
                CoffeeAddInFlavor("Select a Add-ins flavor", 0);
            }
        }
    }
}