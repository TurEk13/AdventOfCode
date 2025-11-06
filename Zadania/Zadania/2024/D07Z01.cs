using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D07Z01 : IZadanie
{
    private Int64 Wynik;
    private List<Int64[]> Dane;
    private List<bool> PoprawneLiczby;

    public D07Z01(bool daneTestowe = false)
    {
        this.Dane = new();
        this.PoprawneLiczby = new();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\07\\proba.txt" : ".\\Dane\\2024\\07\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs!);
        string x = "null";

        while ((x = sr.ReadLine()) != null)
        {
            x = x.Replace(": ", " ");
            this.Dane.Add(x.Split(" ").Select(x => Convert.ToInt64(x)).ToArray<Int64>());
            this.PoprawneLiczby.Add(false);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        //Możliwe działanie '+' i '*'
        //3749

        //7498 za mało

        for (int i = 0; i < this.Dane.Count; i++)
        {
            this.Mnoz(i, this.Dane[i][1], 2);
            this.Dodaj(i, this.Dane[i][1], 2);
        }

        for(int i = 0; i < this.PoprawneLiczby.Count; i++)
        {
            if(this.PoprawneLiczby[i])
            {
                this.Wynik += this.Dane[i][0];
            }
        }
    }

    private void Mnoz(int wiersz, Int64 podsuma, int kolejnaLiczba)
    {
        Int64 sumaCzesciowa = podsuma * this.Dane[wiersz][kolejnaLiczba];

        if (sumaCzesciowa == this.Dane[wiersz][0] && this.Dane[wiersz].Length - 1 == kolejnaLiczba)
        {
            this.PoprawneLiczby[wiersz] = true;
        }

        if (kolejnaLiczba + 1 < this.Dane[wiersz].Length)
        {
            this.Mnoz(wiersz, sumaCzesciowa, kolejnaLiczba + 1);
            this.Dodaj(wiersz, sumaCzesciowa, kolejnaLiczba + 1);
        }
    }   

    private void Dodaj(int wiersz, Int64 podsuma, int kolejnaLiczba)
    {
        Int64 sumaCzesciowa = podsuma + this.Dane[wiersz][kolejnaLiczba];

        if (sumaCzesciowa == this.Dane[wiersz][0] && this.Dane[wiersz].Length - 1 == kolejnaLiczba)
        {
            this.PoprawneLiczby[wiersz] = true;
        }

        if (kolejnaLiczba + 1 < this.Dane[wiersz].Length)
        {
            this.Mnoz(wiersz, sumaCzesciowa, kolejnaLiczba + 1);
            this.Dodaj(wiersz, sumaCzesciowa, kolejnaLiczba + 1);
        }
    }

    public string PokazRozwiazanie()
    {
        return this.Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}