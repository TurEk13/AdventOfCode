using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public class D07Z02 : IZadanie
{
    private Int64 _Wynik;
    private List<string> _Adresy;

    public D07Z02(bool daneTestowe = false)
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
        List<string> trojkiPoza, trojkiW;

        for (int w = 0; w < this._Adresy.Count; w++)
        {
            wiersz = this._Adresy[w];
            trojkiPoza = [];
            trojkiW = [];

            while (wiersz.Length > 0)
            {
                indeks = wiersz.IndexOf('[');

                tmp = indeks != -1 ? wiersz[..indeks] : wiersz[0..];
                wiersz = indeks != -1 ? wiersz[(indeks + 1)..] : string.Empty;

                for (int i = 2; i < tmp.Length; i++)
                {
                    if (tmp[i - 2].Equals(tmp[i]) && !tmp[i].Equals(tmp[i - 1]))
                    {
                        trojkiPoza.Add(tmp.Substring(i - 2, 3));
                    }
                }

                indeks = wiersz.IndexOf(']');

                if (indeks != -1)
                {
                    tmp = wiersz[..indeks];
                    wiersz = wiersz[(indeks + 1)..];

                    for (int i = 2; i < tmp.Length; i++)
                    {
                        if (tmp[i - 2].Equals(tmp[i]) && !tmp[i].Equals(tmp[i - 1]))
                        {
                            trojkiW.Add(tmp.Substring(i - 2, 3));
                        }
                    }
                }
            }

            for (int i = 0; i < trojkiPoza.Count; i++)
            {
                for (int j = 0; j < trojkiW.Count; j++)
                {
                    if (trojkiPoza[i][0].Equals(trojkiW[j][1]) && trojkiPoza[i][2].Equals(trojkiW[j][1]) && trojkiW[j][0].Equals(trojkiPoza[i][1]) && trojkiW[j][2].Equals(trojkiPoza[i][1]))
                    {
                        this._Wynik++;
                        i = trojkiPoza.Count;
                        j = trojkiW.Count;
                    }
                }
            }
        }        
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}