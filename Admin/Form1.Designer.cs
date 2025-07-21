namespace Admin
{
    partial class AdminForm
    {
        private void InitializeComponent()
        {
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // dgvUsers
            this.dgvUsers.Location = new System.Drawing.Point(20, 20);
            this.dgvUsers.Size = new System.Drawing.Size(500, 250);
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.ReadOnly = true;

            // btnDelete
            this.btnDelete.Text = "Delete Selected User";
            this.btnDelete.Location = new System.Drawing.Point(180, 280);
            this.btnDelete.Size = new System.Drawing.Size(180, 35);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // AdminPanelForm
            this.ClientSize = new System.Drawing.Size(540, 340);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.btnDelete);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Text = "Admin Panel";

            this.ResumeLayout(false);
        }
    }
}

