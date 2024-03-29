﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Models
{
    public class MoUser
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Passwd { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
