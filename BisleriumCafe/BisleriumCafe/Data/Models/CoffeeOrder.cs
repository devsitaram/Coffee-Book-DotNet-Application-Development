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
        public string CoffeeName { get; set; }
        public double CoffeePrice { get; set; }
        public string AddFlavorName { get; set; }
        public double AddFlavorPrice { get; set; }
        public long CustomerNumber { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}