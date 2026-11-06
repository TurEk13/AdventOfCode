using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2017;

public class D17Z01 : IZadanie
{
    private readonly int _RozmiarSpinLock;
    private List<int> _Stany;
    private int _Wynik;

    public D17Z01(bool daneTestowe = false)
    {
        this._Stany = [0];
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\17\\proba.txt" : ".\\Dane\\2017\\17\\dane.txt", FileMode.Open, FileAccess.Read));

        this._RozmiarSpinLock = Convert.ToInt32(sr.ReadToEnd());

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        int obecnaPozycja = this._Stany[0], nowaPozycja;

        for(int i = 1; i <= 2017; i++)
        {
            nowaPozycja = (obecnaPozycja + this._RozmiarSpinLock) % this._Stany.Count + 1;
            this._Stany.Insert(nowaPozycja, i);
            obecnaPozycja = nowaPozycja;
        }

        this._Wynik = this._Stany[obecnaPozycja + 1];
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}