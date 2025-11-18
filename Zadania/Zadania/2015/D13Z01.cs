using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D13Z01 : IZadanie
{
    private Dictionary<(string, string), int> _relacje;
    string[] _spisOsob;
    private List<Stol> _stol;
    private List<StolT> _stolT;
    private Int64 _maksymalnaWartosc;
    private bool _daneTestowe;

    public D13Z01(bool daneTestowe = false)
    {
        this._relacje = new Dictionary<(string Osoba1, string Osoba2), int>();
        HashSet<string> spisOsob = new ();
        this._stol = new();
        this._stolT = new();
        this._maksymalnaWartosc = 0;
        this._daneTestowe = daneTestowe;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\13\\proba.txt" : ".\\Dane\\2015\\13\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] tekst;

        while((linia = sr.ReadLine()) is not null)
        {
            tekst = linia.Split(' ');
            this._relacje[(tekst[0], tekst[10][..^1])] = tekst[2].Equals("lose") ? Convert.ToInt32($"-{tekst[3]}") : Convert.ToInt32(tekst[3]);
            
            spisOsob.Add(tekst[0]);
            spisOsob.Add(tekst[10][..^1]);
        }

        sr.Close(); fs.Close();

        this._spisOsob = spisOsob.ToArray<string>();
    }

    public void RozwiazanieZadania()
    {
        if (this._daneTestowe)
        {
            this.ZnajdzParyOsobT();
            this.ZnajdzWartoscUstawieniaT();
        }

        if(!this._daneTestowe)
        {
            this.ZnajdzParyOsob();
            this.ZnajdzWartoscUstawienia();
        }
    }

    private void ZnajdzParyOsobT()
    {
        Permutacje<string> trasy = new();
        List<List<string>> trasyy = trasy.ZnajdzPermutacje(this._spisOsob);

        foreach(List<string> t in trasyy)
        {
            this._stolT.Add(new(t[0], t[1], t[2], t[3]));
        }
    }

    private void ZnajdzWartoscUstawieniaT()
    {
        Int64 WartoscRelacji;

        foreach(StolT t in this._stolT)
        {
            WartoscRelacji = 0;
            WartoscRelacji += this._relacje[(t.P1, t.P2)];
            WartoscRelacji += this._relacje[(t.P2, t.P1)];

            WartoscRelacji += this._relacje[(t.P2, t.P3)];
            WartoscRelacji += this._relacje[(t.P3, t.P2)];

            WartoscRelacji += this._relacje[(t.P3, t.P4)];
            WartoscRelacji += this._relacje[(t.P4, t.P3)];

            WartoscRelacji += this._relacje[(t.P4, t.P1)];
            WartoscRelacji += this._relacje[(t.P1, t.P4)];

            if(this._maksymalnaWartosc < WartoscRelacji)
            {
                this._maksymalnaWartosc = WartoscRelacji;
            }
        }
    }

    private void ZnajdzParyOsob()
    {
        Permutacje<string> trasy = new();
        List<List<string>> trasyy = trasy.ZnajdzPermutacje(this._spisOsob);

        foreach(List<string> t in trasyy)
        {
            this._stol.Add(new(t[0], t[1], t[2], t[3], t[4], t[5], t[6], t[7]));
        }
    }

    private void ZnajdzWartoscUstawienia()
    {
        Int64 WartoscRelacji;
        
        foreach(Stol t in this._stol)
        {
            WartoscRelacji = 0;
            WartoscRelacji += this._relacje[(t.P1, t.P2)];
            WartoscRelacji += this._relacje[(t.P2, t.P1)];

            WartoscRelacji += this._relacje[(t.P2, t.P3)];
            WartoscRelacji += this._relacje[(t.P3, t.P2)];

            WartoscRelacji += this._relacje[(t.P3, t.P4)];
            WartoscRelacji += this._relacje[(t.P4, t.P3)];

            WartoscRelacji += this._relacje[(t.P4, t.P5)];
            WartoscRelacji += this._relacje[(t.P5, t.P4)];

            WartoscRelacji += this._relacje[(t.P5, t.P6)];
            WartoscRelacji += this._relacje[(t.P6, t.P5)];

            WartoscRelacji += this._relacje[(t.P6, t.P7)];
            WartoscRelacji += this._relacje[(t.P7, t.P6)];

            WartoscRelacji += this._relacje[(t.P7, t.P8)];
            WartoscRelacji += this._relacje[(t.P8, t.P7)];

            WartoscRelacji += this._relacje[(t.P8, t.P1)];
            WartoscRelacji += this._relacje[(t.P1, t.P8)];

            if(this._maksymalnaWartosc < WartoscRelacji)
            {
                this._maksymalnaWartosc = WartoscRelacji;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._maksymalnaWartosc.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Stol(string P1, string P2, string P3, string P4, string P5, string P6, string P7, string P8);
    
    // Do testu
    private record StolT(string P1, string P2, string P3, string P4);

    class Permutacje<T>
    {
        public List<List<T>> ZnajdzPermutacje(T[] lista)
        {
            List<List<T>> wynik = new ();
            this.Wykonaj(wynik, lista, 0);
            return wynik;
        }

        private void Wykonaj(List<List<T>> wynik, T[] lista, int start)
        {
            if(start == lista.Length)
            {
                wynik.Add(new List<T>(lista));
                return;
            }

            for(int i = start; i < lista.Length; i++)
            {
                this.Zamien(ref lista[start], ref lista[i]);
                this.Wykonaj(wynik, lista, start + 1); 
                this.Zamien(ref lista[start], ref lista[i]);
            }
        }

        private void Zamien(ref T start, ref T stop)
        {
            T tmp = start;
            start = stop;
            stop = tmp;
        }
    }
}