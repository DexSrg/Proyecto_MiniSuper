using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using EL;

namespace DAL
{
    public class DAL_UnidadesMedidas
    {
        public static int InsertarUnidadMedida(UnidadesMedidas Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("InsertarUnidadMedida", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Descripcion", Entidad.IdUnidadesMedida);
                Cmd.Parameters.AddWithValue("@IdUsuarioRegistro", Entidad.IdUsuarioRegistro);
                int ID = Convert.ToInt32(Cmd.ExecuteScalar());
                return ID;
            }
            catch
            {
                return 0;
            }

        }
        public static bool ActualizarUsuario(UnidadesMedidas Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("ActualizarUnidadMedida", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdUnidadMedida", Entidad.IdUnidadesMedida);
                Cmd.Parameters.AddWithValue("@Descripcion", Entidad.IdUnidadesMedida);
                Cmd.Parameters.AddWithValue("@IdUsuarioActualiza", Entidad.IdUsuarioRegistro);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool AnularUsuario(UnidadesMedidas Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("AnularUnidadMedida", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdUnidadMedida", Entidad.IdUnidadesMedida);
                Cmd.Parameters.AddWithValue("@IdUsuarioActualiza", Entidad.IdUsuarioRegistro);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static DataTable ListarUsuarios(bool Todos, int IdUnidadMedida)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("ListarUnidadMedidas", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Todos", Todos);
                Cmd.Parameters.AddWithValue("@IdUnidadMedida", IdUnidadMedida);
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
