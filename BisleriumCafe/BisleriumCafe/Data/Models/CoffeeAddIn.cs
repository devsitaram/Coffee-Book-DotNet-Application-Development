using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Models
{
    // add-ins model
    public class CoffeeAddIn
    {
        public Guid Id { get; set; } = new Guid();
        public string AddName { get; set; }
        public double AddPrice { get; set; }
    }
}
