﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BisleriumCafe.Data.Enums;
using BisleriumCafe.Data.Services;

namespace BisleriumCafe.Data.Models
{
    // user model 
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public bool HasInitialPassword { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}