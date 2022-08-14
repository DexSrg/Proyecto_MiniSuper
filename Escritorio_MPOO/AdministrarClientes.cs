using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EL;
using static EL.Enums;
using BL;

namespace Escritorio_MPOO
{
    public partial class AdministrarClientes : Form
    {
        static int IdCliente = 0;
        static int IdUsuarioSistema = 0;
        public AdministrarClientes()
        {
            InitializeComponent();
        }

        private void AdministrarClientes_Load(object sender, EventArgs e)
        {
            if (ValidarUsuario())
            {
                CargarGrid();
                //Cargar_DDLRol();
                CerrarDemasFormularios();
            }
            else
            {
                Mensaje("Acceso Restringido", (int)eMessage.Error);
                this.Close();
            }
        }

        #region Seguridad
        private bool ValidarUsuario()
        {
            try
            {

                IdUsuarioSistema = VariablesGlobales.IdUsurioSistema;

                if (IdUsuarioSistema > 0)
                {
                    int IdRol = Convert.ToInt32(BL_Usuarios.ListarUsuarios(false, IdUsuarioSistema, "", null).Rows[0]["IdRol"]);
                    if (IdRol > 0)
                    {
                        if (IdRol == (int)eRol.Cajero || IdRol == (int)eRol.Administrador) //Cambiar según Rol, el rol administrador dejarlo para que tenga acceso a todos los formularios, si queremos agregar otro rol se puede agregar sin problemas
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
                return false;
            }
        }
        private void Mensaje(string Msj, int Tipo)
        {
            try
            {
                switch (Tipo)
                {
                    case 1:
                        //Informacion
                        MessageBox.Show(Msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 2:
                        //Exito
                        MessageBox.Show(Msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 3:
                        //Alerta
                        MessageBox.Show(Msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 4:
                        //Error
                        MessageBox.Show(Msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CerrarDemasFormularios()
        {
            try
            {
                FormCollection formulariosApp = Application.OpenForms;
                foreach (Form frm in formulariosApp)
                {
                    if (frm.Name != this.Name && frm.Name != "Menu")
                    {
                        frm.Hide();
                    }
                }
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
            }
        }
        #endregion

        #region Metodos y Funciones
        private void CargarGrid()
        {
            try
            {
                gridClientes.DataSource = BL_Clientes.ListarClientes(true, 0);
                if (gridClientes.Rows.Count > 0)
                {
                    gridClientes.Columns[0].Visible = false;
    
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
            }
        }
        private void LimpiarControles()
        {
            txtNombreCompleto.Text = "";
            txtIdentificacion.Text = "";
            txtCelular.Text = "";
            txtCorreo.Text = string.Empty;         
            IdCliente = 0;
        }
        private bool ValidarControles()
        {
            try
            {
                if (string.IsNullOrEmpty(txtNombreCompleto.Text) || string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
                {
                    Mensaje("Ingrese el nombre completo del cliente", (int)eMessage.Alerta);
                    return false;
                }
                if (string.IsNullOrEmpty(txtIdentificacion.Text) || string.IsNullOrWhiteSpace(txtIdentificacion.Text))
                {
                    Mensaje("Ingrese la identificación del cliente", (int)eMessage.Alerta);
                    return false;
                }
                if (string.IsNullOrEmpty(txtCelular.Text) || string.IsNullOrWhiteSpace(txtCelular.Text))
                {
                    Mensaje("Ingrese el celular del cliente", (int)eMessage.Alerta);
                    return false;
                }

                if (string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
                {
                    Mensaje("Ingrese el correo del usuario", (int)eMessage.Alerta);
                    return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
                return false;
            }
        }
        private bool Guardar()
        {
            try
            {
                Clientes Entidad = new Clientes();
                Entidad.NombreCompleto = txtNombreCompleto.Text;
                Entidad.Identificacion = txtIdentificacion.Text;
                Entidad.Celular = txtCelular.Text;
                Entidad.Correo = txtCorreo.Text;


                if (IdCliente > 0)
                {
                 
                    Entidad.IdCliente = IdCliente;
                    Entidad.IdUsuarioActualiza = IdUsuarioSistema;
                    bool Exito = BL_Clientes.ActualizarCliente(Entidad);
                    CargarGrid();
                    LimpiarControles();
                    return Exito;
                }
                else
                {
                    Entidad.IdUsuarioRegistro = IdUsuarioSistema;
                    int ID =BL_Clientes.InsertarCliente(Entidad);
                    CargarGrid();
                    LimpiarControles();
                    return ID > 0;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
                return false;
            }
        }
        private bool Anular()
        {
            try
            {
                Clientes Entidad = new Clientes();
                Entidad.IdCliente = IdCliente;
                Entidad.IdUsuarioActualiza = IdUsuarioSistema;
                bool Exito = BL_Clientes.AnularCliente(Entidad);    
                CargarGrid();
                LimpiarControles();
                return Exito;
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
                return false;
            }
        }
        private void Cargar_Controles(int Index)
        {
            try
            {
                IdCliente = Convert.ToInt32(gridClientes.Rows[Index].Cells[0].Value);

                txtNombreCompleto.Text = gridClientes.Rows[Index].Cells[1].Value.ToString();
                txtIdentificacion.Text = gridClientes.Rows[Index].Cells[2].Value.ToString();
                txtCelular.Text = gridClientes.Rows[Index].Cells[3].Value.ToString();
                txtCorreo.Text = gridClientes.Rows[Index].Cells[4].Value.ToString();             
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
            }
        }
        #endregion

        #region Evento de los Controles    
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarControles())
            {
                if (Guardar())
                {
                    Mensaje("Registro Guardado con Exito", (int)eMessage.Exito);
                }
                else
                {
                    Mensaje("Erro al guardar el registro", (int)eMessage.Error);
                }
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (IdCliente > 0)
            {
                if (Anular())
                {
                    Mensaje("Registro anulado con Exito", (int)eMessage.Exito);
                }
                else
                {
                    Mensaje("Error al anular el registro", (int)eMessage.Error);
                }
            }
            else
            {
                Mensaje("Seleccione un registro", (int)eMessage.Alerta);
            }
        }
        private void gridClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cargar_Controles(e.RowIndex);
        }
        #endregion
    }
}
