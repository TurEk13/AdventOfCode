using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public class D01Z02 : IZadanie
{
    private int _Haslo;
    private List<string[]> _Instrukcje;
    public D01Z02(bool daneTestowe = false)
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
        int poprzedniStart, start = 50, przesuniecie, pelneObroty;


        foreach(string[] s in this._Instrukcje)
        {
            przesuniecie = Convert.ToInt32(s[1]) % 100;
            pelneObroty = Convert.ToInt32(s[1]) / 100;
            this._Haslo = pelneObroty > 0 ? this._Haslo + pelneObroty : this._Haslo;
            poprzedniStart = start;

            switch(s[0])
            {
                case "L":
                    start -= przesuniecie;
                    if(start < 0 && poprzedniStart > 0)
                    {
                        start += 100;
                        this._Haslo++;
                    }

                    if(poprzedniStart == 0 && start < 0)
                    {
                        start += 100;
                    }
                    break;
                case "R":
                    start += przesuniecie;
                    if(poprzedniStart < 100 && start > 100)
                    {
                        start -= 100;
                        this._Haslo++;
                    }

                    if(poprzedniStart < 100 && start == 100)
                    {
                        start -= 100;
                    }

                    break;
            }

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