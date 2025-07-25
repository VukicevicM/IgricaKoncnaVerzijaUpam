using BazaIgrice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UrejanjeNastavitev
{
    public partial class NastavitveForm : Form
    {
        private readonly Size[] velikosti = new Size[]
        {
        new Size(800, 600),
        new Size(1024, 768),
        new Size(1280, 720),
        new Size(1600, 900),
        new Size(1920, 1080)
        };

        private Button btnLevo;
        private Button btnDesno;
        private Button btnSkok;
        private ComboBox cbVelikost;
        private CheckBox cbPolnZaslon;
        private Button btnShrani;
        private Button btnNazaj;

        private Button currentAwaitingKeyBtn = null;

        public NastavitveForm()
        {
            // Respect current Nastavitve on load
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += NastavitveForm_KeyDown;

            if (Nastavitve.PolnZaslon)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.Size = Nastavitve.VelikostOkna;
            }

            this.Text = "Nastavitve";

            InitializeControls();
        }

        private void InitializeControls()
        {
            var layout = new TableLayoutPanel()
            {
                RowCount = 6,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoSize = true
            };

            layout.Controls.Add(new Label() { Text = "Tipka levo:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            btnLevo = CreateKeyButton(Nastavitve.tipkaLevo, (key) => Nastavitve.tipkaLevo = key);
            layout.Controls.Add(btnLevo, 1, 0);

            layout.Controls.Add(new Label() { Text = "Tipka desno:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            btnDesno = CreateKeyButton(Nastavitve.tipkaDesno, (key) => Nastavitve.tipkaDesno = key);
            layout.Controls.Add(btnDesno, 1, 1);

            layout.Controls.Add(new Label() { Text = "Tipka skok:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
            btnSkok = CreateKeyButton(Nastavitve.tipkaSkok, (key) => Nastavitve.tipkaSkok = key);
            layout.Controls.Add(btnSkok, 1, 2);

            layout.Controls.Add(new Label() { Text = "Velikost zaslona:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 3);
            cbVelikost = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Left
            };

            foreach (var size in velikosti)
            {
                cbVelikost.Items.Add(size);
            }

            cbVelikost.SelectedItem = velikosti.FirstOrDefault(v => v == Nastavitve.VelikostOkna);
            layout.Controls.Add(cbVelikost, 1, 3);

            cbPolnZaslon = new CheckBox()
            {
                Text = "Celozaslonski način",
                Checked = Nastavitve.PolnZaslon,
                AutoSize = true
            };
            layout.Controls.Add(cbPolnZaslon, 0, 4);
            layout.SetColumnSpan(cbPolnZaslon, 2);

            btnShrani = new Button() { Text = "Shrani", AutoSize = true };
            btnShrani.Click += BtnShrani_Click;
            layout.Controls.Add(btnShrani, 0, 5);

            btnNazaj = new Button() { Text = "Nazaj v meni", AutoSize = true };
            btnNazaj.Click += BtnNazaj_Click;
            layout.Controls.Add(btnNazaj, 1, 5);

            this.Controls.Add(layout);
        }

        private Button CreateKeyButton(Keys initialKey, Action<Keys> onKeySet)
        {
            var button = new Button()
            {
                Text = initialKey.ToString(),
                AutoSize = true,
                Tag = onKeySet
            };

            button.Click += (s, e) =>
            {
                currentAwaitingKeyBtn = button;
                button.Text = "Pritisni tipko...";
            };

            return button;
        }

        private void NastavitveForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentAwaitingKeyBtn != null)
            {
                var key = e.KeyCode;

                if (key == Keys.Escape || key == Keys.ControlKey || key == Keys.ShiftKey || key == Keys.Menu)
                {
                    currentAwaitingKeyBtn.Text = "Neveljavna tipka";
                }
                else
                {
                    currentAwaitingKeyBtn.Text = key.ToString();
                    var action = currentAwaitingKeyBtn.Tag as Action<Keys>;
                    action?.Invoke(key);
                }

                currentAwaitingKeyBtn = null;
            }
        }

        private void BtnShrani_Click(object sender, EventArgs e)
        {
            Nastavitve.VelikostOkna = (Size)cbVelikost.SelectedItem;
            Nastavitve.PolnZaslon = cbPolnZaslon.Checked;

            MessageBox.Show("Nastavitve shranjene.", "Obvestilo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnNazaj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
