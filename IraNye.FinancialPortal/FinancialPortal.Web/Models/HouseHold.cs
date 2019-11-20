﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class HouseHold
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Greeting { get; set; }

        [Required]
        public DateTime Created { get; set; }
    }
}
