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
    private List<int> _odleglosci;
    private bool daneTestowe;

    public D09Z01(bool daneTestowe = false)
    {
        this._kierunki = new();
        this._trasy = new();
        this._odleglosci = new();

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
        /*if (daneTestowe)
        {
            this.ZnajdzTrasyP();
            this.ZnajdzOdleglosciP();
        }*/

        if(!daneTestowe)
        {
            this.ZnajdzTrasy();
            this.ZnajdzOdleglosci();
        }
    }

    /*
    private void ZnajdzTrasyP()
    {
        HashSet<string> lokalizacje = new();

        foreach (Kierunki k in this._kierunki)
        {
            lokalizacje.Add(k.Z);
            lokalizacje.Add(k.Do);
        }

        string[] miasta = lokalizacje.ToArray();

        for (int i = 0; i < miasta.Length; i++)
        {
            this._trasy.Add(new(miasta[i % 3], miasta[(i + 1) % 3], miasta[(i + 2) % 3]));
            this._trasy.Add(new(miasta[(i + 2) % 3], miasta[(i + 1) % 3], miasta[i % 3]));
        }
    }

    private void ZnajdzOdleglosciP()
    {
        foreach(Trasa t in this._trasy)
        {
            this._odleglosci.Add(this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p1) && k.Do.Equals(t.p2)).odleglosc);
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p2) && k.Do.Equals(t.p3)).odleglosc;
        }
    }
    */

    private void ZnajdzTrasy()
    {
        // 578 644+
        HashSet<string> lokalizacje = new();

        foreach (Kierunki k in this._kierunki)
        {
            lokalizacje.Add(k.Z);
            lokalizacje.Add(k.Do);
        }

        string[] miasta = lokalizacje.ToArray();

        for (int i = 0; i < miasta.Length; i++)
        {
            this._trasy.Add(new(miasta[i % 8], miasta[(i + 1) % 8], miasta[(i + 2) % 8], miasta[(i + 3) % 8], miasta[(i + 4) % 8], miasta[(i + 5) % 8], miasta[(i + 6) % 8], miasta[(i + 7) % 8]));
        }
    }

    private void ZnajdzOdleglosci()
    {
        foreach (Trasa t in this._trasy)
        {
            this._odleglosci.Add(this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p1) && k.Do.Equals(t.p2)).odleglosc);
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p2) && k.Do.Equals(t.p3)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p3) && k.Do.Equals(t.p4)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p4) && k.Do.Equals(t.p5)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p5) && k.Do.Equals(t.p6)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p6) && k.Do.Equals(t.p7)).odleglosc;
            this._odleglosci[^1] += this._kierunki.FirstOrDefault(k => k.Z.Equals(t.p7) && k.Do.Equals(t.p8)).odleglosc;
        }
    }


    public string PokazRozwiazanie()
    {
        return this._odleglosci.Min().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Kierunki(string Z, string Do, int odleglosc);
    
    // Do testu
    //private record Trasa(string p1, string p2, string p3);
    private record Trasa(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8);

    https://chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp

    Dane:
    Faerun to Norrath = 129
    Faerun to Tristram = 58
    Faerun to AlphaCentauri = 13
    Faerun to Arbre = 24
    Faerun to Snowdin = 60
    Faerun to Tambi = 71
    Faerun to Straylight = 67
    Norrath to Tristram = 142
    Norrath to AlphaCentauri = 15
    Norrath to Arbre = 135
    Norrath to Snowdin = 75
    Norrath to Tambi = 82
    Norrath to Straylight = 54
    Tristram to AlphaCentauri = 118
    Tristram to Arbre = 122
    Tristram to Snowdin = 103
    Tristram to Tambi = 49
    Tristram to Straylight = 97
    AlphaCentauri to Arbre = 116
    AlphaCentauri to Snowdin = 12
    AlphaCentauri to Tambi = 18
    AlphaCentauri to Straylight = 91
    Arbre to Snowdin = 129
    Arbre to Tambi = 53
    Arbre to Straylight = 40
    Snowdin to Tambi = 15
    Snowdin to Straylight = 99
    Tambi to Straylight = 70

    Proba:
    London to Dublin = 464
    London to Belfast = 518
    Dublin to Belfast = 141

}