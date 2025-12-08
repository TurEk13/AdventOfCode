using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D08Z01 : IZadanie
{
    private readonly List<Skrzynka> _Skrzynki;
    private List<Polaczenie> _Polaczenia = new ();
    private int _Wynik;

    public D08Z01(bool daneTestowe = false)
    {
        this._Skrzynki = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\08\\proba.txt" : ".\\Dane\\2025\\08\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Skrzynki.Add(new (linia.Split(',')));
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for (int i = 0; i < this._Skrzynki.Count - 1; i++)
        {
            for (int j = i + 1; j < this._Skrzynki.Count; j++)
            {
                this._Polaczenia.Add(new (this._Skrzynki[i], this._Skrzynki[j], this._Skrzynki[i].ObliczOdleglosc(this._Skrzynki[j])));
            }
        }

        this._Polaczenia = this._Polaczenia.OrderBy(p => p.Odleglosc).ToList<Polaczenie>();

        List<HashSet<Skrzynka>> obwody = new ();
        
        foreach(Skrzynka s in this._Skrzynki)
        {
            obwody.Add([s]);
        }

        for(int i = 0; i < 1_000; i++)
        {
            (Skrzynka Z, Skrzynka Do, int _) = this._Polaczenia[i];
            HashSet<Skrzynka> ob1 = obwody.First(o => o.Contains(Z));
            HashSet<Skrzynka> ob2 = obwody.First(o => o.Contains(Do));

            if (ob1 != ob2)
            {
                ob1.UnionWith(ob2);
                obwody.Remove(ob2);
            }
        }

        obwody = obwody.OrderByDescending(o => o.Count).ToList();

        this._Wynik = obwody[0].Count * obwody[1].Count * obwody[2].Count;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Skrzynka
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public Skrzynka(string[] wspolrzedne)
        {
            this.X = Convert.ToInt32(wspolrzedne[0]);
            this.Y = Convert.ToInt32(wspolrzedne[1]);
            this.Z = Convert.ToInt32(wspolrzedne[2]);
        }

        public Int32 ObliczOdleglosc(Skrzynka s)
        {
            return Convert.ToInt32(Math.Sqrt(Math.Pow(Math.Abs(this.X - s.X), 2) + Math.Pow(Math.Abs(this.Y - s.Y), 2) + Math.Pow(Math.Abs(this.Z - s.Z), 2)));
        }
    }

    record Polaczenie(Skrzynka Start, Skrzynka Stop, Int32 Odleglosc);
}