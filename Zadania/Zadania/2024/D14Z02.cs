using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace Zadania._2024;

public class D14Z02 : IZadanie
{
	private List<Robot> roboty;
    private List<Point> lokalizacjeRobotow;
    private int szerokosc;
    private int wysokosc;
    private int wynik;

    public D14Z02(bool daneTestowe = false)
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
        this.wynik = this.PrzesunRoboty();
    }

    public string PokazRozwiazanie()
    {
        return this.wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private int PrzesunRoboty()
    {
        StringBuilder sb = new();
        StringBuilder wiersz = new();
        bool koniec = false;

        for (int i = 0; i < 1_000_000; i++)
        {
            this.lokalizacjeRobotow.Clear();
            sb.Clear();

            foreach (Robot r in this.roboty)
            {
                r.PrzesunRobota(this.szerokosc, this.wysokosc);
                this.lokalizacjeRobotow.Add(r.ZwrocPunkt());
            }

            for (int y = 0; y < this.wysokosc; y++)
            {
                wiersz.Clear();

                for (int x = 0; x < this.szerokosc; x++)
                {
                    wiersz.Append(this.lokalizacjeRobotow.Contains(new(x, y)) ? "#" : '.');
                }

                if (wiersz.ToString().Contains("#########"))
                {
                    koniec = true;
                }
                sb.AppendLine(wiersz.ToString());
            }

            if (koniec)
            {
                File.WriteAllText("choinka.txt", sb.ToString());
                return i + 1;
            }
        }

        return -1;
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