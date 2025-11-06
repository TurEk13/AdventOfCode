using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2024;

public class D25Z01 : IZadanie
{
    private Int64 Wynik;

    private List<int[]> Klucze;
    private List<int[]> Zamki;
    public D25Z01(bool daneTestowe = false)
    {
        this.Wynik = 0;
        this.Klucze = new();
        this.Zamki = new();

        List<char[]> Klucz = new();
        List<char[]> Zamek = new();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\25\\proba.txt" : ".\\Dane\\2024\\25\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        string linia;

        //Klucze zaczynają się od ..... a zamki od #####
        while ((linia = sr.ReadLine()) != null)
        {
            if (linia.Contains("....."))
            {
                Klucz.Clear();

                for (int i = 0; i < 5; i++)
                {
                    Klucz.Add(sr.ReadLine().ToCharArray());
                }

                this.Klucze.Add([0, 0, 0, 0, 0]);

                for(int i = 0; i < Klucz.Count; i++)
                {
                    if (Klucz[i][0] == '#') { this.Klucze[^1][0]++; }
                    if (Klucz[i][1] == '#') { this.Klucze[^1][1]++; }
                    if (Klucz[i][2] == '#') { this.Klucze[^1][2]++; }
                    if (Klucz[i][3] == '#') { this.Klucze[^1][3]++; }
                    if (Klucz[i][4] == '#') { this.Klucze[^1][4]++; }
                }
            }

            if (linia.Contains("#####"))
            {
                Zamek.Clear();

                for (int i = 0; i < 5; i++)
                {
                    Zamek.Add(sr.ReadLine().ToCharArray());
                }

                this.Zamki.Add([0, 0, 0, 0, 0]);

                for (int i = 0; i < Zamek.Count; i++)
                {
                    if (Zamek[i][0] == '#') { this.Zamki[^1][0]++; }
                    if (Zamek[i][1] == '#') { this.Zamki[^1][1]++; }
                    if (Zamek[i][2] == '#') { this.Zamki[^1][2]++; }
                    if (Zamek[i][3] == '#') { this.Zamki[^1][3]++; }
                    if (Zamek[i][4] == '#') { this.Zamki[^1][4]++; }
                }
            }

            sr.ReadLine();
            sr.ReadLine();
        }

        int x = 3 + 4;
    }

    public void RozwiazanieZadania()
    {
        for(int k = 0; k < this.Klucze.Count; k++)
        {
            for(int z = 0; z < this.Zamki.Count; z++)
            {
                if(this.Klucze[k][0] + this.Zamki[z][0] < 6 && this.Klucze[k][1] + this.Zamki[z][1] < 6 && this.Klucze[k][2] + this.Zamki[z][2] < 6 && this.Klucze[k][3] + this.Zamki[z][3] < 6 && this.Klucze[k][4] + this.Zamki[z][4] < 6)
                {
                    this.Wynik++;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}