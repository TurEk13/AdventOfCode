using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2025;

public partial class D10Z01 : IZadanie
{
    private List<Maszyna> _Maszyny;
    private Int64 _Wynik;
    [GeneratedRegex(@"\d+")]
    private static partial Regex _Liczby();
    public D10Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Maszyny = new ();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\10\\proba.txt" : ".\\Dane\\2025\\10\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string dane;

        Regex liczby = _Liczby();
        MatchCollection mc;

        while ((linia = sr.ReadLine()) is not null)
        {
            List<bool> stanDocelowy = new ();
            List<List<int>> przyciski = new ();
            int dlugosc;

            dane = linia[..linia.IndexOf(' ')];
            
            for(int i = 1; !dane[i].Equals(']'); i++)
            {
                stanDocelowy.Add(dane[i].Equals('#'));
            }

            dane = linia[(linia.IndexOf(' ') + 1)..linia.LastIndexOf(' ')];

            while(dane.Length > 0)
            {
                przyciski.Add(new ());

                dlugosc = dane.Contains(' ') ? dane.IndexOf(' ') : dane.Length;

                mc = liczby.Matches(dane[..dlugosc]);
                foreach(Match m in mc)
                {
                    przyciski[^1].Add(Convert.ToInt32(m.Value));
                }

                dane = dane.Contains(' ') ? dane[(dlugosc + 1)..] : "";
            }

            this._Maszyny.Add(new ([.. stanDocelowy], przyciski));
        }
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        int maks;
        foreach(Maszyna m in this._Maszyny)
        {
            maks = int.MaxValue;
            for (int obecneKlikniecia = 0; obecneKlikniecia <= m.Przyciski.Count; obecneKlikniecia++)
            {
                if (UruchomMaszyne(m, obecneKlikniecia, 0, 0, new bool[m.StanDocelowy.Length]))
                {
                   if(maks > obecneKlikniecia)
                    {
                        maks = obecneKlikniecia;
                    }
                }
            }
            this._Wynik += maks;
        }
    }

    private bool UruchomMaszyne(Maszyna maszyna, int iloscKlikniec, int start, int poziom, bool[] aktualnyStan)
    {
        if (poziom == iloscKlikniec)
        {
            return aktualnyStan.SequenceEqual(maszyna.StanDocelowy);
        }

        for (int i = start; i <= maszyna.Przyciski.Count - (iloscKlikniec - poziom); i++)
        {
            PrzelaczSwiatla(maszyna.Przyciski[i], aktualnyStan);

            if (UruchomMaszyne(maszyna, iloscKlikniec, i + 1, poziom + 1, aktualnyStan))
            {
                return true;
            }

            PrzelaczSwiatla(maszyna.Przyciski[i], aktualnyStan);
        }
        return false;
    }

    private void PrzelaczSwiatla(List<int> swiatla, bool[] stan)
    {
        foreach (int i in swiatla)
        {
            stan[i] = !stan[i];
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Maszyna(bool[] StanDocelowy, List<List<int>> Przyciski);
}