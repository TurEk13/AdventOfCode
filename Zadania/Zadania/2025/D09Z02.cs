using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D09Z02 : IZadanie
{
    private Int64 _Wynik;

    private List<Punkt> _Punkty;

    private List<Krawedz> _Krawedzie;

    public D09Z02(bool daneTestowe = false)
    {
        this._Punkty = new ();
        this._Krawedzie = new ();
        this._Wynik = Int64.MinValue;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\09\\proba.txt" : ".\\Dane\\2025\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Punkty.Add(new (linia.Split(',')));
        }

        for(int i = 0; i < this._Punkty.Count - 1; i++)
        {
            this._Krawedzie.Add(new(this._Punkty[i], this._Punkty[i + 1]));
        }

        this._Krawedzie.Add(new (this._Punkty[^1], this._Punkty[0]));

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        Int64 pole;

        for(int i = 0; i < this._Punkty.Count - 1; i++)
        {
            for(int j = i + 1; j < this._Punkty.Count; j++)
            {
                pole = this.ObliczPole(this._Punkty[i], this._Punkty[j]);

                if(pole < this._Wynik)
                {
                    continue;
                }

                if(!this._Krawedzie.Any(krawedz => krawedz.Zawiera(this._Punkty[i], this._Punkty[j])))
                {
                    if(pole > this._Wynik)
                    {
                        this._Wynik = pole;
                    }
                }
            }
        }
    }

    private Int64 ObliczPole(Punkt A, Punkt B)
    {
        return (Math.Abs(A.X - B.X) + 1) * (Math.Abs(A.Y - B.Y) + 1);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Punkt
    {
        public Int64 X { get; private set; }
        public Int64 Y { get; private set; }

        public Punkt(string[] punkty)
        {
            this.X = Convert.ToInt64(punkty[0]);
            this.Y = Convert.ToInt64(punkty[1]);
        }
    }

    record Krawedz
    {
        public Punkt Poczatek { get; private set; }
        public Punkt Koniec { get; private set; }

        public Krawedz(Punkt poczatek, Punkt koniec)
        {
            this.Poczatek = poczatek;
            this.Koniec = koniec;
        }

        public bool Zawiera(Punkt pointA, Punkt pointB)
        {
            Int64 pMinX = Math.Min(pointA.X, pointB.X);
            Int64 pMaxX = Math.Max(pointA.X, pointB.X);
            Int64 pMinY = Math.Min(pointA.Y, pointB.Y);
            Int64 pMaxY = Math.Max(pointA.Y, pointB.Y);
    
            Int64 kMinX = Math.Min(this.Poczatek.X, this.Koniec.X);
            Int64 kMaxX = Math.Max(this.Poczatek.X, this.Koniec.X);
            Int64 kMinY = Math.Min(this.Poczatek.Y, this.Koniec.Y);
            Int64 kMaxY = Math.Max(this.Poczatek.Y, this.Koniec.Y);
    
            return kMaxX > pMinX && kMinX < pMaxX && kMaxY > pMinY && kMinY < pMaxY;
        }
    }
}