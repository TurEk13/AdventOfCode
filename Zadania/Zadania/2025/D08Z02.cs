using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D08Z02 : IZadanie
{
    private readonly List<Skrzynka> _Skrzynki;

    private List<Polaczenie> _Polaczenia = new ();

    private Int64 _Wynik;

    public D08Z02(bool daneTestowe = false)
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

    private void ZnajdzPolaczenia()
    {
        for (int i = 0; i < this._Skrzynki.Count - 1; i++)
        {
            for (int j = i + 1; j < this._Skrzynki.Count; j++)
            {
                this._Polaczenia.Add(new (this._Skrzynki[i], this._Skrzynki[j], this._Skrzynki[i].ObliczOdleglosc(this._Skrzynki[j])));
            }
        }
    }

    public void RozwiazanieZadania()
    {
        this.ZnajdzPolaczenia();
        List<HashSet<Skrzynka>> obwody = new ();
        bool szukajDalej = true;
        HashSet<Skrzynka> obwod1;
        HashSet<Skrzynka> obwod2;

        this._Polaczenia = this._Polaczenia.OrderBy(p => p.Odleglosc).ToList<Polaczenie>();

        
        foreach(Skrzynka s in this._Skrzynki)
        {
            obwody.Add([s]);
        }

        for(int i = 0; szukajDalej; i++)
        {
            (Skrzynka Z, Skrzynka Do, Int64 _) = this._Polaczenia[i];
            
            obwod1 = obwody.First(o => o.Contains(Z));
            obwod2 = obwody.First(o => o.Contains(Do));
            
            if (obwod1 != obwod2)
            {
                obwod1.UnionWith(obwod2);
                obwody.Remove(obwod2);
            }

            if(obwody.Count == 1)
            {
                this._Wynik =  Z.X * Do.X;
                szukajDalej = false;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Skrzynka
    {
        public Int64 X { get; private set; }
        public Int64 Y { get; private set; }
        public Int64 Z { get; private set; }

        public Skrzynka(string[] wspolrzedne)
        {
            this.X = Convert.ToInt64(wspolrzedne[0]);
            this.Y = Convert.ToInt64(wspolrzedne[1]);
            this.Z = Convert.ToInt64(wspolrzedne[2]);
        }

        public Int64 ObliczOdleglosc(Skrzynka s)
        {
            return Convert.ToInt64(Math.Sqrt(Math.Pow(Math.Abs(this.X - s.X), 2) + Math.Pow(Math.Abs(this.Y - s.Y), 2) + Math.Pow(Math.Abs(this.Z - s.Z), 2)));
        }
    }

    record Polaczenie(Skrzynka Start, Skrzynka Stop, Int64 Odleglosc);
}