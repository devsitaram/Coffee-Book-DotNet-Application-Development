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

        public static void SaveAllOrders(List<CoffeeOrder> orders)
        {
            string orderFilePath = Utils.GetOrderFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
                Directory.CreateDirectory(appDirectoryFilePath);

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(orderFilePath, json);
        }

        public static List<CoffeeOrder> CreateOrder()
        {
            List<CoffeeOrder> orders = GetAllOrders();

            double price = 0;//Calculate price from the list of coffee and add-ins

            //Fix the model class before adding.
            //orders.Add(
            //    new CoffeeOrder
            //    {
            //        AddIns = ,
            //        CoffeeList = ,
            //        Price = price
            //    });

            SaveAllOrders(orders);
            return orders;
        }

        private List<CoffeesOrder> orders = new List<CoffeesOrder>();

        public async Task PlaceOrder(CoffeesOrder order)
        {
            // Simulate asynchronous database operation to save the order
            await Task.Delay(100); // Simulating delay for database interaction
            orders.Add(order);
        }

        //public List<CoffeesOrder> GetAllOrders()
        //{
        //    return orders;
        //}
    }
}
