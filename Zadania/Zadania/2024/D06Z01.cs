using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D06Z01 : IZadanie
{
    private List<char[]> mapa;
    private Kierunek kierunek;
    private Straznik straznik;
    private Straznik nowyStraznik;
    private int wysokosc, szerokosc, zwrot;
    private int dlugoscDrogi;
    private List<PrzebytaDroga> przebytaDroga;

    public D06Z01(bool daneTestowe = false)
    {
        this.mapa = new();
        this.kierunek = new();
        this.straznik = null;
        this.nowyStraznik = null;
        this.wysokosc = 0;
        this.szerokosc = 0;
        this.zwrot = 0;
        this.dlugoscDrogi = 1;
        this.przebytaDroga = new();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\06\\proba.txt" : ".\\Dane\\2024\\06\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs!);
        string x = "null";

        while ((x = sr.ReadLine()) != null)
        {
            if (x.Contains('^'))
            {
                this.straznik = new(x.IndexOf('^'), this.wysokosc);
                this.szerokosc = x.Length;
            }

            this.mapa.Add(x.ToCharArray());
            this.wysokosc++;
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
       this.nowyStraznik = this.straznik + this.kierunek[zwrot];
        while (this.nowyStraznik.X >= 0 & this.nowyStraznik.Y >= 0 && this.nowyStraznik.X < this.szerokosc && this.nowyStraznik.Y < this.wysokosc)
        {
            if (this.mapa[this.nowyStraznik.Y][this.nowyStraznik.X] != '#')
            {
                if (this.mapa[this.nowyStraznik.Y][this.nowyStraznik.X] != 'X')
                {
                    this.dlugoscDrogi++;
                }

                this.mapa[this.nowyStraznik.Y][this.nowyStraznik.X] = '^';
                this.mapa[this.straznik.Y][this.straznik.X] = 'X';
                this.straznik = this.nowyStraznik;
            }

            if (this.mapa[this.nowyStraznik.Y][this.nowyStraznik.X] == '#')
            {
                this.zwrot++;
            }

            this.nowyStraznik = this.straznik + this.kierunek[this.zwrot % 4];
        }
    }

    public string PokazRozwiazanie()
    {
        return this.dlugoscDrogi.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record PrzebytaDroga(Straznik straznik, int kierunek);

    private class Straznik
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Straznik(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Straznik operator +(Straznik straznik, Kierunek.Kierunki kierunek) => new Straznik(straznik.X + kierunek.X, straznik.Y + kierunek.Y);
    }

    private class Kierunek
    {
        public record Kierunki(int X, int Y);

        private Kierunki[] kierunki;

        public Kierunki this[int x]
        {
            get
            {
                return this.kierunki[x];
            }
        }

        public Kierunek()
        {
            this.kierunki = [new Kierunki(0, -1), new Kierunki(1, 0), new Kierunki(0, 1), new Kierunki(-1, 0)];
        }
    }
}