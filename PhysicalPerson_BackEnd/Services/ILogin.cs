using PhysicalPerson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Services
{
    public interface ILogin
    {
        Task<string> CreateUser(MoUser oUser);
        Task<MoLoginResponse> DoLogin(MoUser oUser);
        string RefreshToken(string sToken);
        List<MoUser> olsUser();
    }
}
