using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D04Z02 : IZadanie
{
    private List<string> _Pokoje;
    private Int64 _Wynik;
    public D04Z02(bool daneTestowe = false)
    {
        this._Pokoje = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\04\\proba.txt" : ".\\Dane\\2016\\04\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string pokoj = string.Empty;
        this._Wynik = 0;

        while((pokoj = sr.ReadLine()) is not null)
        {
            this._Pokoje.Add(pokoj);
        }

        sr.Close(); fs!.Close();
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex IdPokoju();

    public void RozwiazanieZadania()
    {
        Regex r = IdPokoju();
        int id, reszta;
        string pokoj;
        StringBuilder sb;
        string wynik;

        foreach(string s in this._Pokoje)
        {
            if(s.Count(p => p.Equals('-')) != 3)
            {
                continue;
            }

            pokoj = s[..s.LastIndexOf('-')];
            id = Convert.ToInt32(r.Match(s).Value);
            reszta = id % 26;
            sb = new (pokoj);

            for(int i = 0; i < pokoj.Length; i++)
            {
                if(sb[i].Equals('-'))
                {
                    sb[i] = ' ';
                    continue;
                }

                int c = (pokoj[i] - 'a' + reszta) % 26 + 'a';
                sb[i] = Convert.ToChar(c);
            }

            wynik = sb.ToString();

            if(wynik.Contains("north"))
            {
                this._Wynik = id;
                return;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));

    }
}