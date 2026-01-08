using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D09Z02 : IZadanie
{
    private Int64 _Wynik;
    private string _Kompresja;
    
    public D09Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\09\\proba.txt" : ".\\Dane\\2016\\09\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);

        this._Kompresja = sr.ReadToEnd();

        sr.Close(); fs!.Close();
    }

    //[GeneratedRegex(@"\((?<ileZnakow>[0-9]{1,})x(?<mnoznik>[0-9]{1,})\)|(?:\w{1})\((?<ileZnakow>[0-9]{1,})x(?<mnoznik>[0-9]{1,})\)(?:[\(])")]
    [GeneratedRegex(@"\((?<ileZnakow>[0-9]{1,})x(?<mnoznik>[0-9]{1,})\)|\((?<ileZnakow>[0-9]{1,})x(?<mnoznik>[0-9]{1,})\)")]
    private static partial Regex Zakresy();
    public void RozwiazanieZadania()
    {
        Regex r = Zakresy();
        MatchCollection mc = r.Matches(this._Kompresja);

        for (int i = 0, m = 0; i < this._Kompresja.Length;)
        {
            if(i != mc[m].Index)
            {
                this._Wynik++;
                i++;
            }

            if(i == mc[m].Index)
            {
                (Int64 Dlugosc, i) = this.ObliczDlugosc(mc[m], 0);
                this._Wynik += Dlugosc;
                m++;

                int indeks = mc[m].Index;

                while (m < mc.Count && !(i <= indeks))
                {
                    m++;
                    indeks = mc[m].Index;
                }
            }
        }
    }

    public (Int64 Dlugosc, int NowaPozycja) ObliczDlugosc(Match m, int indeks)
    {
        //(141x10)(20x4)PSFDROQLSZCXJYTATIBY(2x9)NN(60x14)(3x15)WUO(2x13)WF(10x14)KRXBNHFEGQ(20x4)SWJUMHNRCRJUPDVFAKMI[(35x8)(3x14)VZB(8x15)SWKZSEFU(7x1)FZTLTXZ]
        Int64 dlugosc;
        int ileZnakow = Convert.ToInt32(m.Groups["ileZnakow"].Value);
        int mnoznik = Convert.ToInt32(m.Groups["mnoznik"].Value);
        int poczatekZakresu = Convert.ToInt32(indeks + m.Groups["mnoznik"].Index + m.Groups["mnoznik"].Length + 1);
        int koniecZakresu = Convert.ToInt32(indeks + m.Groups["mnoznik"].Index + m.Groups["mnoznik"].Length + 1 + ileZnakow);

        dlugosc = 0;

        string s = this._Kompresja[poczatekZakresu .. koniecZakresu];

        if(s.Contains('('))
        {
            Regex r = Zakresy();
            MatchCollection mc = r.Matches(s);

            if(mc.Count > 1)
            {
                if(mc[1].Index == mc[0].Index + mc[0].Length)
                {
                    (Int64 DodatkowaDlugosc, _) = this.ObliczDlugosc(mc[0], poczatekZakresu); 
                    dlugosc = DodatkowaDlugosc * mnoznik;
                }

                if(mc[1].Index > mc[0].Index + mc[0].Length)
                {
                    foreach(Match mN in mc)
                    {
                        (Int64 DodatkowaDlugosc, _) = this.ObliczDlugosc(mN, poczatekZakresu); 
                        dlugosc += DodatkowaDlugosc;
                    }
                    
                    dlugosc *= mnoznik;
                }
            }

            if(mc.Count == 1)
            {
                (Int64 DodatkowaDlugosc, _) = this.ObliczDlugosc(mc[0], poczatekZakresu); 
                dlugosc = DodatkowaDlugosc * mnoznik;
            }
        }

        if(!s.Contains('('))
        {
            dlugosc = ileZnakow * mnoznik;
        }

        return (dlugosc, koniecZakresu);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}