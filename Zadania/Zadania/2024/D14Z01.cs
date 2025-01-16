using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace Zadania._2024;

public class D14Z01 : IZadanie
{
    private List<Robot> roboty;
    private List<Point> lokalizacjeRobotow;
    private int szerokosc;
    private int wysokosc;
    private Int64 wynik;

    public D14Z01(bool daneTestowe = false)
    {
        this.roboty = new();
        this.lokalizacjeRobotow = new();
        this.szerokosc = 101;
        this.wysokosc = 103;
        this.wynik = 0;
        FileStream tekst = new(daneTestowe ? ".\\Dane\\2024\\14\\proba.txt" : ".\\Dane\\2024\\14\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(tekst);
        string linia;
        string[] pv, p, v;

        while ((linia = sr.ReadLine()) != null)
        {
            pv = linia.Split(' ');
            p = pv[0].Substring(2).Split(',');
            v = pv[1].Substring(2).Split(',');
            roboty.Add(new(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(v[0]), Convert.ToInt32(v[1])));
        }
    }

    public void RozwiazanieZadania()
    {
        int srodekSZ = this.szerokosc / 2;
        int srodekW = this.wysokosc / 2;

        for (int i = 0; i < 100; i++)
        {
            foreach (Robot r in this.roboty)
            {
                r.PrzesunRobota(this.szerokosc, this.wysokosc);
            }
        }

        Point tmpP;
        foreach (Robot r in this.roboty)
        {
            tmpP = r.ZwrocPunkt();
            if (tmpP.X == srodekSZ || tmpP.Y == srodekW)
            {
                continue;
            }
            else
            {
                this.lokalizacjeRobotow.Add(tmpP);
            }
        }

        Int64 sumaLG = 0, sumaPG = 0, sumaLD = 0, sumaPD = 0;


        foreach (Point p in this.lokalizacjeRobotow)
        {
            //lewa góra
            if (0 <= p.X && p.X < srodekSZ && 0 <= p.Y && p.Y < srodekW)
            {
                sumaLG++;
            }

            //prawa góra
            if (srodekSZ < p.X && p.X < this.szerokosc && 0 <= p.Y && p.Y < srodekW)
            {
                sumaPG++;
            }

            //lewy dół
            if (0 <= p.X && p.X < srodekSZ && srodekW < p.Y && p.Y < this.wysokosc)
            {
                sumaLD++;
            }

            //prawy dół
            if (srodekSZ < p.X && p.X < this.szerokosc && srodekW < p.Y && p.Y < this.wysokosc)
            {
                sumaPD++;
            }
        }

        this.wynik = sumaLG * sumaPG * sumaLD * sumaPD;
    }

    public string PokazRozwiazanie()
    {
        return this.wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private record Predkosc(int predkoscX, int predkoscY);
    
	private class Robot
    {
        private Point lokalizacja;
        private Predkosc predkosc;
        
		public Robot(int lokalizacjaX, int lokalizacjaY, int predkoscX, int predkoscY)
        {
            this.lokalizacja = new(lokalizacjaX, lokalizacjaY);
            this.predkosc = new(predkoscX, predkoscY);
        }

        public Point ZwrocPunkt()
        {
            return this.lokalizacja;
        }

        public void PrzesunRobota(int szerokoscPola, int wysokoscPola)
        {
            int nowyX = this.lokalizacja.X + this.predkosc.predkoscX;
            int nowyY = this.lokalizacja.Y + this.predkosc.predkoscY;

            if (0 <= nowyX && nowyX < szerokoscPola)
            {
                this.lokalizacja.X = nowyX;
            }
            else
            {
                if (nowyX <= 0)
                {
                    this.lokalizacja.X = nowyX + szerokoscPola;
                }

                if (szerokoscPola <= nowyX)
                {
                    this.lokalizacja.X = nowyX - szerokoscPola;
                }
            }

            if (0 <= nowyY && nowyY < wysokoscPola)
            {
                this.lokalizacja.Y = nowyY;
            }
            else
            {
                if (wysokoscPola <= nowyY)
                {
                    this.lokalizacja.Y = nowyY - wysokoscPola;
                }

                if (nowyY <= 0)
                {
                    this.lokalizacja.Y = nowyY + wysokoscPola;
                }
            }
        }
    }
}