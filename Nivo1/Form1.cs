using BazaIgrice;
using Knjiznica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Nivo1
{
    public partial class Lvl1 : Form
    {
        private Image ozadje;
        private int premikKamere = 0;
        private Macka macka;
        private Timer timer;
        private bool levo, desno;
        private List<Block> tla = new List<Block>(); // multiple ground blocks
        private CatBed postelja;
        private bool fadeOut = false;
        private int fadeAlpha = 0;
        private int smrtnaMejaY;
        private bool gameOverTriggered = false;

        private List<Ribica> ribice = new List<Ribica>();
        private TimeSpan casVidneZbrane = TimeSpan.FromMilliseconds(500);


        public Lvl1()
        {
            this.Text = "Mačja Pustolovščina - Nivo 2";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

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

            ozadje = Properties.Resources.Nebo2;

            int groundY = this.ClientSize.Height - Block.StandardHeight;
            smrtnaMejaY = groundY + 300;

            // === FLOOR WITH GAPS ===
            tla.Add(new Block(0, groundY, 400, Block.StandardHeight));
            tla.Add(new Block(600, groundY, 300, Block.StandardHeight));
            tla.Add(new Block(1000, groundY, 250, Block.StandardHeight));
            tla.Add(new Block(1400, groundY, 500, Block.StandardHeight));


            // === CAT ===
            macka = new Macka(100, groundY - 100);

            // === BED ===
            postelja = new CatBed(1800, groundY - Block.StandardHeight);

            // === FISH ===
            ribice.Add(new Ribica(300, groundY - 40));
            ribice.Add(new Ribica(650, groundY - 120));
            ribice.Add(new Ribica(1100, groundY - 180));

            this.Paint += Lvl_Paint;
            this.KeyDown += Lvl_KeyDown;
            this.KeyUp += Lvl_KeyUp;

            timer = new Timer { Interval = 16 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Lvl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int ozX = ozadje.Width;

            for (int x = -premikKamere % ozX; x < this.ClientSize.Width; x += ozX)
            {
                g.DrawImage(ozadje, x, 0, ozX, this.ClientSize.Height);
            }

            foreach (var blok in tla)
                blok.Narisi(g, premikKamere);

            if (!fadeOut)
                macka.Narisi(g, premikKamere);

            foreach (var ribica in ribice)
            {
                if (ribica.JeSeVidna(casVidneZbrane))
                    ribica.Narisi(g, premikKamere);
            }

            postelja.Narisi(g, premikKamere);

            if (fadeOut)
            {
                fadeAlpha = Math.Min(fadeAlpha + 8, 255);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(fadeAlpha, 0, 0, 0)))
                {
                    g.FillRectangle(b, this.ClientRectangle);
                }

                if (fadeAlpha >= 255)
                {
                    if (postelja.Dosezena && postelja.PretekelCas())
                        ZakljuciStopnjo();
                    else
                        this.Close();
                }
            }
        }

        private void ZakljuciStopnjo()
        {
            string username = Nastavitve.ImeIgralca;
            RazredBazaIgrice.PosodobiNivoInTocke(username, 2, macka.SteviloRibic);
            this.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (levo) macka.PremakniLevo();
            if (desno) macka.PremakniDesno();

            // Check ground below cat
            bool jeNaTleh = false;
            foreach (var blok in tla)
            {
                if (macka.IsAboveGround(blok))
                {
                    jeNaTleh = true;
                    break;
                }
            }

            if (!jeNaTleh)
            {
                macka.JeNaTleh = false;
                macka.Posodobi();
                foreach (var blok in tla)
                    macka.PreveriKolizijo(blok);
            }
            else if (macka.HitrostY != 0)
            {
                macka.Posodobi();
                foreach (var blok in tla)
                    macka.PreveriKolizijo(blok);
            }
            else
            {
                macka.JeNaTleh = true;
                macka.PosodobiAnimacijaPoCasu();
            }

            if (!gameOverTriggered && macka.Obmocje.Bottom > smrtnaMejaY)
            {
                gameOverTriggered = true;
                GameOver();
                return;
            }

            postelja.PreveriKolizijo(macka);

            if (postelja.Dosezena && !fadeOut)
            {
                fadeOut = true;
                fadeAlpha = 0;
            }

            int screenWidth = this.ClientSize.Width;

            if (macka.SvetX - premikKamere > screenWidth / 2)
                premikKamere = macka.SvetX - screenWidth / 2;
            else if (macka.SvetX - premikKamere < 100)
                premikKamere = macka.SvetX - 100;

            premikKamere = Math.Max(0, premikKamere);

            foreach (var ribica in ribice)
            {
                ribica.Posodobi();
                ribica.PreveriZajetje(macka);
            }

            Invalidate();
        }

        private void Lvl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Nastavitve.tipkaLevo) levo = true;
            if (e.KeyCode == Nastavitve.tipkaDesno) desno = true;
            if (e.KeyCode == Nastavitve.tipkaSkok) macka.Skoci();
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void Lvl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Nastavitve.tipkaLevo) levo = false;
            if (e.KeyCode == Nastavitve.tipkaDesno) desno = false;
        }

        private void GameOver()
        {
            MessageBox.Show("Muca je padla v luknjo! Poskusi znova.", "Game Over",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
