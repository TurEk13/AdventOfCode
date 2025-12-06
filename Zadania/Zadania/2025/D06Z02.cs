using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D06Z02 : IZadanie
{
    private List<List<string>> _Liczby;
    private List<char> _Znaki;
    private List<Int64> _PodSumy;
    private Int64 _Suma;

    public D06Z02(bool daneTestowe = false)
    {
        this._Liczby = new ();
        this._Znaki = new ();
        this._PodSumy = new ();
        this._Suma = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\06\\proba.txt" : ".\\Dane\\2025\\06\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        List<string> dane = new ();

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
                dane.Add(linia + " ");
                this._Liczby.Add(new());
            }
        }

        while(0 < dane[0].Length)
        {
            for(int j = 0; j < dane.Count; j++)
            {
                if (dane[0][j + 1].Equals(' ') && dane[1][j + 1].Equals(' ') && dane[2][j + 1].Equals(' ') && dane[3][j + 1].Equals(' '))
                {
                    this._Liczby[0].Add(dane[0][..(j + 1)]); dane[0] = dane[0][(j + 2)..];
                    this._Liczby[1].Add(dane[1][..(j + 1)]); dane[1] = dane[1][(j + 2)..];
                    this._Liczby[2].Add(dane[2][..(j + 1)]); dane[2] = dane[2][(j + 2)..];
                    this._Liczby[3].Add(dane[3][..(j + 1)]); dane[3] = dane[3][(j + 2)..];
                    j = dane.Count;
                }
            }
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        // 11 052 310 600 986
        for (int i = this._Znaki.Count - 1; i > -1 ; i--)
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
        string[] dzialanie = new string[this._Liczby.Count];
        Int32[] dzialanieI = new Int32[this._Liczby.Count];

        for(int i = 0; i < this._Liczby.Count; i++)
        {
            for(int j = this._Liczby[i][kolumna].Length - 1; j > -1; j--)
            {
                if (!this._Liczby[i][kolumna][j].Equals(' '))
                {
                    dzialanie[3 - j] += this._Liczby[i][kolumna][j];
                }
            }
        }

        foreach(string x in dzialanie)
        {
            wynik += Convert.ToInt64(x);
        }

        this._PodSumy.Add(wynik);
    }

    private void DzialanieRazy(int kolumna)
    {
        Int64 wynik = 1;
        string[] dzialanie = new string[this._Liczby.Count];

        for (int i = 0; i < this._Liczby.Count; i++)
        {
            for (int j = this._Liczby[i][kolumna].Length - 1; j > -1; j--)
            {
                if (!this._Liczby[i][kolumna][j].Equals(' '))
                {
                    dzialanie[3 - j] += this._Liczby[i][kolumna][j];
                }
            }
        }

        foreach (string x in dzialanie)
        {
            if (x is not null)
            {
                wynik *= Convert.ToInt64(x);
            }
        }

        this._PodSumy.Add(wynik);
    }

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}