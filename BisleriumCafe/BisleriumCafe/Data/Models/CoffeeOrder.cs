using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Models
{
    public class CoffeeOrder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public required List<Dictionary<Coffee, int>> CoffeeList { get; set; }
        public required List<CoffeeAddIn?> AddIns { get; set; }
        public double Price { get; set; }
    }

    public class CoffeesOrder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CoffeeType { get; set; }
        public List<string> AddIns { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string PhoneNumber { get; set; }
        public double TotalPrice { get; set; }
    }
}