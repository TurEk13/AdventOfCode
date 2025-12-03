using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public class D03Z01 : IZadanie
{
    private int _PrawidloweTrojkaty;
    List<int[]> _SpisWielkosci;

    public D03Z01()
    {
        this._PrawidloweTrojkaty = 0;
        this._SpisWielkosci = new ();

        FileStream fs = new(".\\Dane\\2016\\03\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        Regex liczby = new Regex(@"(\d+)");
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
        foreach (int[] trojkat in this._SpisWielkosci)
        {
            if (trojkat[0] + trojkat[1] > trojkat[2] && trojkat[1] + trojkat[2] > trojkat[0] && trojkat[2] + trojkat[0] > trojkat[1])
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