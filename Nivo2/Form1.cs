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

namespace Nivo2
{
    public partial class Lvl2 : Form
    {
        private Image ozadje;
        private int premikKamere = 0;
        private Macka macka;
        private Timer timer;
        private bool levo, desno;
        private List<Block> tla = new List<Block>();
        private List<Block> platforme = new List<Block>();
        private List<Ribica> ribice = new List<Ribica>();
        private CatBed postelja;
        private int smrtnaMejaY;
        private bool fadeOut = false;
        private int fadeAlpha = 0;
        private bool gameOverTriggered = false;
        private TimeSpan casVidneZbrane = TimeSpan.FromMilliseconds(500);

        public Lvl2()
        {
            this.Text = "Mačja Pustolovščina - Level 2";
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

            int groundY = Nastavitve.PolnZaslon
                ? Screen.PrimaryScreen.Bounds.Height - Block.StandardHeight
                : Nastavitve.VelikostOkna.Height - Block.StandardHeight;

            // Main ground with a gap
            tla.Add(new Block(0, groundY, 400, Block.StandardHeight));
            tla.Add(new Block(1100, groundY, 300, Block.StandardHeight));

            // Platforms
            platforme.Add(new Block(500, groundY - 100, 100, Block.StandardHeight));
            platforme.Add(new Block(800, groundY - 150, 100, Block.StandardHeight));
            platforme.Add(new Block(1050, groundY - 200, 100, Block.StandardHeight));

            macka = new Macka(100, groundY - 100);
            postelja = new CatBed(1200, groundY - Block.StandardHeight);
            smrtnaMejaY = groundY + 300;

            // Fish on platforms
            ribice.Add(new Ribica(520, groundY - 120));
            ribice.Add(new Ribica(820, groundY - 170));
            ribice.Add(new Ribica(1070, groundY - 220));

            timer = new Timer { Interval = 16 };
            timer.Tick += Timer_Tick;
            timer.Start();

            this.Paint += Lvl2_Paint;
            this.KeyDown += Lvl2_KeyDown;
            this.KeyUp += Lvl2_KeyUp;
        }

        private void Lvl2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int ozX = ozadje.Width;

            for (int x = -premikKamere % ozX; x < this.ClientSize.Width; x += ozX)
                g.DrawImage(ozadje, x, 0, ozX, this.ClientSize.Height);

            foreach (var blok in tla) blok.Narisi(g, premikKamere);
            foreach (var p in platforme) p.Narisi(g, premikKamere);

            if (!fadeOut) macka.Narisi(g, premikKamere);
            foreach (var r in ribice)
                if (r.JeSeVidna(casVidneZbrane)) r.Narisi(g, premikKamere);

            postelja.Narisi(g, premikKamere);

            if (fadeOut)
            {
                fadeAlpha = Math.Min(fadeAlpha + 8, 255);
                using (SolidBrush b = new SolidBrush(Color.FromArgb(fadeAlpha, 0, 0, 0)))
                    g.FillRectangle(b, this.ClientRectangle);
                if (fadeAlpha >= 255)
                {
                    if (postelja.Dosezena && postelja.PretekelCas())
                        ZakljuciStopnjo();
                    else
                        this.Close();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (levo) macka.PremakniLevo();
            if (desno) macka.PremakniDesno();

            bool onGround = tla.Concat(platforme).Any(b => macka.IsAboveGround(b));

            if (!onGround)
            {
                macka.JeNaTleh = false;
                macka.Posodobi();
                foreach (var b in tla.Concat(platforme)) macka.PreveriKolizijo(b);
            }
            else if (macka.HitrostY != 0)
            {
                macka.Posodobi();
                foreach (var b in tla.Concat(platforme)) macka.PreveriKolizijo(b);
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

            foreach (var r in ribice)
            {
                r.Posodobi();
                r.PreveriZajetje(macka);
            }

            Invalidate();
        }

        private void ZakljuciStopnjo()
        {
            string username = Nastavitve.ImeIgralca;
            RazredBazaIgrice.PosodobiNivoInTocke(username, 2, macka.SteviloRibic);
            this.Close();
        }

        private void Lvl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Nastavitve.tipkaLevo) levo = true;
            if (e.KeyCode == Nastavitve.tipkaDesno) desno = true;
            if (e.KeyCode == Nastavitve.tipkaSkok) macka.Skoci();
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void Lvl2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Nastavitve.tipkaLevo) levo = false;
            if (e.KeyCode == Nastavitve.tipkaDesno) desno = false;
        }

        private void GameOver()
        {
            MessageBox.Show("Muca je padla s sveta! Poskusi znova.", "Game Over",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
