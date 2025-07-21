using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BazaIgrice; 

namespace Admin
{
    public partial class AdminForm : Form
    {
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnDelete;

        public AdminForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgvUsers.DataSource = RazredBazaIgrice.PridobiUporabnike();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Prosim izberi uporabnike za izbrisati.");
                return;
            }

            string username = dgvUsers.SelectedRows[0].Cells["username"].Value.ToString();

            if (username == "admin")
            {
                System.Windows.Forms.MessageBox.Show("Ne mores izbrisati admina.");
                return;
            }

            RazredBazaIgrice.IzbrisiUporabnika(username);
            LoadUsers();
        }
    }
}
