using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D22Z02 : IZadanie
{
    private List<Int64> _Ceny;

    public D22Z02(bool daneTestowe = false)
    {
        this._Ceny = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\22\\proba.txt" : ".\\Dane\\2024\\22\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Ceny.Add(Convert.ToInt64(linia));
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for(int i = 0; i < 2_000; i++)
        {
            for(int l = 0; l < this._Ceny.Count; l++)
            {
                this._Ceny[l] = this.GenerujNowaLiczba(this._Ceny[l]);
            }
        }
    }

    private Int64 GenerujNowaLiczba(Int64 liczba)
    {
        return this.Krok3(this.Krok2(this.Krok1(liczba)));
    }

    private Int64 Krok1(Int64 liczba)
    {
        Int64 tmp = liczba * 64;
        liczba = this.Mix(tmp, liczba);
        return this.Prune(liczba);
    }

    private Int64 Krok2(Int64 liczba)
    {
        Int64 tmp = Convert.ToInt64(Math.Floor(liczba / 32.0));
        liczba = this.Mix(tmp, liczba);
        return this.Prune(liczba);
    }

    private Int64 Krok3(Int64 liczba)
    {
        Int64 tmp = liczba * 2048;
        liczba = this.Mix(tmp, liczba);
        return this.Prune(liczba);
    }

    private Int64 Mix(Int64 nowaLiczba, Int64 sekretnaLiczba)
    {
        return nowaLiczba ^ sekretnaLiczba;
    }

    private Int64 Prune(Int64 nowaLiczba)
    {
        return nowaLiczba % 16_777_216;
    }

    public string PokazRozwiazanie()
    {
        return this._Ceny.Sum().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}