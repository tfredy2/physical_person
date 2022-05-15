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
    public class LoginController : ControllerBase
    {
        private ILogin _login;
        private string sUser = "";

        public LoginController(ILogin login)
        {
            _login = login;
        }
        
        [HttpPost]
        public async Task<IActionResult> DoLogin([FromBody] MoUser oUser)
        {
            JObject Result = new();
            Result["PersonInfo"] = JToken.FromObject(await _login.DoLogin(oUser));
            return Content(Result.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] MoUser oUser)
        {
            JObject Result = new();
            
            Result["sResponse"] = JToken.FromObject(await _login.CreateUser(oUser));            
            return Content(Result.ToString());
        }
    }
}
