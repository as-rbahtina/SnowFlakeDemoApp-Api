﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allied.SnowFlakeDemoApp.Domain.Configurations
{
    public class SnowFlakeDemoAppConfiguration
    {
        [Required]
        public string? AccountName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
