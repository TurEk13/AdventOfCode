using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public class D03Z02 : IZadanie
{
    private int _PrawidloweTrojkaty;
    List<int[]> _SpisWielkosci;


    public D03Z02()
    {
        this._PrawidloweTrojkaty = 0;
        this._SpisWielkosci = new ();

        FileStream fs = new(".\\Dane\\2016\\03\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        Regex liczby = new (@"(\d+)");
        MatchCollection mc;

        while ((linia = sr.ReadLine()) is not null)
        {
            mc = liczby.Matches(linia);
            this._SpisWielkosci.Add(mc.Select(bok => Convert.ToInt32(bok.Value)).ToArray<int>());
        }
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for (int i = 0; i < this._SpisWielkosci.Count; i += 3)
        {
            if (this._SpisWielkosci[i][0] + this._SpisWielkosci[i + 1][0] > this._SpisWielkosci[i + 2][0] && this._SpisWielkosci[i + 1][0] + this._SpisWielkosci[i + 2][0] > this._SpisWielkosci[i][0] && this._SpisWielkosci[i + 2][0] + this._SpisWielkosci[i][0] > this._SpisWielkosci[i + 1][0])
            {
                this._PrawidloweTrojkaty++;
            }
            if (this._SpisWielkosci[i][1] + this._SpisWielkosci[i + 1][1] > this._SpisWielkosci[i + 2][1] && this._SpisWielkosci[i + 1][1] + this._SpisWielkosci[i + 2][1] > this._SpisWielkosci[i][1] && this._SpisWielkosci[i + 2][1] + this._SpisWielkosci[i][1] > this._SpisWielkosci[i + 1][1])
            {
                this._PrawidloweTrojkaty++;
            }

            if (this._SpisWielkosci[i][2] + this._SpisWielkosci[i + 1][2] > this._SpisWielkosci[i + 2][2] && this._SpisWielkosci[i + 1][2] + this._SpisWielkosci[i + 2][2] > this._SpisWielkosci[i][2] && this._SpisWielkosci[i + 2][2] + this._SpisWielkosci[i][2] > this._SpisWielkosci[i + 1][2])
            {
                this._PrawidloweTrojkaty++;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._PrawidloweTrojkaty.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}