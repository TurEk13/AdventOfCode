using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Zadania._2024;

public class D15Z02 : IZadanie
{
    /// <summary>
    /// Mapa do poruszania się robota
    /// </summary>
    private List<char[]> mapa;

    /// <summary>
    /// Spis ruchów robota
    /// </summary>
    private string ruchy;

    /// <summary>
    /// Lokalizacja robota
    /// </summary>
    private Point robot;

    /// <summary>
    /// Odzytuanie kierunku ze spisu ruchów
    /// </summary>
    private Kierunek kierunki;

    public D15Z02(bool daneTestowe = false)
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
            linia = linia.Replace("#", "##");
            linia = linia.Replace("O", "[]");
            linia = linia.Replace(".", "..");
            linia = linia.Replace("@", "@.");

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

    /// <summary>
    /// Przesunięcie robota
    /// </summary>
    /// <param name="lokalizacjaRobota">Lokalizacja robota</param>
    /// <param name="kierunek">Kierunek przesunięcia robota</param>
    /// <returns></returns>
    private Point Przesun(Point lokalizacjaRobota, Point kierunek)
    {
        Point docelowaLokalizacjaRobota = lokalizacjaRobota.Dodaj(kierunek);

        switch(this.mapa[docelowaLokalizacjaRobota.Y][docelowaLokalizacjaRobota.X])
        {
            case '#':
                return lokalizacjaRobota;
            case '.':
                this.mapa[docelowaLokalizacjaRobota.Y][docelowaLokalizacjaRobota.X] = this.mapa[lokalizacjaRobota.Y][lokalizacjaRobota.X];
                this.mapa[lokalizacjaRobota.Y][lokalizacjaRobota.X] = '.';
                return docelowaLokalizacjaRobota;
            case '[':
            case ']':
                if(kierunek.Y == 0)
                {
                    if(this.CzyPrzesunac(docelowaLokalizacjaRobota, kierunek))
                    {
                        this.PrzesunPrzeszkode(docelowaLokalizacjaRobota, kierunek);

                        this.mapa[docelowaLokalizacjaRobota.Y][docelowaLokalizacjaRobota.X] = this.mapa[lokalizacjaRobota.Y][lokalizacjaRobota.X];
                        this.mapa[lokalizacjaRobota.Y][lokalizacjaRobota.X] = '.';

                        return docelowaLokalizacjaRobota;
                    }
                }
                else if(kierunek.X == 0)
                {
                    Point drugipunktPrzeszkody = docelowaLokalizacjaRobota.Dodaj(this.mapa[docelowaLokalizacjaRobota.Y][docelowaLokalizacjaRobota.X] == ']' ? this.kierunki['<'] : this.kierunki['>']);

                    if(this.CzyPrzesunac(docelowaLokalizacjaRobota, kierunek) && this.CzyPrzesunac(drugipunktPrzeszkody, kierunek))
                    {
                        //

                        this.PrzesunPrzeszkode(docelowaLokalizacjaRobota, kierunek);
                        this.PrzesunPrzeszkode(drugipunktPrzeszkody, kierunek);

                        this.mapa[docelowaLokalizacjaRobota.Y][docelowaLokalizacjaRobota.X] = this.mapa[lokalizacjaRobota.Y][lokalizacjaRobota.X];
                        this.mapa[lokalizacjaRobota.Y][lokalizacjaRobota.X] = '.';
                        this.mapa[drugipunktPrzeszkody.Y][drugipunktPrzeszkody.X] = '.';

                        return docelowaLokalizacjaRobota;
                    }
                }
                return lokalizacjaRobota;
        }

        return lokalizacjaRobota;
    }

