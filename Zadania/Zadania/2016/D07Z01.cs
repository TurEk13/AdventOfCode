using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public class D07Z01 : IZadanie
{
    private Int64 _Wynik;
    private List<string> _Adresy;

    public D07Z01(bool daneTestowe = false)
    {
        this._Adresy = new();
        this._Wynik = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\07\\proba.txt" : ".\\Dane\\2016\\07\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string wiersz;

        while((wiersz = sr.ReadLine()) is not null)
        {
            this._Adresy.Add(wiersz);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        string tmp, wiersz;
        int indeks;
        bool ABBApoza, ABBAw;

        for (int w = 0; w < this._Adresy.Count; w++)
        {
            ABBApoza = false;
            ABBAw = false;
            wiersz = this._Adresy[w];

            while (wiersz.Length > 0)
            {
                indeks = wiersz.IndexOf('[');

                tmp = indeks != -1 ? wiersz[..indeks] : wiersz[0..];
                wiersz = indeks != -1 ? wiersz[(indeks + 1)..] : string.Empty;

                for (int i = 3; i < tmp.Length; i++)
                {
                    if (tmp[i - 3].Equals(tmp[i]) && tmp[i - 2].Equals(tmp[i - 1]) && !tmp[i].Equals(tmp[i - 1]))
                    {
                        ABBApoza = true;
                    }
                }

                indeks = wiersz.IndexOf(']');

                if (indeks != -1)
                {
                    tmp = wiersz[..indeks];
                    wiersz = wiersz[(indeks + 1)..];

                    for (int i = 3; i < tmp.Length; i++)
                    {
                        if (tmp[i - 3].Equals(tmp[i]) && tmp[i - 2].Equals(tmp[i - 1]) && !tmp[i].Equals(tmp[i - 1]))
                        {
                            ABBAw = true;
                        }
                    }
                }
            }

            if (ABBApoza & !ABBAw)
            {
                this._Wynik++;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}