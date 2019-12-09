﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IraNye.WebApi.Models
{
    public class Household
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
