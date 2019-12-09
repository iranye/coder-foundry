﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }

        [StringLength(120, ErrorMessage = "Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public string OwnerId { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
