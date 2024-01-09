using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Models
{
	public class Customer
	{
		public required string CustomerNumber { get; set; }
		public int Frequency { get; set; } = 1;
	}
}
