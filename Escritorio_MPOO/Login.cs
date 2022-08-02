using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EL.Enums;
using EL;
using BL;

namespace Escritorio_MPOO
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        #region Metodos y funciones
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
        private bool ValidarControles()
        {
            try
            {
                if (txtLogin.Text == string.Empty)
                {
                    Mensaje("Ingrese el Login", (int)eMessage.Alerta);
                    return false;
                }
                if (txtPassword.Text == string.Empty)
                {
                    Mensaje("Ingrese el Password", (int)eMessage.Alerta);
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
        private void ValidarCredenciales()
        {
            try
            {
                if (ValidarControles())
                {
                    DataTable dt = new DataTable();
                    dt = BL_Usuarios.ListarUsuarios(false, 0, txtLogin.Text.Trim(), BL_Usuarios.Encrypt(txtPassword.Text.Trim()));

                    if (dt.Rows.Count > 0)
                    {
                       VariablesGlobales.IdUsurioSistema = Convert.ToInt32(dt.Rows[0]["IdUsuario"]);

                        if (VariablesGlobales.IdUsurioSistema > 0)
                        {                         
                            Menu frmMenu = new Menu();
                            frmMenu.Show();
                        }
                        else
                        {
                            Mensaje("Error al iniciar sesión", (int)eMessage.Alerta);
                        }
                    }
                    else
                    {
                        Mensaje("Credenciales invalidas", (int)eMessage.Alerta);
                    }
                }
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
            Application.Exit();
        }
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            ValidarCredenciales();
        }
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ValidarCredenciales();
            }
        }
        #endregion
    }
}
