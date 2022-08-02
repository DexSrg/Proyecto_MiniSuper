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
using System.Windows.Controls;

namespace Escritorio_MPOO
{
    public partial class AdministrarUsuarios : Form
    {
        static int IdUsuario = 0;
        static int IdUsuarioSistema = 0;
        public AdministrarUsuarios()
        {
            InitializeComponent();
        }
        private void AdministrarUsuarios_Load(object sender, EventArgs e)
        {
            if (ValidarUsuario())
            {
                CargarGrid();
                Cargar_DDLRol();
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
                Login frmLogin = new Login();

                IdUsuarioSistema = VariablesGlobales.IdUsurioSistema;

                if (IdUsuarioSistema > 0)
                {
                    int IdRol = Convert.ToInt32(BL_Usuarios.ListarUsuarios(false, IdUsuarioSistema, "", null).Rows[0]["IdRol"]);
                    if (IdRol > 0)
                    {
                        if (IdRol == (int)eRol.Administrador) //Cambiar según Rol
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
                    if (frm.Name != this.Name && frm.Name !="Menu")
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
                gridUsuarios.DataSource = BL_Usuarios.ListarUsuarios(true, 0, "", null);
                if (gridUsuarios.Rows.Count > 0)
                {
                    gridUsuarios.Columns[0].Visible = false;
                    gridUsuarios.Columns[6].Visible = false;
                }

            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
            }
        }
        private void Cargar_DDLRol()
        {
            try
            {
                ddlRol.Items.Clear();
                ddlRol.DataSource = BL_Rol.ListarRoles();
                ddlRol.DisplayMember = "Rol";
                ddlRol.ValueMember = "IdRol";
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
            }
        }
        private void LimpiarControles()
        {
            txtNombre.Text = "";
            txtCorreo.Text = String.Empty;
            txtCargo.Text = String.Empty;
            txtLogin.Text = String.Empty;
            txtPassword.Text = String.Empty;
            ddlRol.SelectedIndex = 0;
            IdUsuario = 0;
        }
        private bool ValidarControles()
        {
            try
            {
                if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    Mensaje("Ingrese el nombre del usuario", (int)eMessage.Alerta);
                    return false;
                }

                if (string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
                {
                    Mensaje("Ingrese el correo del usuario", (int)eMessage.Alerta);
                    return false;
                }

                if (string.IsNullOrEmpty(txtCargo.Text) || string.IsNullOrWhiteSpace(txtCargo.Text))
                {
                    Mensaje("Ingrese el cargo del usuario", (int)eMessage.Alerta);
                    return false;
                }
                if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrWhiteSpace(txtLogin.Text))
                {
                    Mensaje("Ingrese el login del usuario", (int)eMessage.Alerta);
                    return false;
                }

                if (IdUsuario == 0)
                {
                    DataTable dt = new DataTable();
                    dt = BL_Usuarios.ListarUsuarios(false, 0, txtLogin.Text.Trim(), null);

                    if (dt.Rows.Count > 0)
                    {
                        Mensaje("El Login de usuario ya existe, por favor ingrese otro", (int)eMessage.Alerta);
                        return false;
                    }

                    if (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        Mensaje("Ingrese el password del usuario", (int)eMessage.Alerta);
                        return false;
                    }
                }

                if (IdUsuario > 0)
                {
                    DataTable dt = new DataTable();
                    dt = BL_Usuarios.ListarUsuarios(false, 0, txtLogin.Text.Trim(), null);
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["IdUsuario"]) != IdUsuario)
                        {
                            Mensaje("El Login ingresado no puede ser utilizado, por favor ingrese otro", (int)eMessage.Alerta);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
                return false;
            }
        }
        private bool GuardarUsuario()
        {
            try
            {
                Usuarios Entidad = new Usuarios();
                Entidad.IdRol = Convert.ToInt32(ddlRol.SelectedValue);
                Entidad.NombreCompleto = txtNombre.Text;
                Entidad.Correo = txtCorreo.Text;
                Entidad.Cargo = txtCargo.Text;
                Entidad.Login = txtLogin.Text;


                if (IdUsuario > 0)
                {
                    if (txtPassword.Text != String.Empty)
                    {
                        Entidad.Password = BL_Usuarios.Encrypt(txtPassword.Text);
                    }
                    else
                    {
                        Entidad.Password = null;
                    }
                    Entidad.IdUsuario = IdUsuario;
                    Entidad.IdUsuarioActualiza = IdUsuarioSistema;
                    bool Exito = BL_Usuarios.ActualizarUsuario(Entidad);
                    CargarGrid();
                    LimpiarControles();
                    return Exito;
                }
                else
                {
                    Entidad.IdUsuarioRegistro = IdUsuarioSistema;
                    Entidad.Password = BL_Usuarios.Encrypt(txtPassword.Text);
                    int ID = BL_Usuarios.InsertarUsuario(Entidad);
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
        private bool AnularUsuario()
        {
            try
            {
                Usuarios Entidad = new Usuarios();
                Entidad.IdUsuario = IdUsuario;
                Entidad.IdUsuarioActualiza = IdUsuarioSistema;
                bool Exito = BL_Usuarios.AnularUsuario(Entidad);
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
                IdUsuario = Convert.ToInt32(gridUsuarios.Rows[Index].Cells[0].Value);

                txtNombre.Text = gridUsuarios.Rows[Index].Cells[1].Value.ToString();
                txtCorreo.Text = gridUsuarios.Rows[Index].Cells[2].Value.ToString();
                txtCargo.Text = gridUsuarios.Rows[Index].Cells[3].Value.ToString();
                txtLogin.Text = gridUsuarios.Rows[Index].Cells[4].Value.ToString();
                ddlRol.SelectedValue = gridUsuarios.Rows[Index].Cells[6].Value.ToString();
            }
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
            }
        }
        #endregion

        #region Eventos de los Controles
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarControles())
            {
                if (GuardarUsuario())
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
            if (IdUsuario > 0)
            {
                if (AnularUsuario())
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
        private void gridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cargar_Controles(e.RowIndex);
        }
        #endregion
    }
}
