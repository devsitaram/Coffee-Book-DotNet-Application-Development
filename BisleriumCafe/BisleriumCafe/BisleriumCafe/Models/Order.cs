using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; } = DateTime.Now;
        //public required List<Dictionary<Coffee,int>> CoffeeList { get; set; }
        //public required List<AddIn?> AddIns{ get; set; }
        public double Price { get; set; }
    }
}
