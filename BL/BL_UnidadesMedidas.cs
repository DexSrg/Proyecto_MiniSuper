using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
using EL;

namespace BL
{
    public class BL_UnidadesMedidas
    {
        public static int InsertarUnidadMedida(UnidadesMedidas Entidad)
        {
            return DAL_UnidadesMedidas.InsertarUnidadMedida(Entidad);
        }
        public static bool ActualizarUsuario(UnidadesMedidas Entidad)
        {
            return DAL_UnidadesMedidas.ActualizarUsuario(Entidad);
        }
        public static bool AnularUsuario(UnidadesMedidas Entidad)
        {
            return DAL_UnidadesMedidas.AnularUsuario(Entidad);
        }
        public static DataTable ListarUsuarios(bool Todos, int IdUnidadMedida)
        {
          return  DAL_UnidadesMedidas.ListarUsuarios(Todos, IdUnidadMedida);
        }
    }
}
