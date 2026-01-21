using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D09Z02 : IZadanie
{
    private UInt64 _Wynik;
    private string _Kompresja;
    public D09Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\09\\proba.txt" : ".\\Dane\\2016\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);

        this._Kompresja = sr.ReadToEnd();
        
        sr.Close(); fs!.Close();
    }

    [GeneratedRegex(@"\((?'ileZnakow'[0-9]{1,})x(?'mnoznik'[0-9]{1,})\)")]
    private static partial Regex Zakresy();
    public void RozwiazanieZadania()
    {
        Regex r = Zakresy();
        Match m;
        string tresc;

        tresc = this._Kompresja;

        for(int i = 0; i < tresc.Length;)
        {
            if(char.IsLetter(tresc[i]))
            {
                this._Wynik++;
                i++;
                continue;
            }

            if(tresc[i].Equals('('))
            {
                m = r.Match(tresc[i..]);
                int start = m.Length + i;
                int dlugoscc = Convert.ToInt32(m.Groups["ileZnakow"].Value);
                (int przesuniecie, UInt64 dlugosc) = this.ObliczDlugosc(tresc.Substring(start, dlugoscc), Convert.ToInt32(m.Groups["ileZnakow"].Value), Convert.ToInt32(m.Groups["mnoznik"].Value));

                i += przesuniecie + m.Length;
                this._Wynik += dlugosc;
                continue;
            }
        }
    }

    private (int przesuniecie, UInt64 dlugosc) ObliczDlugosc(string tresc, int dlugoscCiagu, int mnoznikCiagu)
    {
        UInt64 dlugosc = 0;
        Match m;
        int start, dlugoscCiaguNowa;

        for(int i = 0; i < tresc.Length;)
        {
            if(tresc[i].Equals('('))
            {
                m = Zakresy().Match(tresc[i..]);

                start = m.Length + i;
                dlugoscCiaguNowa = Convert.ToInt32(m.Groups["ileZnakow"].Value);

                (int przesuniecie, UInt64 dlugoscNowa) = this.ObliczDlugosc(tresc.Substring(start, dlugoscCiaguNowa), Convert.ToInt32(m.Groups["ileZnakow"].Value), Convert.ToInt32(m.Groups["mnoznik"].Value));

                i += przesuniecie + m.Length;
                dlugosc += (UInt64)mnoznikCiagu * dlugoscNowa;
                continue;
            }

            if(char.IsLetter(tresc[i]) && dlugoscCiagu != tresc.Length)
            {
                dlugosc++;
                i++;
                continue;
            }

            if(char.IsLetter(tresc[i]) && dlugoscCiagu == tresc.Length)
            {
                dlugosc = (UInt64)(mnoznikCiagu * dlugoscCiagu);
                i = tresc.Length;
                continue;
            }
        }

        return (tresc.Length, dlugosc);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}