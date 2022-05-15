using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Models
{
    public class MoLoginResponse
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserToken { get; set; }
        public DateTime Duration { get; set; }
    }
}
