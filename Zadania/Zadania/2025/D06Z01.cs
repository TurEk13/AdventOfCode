using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2025;

public partial class D06Z01 : IZadanie
{
    private List<int[]> _Liczby;
    private List<char> _Znaki;
    private List<Int64> _PodSumy;
    private Int64 _Suma;

    [GeneratedRegex(@"\d+")]
    private static partial Regex Liczby();

    public D06Z01(bool daneTestowe = false)
    {
        this._Liczby = new ();
        this._Znaki = new ();
        this._PodSumy = new ();
        this._Suma = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\06\\proba.txt" : ".\\Dane\\2025\\06\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        Regex r = Liczby();
        MatchCollection mc;

        while ((linia = sr.ReadLine()) is not null)
        {
            if (linia.Contains('+') || linia.Contains('*'))
            {
                for (int i = 0; i < linia.Length; i++)
                {
                    if (linia[i] == '+' || linia[i] == '*')
                    {
                        this._Znaki.Add(linia[i]);
                    }
                }
            }

            if (!linia.Contains('+') && !linia.Contains('*'))
            {
                mc = r.Matches(linia);
                this._Liczby.AddRange(mc.Select(l => Convert.ToInt32(l.Value)).ToArray());
            }
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for(int i = 0; i < this._Znaki.Count; i++)
        {
            switch (this._Znaki[i])
            {
                case '+':
                    this.DzialaniePlus(i);
                    break;
                case '*':
                    this.DzialanieRazy(i);
                    break;
            }
        }

        this._Suma = this._PodSumy.Sum();
    }

    private void DzialaniePlus(int kolumna)
    {
        Int64 wynik = 0;

        for(int i = 0; i < this._Liczby.Count; i++)
        {
            wynik += this._Liczby[i][kolumna];
        }

        this._PodSumy.Add(wynik);
    }

    private void DzialanieRazy(int kolumna)
    {
        Int64 wynik = 1;

        for (int i = 0; i < this._Liczby.Count; i++)
        {
            wynik *= this._Liczby[i][kolumna];
        }

        this._PodSumy.Add(wynik);
    }

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}