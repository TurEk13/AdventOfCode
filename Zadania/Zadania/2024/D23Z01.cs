using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D23Z01 : IZadanie
{
    private Int64 Wynik;
    private Dictionary<string, HashSet<string>> Siec;
    private HashSet<string> Uzytkownicy;
    private List<string[]> Dane;

    public D23Z01(bool daneTestowe = false)
    {
        this.Siec = new();
        this.Uzytkownicy = new();
        this.Dane = new();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\23\\proba.txt" : ".\\Dane\\2024\\23\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this.Dane.Add(linia.Split('-'));
        }
    }

    public void RozwiazanieZadania()
    {
        foreach (string[] polaczenie in this.Dane)
        {
            foreach (string komputer in polaczenie)
            {
                if(!this.Siec.ContainsKey(komputer))
                {
                    this.Siec[komputer] = new HashSet<string>();
                }

                if(!this.Uzytkownicy.Contains(komputer))
                {
                    this.Uzytkownicy.Add(komputer);
                }
            }

            this.Siec[polaczenie[0]].Add(polaczenie[1]);
            this.Siec[polaczenie[1]].Add(polaczenie[0]);
        }

        this.Wynik = this.SiecAdministrator().Count;
    }

    private HashSet<string> SiecAdministrator()
    {
        HashSet<string> wynik = new();

        foreach(string pierwszy in this.Uzytkownicy.Where(p => p.StartsWith("t")))
        {
            foreach(string drugi in this.Uzytkownicy)
            {
                foreach (string trzeci in this.Uzytkownicy)
                {
                    if (this.Siec[pierwszy].Contains(drugi) && this.Siec[drugi].Contains(trzeci) && this.Siec[trzeci].Contains(pierwszy))
                    {
                        wynik.Add(string.Join(", ", new string[] { pierwszy, drugi, trzeci }.OrderBy(u => u)));
                    }
                }
            }
        }
        
        return wynik;
    }

    public string PokazRozwiazanie()
    {
        return this.Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}