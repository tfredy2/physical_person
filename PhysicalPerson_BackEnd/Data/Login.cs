using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhysicalPerson.Data.Context;
using PhysicalPerson.General;
using PhysicalPerson.Models;
using PhysicalPerson.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalPerson.Data
{
    public class Login : ILogin
    {
        private readonly IManejoErrores _manejoErrores;
        private readonly AppSettingsToken _appSettingsToken;

        public Login(IManejoErrores manejoErrores,IOptions<AppSettingsToken> appSettings)
        {
            _manejoErrores = manejoErrores;
            _appSettingsToken = appSettings.Value;
        }

        public async Task<string> CreateUser(MoUser oUser)
        {
            bool exists = false;
            string msj = "El usuario ya existe";
            try
            {
                using (TokaContext context = new())
                {
                    exists =  await context.Users.AnyAsync(e => e.Email == oUser.Email);
                    if (!exists)
                    {
                        oUser.Passwd = EncripPassd(oUser.Passwd);
                        await context.AddAsync(oUser);
                        await context.SaveChangesAsync();
                        msj = "El usuario fue creado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                msj = "Ocurrio un error al crear su usuario";
                _manejoErrores.Incident(ex.Message,oUser.Email, "CreateUser");
            }
            return msj;
        }

        private string EncripPassd(string passwd)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(passwd));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public string RefreshToken(string sToken)
        {
            throw new NotImplementedException();
        }

        public List<MoUser> olsUser()
        {
            List<MoUser> Users = new();
            using (TokaContext toka = new())
            {
                Users = toka.Users.ToList();
            }
            return Users;
        }

        public async Task<MoLoginResponse> DoLogin(MoUser oUser)
        {
            MoLoginResponse response = new();
            MoUser user = new();
            try
            {
                using (TokaContext toka = new())
                {
                    user =  await toka.Users.Where(u=>u.Email ==oUser.Email && u.Passwd == EncripPassd(oUser.Passwd)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        response.Email = user.Email;
                        response.ID = user.Id;
                        response.Name = user.Name;
                        response.UserToken = CreateToken(user);
                        response.Duration = DateTime.UtcNow.AddHours(5); 
                    }
                }
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message,oUser.Id.ToString(), "DoLogin");
            }
            return response;
        }

        private string CreateToken(MoUser model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettingsToken.KeyToken);
            string sToken = string.Empty;
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Name, model.Name)
                    }),
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var secToken = tokenHandler.CreateToken(tokenDescriptor);
                sToken = tokenHandler.WriteToken(secToken);
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, model.Id.ToString(), "CreateToken");
            }
            return sToken;
        }
    }
}
