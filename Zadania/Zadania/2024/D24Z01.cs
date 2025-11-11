using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Zadania._2024;

public class D24Z01 : IZadanie
{
    private Int64 WynikDec;
    private List<string> Lewy;
    private List<string> Operator;
    private List<string> Prawy;
    private List<string> Wynik;
    private SortedDictionary<string, int> Wartosci;

    public D24Z01(bool daneTestowe = false)
    {
        this.Lewy = new();
        this.Operator = new();
        this.Prawy = new();
        this.Wartosci = new();
        this.Wynik = new();
        this.WynikDec = 0;

        string[] wiersz;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\24\\proba.txt" : ".\\Dane\\2024\\24\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        string linia;

        //Wczytanie wartości początkowych
        while ((linia = sr.ReadLine()).Contains(':'))
        {
            this.Wartosci.Add(linia.Substring(0, 3), Convert.ToInt32(linia.Substring(linia.Length - 1)));
        }

        while ((linia = sr.ReadLine()) != null)
        {
            wiersz = linia.Split(' ');
            this.Lewy.Add(wiersz[0]);
            this.Operator.Add(wiersz[1]);
            this.Prawy.Add(wiersz[2]);
            this.Wynik.Add(wiersz[4]);
        }

        for (int i = 0; i < this.Lewy.Count; i++)
        {
            if (!this.Wartosci.ContainsKey(this.Lewy[i]))
            {
                this.Wartosci.Add(this.Lewy[i], -1);
            }

            if (!this.Wartosci.ContainsKey(this.Prawy[i]))
            {
                this.Wartosci.Add(this.Prawy[i], -1);
            }

            if (!this.Wartosci.ContainsKey(this.Wynik[i]))
            {
                this.Wartosci.Add(this.Wynik[i], -1);
            }
        }
    }

    public void RozwiazanieZadania()
    {
        bool lewy, prawy, wynik;
        while (this.Wartosci.ContainsValue(-1))
        {
            for (int i = 0; i < this.Lewy.Count; i++)
            {
                if (this.Wartosci[this.Lewy[i]] != -1 && this.Wartosci[this.Prawy[i]] != -1)
                {
                    lewy = this.Wartosci[this.Lewy[i]] == 0 ? false : true;
                    prawy = this.Wartosci[this.Prawy[i]] == 0 ? false : true;

                    wynik = this.Operator[i] switch
                    {
                        "AND" => lewy & prawy,
                        "OR" => lewy | prawy,
                        "XOR" => lewy ^ prawy,
                    };

                    this.Wartosci[this.Wynik[i]] = wynik ? 1 : 0;
                }
            }
        }

        StringBuilder sb = new();

        for (int i = 45; i > 9; i--)
        {
            sb.Append(this.Wartosci[$"z{i}"]);
        }

        for (int i = 9; i > -1; i--)
        {
            sb.Append(this.Wartosci[$"z0{i}"]);
        }

        string w = sb.ToString();

        for(int i = w.Length - 1, p = 0; i > -1; i--, p++)
        {
            if (w[i] == '1')
            {
                this.WynikDec += Convert.ToInt64(Math.Pow(2, p));
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.WynikDec.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}