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
    private Dictionary<Skrzynka, List<(Skrzynka Id, Int32 Odleglosc)>> _Siec;

    public D08Z01(bool daneTestowe = false)
    {
        this._Skrzynki = new ();
        this._Siec = new ();
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
        int odleglosc;
        HashSet<Skrzynka> sk = new ();

        for (int i = 0; i < this._Skrzynki.Count - 1; i++)
        {
            for (int j = i + 1; j < this._Skrzynki.Count; j++)
            {
                odleglosc = this.ObliczOdleglosc(this._Skrzynki[i], this._Skrzynki[j]);

                this._Polaczenia.Add(new (this._Skrzynki[i], this._Skrzynki[j], odleglosc));
            }
        }

        this._Polaczenia = this._Polaczenia.OrderBy(p => p.Odleglosc).ToList<Polaczenie>();

        this._Polaczenia = this._Polaczenia.Take(11).ToList<Polaczenie>();

        for(int i = 0; i < this._Polaczenia.Count; i++)
        {
            if(!this._Siec.ContainsKey(this._Polaczenia[i].Start))
            {
                this._Siec[this._Polaczenia[i].Start] = new ();
            }

            if(!this._Siec.ContainsKey(this._Polaczenia[i].Stop))
            {
                this._Siec[this._Polaczenia[i].Stop] = new ();
            }

            //sk.Add(this._Polaczenia[i].Start);
            //sk.Add(this._Polaczenia[i].Stop);

            this._Siec[this._Polaczenia[i].Start].Add(new (this._Polaczenia[i].Stop, this._Polaczenia[i].Odleglosc));
            
            //this._Siec[this._Polaczenia[i].Stop].Add(new (this._Polaczenia[i].Start, this._Polaczenia[i].Odleglosc));
        }


    }

    private Int32 ObliczOdleglosc(Skrzynka s1, Skrzynka s2)
    {
        return Convert.ToInt32(Math.Sqrt(Math.Pow(Math.Abs(s1.X - s2.X), 2) + Math.Pow(Math.Abs(s1.Y - s2.Y), 2) + Math.Pow(Math.Abs(s1.Z - s2.Z), 2)));
    }

    public string PokazRozwiazanie()
    {
        return 0.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
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

        public override string ToString()
        {
            return $"X: {this.X}, Y: {this.Y}, Z: {this.Z}";
        }
    }

    record Polaczenie(Skrzynka Start, Skrzynka Stop, Int32 Odleglosc);
}