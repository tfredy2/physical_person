using Microsoft.AspNetCore.Http;
using PhysicalPerson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PhysicalPerson.Services
{
    public interface IManejoErrores
    {
        void InsertLog(MoErrores oError);
        void Incident(string Error, string User,string metod);
    }
}
