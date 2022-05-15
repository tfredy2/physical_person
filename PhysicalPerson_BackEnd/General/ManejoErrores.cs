using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PhysicalPerson.Models;
using PhysicalPerson.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalPerson.General
{
    public class ManejoErrores : IManejoErrores
    {
        private IPAddress[] _host;
        public ManejoErrores()
        {
            _host = Dns.GetHostAddresses(Environment.MachineName);
        }

        public void InsertLog(MoErrores oError)
        {
            string Getpath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            string path = Getpath + "\\incidents.txt";
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Mensaje de error: " + oError.MsjError);
                        sw.WriteLine("Fecha: " + oError.FechaError);
                        sw.WriteLine("Servidor: " + oError.Servidor);
                        sw.WriteLine("Usuario: " + oError.Usuario);
                        sw.WriteLine("Usuario: " + oError.IpClient + "\n");
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("Mensaje de error: " + oError.MsjError);
                        sw.WriteLine("Fecha: " + oError.FechaError);
                        sw.WriteLine("Servidor: " + oError.Servidor);
                        sw.WriteLine("Usuario: " + oError.Usuario);
                        sw.WriteLine("Usuario: " + oError.IpClient + "\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Incident(ex.InnerException.Message, null, "InsertLog");
            }
        }

        public void Incident(string Error, string Usuario, string metod)
        {
            try
            {
                MoErrores oError = new MoErrores
                {
                    FechaError = DateTime.Now,
                    Servidor = Dns.GetHostName() + "\t" + _host[_host.Length / 2 - 1].ToString() + "\t" + _host[_host.Length - 1].ToString(),
                    MsjError = Error,
                    Usuario = Usuario,
                    Sistema = metod
                };
                InsertLog(oError);

            }
            catch (Exception ex)
            {
                Incident(ex.InnerException.Message,null,"Incident");
            }
        }
    }
}
