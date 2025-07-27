using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;
using BazaIgrice;

namespace Knjiznica
{
    public class Macka
    {
        public int SvetX = 0;
        public int SvetY = 0;
        public int HitrostY = 0;
        public bool JeNaTleh = false;
        public int HitrostX = 10;
        public int MocSkoka = 15;

        public Image[] SlikeHoje;
        public Image slikeStoji;
        public Image[] SlikeSkoka;
        private int trenutniFrame = 0;
        private DateTime casZadnjeAnimacije = DateTime.Now;
        private const int MILISEKUND_NA_FRAME = 100;

        public bool sePremika = false;
        public bool gledaDesno = true;
        private bool jeSKakal = false;
        private bool jeVZraku = false;

        public int SteviloRibic = 0;

        public int Sirina => 100;
        public int Visina => 60;

        public Rectangle Obmocje => new Rectangle(SvetX, SvetY, Sirina, Visina);

        public Macka(int x, int y)
        {
            SvetX = x;
            SvetY = y;

            Image h1 = Properties.Resources.H1;
            Image h2 = Properties.Resources.H2;
            Image h3 = Properties.Resources.H3;
            Image h4 = Properties.Resources.H4;
            Image h5 = Properties.Resources.H5;
            Image h6 = Properties.Resources.H6;
            Image h7 = Properties.Resources.H7;
            Image h8 = Properties.Resources.H8;
            Image h9 = Properties.Resources.H9;
            Image h10 = Properties.Resources.H10;
            Image h11 = Properties.Resources.H11;
            Image h12 = Properties.Resources.H12;

            Image s1 = Properties.Resources.S1;
            Image s2 = Properties.Resources.S2;
            Image s3 = Properties.Resources.S3;
            Image s4 = Properties.Resources.S4;
            Image s5 = Properties.Resources.S5;
            Image s6 = Properties.Resources.S6;

            SlikeHoje = new Image[] { h1, h2, h3, h4, h5, h6, h7, h8, h9, h10, h11, h12 };
            SlikeSkoka = new Image[] { s1, s2, s3, s4, s5, s6 };

            slikeStoji = h1;
        }

        public void PremakniLevo()
        {
            SvetX = Math.Max(0, SvetX - HitrostX);
            sePremika = true;
            gledaDesno = false;
        }

        public void PremakniDesno()
        {
            SvetX += HitrostX;
            sePremika = true;
            gledaDesno = true;
        }

        public void Skoci()
        {
            if (JeNaTleh)
            {
                HitrostY = -MocSkoka;
                JeNaTleh = false;
                trenutniFrame = 0;
                jeSKakal = false;
                jeVZraku = false;
                casZadnjeAnimacije = DateTime.Now;
            }
        }

        public void Posodobi()
        {
            SvetY += HitrostY;

            if (!JeNaTleh)
                HitrostY++;  // gravity

            PosodobiAnimacijaPoCasu();
        }


        public void PosodobiAnimacijaPoCasu()
        {
            TimeSpan razlika = DateTime.Now - casZadnjeAnimacije;
            if (razlika.TotalMilliseconds >= MILISEKUND_NA_FRAME)
            {
                if (JeNaTleh && sePremika)
                {
                    trenutniFrame = (trenutniFrame + 1) % SlikeHoje.Length;
                }
                else if (!JeNaTleh)
                {
                    if (!jeSKakal)
                    {
                        trenutniFrame++;
                        if (trenutniFrame >= 4)
                        {
                            trenutniFrame = 4;
                            jeSKakal = true;
                            jeVZraku = true;
                        }
                    }
                    else if (jeVZraku)
                    {
                        trenutniFrame = 4;
                    }
                }
                casZadnjeAnimacije = DateTime.Now;
            }
        }

        public void Narisi(Graphics g, int kameraX)
        {
            Image trenutnaSlika = slikeStoji;

            if (!JeNaTleh && SlikeSkoka.Length > 0)
            {
                if (jeVZraku && trenutniFrame == 4 && SlikeSkoka.Length > 4)
                    trenutnaSlika = SlikeSkoka[4];
                else if (trenutniFrame < SlikeSkoka.Length)
                    trenutnaSlika = SlikeSkoka[trenutniFrame];
            }
            else if (JeNaTleh && sePremika && SlikeHoje.Length > 0)
            {
                trenutnaSlika = SlikeHoje[trenutniFrame % SlikeHoje.Length];
            }

            Rectangle zaslonsko = new Rectangle(SvetX - kameraX, SvetY, Sirina, Visina);

            if (gledaDesno)
            {
                g.DrawImage(trenutnaSlika, zaslonsko);
            }
            else
            {
                g.DrawImage(trenutnaSlika, zaslonsko,
                    new Rectangle(trenutnaSlika.Width, 0, -trenutnaSlika.Width, trenutnaSlika.Height), GraphicsUnit.Pixel);
            }

            sePremika = false;
        }

