using System;


namespace EL
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Cargo { get; set; }
        public string Login { get; set; }
        public  byte[] Password { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? IdUsuarioActualiza { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }
    }
}
