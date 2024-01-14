using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Models
{
	// customer model where customer number is required
	public class Customer
	{
		public required long CustomerNumber { get; set; }
		public int Frequency { get; set; } = 1;
	}
}
