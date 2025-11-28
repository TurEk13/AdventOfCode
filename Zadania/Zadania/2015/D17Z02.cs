using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Zadania._2015;

public class D17Z02 : IZadanie
{
    private int _Pojemnosc;
    private List<Pojemniki> _Pojemniki;
    private List<int> _SpisPojemnosci;
    private int ileRund;
    public D17Z02(bool daneTestowe = false)
    {
        this.ileRund = 0;
        this._Pojemniki = new ();
        this._SpisPojemnosci = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\17\\proba.txt" : ".\\Dane\\2015\\17\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while((linia = sr.ReadLine()) != string.Empty)
        {
            this._SpisPojemnosci.Add(Convert.ToInt32(linia));
        }

        this._Pojemnosc = Convert.ToInt32(sr.ReadLine());
        this._SpisPojemnosci.Sort();
    }

    public void RozwiazanieZadania()
    {
        Pojemniki p = new ();
        this.UzupelnijPojemniki(p);
    }

    private void UzupelnijPojemniki(Pojemniki pojemnik, int indeks = 0)
    {
        if(indeks < this._SpisPojemnosci.Count)
        {
            ileRund++;
            for(int i = 0; i < 2; i++)
            {
                if(i == 0)
                {
                    pojemnik.UsunPojemnik(this._SpisPojemnosci[indeks]);
                }

                if(i == 1)
                {
                    pojemnik.DodajPojemnik(this._SpisPojemnosci[indeks]);
                }

                if(indeks < this._SpisPojemnosci.Count - 1)
                {
                    this.UzupelnijPojemniki(pojemnik, indeks + 1);
                }

                if(indeks == this._SpisPojemnosci.Count - 1 && pojemnik.PodajSume == this._Pojemnosc)
                {
                    Pojemniki p = new (pojemnik);
                    this._Pojemniki.Add(p);
                    return;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Pojemniki.Where(p => p.IlePojemnikow == this._Pojemniki.Min(p => p.IlePojemnikow)).ToList<Pojemniki>().Count.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Pojemniki
    {
        private List<int> _Pojemniki;
        public int IlePojemnikow { get { return this._Pojemniki.Count; } }
        public int PodajSume { get { return this._Pojemniki.Sum(); } }

        public Pojemniki()
        {
            this._Pojemniki = new ();
        }

        public Pojemniki(Pojemniki p)
        {
            this._Pojemniki = new ();

            for(int i = 0; i < p._Pojemniki.Count; i++)
            {
                this._Pojemniki.Add(p._Pojemniki[i]);
            }
        }

        public void DodajPojemnik(int wartosc)
        {
            this._Pojemniki.Add(wartosc);
        }

        public void UsunPojemnik(int wartosc)
        {
            int indeks = this._Pojemniki.LastIndexOf(wartosc);
            if(indeks > -1)
            {
                this._Pojemniki.RemoveAt(indeks);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new ();

            for(int i = 0; i < this._Pojemniki.Count; i++)
            {
                sb.Append($" {this._Pojemniki[i]} ");
            }

            return sb.ToString();
        }
    }
}