        public void PreveriKolizijo(Block blok)
        {
            Rectangle m = Obmocje;
            Rectangle b = blok.Obmocje;
            Rectangle prejsnji = new Rectangle(m.X, m.Y - HitrostY, m.Width, m.Height);
            JeNaTleh = false;

            if (!m.IntersectsWith(b)) return;

            bool odZgoraj = prejsnji.Bottom <= b.Top && m.Bottom >= b.Top;
            bool odSpodaj = prejsnji.Top >= b.Bottom && m.Top <= b.Bottom;
            bool zLeve = prejsnji.Right <= b.Left && m.Right >= b.Left;
            bool zDesne = prejsnji.Left >= b.Right && m.Left <= b.Right;

            if (odZgoraj && HitrostY >= 0)
            {
                SvetY = b.Top - m.Height;
                HitrostY = 0;
                JeNaTleh = true;
            }
            else if (odSpodaj && HitrostY < 0)
            {
                SvetY = b.Bottom;
                HitrostY = 0;
            }

            if (zLeve)
                SvetX = b.Left - m.Width;
            else if (zDesne)
                SvetX = b.Right;
        }

        public Rectangle GetWorldRectangle()
        {
            return new Rectangle(SvetX, SvetY, Sirina, Visina);
        }

        public bool IsAboveGround(Block blok)
        {
            Rectangle m = Obmocje;
            Rectangle b = blok.Obmocje;

            bool horizontallyAligned = m.Right > b.Left && m.Left < b.Right;
            bool verticallyClose = Math.Abs(m.Bottom - b.Top) <= 5;

            return horizontallyAligned && verticallyClose;
        }

    }

    public class Block
    {
        public Rectangle Obmocje;
        public static Image SlikaBloka = Properties.Resources.Trava2;
        public const int StandardHeight = 50;

        public Block(int x, int y, int width, int height)
        {
            Obmocje = new Rectangle(x, y, width, height);
        }

        public void Narisi(Graphics g, int kameraX)
        {
            Rectangle prikaz = new Rectangle(Obmocje.X - kameraX, Obmocje.Y, Obmocje.Width, Obmocje.Height);
            g.DrawImage(SlikaBloka, prikaz);
        }

        public static Block CreateGround(int xStart, int lengthPixels)
        {
            int screenHeight = Nastavitve.PolnZaslon
                ? Screen.PrimaryScreen.Bounds.Height
                : Nastavitve.VelikostOkna.Height;

            int y = screenHeight - StandardHeight;

            return new Block(xStart, y, lengthPixels, StandardHeight);
        }
    }

    public class CatBed
    {
        public Rectangle Obmocje;
        private Image slikaPostelje = Properties.Resources.PraznaPostelja;
        private Image slikaMackaSpi = Properties.Resources.PosteljaSaMackom;

        public bool Dosezena { get; private set; } = false;
        private DateTime casDosega;

        public CatBed(int x, int y)
        {
            Obmocje = new Rectangle(x, y, 100, 60);
        }

        public void Narisi(Graphics g, int kameraX)
        {
            var img = Dosezena ? slikaMackaSpi : slikaPostelje;
            Rectangle prikaz = new Rectangle(Obmocje.X - kameraX, Obmocje.Y, Obmocje.Width, Obmocje.Height);
            g.DrawImage(img, prikaz);
        }

        public void PreveriKolizijo(Macka macka)
        {
            Rectangle m = macka.GetWorldRectangle();
            if (!Dosezena && Obmocje.IntersectsWith(m))
            {
                Dosezena = true;
                casDosega = DateTime.Now;
            }
        }

        public bool PretekelCas()
        {
            return Dosezena && (DateTime.Now - casDosega).TotalMilliseconds >= 1000;
        }
    }

    public class Ribica
    {
        private int x, y;
        private int originalY;
        private int amplitude = 5;
        private int smer = 1;
        private int animCounter = 0;
        private const int MAX_COUNTER = 15;
        private bool zbrana = false;
        public Rectangle Obmocje;

        private Image r1 = Properties.Resources.R1;
        private Image r2 = Properties.Resources.R2;

        public Ribica(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.originalY = y;
            Obmocje = new Rectangle(x, y, 25, 15);
        }

        public void Posodobi()
        {
            if (zbrana) return;

            animCounter++;
            if (animCounter >= MAX_COUNTER)
            {
                smer *= -1;
                animCounter = 0;
            }

            y += smer;
            Obmocje.Y = y;
        }

        public void Narisi(Graphics g, int kameraX)
        {
            if (zbrana)
            {
                g.DrawImage(r2, Obmocje.X - kameraX, Obmocje.Y);
            }
            else
            {
                g.DrawImage(r1, Obmocje.X - kameraX, Obmocje.Y);
            }
        }

        public void PreveriZajetje(Macka macka)
        {
            if (!zbrana && Obmocje.IntersectsWith(macka.GetWorldRectangle()))
            {
                zbrana = true;
                macka.SteviloRibic++;
            }
        }
    }

}
