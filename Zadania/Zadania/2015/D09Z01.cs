using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D09Z01 : IZadanie
{
    private List<Kierunki> _kierunki;
    private List<Trasa> _trasy;
    private List<TrasaT> _trasyT;
    private List<int> _odleglosci;
    private bool _daneTestowe;

    public D09Z01(bool daneTestowe = false)
    {
        this._kierunki = new();
        this._trasy = new();
        this._trasyT = new();
        this._odleglosci = new();
        this._daneTestowe = daneTestowe;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\09\\proba.txt" : ".\\Dane\\2015\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] tekst;

        while((linia = sr.ReadLine()) is not null)
        {
            tekst = linia.Split(' ');
            this._kierunki.Add(new(tekst[0], tekst[2], Convert.ToInt32(tekst[4])));
            this._kierunki.Add(new(tekst[2], tekst[0], Convert.ToInt32(tekst[4])));
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        if (this._daneTestowe)
        {
            this.ZnajdzTrasyT();
            this.ZnajdzOdleglosciT();
        }

        if(!this._daneTestowe)
        {
            this.ZnajdzTrasy();
            this.ZnajdzOdleglosci();
        }
    }

    private void ZnajdzTrasyT()
    {
        HashSet<string> lokalizacje = new();

        foreach (Kierunki k in this._kierunki)
        {
            lokalizacje.Add(k.Z);
            lokalizacje.Add(k.Do);
        }

        string[] miasta = lokalizacje.ToArray();
        Permutacje<string> trasy = new();
        List<List<string>> trasyy = trasy.ZnajdzPermutacje(miasta);

        foreach(List<string> t in trasyy)
        {
            this._trasyT.Add(new(t[0], t[1], t[2]));
        }
    }

    private void ZnajdzOdleglosciT()
    {
        foreach(TrasaT t in this._trasyT)
        {
            this._odleglosci.Add(this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P1) && k.Do.Equals(t.P2)).odleglosc);
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P2) && k.Do.Equals(t.P3)).odleglosc;
        }
    }

    private void ZnajdzTrasy()
    {
        HashSet<string> lokalizacje = new();

        foreach (Kierunki k in this._kierunki)
        {
            lokalizacje.Add(k.Z);
            lokalizacje.Add(k.Do);
        }

        string[] miasta = lokalizacje.ToArray();

        Permutacje<string> trasy = new();
        List<List<string>> trasyy = trasy.ZnajdzPermutacje(miasta);

        foreach(List<string> t in trasyy)
        {
            this._trasy.Add(new(t[0], t[1], t[2], t[3], t[4], t[5], t[6], t[7]));
        }
    }

    private void ZnajdzOdleglosci()
    {
        foreach (Trasa t in this._trasy)
        {
            this._odleglosci.Add(this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P1) && k.Do.Equals(t.P2)).odleglosc);
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P2) && k.Do.Equals(t.P3)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P3) && k.Do.Equals(t.P4)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P4) && k.Do.Equals(t.P5)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P5) && k.Do.Equals(t.P6)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P6) && k.Do.Equals(t.P7)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.P7) && k.Do.Equals(t.P8)).odleglosc;
        }
    }

    public string PokazRozwiazanie()
    {
        int Min = this._odleglosci.Min();
        int Max = this._odleglosci.Max();
        return $"\r\nNajkrotsza droga: {Min.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}\r\nNajdłuższa droga: {Max.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}";
    }

    private record Kierunki(string Z, string Do, int odleglosc);
    private record Trasa(string P1, string P2, string P3, string P4, string P5, string P6, string P7, string P8);
    
    // Do testu
    private record TrasaT(string P1, string P2, string P3);

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