using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public partial class D09Z01 : IZadanie
{
    private Int64 _Wynik;
    private List<Punkt> _Punkty;

    public D09Z01(bool daneTestowe = false)
    {
        this._Punkty = new ();
        this._Wynik = Int64.MinValue;;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\09\\proba.txt" : ".\\Dane\\2025\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Punkty.Add(new (linia.Split(',')));
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        Int64 pole;

        for(int i = 0; i < this._Punkty.Count - 1; i++)
        {
            for(int j = i + 1; j < this._Punkty.Count; j++)
            {
                pole = (Math.Abs(this._Punkty[i].X - this._Punkty[j].X) + 1) * (Math.Abs(this._Punkty[i].Y - this._Punkty[j].Y) + 1);

                if(this._Wynik < pole)
                {
                    this._Wynik = pole;
                }
            }
        }
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
}