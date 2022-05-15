using Microsoft.Extensions.Configuration;
using PhysicalPerson.Configurations;
using PhysicalPerson.General;
using PhysicalPerson.Models;
using PhysicalPerson.Services;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Data
{
    public class Crud : ICrud, IDisposable
    {
        private SqlConnection _sql;
        private SqlCommand _command;
        private SqlDataAdapter _dataAdapter;
        private DataTable _dtResul;
        private readonly IManejoErrores _manejoErrores;
        public Crud(IManejoErrores manejoErrores)
        {
            _sql = new(ServiceExtensions.Configuracion.GetConnectionString("SQLConnection"));
            _dataAdapter = new();
            _dtResul = new();
            _manejoErrores = manejoErrores;
        }

        public string AddPerson(MoPhysicalPerson person)
        {
            string output = string.Empty;
            try
            {
                _sql.Open();
                _command = new("dbo.sp_AgregarPersonaFisica", _sql);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.Add("Nombre", SqlDbType.VarChar).Value = person.Nombre;
                _command.Parameters.Add("ApellidoPaterno", SqlDbType.VarChar).Value = person.ApellidoPaterno;
                _command.Parameters.Add("ApellidoMaterno", SqlDbType.VarChar).Value = person.ApellidoMaterno;
                _command.Parameters.Add("RFC", SqlDbType.VarChar).Value = person.RFC;
                _command.Parameters.Add("FechaNacimiento", SqlDbType.DateTime).Value = person.FechaNacimiento;
                _command.Parameters.Add("UsuarioAgrega", SqlDbType.Int).Value = person.UsuarioAgrega;
                _dataAdapter.SelectCommand = _command;
                _dataAdapter.Fill(_dtResul);                
                output = _dtResul.Rows[0][1].ToString();

            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message,person.UsuarioAgrega.ToString(), "AddPerson");
                throw new Exception();
            }
            finally
            {
                _sql.Close();
                _command.Dispose();
                _dataAdapter.Dispose();
                _dtResul.Clear();
                _dtResul.Reset();
                _dtResul.Dispose();
            }
            return output;
        }

        public string DeletePerson(MoPhysicalPerson person)
        {
            string output = "Ocurrio un error y no se pudo eliminar el registro correctamente";
            try
            {
                _sql.Open();
                _command = new("dbo.sp_EliminarPersonaFisica", _sql);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.Add("@IdPersonaFisica", SqlDbType.VarChar).Value = person.ID;
                _dataAdapter.SelectCommand = _command;
                _dataAdapter.Fill(_dtResul);
                output = _dtResul.Rows[0][1].ToString();

            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, person.UsuarioAgrega.ToString(), "DeletePerson");
            }
            finally
            {
                _sql.Close();
                _command.Dispose();
                _dataAdapter.Dispose();
                _dtResul.Clear();
                _dtResul.Reset();
                _dtResul.Dispose();
            }

            return output;
        }

        public List<MoPhysicalPerson> ListPerson()
        {
            List<MoPhysicalPerson> persons = new();
            string query = "SELECT IdPersonaFisica,FechaRegistro,FechaActualizacion,Nombre,ApellidoPaterno,ApellidoMaterno,RFC,FechaNacimiento,UsuarioAgrega,Activo FROM Tb_PersonasFisicas where Activo = 1;";
            try
            {
            _command = new(query, _sql);
            _command.CommandType = CommandType.Text;            
            _sql.Open();
            _dataAdapter.SelectCommand = _command;
            _dataAdapter.Fill(_dtResul);
            persons = (from DataRow row in _dtResul.Rows
                       select new MoPhysicalPerson
                       {
                           ID = Convert.ToInt32(row["IdPersonaFisica"].ToString()),
                           Activo = Convert.ToBoolean(row["Activo"].ToString()),
                           ApellidoMaterno = row["ApellidoPaterno"].ToString(),
                           ApellidoPaterno = row["ApellidoMaterno"].ToString(),
                           FechaActualizacion = row["FechaActualizacion"].ToString() == "" ? null : DateTime.Parse(row["FechaActualizacion"].ToString()),
                           FechaNacimiento = row["FechaNacimiento"].ToString() == "" ? null : DateTime.Parse(row["FechaNacimiento"].ToString()),
                           FechaRegistro = row["FechaRegistro"].ToString() == "" ? null : DateTime.Parse(row["FechaRegistro"].ToString()),
                           Nombre = row["Nombre"].ToString(),
                           RFC = row["RFC"].ToString(),
                           UsuarioAgrega = Convert.ToInt32(row["UsuarioAgrega"].ToString())
                       }).Where(e=> e.Activo == true).OrderBy(e=> e.ID).ToList();
            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, null, "ListPerson");
            }
            finally
            {
                _sql.Close();
                _command.Dispose();
                _dataAdapter.Dispose();
                _dtResul.Clear();
                _dtResul.Reset();
                _dtResul.Dispose();
            }
            return persons;
        }

        public string UpdatePerson(MoPhysicalPerson person)
        {
            string output = "Ocurrio un error al actualizar el registro";
            try
            {
                _sql.Open();
                _command = new("dbo.sp_ActualizarPersonaFisica", _sql);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.Add("IdPersonaFisica", SqlDbType.Int).Value = person.ID;
                _command.Parameters.Add("Nombre", SqlDbType.VarChar).Value = person.Nombre;
                _command.Parameters.Add("ApellidoPaterno", SqlDbType.VarChar).Value = person.ApellidoPaterno;
                _command.Parameters.Add("ApellidoMaterno", SqlDbType.VarChar).Value = person.ApellidoMaterno;
                _command.Parameters.Add("RFC", SqlDbType.VarChar).Value = person.RFC;
                _command.Parameters.Add("FechaNacimiento", SqlDbType.DateTime).Value = person.FechaNacimiento;
                _command.Parameters.Add("UsuarioAgrega", SqlDbType.Int).Value = person.UsuarioAgrega;
                _dataAdapter.SelectCommand = _command;
                _dataAdapter.Fill(_dtResul);
                output = _dtResul.Rows[0][1].ToString();

            }
            catch (Exception ex)
            {
                _manejoErrores.Incident(ex.Message, person.UsuarioAgrega.ToString(), "UpdatePerson");
            }
            finally
            {
                _sql.Close();
                _command.Dispose();
                _dataAdapter.Dispose();
                _dtResul.Clear();
                _dtResul.Reset();
                _dtResul.Dispose();
            }
            return output;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void DocExcel()
        {
            throw new NotImplementedException();
        }
    }
}