    /// <summary>
    /// Przesunięcie przeszkody
    /// </summary>
    /// <param name="lokalizacjaPrzeszkody">Lokalizacja przeszkody</param>
    /// <param name="kierunek">Kierunek przesunięcia przeszkody</param>
    private void PrzesunPrzeszkode(Point lokalizacjaPrzeszkody, Point kierunek)
    {
        Point docelowaLokalizacjaPrzeszkody = lokalizacjaPrzeszkody.Dodaj(kierunek);

        switch(this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X])
        {
            case '.':
                this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X] = this.mapa[lokalizacjaPrzeszkody.Y][lokalizacjaPrzeszkody.X];
                break;
            case '[':
            case ']':
                if(kierunek.Y == 0)
                {
                    this.PrzesunPrzeszkode(docelowaLokalizacjaPrzeszkody, kierunek);

                    this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X] = this.mapa[lokalizacjaPrzeszkody.Y][lokalizacjaPrzeszkody.X];
                    this.mapa[lokalizacjaPrzeszkody.Y][lokalizacjaPrzeszkody.X] = '.';
                }
                else if(kierunek.X == 0)
                {
                    Point drugiDocelowyPunktPrzeszkody = docelowaLokalizacjaPrzeszkody.Dodaj(this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X] == ']' ? this.kierunki['<'] : this.kierunki['>']);

                    Point drugaLokalizacjaPrzeszkody = drugiDocelowyPunktPrzeszkody.Dodaj(this.mapa[drugiDocelowyPunktPrzeszkody.Y][drugiDocelowyPunktPrzeszkody.X] == ']' ? this.kierunki['<'] : this.kierunki['>']);

                    this.PrzesunPrzeszkode(docelowaLokalizacjaPrzeszkody, kierunek);
                    this.PrzesunPrzeszkode(drugiDocelowyPunktPrzeszkody, kierunek);

                    this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X] = this.mapa[lokalizacjaPrzeszkody.Y][lokalizacjaPrzeszkody.X];
                    this.mapa[drugiDocelowyPunktPrzeszkody.Y][drugiDocelowyPunktPrzeszkody.X] = this.mapa[drugaLokalizacjaPrzeszkody.Y][drugaLokalizacjaPrzeszkody.X];

                    this.mapa[lokalizacjaPrzeszkody.Y][lokalizacjaPrzeszkody.X] = '.';
                    this.mapa[drugiDocelowyPunktPrzeszkody.Y][drugiDocelowyPunktPrzeszkody.X] = '.';
                }
                break;
        }
    }

    /// <summary>
    /// Sprawdzenie czy można przesunąć przeszkodę
    /// </summary>
    /// <param name="lokalizacjaPrzeszkody">Lokalizacja przeszkody</param>
    /// <param name="kierunek">Kierunek przesunięcia przeszkody</param>
    private bool CzyPrzesunac(Point lokalizacjaPrzeszkody, Point kierunek)
    {
        Point docelowaLokalizacjaPrzeszkody = lokalizacjaPrzeszkody.Dodaj(kierunek);

        switch(this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X])
        {
            case '#':
                return false;
            case '.':
                return true;
            case '[':
            case ']':
                if(kierunek.Y == 0)
                {
                    return this.CzyPrzesunac(docelowaLokalizacjaPrzeszkody, kierunek);
                }
                else if(kierunek.X == 0)
                {
                    Point drugipunktPrzeszkody = docelowaLokalizacjaPrzeszkody.Dodaj(this.mapa[docelowaLokalizacjaPrzeszkody.Y][docelowaLokalizacjaPrzeszkody.X] == ']' ? this.kierunki['<'] : this.kierunki['>']);

                    return this.CzyPrzesunac(docelowaLokalizacjaPrzeszkody, kierunek) && this.CzyPrzesunac(drugipunktPrzeszkody, kierunek);
                }
                return false;
        }

        return false;
    }

    public Int64 GPS()
    {
        Int64 gps = 0;

        for (int y = 1; y < this.mapa.Count; y++)
        {
            for (int x = 1; x < this.mapa[y].Length; x++)
            {
                if (this.mapa[y][x] == '[')
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