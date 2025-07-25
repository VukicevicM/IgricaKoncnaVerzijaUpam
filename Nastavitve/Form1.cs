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

namespace Nastavitve
{
    public partial class NastavitveForm : Form
    {
        private MeniForm meniForm;
        private ComboBox screenSizeCombo;
        private ComboBox keyLeftCombo;
        private ComboBox keyRightCombo;
        private ComboBox keyJumpCombo;

        public NastavitveForm(MeniForm meniForm)
        {
            this.meniForm = meniForm;
            this.Text = "Nastavitve";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = Nastavitve.VelikostOkna;

            InitializeControls();
        }

        private void InitializeControls()
        {
            var layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(20),
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            layout.Controls.Add(new Label() { Text = "Velikost zaslona:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            screenSizeCombo = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Anchor = AnchorStyles.Left };
            screenSizeCombo.Items.AddRange(new string[] { "800x600", "1024x768", "1280x720", "Fullscreen" });

            string currentSize = Nastavitve.PolnZaslon ? "Fullscreen" : $"{Nastavitve.VelikostOkna.Width}x{Nastavitve.VelikostOkna.Height}";
            screenSizeCombo.SelectedItem = currentSize;
            layout.Controls.Add(screenSizeCombo, 1, 0);

            layout.Controls.Add(new Label() { Text = "Tipka levo:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            keyLeftCombo = CreateKeyComboBox(Nastavitve.tipkaLevo);
            layout.Controls.Add(keyLeftCombo, 1, 1);

            layout.Controls.Add(new Label() { Text = "Tipka desno:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            keyRightCombo = CreateKeyComboBox(Nastavitve.tipkaDesno);
            layout.Controls.Add(keyRightCombo, 1, 2);

            layout.Controls.Add(new Label() { Text = "Tipka skok:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            keyJumpCombo = CreateKeyComboBox(Nastavitve.tipkaSkok);
            layout.Controls.Add(keyJumpCombo, 1, 3);

            var btnShrani = new Button()
            {
                Text = "Shrani",
                Width = 100,
                Anchor = AnchorStyles.None,
                Margin = new Padding(10)
            };
            btnShrani.Click += (s, e) => ShraniNastavitve();
            layout.Controls.Add(btnShrani, 0, 4);

            var btnNazaj = new Button()
            {
                Text = "Nazaj",
                Width = 100,
                Anchor = AnchorStyles.None,
                Margin = new Padding(10)
            };
            btnNazaj.Click += (s, e) => {
                this.Close();
                meniForm.PokaziMeni();
            };
            layout.Controls.Add(btnNazaj, 1, 4);

            this.Controls.Add(layout);
        }

        private ComboBox CreateKeyComboBox(Keys selectedKey)
        {
            var combo = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Left
            };
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                if (k == Keys.None || k >= Keys.MouseButtons) continue;
                combo.Items.Add(k);
            }
            combo.SelectedItem = selectedKey;
            return combo;
        }

        private void ShraniNastavitve()
        {
            // Tipke
            Nastavitve.tipkaLevo = (Keys)keyLeftCombo.SelectedItem;
            Nastavitve.tipkaDesno = (Keys)keyRightCombo.SelectedItem;
            Nastavitve.tipkaSkok = (Keys)keyJumpCombo.SelectedItem;

            // Zaslon
            string sizeText = screenSizeCombo.SelectedItem.ToString();
            if (sizeText == "Fullscreen")
            {
                Nastavitve.PolnZaslon = true;
            }
            else
            {
                Nastavitve.PolnZaslon = false;
                var parts = sizeText.Split('x');
                int width = int.Parse(parts[0]);
                int height = int.Parse(parts[1]);
                Nastavitve.VelikostOkna = new Size(width, height);
            }

            MessageBox.Show("Nastavitve shranjene!", "Obvestilo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}
