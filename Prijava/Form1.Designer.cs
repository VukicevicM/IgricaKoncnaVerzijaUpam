namespace Prijava
{
    partial class LogIn
    {
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Panel panel;

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();

            // Form
            this.ClientSize = new System.Drawing.Size(400, 280);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.Text = "Game Login";

            // Panel
            this.panel.Size = new System.Drawing.Size(360, 220);
            this.panel.Location = new System.Drawing.Point(20, 20);
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // lblTitle
            this.lblTitle.Text = "Log In to Start Game";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(80, 10);
            this.lblTitle.Size = new System.Drawing.Size(220, 30);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblUsername
            this.lblUsername.Text = "Username:";
            this.lblUsername.Location = new System.Drawing.Point(40, 60);
            this.lblUsername.Size = new System.Drawing.Size(80, 20);

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(130, 60);
            this.txtUsername.Size = new System.Drawing.Size(180, 23);

            // lblPassword
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(40, 100);
            this.lblPassword.Size = new System.Drawing.Size(80, 20);

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(130, 100);
            this.txtPassword.Size = new System.Drawing.Size(180, 23);
            this.txtPassword.UseSystemPasswordChar = true;

            // btnLogin
            this.btnLogin.Text = "Log In";
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.Location = new System.Drawing.Point(60, 150);
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogin.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // btnRegister
            this.btnRegister.Text = "Register";
            this.btnRegister.Size = new System.Drawing.Size(100, 35);
            this.btnRegister.Location = new System.Drawing.Point(190, 150);
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRegister.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            // Add controls to panel
            this.panel.Controls.Add(this.lblTitle);
            this.panel.Controls.Add(this.lblUsername);
            this.panel.Controls.Add(this.txtUsername);
            this.panel.Controls.Add(this.lblPassword);
            this.panel.Controls.Add(this.txtPassword);
            this.panel.Controls.Add(this.btnLogin);
            this.panel.Controls.Add(this.btnRegister);

            // Add to form
            this.Controls.Add(this.panel);
        }
    }
}

