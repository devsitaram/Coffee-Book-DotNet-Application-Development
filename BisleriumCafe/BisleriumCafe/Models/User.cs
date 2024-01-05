using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BisleriumCafe.Enums;

namespace BisleriumCafe.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
