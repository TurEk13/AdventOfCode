using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D23Z02 : IZadanie
{
    private string Wynik;
    private Dictionary<string, HashSet<string>> Siec;
    private HashSet<string> Uzytkownicy;
    private List<string[]> Dane;

    public D23Z02(bool daneTestowe = false)
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

        HashSet<string> podSieci = this.SzukajTrojekUzytkownikow();

        this.Wynik = SzukajHasla(podSieci);
    }

    private HashSet<string> SzukajTrojekUzytkownikow()
    {
        HashSet<string> wynik = new();

        foreach(string pierwszy in this.Uzytkownicy)
        {
            foreach(string drugi in this.Uzytkownicy)
            {
                foreach (string trzeci in this.Uzytkownicy)
                {
                    if (this.Siec[pierwszy].Contains(drugi) && this.Siec[drugi].Contains(trzeci) && this.Siec[trzeci].Contains(pierwszy))
                    {
                        wynik.Add(string.Join("-", new string[] { pierwszy, drugi, trzeci }.OrderBy(u => u)));
                    }
                }
            }
        }

        return wynik;
    }

    private string SzukajHasla(HashSet<string> Sieci)
    {
        string[] uzytkownicySieci;
        string[] posortowaniUzytkownicy = [.. this.Uzytkownicy.OrderBy(u => u)];

        HashSet<string> obecneSieci = Sieci;

        HashSet<string> noweSieci = obecneSieci;

        do
        {
            obecneSieci = noweSieci;
            noweSieci = new HashSet<string>();

            foreach (string siec in obecneSieci)
            {
                uzytkownicySieci = siec.Split("-");

                foreach (string uzytkownik in posortowaniUzytkownicy)
                {
                    if (!siec.Contains(uzytkownik))
                    {
                        if (uzytkownicySieci.All(u => this.Siec[u].Contains(uzytkownik)))
                        {
                            noweSieci.Add(string.Join("-", uzytkownicySieci.Append(uzytkownik).OrderBy(u => u)));
                        }
                    }
                }
            }
        } while (noweSieci.Count != 0);

        return obecneSieci.First().Replace('-', ',');
    }

    public string PokazRozwiazanie()
    {
        return this.Wynik;
    }
}