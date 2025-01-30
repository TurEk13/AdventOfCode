using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D15Z01 : IZadanie
{
    private List<char[]> mapa;
    private string ruchy;
    private Point robot;
    private Kierunek kierunki;
    public D15Z01(bool daneTestowe = false)
    {
        int y = 0;
        this.mapa = new();
        this.ruchy = null;
        this.kierunki = new();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\15\\proba.txt" : ".\\Dane\\2024\\15\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        string linia;

        //Wczytanie mapy
        linia = sr.ReadLine();
        while (linia.Contains('#'))
        {
            this.mapa.Add(linia.ToCharArray());
            if (this.mapa[^1].Contains('@'))
            {
                this.robot = new(linia.IndexOf('@'), y);
            }
            linia = sr.ReadLine();
            y++;
        }

        //Wczytanie ruchów
        while ((linia = sr.ReadLine()) != null)
        {
            this.ruchy += linia;
        }
    }

    public void RozwiazanieZadania()
    {
       for (int i = 0; i < this.ruchy.Length; i++)
        {
            this.robot = this.Przesun(robot, this.kierunki[this.ruchy[i]]);
        }
    }

    public string PokazRozwiazanie()
    {
        return this.GPS().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private Point Przesun(Point punkDoPrzesuniecia, Point kierunek)
    {
        Point nowyPunkt = punkDoPrzesuniecia.Dodaj(kierunek);

        switch (this.mapa[nowyPunkt.Y][nowyPunkt.X])
        {
            case '.':
                this.mapa[nowyPunkt.Y][nowyPunkt.X] = this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X];
                this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X] = '.';
                return nowyPunkt;
            case 'O':
                if (this.CzyPrzesunac(nowyPunkt, kierunek))
                {
                    this.PrzesunPrzeszkode(nowyPunkt, kierunek);
                    this.mapa[nowyPunkt.Y][nowyPunkt.X] = this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X];
                    this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X] = '.';
                    return nowyPunkt;
                }
                return punkDoPrzesuniecia;
            case '#':
            default:
                return punkDoPrzesuniecia;
        }
    }

    private void PrzesunPrzeszkode(Point punkDoPrzesuniecia, Point kierunek)
    {
        Point nowyPunkt = punkDoPrzesuniecia.Dodaj(kierunek);

        switch (this.mapa[nowyPunkt.Y][nowyPunkt.X])
        {
            case '.':
                this.mapa[nowyPunkt.Y][nowyPunkt.X] = this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X];
                this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X] = '.';
                break;
            case 'O':
                this.PrzesunPrzeszkode(nowyPunkt, kierunek);
                this.mapa[nowyPunkt.Y][nowyPunkt.X] = this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X];
                this.mapa[punkDoPrzesuniecia.Y][punkDoPrzesuniecia.X] = '.';
                break;
            case '#':
            default:
                break;
        }
    }

    private bool CzyPrzesunac(Point punktDoPrzesuniecia, Point kierunek)
    {
        Point nowyPunkt = punktDoPrzesuniecia.Dodaj(kierunek);

        switch (this.mapa[nowyPunkt.Y][nowyPunkt.X])
        {
            case '.':
                return true;
            case 'O':
                return this.CzyPrzesunac(nowyPunkt, kierunek);
            case '#':
            default:
                return false;
        }
    }

    public Int64 GPS()
    {
        Int64 gps = 0;

        for (int y = 1; y < this.mapa.Count; y++)
        {
            for (int x = 1; x < this.mapa[y].Length; x++)
            {
                if (this.mapa[y][x] == 'O' || this.mapa[y][x] == '[')
                {
                    gps += y * 100 + x;
                }
            }
        }

        return gps;
    }

    private class Kierunek
    {
        private Point[] kierunki;

        public Point this[char x]
        {
            get
            {
                switch (x)
                {
                    case '^':
                        return this.kierunki[0];
                    case '>':
                        return this.kierunki[1];
                    case 'v':
                        return this.kierunki[2];
                    case '<':
                        return this.kierunki[3];
                    default:
                        return new Point(0, 0);
                }
            }
        }

        public Kierunek()
        {
            this.kierunki = [new(0, -1), new(1, 0), new(0, 1), new(-1, 0)];
        }
    }
}

public static partial class Rozszerzenia
{
    public static Point Dodaj(this Point a, Point b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Point Odejmij(this Point a, Point b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }
}