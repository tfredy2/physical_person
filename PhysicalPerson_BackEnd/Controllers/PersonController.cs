using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PhysicalPerson.Models;
using PhysicalPerson.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhysicalPerson.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly IManejoErrores _manejoErrores;


        public PersonController(ICrud crud,IManejoErrores errores)
        {
            _crud = crud;
            _manejoErrores = errores;
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] MoPhysicalPerson physicalPerson)
        {
            JObject Result = new();
            try
            {
                Result["Mensaje"] = JToken.FromObject(_crud.AddPerson(physicalPerson));
                Result["oLsPerson"] = JToken.FromObject(_crud.ListPerson());
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message,physicalPerson.UsuarioAgrega.ToString(), "AddPerson");
            }
            return Content(Result.ToString());
        }

        [HttpGet]
        public IActionResult ListPerson()
        {
            JObject Result = new();
            try
            {
                Result["oLsPerson"] = JToken.FromObject(_crud.ListPerson());
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, null, "ListPerson");
            }
            return Content(Result.ToString());
        }

        [HttpPut]
        public IActionResult UpdatePerson([FromBody] MoPhysicalPerson physicalPerson)
        {
            JObject Result = new();
            try
            {
                Result["Mensaje"] = JToken.FromObject(_crud.UpdatePerson(physicalPerson));
                Result["oLsPerson"] = JToken.FromObject(_crud.ListPerson());
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, physicalPerson.UsuarioAgrega.ToString(), "UpdatePerson");
            }
            return Content(Result.ToString());
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePerson(int id)
        {
            JObject Result = new();
            MoPhysicalPerson physicalPerson = new MoPhysicalPerson{ ID=id };
            try
            {
                Result["Mensaje"] = JToken.FromObject(_crud.DeletePerson(physicalPerson));
                Result["oLsPerson"] = JToken.FromObject(_crud.ListPerson());
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, physicalPerson.UsuarioAgrega.ToString(), "DeletePerson");
            }
            return Content(Result.ToString());
        }

    }
}
