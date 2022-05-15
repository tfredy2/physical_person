using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Models
{
    public class MoErrores
    {
        public string MsjError { get; set; }
        public DateTime FechaError { get; set; }
        public string Servidor { get; set; }
        public string Sistema { get; set; }
        public string Usuario { get; set; }
        public string IpClient { get; set; }
    }
}
