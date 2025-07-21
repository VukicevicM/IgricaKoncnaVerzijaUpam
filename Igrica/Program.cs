using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BazaIgrice;
using Prijava;

namespace Igrica
{
    internal static class Program
    {
        [System.STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            LogIn loginForm = new LogIn();
            if (loginForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Placeholder — to be replaced with MainMenuForm later
                System.Windows.Forms.MessageBox.Show("Login successful! Welcome " + Nastavitve.ImeIgralca);
                // System.Windows.Forms.Application.Run(new MainMenuForm());
            }
        }
    }
}
