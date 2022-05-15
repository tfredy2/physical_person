using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Models
{
    public class MoPhysicalPerson
    {
        [Required]
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string Nombre { get; set; }
        [Required, MaxLength(50)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(50)]
        public string ApellidoMaterno { get; set; }
        [Required, MaxLength(13)]
        public string RFC { get; set; }
        [Required]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        public int UsuarioAgrega { get; set; }
        [Required]
        public bool Activo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
