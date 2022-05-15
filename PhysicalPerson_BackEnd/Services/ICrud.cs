using PhysicalPerson.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Services
{
    public interface ICrud
    {
        public string AddPerson(MoPhysicalPerson person);
        public string UpdatePerson(MoPhysicalPerson person);
        public string DeletePerson(MoPhysicalPerson person);
        public List<MoPhysicalPerson> ListPerson();
    }
}
