using BisleriumCafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Services
{
    public class OrderService
    {
        public static List<Order> GetAllOrders()
        {
            string orderFilePath = Utils.GetOrderFilePath();
            if (File.Exists(orderFilePath))
            {
                var json = File.ReadAllText(orderFilePath);
                return JsonSerializer.Deserialize<List<Order>>(json);
            }

            return new List<Order>();
        }

        public static void SaveAllOrders(List<Order> orders)
        {
            string orderFilePath = Utils.GetOrderFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
                Directory.CreateDirectory(appDirectoryFilePath);

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(orderFilePath, json);
        }

        public static List<Order> CreateOrder()
        {
            List<Order> orders = GetAllOrders();

            double price = 0;//Calculate price from the list of coffee and add-ins

            //Fix the model class before adding.
            //orders.Add(
            //    new Order
            //    {
                    
            //        AddIns = ,
            //        CoffeeList = ,
            //        Price = price
            //    });

            SaveAllOrders(orders);
            return orders;
        }
    }
}
