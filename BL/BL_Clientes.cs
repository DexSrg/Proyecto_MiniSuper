using System.Data;
using DAL;
using EL;

namespace BL
{
    public class BL_Clientes
    {
        public static int InsertarCliente(Clientes Entidad)
        {
            return DAL_Clientes.InsertarCliente(Entidad);
        }
        public static bool ActualizarCliente(Clientes Entidad)
        {
            return DAL_Clientes.ActualizarCliente(Entidad);
        }
        public static bool AnularCliente(Clientes Entidad)
        {
            return DAL_Clientes.AnularCliente(Entidad);
        }
        public static DataTable ListarClientes(bool Todos, int IdCliente)
        {
            return DAL_Clientes.ListarClientes(Todos, IdCliente);
        }

    }
}
