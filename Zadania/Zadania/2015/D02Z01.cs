using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D02Z01 : IZadanie
{
    private List<int[]> WymiaryPrezentow;
    private Int64 IloscPapieru;
    public D02Z01()
    {
        this.WymiaryPrezentow = new();
        this.IloscPapieru = 0;
        FileStream fs = new(".\\Dane\\2015\\02\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;
        StreamReader sr = new(fs);

        while((linia = sr.ReadLine()) != null)
        {
            this.WymiaryPrezentow.Add(linia.Split('x').Select(i => int.Parse(i)).ToArray());
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        Int64[] tmp;

        foreach (int[] prezent in this.WymiaryPrezentow)
        {
            tmp = prezent.OrderBy(p => p).Select(p => Convert.ToInt64(p)).ToArray();

            this.IloscPapieru += 3 * tmp[0] * tmp[1] + 2 * tmp[1] * tmp[2] + 2 * tmp[2] * tmp[0];

        }
    }

    public string PokazRozwiazanie()
    {
        return this.IloscPapieru.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}