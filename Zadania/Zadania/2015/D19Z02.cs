using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public class D19Z02 : IZadanie
{
    private string _MolekulaDocelowa;
    private Dictionary<string, Zmiany> _ListaZmian;
    private List<string> _Molekuly;
    private int _Wynik;

    public D19Z02(bool daneTestowe = false)
    {
        this._ListaZmian = new ();
        this._Molekuly = new ();
        this._Wynik = int.MaxValue;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\19\\proba.txt" : ".\\Dane\\2015\\19\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] liniaA;

        while((linia = sr.ReadLine()) != string.Empty)
        {
            liniaA = [.. linia.Split("=>").Select(l => l.Trim())];

            if(!this._ListaZmian.TryGetValue(liniaA[0], out Zmiany _))
            {
                this._ListaZmian[liniaA[0]] = new Zmiany(liniaA[1]);
                this._Molekuly.Add(liniaA[0]);
            }

            if(this._ListaZmian.TryGetValue(liniaA[0], out Zmiany wartosc))
            {
                wartosc.WstawNaCoZmienic(liniaA[1]);
            }
        }

        this._MolekulaDocelowa = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        this.Uruchom("e");
    }

    private void Uruchom(string czastka, int runda = 0)
    {
        Regex r;
        MatchCollection mc;
        StringBuilder sb;

        if(this._MolekulaDocelowa.Equals(czastka))
        {
            if(runda < this._Wynik)
            {
                this._Wynik = runda;
            }
            return;
        }

        if(czastka.Length > this._MolekulaDocelowa.Length)
        {
            return;
        }

        foreach(string molekula in this._Molekuly)
        {
            if(czastka.Contains(molekula))
            {
                r = new (molekula);
                mc = r.Matches(molekula);

                if(mc.Count > 0)
                {
                    foreach(Match m in mc)
                    {
                        for(int i = 0; i < this._ListaZmian[m.Value].PokazIloscZmian; i++)
                        {
                            sb = new (czastka);
                            sb.Remove(m.Index, m.Length);
                            sb.Insert(m.Index, this._ListaZmian[molekula].PokazNaCoZmienic(i));

                            this.Uruchom(sb.ToString(), runda + 1);
                            sb.Clear();
                        }
                    }
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    class Zmiany
    {
        public string CoZamienic { get; private set; }
        private List<string> NaCoZmienic;

        public int PokazIloscZmian { get { return this.NaCoZmienic.Count; }}

        public Zmiany(string CoZamienic)
        {
            this.CoZamienic = CoZamienic;
            this.NaCoZmienic = new ();
        }

        public void WstawNaCoZmienic(string zrodlo)
        {
            this.NaCoZmienic.Add(zrodlo);
        }

        public string PokazNaCoZmienic(int i)
        {
            if(i > -1 && i < this.NaCoZmienic.Count)
            {
                return this.NaCoZmienic[i];
            }

            throw new IndexOutOfRangeException();
        }
    }
}