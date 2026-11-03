using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2017;

public partial class D10Z01 : IZadanie
{
    private List<int> _Elementy;
    private readonly List<int> _Operacje;

    public D10Z01(bool daneTestowe = false)
    {
        int dlugosc = daneTestowe ? 5 : 256;
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\10\\proba.txt" : ".\\Dane\\2017\\10\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Operacje = [.. sr.ReadToEnd().Split(',').Select(o => Convert.ToInt32(o))];

        sr.Close();

        this._Elementy = [.. Enumerable.Range(0, dlugosc)];
    }

    public void RozwiazanieZadania()
    {
        int obecnaPozycja = 0, przesun = 0, dlugosc = this._Elementy.Count;
        int[] tmp;

        foreach(int polecenie in this._Operacje)
        {
            tmp = new int[polecenie];

            if(polecenie + obecnaPozycja <= dlugosc)
            {
                this._Elementy.CopyTo(obecnaPozycja, tmp, 0, polecenie);
            }

            if(polecenie + obecnaPozycja > dlugosc)
            {
                this._Elementy.CopyTo(obecnaPozycja, tmp, 0, dlugosc - obecnaPozycja);
                this._Elementy.CopyTo(0, tmp, dlugosc - obecnaPozycja, tmp.Length - (tmp.Length - (polecenie - (dlugosc - obecnaPozycja))));
            }

            tmp = [.. tmp.Reverse()];
            for(int i = 0; i < tmp.Length; i++)
            {
                this._Elementy[(obecnaPozycja + i) % dlugosc] = tmp[i];
            }

            obecnaPozycja = (obecnaPozycja + przesun + polecenie) % dlugosc;
            przesun++;
        }

    }

    public string PokazRozwiazanie()
    {
        return (this._Elementy[0] * this._Elementy[1]).ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}