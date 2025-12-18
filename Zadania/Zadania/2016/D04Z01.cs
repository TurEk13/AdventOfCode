using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D04Z01 : IZadanie
{
    private List<string> _Pokoje;
    private Int64 _Wynik;
    public D04Z01(bool daneTestowe = false)
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
        Dictionary<char, int> iloscWystapien;
        List<KeyValuePair<char, int>> iloscWystapienPosortowane;
        char[] sumaKontrolna;
        int poczatek = 0, koniec = 0;
        bool prawidlowyPokoj;

        foreach(string s in this._Pokoje)
        {
            iloscWystapien = [];
            iloscWystapienPosortowane = [];
            prawidlowyPokoj = true;

            for(int i = 0; !char.IsDigit(s[i]); i++)
            {
                if(s[i].Equals('-'))
                {
                    continue;
                }

                if(!iloscWystapien.TryAdd(s[i], 1))
                {
                    iloscWystapien[s[i]]++;
                }
            }

            foreach(KeyValuePair<char, int> kvp in iloscWystapien)
            {
                iloscWystapienPosortowane.Add(kvp);
            }

            iloscWystapienPosortowane = iloscWystapienPosortowane.OrderByDescending(iwp => iwp.Value).ToList();

            for(int i = 1; i < iloscWystapienPosortowane.Count; i++)
            {
                if(iloscWystapienPosortowane[i - 1].Value == iloscWystapienPosortowane[i].Value)
                {
                    poczatek = i - 1;

                    for(;i < iloscWystapienPosortowane.Count && iloscWystapienPosortowane[i - 1].Value == iloscWystapienPosortowane[i].Value; i++)
                    {
                        koniec = i;
                    }

                    this.Sortuj(ref iloscWystapienPosortowane, poczatek, koniec);
                }
            }

            poczatek = s.IndexOf('[') + 1;
            koniec = s.IndexOf(']');

            sumaKontrolna = s[poczatek..koniec].ToCharArray();

            for(int i = 0; i < sumaKontrolna.Length; i++)
            {
                if(!sumaKontrolna.Contains(iloscWystapienPosortowane[i].Key))
                {
                    prawidlowyPokoj = false;
                    i = sumaKontrolna.Length;
                }
            }

            for(int i = 1; i < sumaKontrolna.Length & prawidlowyPokoj; i++)
            {
                if(iloscWystapienPosortowane[i - 1].Value == iloscWystapienPosortowane[i].Value & iloscWystapienPosortowane[i - 1].Key - '0' > iloscWystapienPosortowane[i].Key
                 - '0')
                {
                    prawidlowyPokoj = false;
                    i = sumaKontrolna.Length;
                }
            }

            if(prawidlowyPokoj)
            {
                this._Wynik += Convert.ToInt64(r.Match(s).Value);
            }
        }
    }

    private void Sortuj(ref List<KeyValuePair<char, int>> lista, int poczatek, int koniec)
    {
        KeyValuePair<char, int>[] listaTMP = new KeyValuePair<char, int>[koniec - poczatek + 1];
        lista.CopyTo(poczatek, listaTMP, 0, koniec - poczatek + 1);

        listaTMP = listaTMP.OrderBy(iwp => iwp.Key).ToArray();

        for(int i = poczatek; i < listaTMP.Length + poczatek; i++)
        {
            lista[i] = listaTMP[i - poczatek];
        }

    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));

    }
}