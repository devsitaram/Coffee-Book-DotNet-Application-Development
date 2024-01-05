using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Models
{
    public class GlobalState
    {
        // The current user of the application
        public User CurrentUser { get; set; }
    }
}