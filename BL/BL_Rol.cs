using System.Data;
using DAL;

namespace BL
{
    public class BL_Rol
    {
        public static DataTable ListarRoles(bool Todos = true, int IdRol = 0)
        {
            return DAL_Rol.ListarRoles(Todos, IdRol);
        }
    }
}
