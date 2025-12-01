using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public class D19Z01 : IZadanie
{
    private string _MolekulaBazowa;
    Dictionary<string, Zmiany> _ListaZmian;
    List<string> _Molekuly;
    HashSet<string> _Wynik;

    public D19Z01(bool daneTestowe = false)
    {
        this._ListaZmian = new ();
        this._Molekuly = new ();
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

        this._MolekulaBazowa = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        StringBuilder sb = new (this._MolekulaBazowa);
        this._Wynik = new ();
        Regex r;
        MatchCollection mc;
        int i;

        foreach(string molekula in this._Molekuly)
        {
            r = new Regex(molekula);
            mc = r.Matches(this._MolekulaBazowa);

            foreach(Match m in mc)
            {
                i = 0;
                
                while(i < this._ListaZmian[molekula].PokazIloscZmian)
                {
                    sb.Remove(m.Index, m.Length);
                    sb.Insert(m.Index, this._ListaZmian[molekula].PokazNaCoZmienic(i));
                    this._Wynik.Add(sb.ToString());
                    sb = new (this._MolekulaBazowa);
                    i++;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.Count.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
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