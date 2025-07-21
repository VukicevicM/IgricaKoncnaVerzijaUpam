using Admin;
using BazaIgrice; 
using Prijava;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prijava
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "admin" && password == "adminpass")
            {
                this.Hide();
                AdminForm adminForm = new AdminForm();
                adminForm.ShowDialog();
                this.Close();
                return;
            }

            if (RazredBazaIgrice.Prijava(username, password))
            {
                Nastavitve.ImeIgralca = username;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Prijava ni uspela. Preverite uporabniško ime in geslo.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (RazredBazaIgrice.Registracija(username, password))
            {
                MessageBox.Show("Registracija uspešna! Sedaj se lahko prijavite.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Registracija ni uspela. Uporabniško ime morda že obstaja.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
