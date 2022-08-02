using System.Data;
using EL;
using DAL;
using System.Text;

namespace BL
{
    public class BL_Usuarios
    {
        private static byte[] KeyHash24Bits()
        {
            return Encoding.ASCII.GetBytes("D3v3l0P3r.1Ng3n13r0M3j14");
        }
        private static byte[] KeyHash16Bits()
        {
            return Encoding.ASCII.GetBytes("D3v3l0P3r.1Ng3n1");
        }

        public static int InsertarUsuario(Usuarios Entidad)
        {
            return DAL_Usuarios.InsertarUsuario(Entidad);
        }
        public static bool ActualizarUsuario(Usuarios Entidad)
        {
            return DAL_Usuarios.ActualizarUsuario(Entidad);
        }
        public static bool AnularUsuario(Usuarios Entidad)
        {
            return DAL_Usuarios.AnularUsuario(Entidad);
        }
        public static DataTable ListarUsuarios(bool Todos, int IdUsuario, string Login, byte[] Password)
        {
            return DAL_Usuarios.ListarUsuarios(Todos, IdUsuario,Login,Password);
        }
        public static byte[] Encrypt(string FlatString)
        {
            return DAL_Usuarios.Encrypt(FlatString, KeyHash24Bits(),KeyHash16Bits());
        }
    }
}
