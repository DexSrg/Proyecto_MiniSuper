using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Rol
    {
        public static DataTable ListarRoles(bool Todos = true,int IdRol = 0)
        {
            SqlConnection Conexion = new SqlConnection(Properties.Settings.Default.Conexion);
            Conexion.Open();
            SqlCommand Cmd = new SqlCommand("ListarRol",Conexion);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@Todos", Todos);
            Cmd.Parameters.AddWithValue("@IdRol", IdRol);
            SqlDataAdapter Da = new SqlDataAdapter(Cmd);
            DataTable dt = new DataTable();
            Da.Fill(dt);
            Conexion.Close();
            Conexion.Dispose();
            Da.Dispose();
            return dt;
        }
    }
}
