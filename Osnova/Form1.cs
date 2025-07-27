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
using Knjiznica;

namespace Osnova
{
    public partial class Lvl1 : Form
    {
        private Image ozadje;
        private int premikKamere = 0;
        private Macka macka;
        private Timer timer;
        private bool levo, desno;
        private Block tlo;
        private CatBed postelja;
        private int stTock = 0; // hardcoded for now
        private bool fadeOut = false;
        private int fadeAlpha = 0;
        private int smrtnaMejaY;
        private bool gameOverTriggered = false;


        public Lvl1()
        {
            this.Text = "Mačja Pustolovščina - Igra";
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

            ozadje = Properties.Resources.test;

            tlo = Block.CreateGround(0, 5000);  // ONE long block now

            int groundY = tlo.Obmocje.Y;
            macka = new Macka(100, groundY - 100);
            postelja = new CatBed(4600, groundY - Block.StandardHeight);
            smrtnaMejaY = groundY + 300;

            this.Paint += OsnovaForm_Paint;
            this.KeyDown += OsnovaForm_KeyDown;
            this.KeyUp += OsnovaForm_KeyUp;

            timer = new Timer { Interval = 16 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void OsnovaForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int ozX = ozadje.Width;

            for (int x = -premikKamere % ozX; x < this.ClientSize.Width; x += ozX)
            {
                g.DrawImage(ozadje, x, 0, ozX, this.ClientSize.Height);
            }

            tlo.Narisi(g, premikKamere);

            if (!fadeOut)
                macka.Narisi(g, premikKamere);  // ✅ FIXED: kameraX passed in

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
            RazredBazaIgrice.PosodobiNivoInTocke(username, 1, stTock);
            this.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (levo) macka.PremakniLevo();
            if (desno) macka.PremakniDesno();

            bool aboveGround = macka.IsAboveGround(tlo);

            if (!aboveGround)
            {
                macka.JeNaTleh = false;
                macka.Posodobi();
                macka.PreveriKolizijo(tlo);
            }
            else if (macka.HitrostY != 0)
            {
                macka.Posodobi();
                macka.PreveriKolizijo(tlo);
            }
            else
            {
                macka.SvetY = tlo.Obmocje.Y - macka.Visina;
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

            Invalidate();
        }



        private void OsnovaForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Nastavitve.tipkaLevo) levo = true;
            if (e.KeyCode == Nastavitve.tipkaDesno) desno = true;
            if (e.KeyCode == Nastavitve.tipkaSkok) macka.Skoci();
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void OsnovaForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Nastavitve.tipkaLevo) levo = false;
            if (e.KeyCode == Nastavitve.tipkaDesno) desno = false;
        }

        public void PremakniKamero(int delta)
        {
            premikKamere += delta;
            Invalidate();
        }

        private void GameOver()
        {
            MessageBox.Show("Muca je padla s sveta! Poskusi znova.", "Game Over",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }


}
