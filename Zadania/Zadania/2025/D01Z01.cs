using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public class D01Z01 : IZadanie
{
    private int _Haslo;
    private List<string[]> _Instrukcje;
    public D01Z01(bool daneTestowe = false)
    {
        this._Haslo = 0;
        this._Instrukcje = new ();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\01\\proba.txt" : ".\\Dane\\2025\\01\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;

        StreamReader sr = new(fs);

        while((linia = sr.ReadLine()) is not null)
        {
            this._Instrukcje.Add([linia[..1], linia[1..]]);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        int start = 50;
        int przesuniecie;

        foreach(string[] s in this._Instrukcje)
        {
            przesuniecie = Convert.ToInt32(s[1]) % 100;
            start = s[0].Equals("L") ? start - przesuniecie : start + przesuniecie;
            start = start > 99 ? start - 100 : start < 0 ? start + 100 : start;

            if(start == 0)
            {
                this._Haslo++;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Haslo.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}