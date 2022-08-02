using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using EL;

namespace DAL
{
    public class DAL_Usuarios
    {
        public static int InsertarUsuario(Usuarios Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("InsertarUsuario", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdRol", Entidad.IdRol);
                Cmd.Parameters.AddWithValue("@NombreCompleto", Entidad.NombreCompleto);
                Cmd.Parameters.AddWithValue("@Correo", Entidad.Correo);
                Cmd.Parameters.AddWithValue("@Cargo", Entidad.Cargo);
                Cmd.Parameters.AddWithValue("@Login", Entidad.Login);
                Cmd.Parameters.AddWithValue("@Password", Entidad.Password);
                Cmd.Parameters.AddWithValue("@IdUsuarioRegistro", Entidad.IdUsuarioRegistro);
                int ID = Convert.ToInt32(Cmd.ExecuteScalar());
                return ID;
            }
            catch
            {
                return 0;
            }
          
        }
        public static bool ActualizarUsuario(Usuarios Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("ActualizarUsuario", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdUsuario", Entidad.IdUsuario);
                Cmd.Parameters.AddWithValue("@IdRol", Entidad.IdRol);
                Cmd.Parameters.AddWithValue("@NombreCompleto", Entidad.NombreCompleto);
                Cmd.Parameters.AddWithValue("@Correo", Entidad.Correo);
                Cmd.Parameters.AddWithValue("@Cargo", Entidad.Cargo);
                Cmd.Parameters.AddWithValue("@Login", Entidad.Login);
                Cmd.Parameters.AddWithValue("@Password", Entidad.Password);
                Cmd.Parameters.AddWithValue("@IdUsuarioActualiza", Entidad.IdUsuarioActualiza);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public static bool AnularUsuario(Usuarios Entidad)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("AnularUsuario", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@IdUsuario", Entidad.IdUsuario);
                Cmd.Parameters.AddWithValue("@IdUsuarioActualiza", Entidad.IdUsuarioActualiza);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public static DataTable ListarUsuarios(bool Todos, int IdUsuario,string Login, byte[] Password)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection Conexion = new SqlConnection(ConexionBD.CadenaConexion);
                Conexion.Open();
                SqlCommand Cmd = new SqlCommand("ListarUsuarios", Conexion);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Todos", Todos);
                Cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                Cmd.Parameters.AddWithValue("@Login", Login);
                Cmd.Parameters.AddWithValue("@Password",Password);
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
        public static byte[] Encrypt(string FlatString, byte[] key = null, byte[] IV = null)
        {
            using (Aes AesAlgoritmo = Aes.Create())
            {
                if (key != null) AesAlgoritmo.Key = key;
                if (IV != null) AesAlgoritmo.IV = IV;
                ICryptoTransform Encryptor = AesAlgoritmo.CreateEncryptor(AesAlgoritmo.Key, AesAlgoritmo.IV);
                byte[] Encrypted;

                using (var MsEncrypt = new MemoryStream())
                {
                    using (var CsEncrypt = new CryptoStream(MsEncrypt, Encryptor, CryptoStreamMode.Write))
                    {
                        using (var SwEncrypt = new StreamWriter(CsEncrypt))
                        {
                            SwEncrypt.Write(FlatString);
                        }
                        Encrypted = MsEncrypt.ToArray();
                    }
                }
                return Encrypted;
            }
        }
    }
}
