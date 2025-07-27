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
using UrejanjeNastavitev;
using Osnova;

namespace Meni
{
    public partial class MeniForm : Form
    {
        private const int STEVILO_NIVOJEV = 5;
        private List<Button> gumbiNivojev = new List<Button>();

        public MeniForm()
        {
            this.Text = $"Mačja Pustolovščina - Dobrodošel, {Nastavitve.ImeIgralca}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = Nastavitve.VelikostOkna;

            if (Nastavitve.PolnZaslon)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }

            InitializeControls();
        }

        private void InitializeControls()
        {
            int najvisjiNivo = RazredBazaIgrice.DobijNajvisjiNivo(Nastavitve.ImeIgralca);


            var layout = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(this.Width / 10),
                WrapContents = false
            };

            for (int i = 1; i <= STEVILO_NIVOJEV; i++)
            {
                var levelBtn = new Button()
                {
                    Text = $"Level {i}",
                    Width = this.Width / 3,
                    Height = this.Height / 12,
                    Font = new Font("Segoe UI", this.Height / 40f, FontStyle.Bold),
                    Margin = new Padding(10)
                };

                int level = i;

                if (i <= najvisjiNivo + 1)
                {
                    levelBtn.BackColor = Color.LightGreen;
                    levelBtn.Click += (s, e) => ZaženiLevel(level);
                }
                else
                {
                    levelBtn.Enabled = false;
                    levelBtn.BackColor = Color.LightGray;
                }

                gumbiNivojev.Add(levelBtn);
                layout.Controls.Add(levelBtn);
            }

            var btnNastavitve = new Button()
            {
                Text = "Nastavitve",
                Width = this.Width / 3,
                Height = this.Height / 12,
                Font = new Font("Segoe UI", this.Height / 45f, FontStyle.Regular),
                Margin = new Padding(10)
            };

            var btnIzhod = new Button()
            {
                Text = "Izhod iz igre",
                Width = this.Width / 3,
                Height = this.Height / 12,
                Font = new Font("Segoe UI", this.Height / 45f, FontStyle.Regular),
                Margin = new Padding(10),
                BackColor = Color.LightCoral
            };

            btnIzhod.Click += (s, e) =>
            {
                var result = MessageBox.Show("Ali res želiš zapreti igro?", "Potrditev", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };

            layout.Controls.Add(btnIzhod);

            btnNastavitve.Click += (s, e) =>
            {
                this.Hide();
                using (var nastavitveForm = new NastavitveForm())
                {
                    nastavitveForm.ShowDialog();
                }

                // 🔁 Fix fullscreen exit glitch: Reset state *first*
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable; // Temp value to avoid stuck state

                // ✅ Apply updated settings
                if (Nastavitve.PolnZaslon)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                    this.Size = Nastavitve.VelikostOkna;
                    this.StartPosition = FormStartPosition.CenterScreen;
                }

                this.Text = $"Mačja Pustolovščina - Dobrodošel, {Nastavitve.ImeIgralca}";

                this.Controls.Clear();
                InitializeControls();

                this.Show();
            };

            layout.Controls.Add(btnNastavitve);

            var lblUporabnik = new Label()
            {
                Text = $"Uporabnik: {Nastavitve.ImeIgralca}",
                AutoSize = true,
                Font = new Font("Segoe UI", this.Height / 50f, FontStyle.Italic),
                ForeColor = Color.DarkSlateGray,
                Margin = new Padding(10)
            };

            layout.Controls.Add(lblUporabnik);
            this.Controls.Add(layout);
        }

        private void ZaženiLevel(int nivo)
        {
            //MessageBox.Show($"Začenjam Level {nivo} - tukaj bo odprta nova forma za igro.");
            // TODO: Implementiraj zagon dejanske igre/forma za nivo
            //Temporary
            this.Hide();

            using (var levelForm = new Lvl1())
            {
                levelForm.ShowDialog();
            }

            this.Show();
        }

        public void PokaziMeni()
        {
            this.Show();
        }
    }
}
