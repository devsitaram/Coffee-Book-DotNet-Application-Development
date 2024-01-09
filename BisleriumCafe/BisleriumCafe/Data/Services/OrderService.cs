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

        public static string CreateNewOrder(string CoffeeName, double CoffeePrice, string AddFlavorName, double AddFlavorPrice, string CustomerNumber, double TotalPrice)
        {
            if (string.IsNullOrEmpty(CoffeeName))
            {
                return "Coffee is empty!";
            }

            if (CoffeePrice <= 0)
            {
                return "Invalid coffee price!";
            }

            if (string.IsNullOrEmpty(CustomerNumber))
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
                CustomerPhoneNumber = CustomerNumber,
                TotalPrice = TotalPrice,
            });

            SaveAllOrders(orders);
            return "success";
        }

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