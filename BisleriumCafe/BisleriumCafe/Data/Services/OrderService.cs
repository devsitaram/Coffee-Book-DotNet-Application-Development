using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services
{
    public class OrderService
    {
        // get all the orders
        public static List<CoffeeOrder> GetAllOrders()
        {
            string orderFilePath = Utils.GetOrderFilePath();
            if (File.Exists(orderFilePath))
            {
                var json = File.ReadAllText(orderFilePath);
                return JsonSerializer.Deserialize<List<CoffeeOrder>>(json);
            }
            return new List<CoffeeOrder>();
        }

        // create the new order
        public static string CreateNewOrder(string CoffeeName, double CoffeePrice, string AddFlavorName, double AddFlavorPrice, long CustomerNumber, double TotalPrice)
        {
            if (string.IsNullOrEmpty(CoffeeName))
            {
                return "Coffee is empty!";
            }

            if (CoffeePrice <= 0)
            {
                return "Invalid coffee price!";
            }

            if (CustomerNumber == 0)
            {
                return $"The customer number is empty!";
            }
            else
            {
                if (!CustomerService.UpdateCustomer(CustomerNumber))
                {
					CustomerService.CreateCustomer(CustomerNumber);
				}
			}

            List<CoffeeOrder> orders = GetAllOrders();

            orders.Add(new CoffeeOrder
            {
                CoffeeName = CoffeeName,
                CoffeePrice = CoffeePrice,
                AddFlavorName = AddFlavorName,
                AddFlavorPrice = AddFlavorPrice,
                CustomerNumber = CustomerNumber,
                TotalPrice = TotalPrice,
            });

            SaveAllOrders(orders);
            return "success";
        }

        // save the order in order pathss
        public static void SaveAllOrders(List<CoffeeOrder> orders)
        {
            string orderFilePath = Utils.GetOrderFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
                Directory.CreateDirectory(appDirectoryFilePath);

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(orderFilePath, json);
        }
	}
}