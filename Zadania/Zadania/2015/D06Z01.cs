using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D06Z01 : IZadanie
{
    private bool[,] Zarowki;
    private List<string> Instrukcja;
    private Int64 ZapaloneZarowki;

    public D06Z01()
    {
        this.Zarowki = new bool[1000, 1000];
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 1000; j++)
            {
                this.Zarowki[i, j] = false;
            }
        }

        this.Instrukcja = new();
        this.ZapaloneZarowki = 0;
        FileStream fs = new(".\\Dane\\2015\\06\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while((linia = sr.ReadLine()) is not null)
        {
            this.Instrukcja.Add(linia);
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        /*
        int[,] x = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } }; // [4, 3] [wiersz, kolumna]
        int y = x[2, 0]; // 7
        */

        int wiersz, kolumna;
        string[] instrukcjaSlowa;
        int[] poczatek, koniec;

        foreach(string linia in this.Instrukcja)
        {
            instrukcjaSlowa = linia.Split(' ');
            poczatek = instrukcjaSlowa[^3].Split(',').Select(i => Convert.ToInt32(i)).ToArray();
            koniec = instrukcjaSlowa[^1].Split(",").Select(i => Convert.ToInt32(i)).ToArray();

            if (instrukcjaSlowa[0].Equals("toggle"))
            {
                for (wiersz = poczatek[0]; wiersz <= koniec[0]; wiersz++)
                {
                    for (kolumna = poczatek[1]; kolumna <= koniec[1]; kolumna++)
                    {
                        this.Zarowki[wiersz, kolumna] = !this.Zarowki[wiersz, kolumna];
                    }
                }

                continue;
            }

            if (instrukcjaSlowa[1].Equals("on"))
            {
                for (wiersz = poczatek[0]; wiersz <= koniec[0]; wiersz++)
                {
                    for (kolumna = poczatek[1]; kolumna <= koniec[1]; kolumna++)
                    {
                        this.Zarowki[wiersz, kolumna] = true;
                    }
                }

                continue;
            }

            if (instrukcjaSlowa[1].Equals("off"))
            {
                for (wiersz = poczatek[0]; wiersz <= koniec[0]; wiersz++)
                {
                    for (kolumna = poczatek[1]; kolumna <= koniec[1]; kolumna++)
                    {
                        this.Zarowki[wiersz, kolumna] = false;
                    }
                }

                continue;
            }
        }

        for (wiersz = 0; wiersz < 1000; wiersz++)
        {
            for (kolumna = 0; kolumna < 1000; kolumna++)
            {
                if(this.Zarowki[wiersz, kolumna])
                {
                    this.ZapaloneZarowki++;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.ZapaloneZarowki.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}