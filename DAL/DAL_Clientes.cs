using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EL;

namespace DAL
{
    public class DAL_Clientes
    {
        public static int InsertarCliente(Clientes Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("InsertarCliente", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@NombreCompleto", Entidad.NombreCompleto);
                Cmd.Parameters.AddWithValue("@Identificacion", Entidad.Identificacion);
                Cmd.Parameters.AddWithValue("@Celular", Entidad.Celular);
                Cmd.Parameters.AddWithValue("@Correo", Entidad.Correo);
                Cmd.Parameters.AddWithValue("@IdUsuarioRegistro", Entidad.IdUsuarioRegistro);
                int ID = Convert.ToInt32(Cmd.ExecuteScalar());
                return ID;
            }
            catch
            {
                return 0;
            }

        }
        public static bool ActualizarCliente(Clientes Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("ActualizarCliente", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdCliente", Entidad.IdCliente);
                Cmd.Parameters.AddWithValue("@NombreCompleto", Entidad.NombreCompleto);
                Cmd.Parameters.AddWithValue("@Identificacion", Entidad.Identificacion);
                Cmd.Parameters.AddWithValue("@Celular", Entidad.Celular);
                Cmd.Parameters.AddWithValue("@Correo", Entidad.Correo);               
                Cmd.Parameters.AddWithValue("@IdUsuarioActualizar", Entidad.IdUsuarioActualiza);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool AnularCliente(Clientes Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("AnularCliente", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdCliente", Entidad.IdCliente);
                Cmd.Parameters.AddWithValue("@IdUsuarioActualizar", Entidad.IdUsuarioActualiza);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static DataTable ListarClientes(bool Todos, int IdCliente)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("ListarClientes", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Todos", Todos);
                Cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                Da.Fill(dt);
                Conexion.Close();
                Conexion.Dispose();
                Da.Dispose();
                return dt;
            }
            catch
            {
                return dt;
            }

        }
    }
}
