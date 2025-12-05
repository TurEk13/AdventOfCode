using System;
using System.Globalization;

namespace Zadania._2015;

public class D25Z01 : IZadanie
{
    private UInt64 _Wartosc;
    public D25Z01()
    {
    }

    public void RozwiazanieZadania()
    {
        int kolumna, wiersz = 1, wierszMax;
        this._Wartosc = 20_151_125;
        bool stop = false;

        for(kolumna = 1, wierszMax = 2;; wierszMax++)
        {
            for(wiersz = wierszMax; wiersz > 0; wiersz--)
            {
                if(kolumna == 3_019 && wiersz == 3_010)
                {
                    stop = true;
                }

                this._Wartosc = this._Wartosc * 252_533 % 33_554_393;
                kolumna++;

                if(stop)
                {
                    return;
                }
            }

            kolumna = 1;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wartosc.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}