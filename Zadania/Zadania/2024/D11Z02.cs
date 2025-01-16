using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D11Z02 : IZadanie
{
    private Dictionary<Int64, Int64> kamienie;
    private Int128 wynik;

    public D11Z02(bool daneTestowe = false)
    {
        this.kamienie = new();
        string tekst = File.ReadAllText(daneTestowe ? ".\\Dane\\2024\\11\\proba.txt" : ".\\Dane\\2024\\11\\dane.txt");
        this.wynik = 0;
        Int64[] test = tekst.Split(' ').Select(x => Convert.ToInt64(x)).ToArray();

        foreach (Int64 x in test)
        {
            if (this.kamienie.ContainsKey(x))
            {
                this.kamienie[x]++;
            }

            if (!this.kamienie.ContainsKey(x))
            {
                this.kamienie.Add(x, 1);
            }
        }
    }

    public void RozwiazanieZadania()
    {
        int maks = 75;

        List<Int64> Indeksy;
        List<Int64> Wartosci;
        List<Int64> odp;
        Dictionary<Int64, Int64> noweKamienie;
        Int64 stary = 0;

        for (int i = 0; i < maks; i++)
        {
            noweKamienie = new();
            Indeksy = this.kamienie.Keys.ToList<Int64>();
            Wartosci = this.kamienie.Values.ToList<Int64>();

            for (int j = 0; j < Indeksy.Count; j++)
            {
                odp = this.Zasady(Indeksy[j]);

                foreach (Int64 x in odp)
                {
                    if (noweKamienie.ContainsKey(x))
                    {
                        noweKamienie[x] += Wartosci[j];
                    }

                    if (!noweKamienie.ContainsKey(x))
                    {
                        noweKamienie.Add(x, Wartosci[j]);
                    }
                }

                this.kamienie = noweKamienie;
            }
        }

        List<Int64> wartosci = this.kamienie.Values.ToList();
        this.wynik = wartosci.Sum();
    }

    public string PokazRozwiazanie()
    {
        return this.wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private List<Int64> Zasady(Int64 liczba)
    {
        if (liczba == 0)
        {
            return [1];
        }

        if (liczba.ToString().Length % 2 == 0)
        {

            return [Convert.ToInt64(liczba.ToString().Substring(0, liczba.ToString().Length / 2)), Convert.ToInt64(liczba.ToString().Substring(liczba.ToString().Length / 2))];
        }

        return [liczba * 2024];
    }
}