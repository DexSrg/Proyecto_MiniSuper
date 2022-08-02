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
    public partial class Menu : Form
    {
        static int IdUsuarioSistema = 0;
        static int IdRolSistema = 0;
        static bool CerrarSesion = false;
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            CerrarSesion=false;
            if (ValidarUsuario())
            {
                PermisosMenu();
                CerrarDemasFormularios();
            }
            else
            {
                Mensaje("Acceso Restringido", (int)eMessage.Error);
                this.Close();
            }
        }

        #region Seguridad
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
                        IdRolSistema = IdRol;
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
            catch (Exception Ex)
            {
                Mensaje(Ex.Message, (int)eMessage.Error);
                return false;
            }
        }
        private void PermisosMenu()
        {
            switch (IdRolSistema)
            {
                case (int)eRol.Administrador:
                    btnFacturar.Visible = true;
                    panelBtnFacturar.Visible = true;

                    btnInventario.Visible = true;
                    panelBtnInventario.Visible = true;

                    btnProductos.Visible = true;
                    panelBtnProductos.Visible = true;

                    panelUnidadMedida.Visible = true;
                    btnUnidadMedida.Visible = true;

                    btnClientes.Visible = true;
                    panelBtnClientes.Visible = true;

                    btnUsuarios.Visible = true;
                    panelBtnUsuarios.Visible = true;

                    break;
                case (int)eRol.Gerente:
                    btnFacturar.Visible = true;
                    panelBtnFacturar.Visible = true;

                    btnInventario.Visible = true;
                    panelBtnInventario.Visible = true;

                    btnProductos.Visible = true;
                    panelBtnProductos.Visible = true;

                    panelUnidadMedida.Visible = true;
                    btnUnidadMedida.Visible = true;

                    btnClientes.Visible = true;
                    panelBtnClientes.Visible = true;

                    btnUsuarios.Visible = false;
                    panelBtnUsuarios.Visible = false;
                    break;
                case (int)eRol.Cajero:
                    btnFacturar.Visible = true;
                    panelBtnFacturar.Visible = true;

                    btnInventario.Visible = true;
                    panelBtnInventario.Visible = true;

                    btnProductos.Visible = false;
                    panelBtnProductos.Visible = false;

                    panelUnidadMedida.Visible = false;
                    btnUnidadMedida.Visible = false;

                    btnClientes.Visible = false;
                    panelBtnClientes.Visible = false;

                    btnUsuarios.Visible = false;
                    panelBtnUsuarios.Visible = false;
                    break;


                default:
                    btnFacturar.Visible = false;
                    panelBtnFacturar.Visible = false;

                    btnInventario.Visible = false;
                    panelBtnInventario.Visible = false;

                    btnProductos.Visible = false;
                    panelBtnProductos.Visible = false;

                    btnClientes.Visible = false;
                    panelBtnClientes.Visible = false;

                    btnUsuarios.Visible = false;
                    panelBtnUsuarios.Visible = false;
                    break;
            }
        }
        private void CerrarDemasFormularios()
        {
            try
            {
                FormCollection formulariosApp = Application.OpenForms;
                foreach (Form frm in formulariosApp)
                {
                    if (frm.Name != this.Name)
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

        #endregion

        #region Evento de los Controles
        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void btnFacturar_Click(object sender, EventArgs e)
        {

        }

        private void btnInventario_Click(object sender, EventArgs e)
        {

        }

        private void btnProductos_Click(object sender, EventArgs e)
        {

        }

        private void btnClientes_Click(object sender, EventArgs e)
        {

        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AdministrarUsuarios frmAdministracionUsuarios = new AdministrarUsuarios();
            frmAdministracionUsuarios.Show();
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (!CerrarSesion)
            {
                Application.Exit();
            }
        }
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            CerrarSesion = true;
           
            Login frmLogin = new Login();
            frmLogin.Show();
            this.Hide();
        }
        #endregion


    }
}
