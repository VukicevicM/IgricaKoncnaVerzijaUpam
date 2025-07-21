using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;


namespace BazaIgrice
{
    public static class RazredBazaIgrice
    {
        private static string baza_path = "baza_igrice.db";
        private static string connectionString = $"Data Source={baza_path};Version=3;";

        static RazredBazaIgrice()
        {
            if (!File.Exists(baza_path))
            {
                SQLiteConnection.CreateFile(baza_path);
                var conn = new System.Data.SQLite.SQLiteConnection(connectionString);
                conn.Open();

                string narediTabelo = @"
                CREATE TABLE users(
                        username TEXT PRIMARY KEY,
                        password TEXT NOT NULL,
                        lvl1_narejen INTEGER DEFAULT 0,
                        lvl1_tocke INTEGER DEFAULT 0,
                        lvl2_narejen INTEGER DEFAULT 0,
                        lvl2_tocke INTEGER DEFAULT 0
                    );";

                new System.Data.SQLite.SQLiteCommand(narediTabelo, conn).ExecuteNonQuery();
                conn.Close();
            }
        }

        public static bool Registracija(string username, string password)
        {
            try
            {
                var conn = new System.Data.SQLite.SQLiteConnection(connectionString);
                conn.Open();

                var preveriUporabnika = new System.Data.SQLite.SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @u", conn);
                preveriUporabnika.Parameters.AddWithValue("@u", username);
                object result = preveriUporabnika.ExecuteScalar();
                if(System.Convert.ToInt32(result) > 0)
                {
                    conn.Close();
                    return false; // Uporabnik že obstaja
                }

                var vnesiUporabnika = new System.Data.SQLite.SQLiteCommand("INSERT INTO users (username, password) VALUES (@u, @p)", conn);
                vnesiUporabnika.Parameters.AddWithValue("@u", username);
                vnesiUporabnika.Parameters.AddWithValue("@p", password);
                vnesiUporabnika.ExecuteNonQuery();
                conn.Close();
                return true; // Registracija uspešna
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Napaka pri registraciji: " + ex.Message);
                return false; // Napaka pri registraciji
            }
        }

        public static bool Prijava(string username, string password)
        {
            try
            {
                var conn = new System.Data.SQLite.SQLiteConnection(connectionString);
                conn.Open();
                var preveriUporabnika = new System.Data.SQLite.SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @u AND password = @p", conn);
                preveriUporabnika.Parameters.AddWithValue("@u", username);
                preveriUporabnika.Parameters.AddWithValue("@p", password);
                object result = preveriUporabnika.ExecuteScalar();
                conn.Close();
                return System.Convert.ToInt32(result) > 0; // Prijava uspešna
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Napaka pri prijavi: " + ex.Message);
                return false; // Napaka pri prijavi
            }
        }

        public static System.Data.DataTable PridobiUporabnike()
        {
            var conn = new System.Data.SQLite.SQLiteConnection(connectionString);
            conn.Open();

            var selectCommand = new System.Data.SQLite.SQLiteCommand("SELECT * FROM users", conn);
            var adapter = new System.Data.SQLite.SQLiteDataAdapter(selectCommand);
            var dataTable = new System.Data.DataTable();
            adapter.Fill(dataTable);
            conn.Close();

            return dataTable; // Vrnemo podatke o uporabnikih
        }

        public static void IzbrisiUporabnika(string username)
        {
            try
            {
                var conn = new System.Data.SQLite.SQLiteConnection(connectionString);
                conn.Open();

                var deleteCommand = new System.Data.SQLite.SQLiteCommand("DELETE FROM users WHERE username = @u", conn);
                deleteCommand.Parameters.AddWithValue("@u", username);
                deleteCommand.ExecuteNonQuery();

                conn.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Napaka pri brisanju uporabnika: " + ex.Message);
            }
        }
    }


    public static class Nastavitve
    {

        private static string imeIgralca = "Igralec";
        public static string ImeIgralca
        {
            get { return imeIgralca; }
            set { imeIgralca = value; }
        }

        private static System.Drawing.Size velikostOkna = new System.Drawing.Size(800, 600);

        public static System.Windows.Forms.Keys tipkaLevo { get; set; } = System.Windows.Forms.Keys.Left;
        public static System.Windows.Forms.Keys tipkaDesno { get; set; } = System.Windows.Forms.Keys.Right;
        public static System.Windows.Forms.Keys tipkaSkok { get; set; } = System.Windows.Forms.Keys.Space;

        public static bool PolnZaslon { get; set; } = false;

        public static System.Drawing.Size VelikostOkna
        {
            get { return velikostOkna; }
            set
            {
                int minW = 800;
                int minH = 600;

                int maxW = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int maxH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                int w = Math.Max(minW, Math.Min(maxW, value.Width));
                int h = Math.Max(minH, Math.Min(maxH, value.Height));

                velikostOkna = new System.Drawing.Size(w, h);
            }
        }
    }
}
