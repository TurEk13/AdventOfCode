using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D12Z01 : IZadanie
{
    private List<string> JSON;
    private List<Int64> Suma;
    public D12Z01(bool daneTestowe = false)
    {
        this.JSON = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\12\\proba.txt" : ".\\Dane\\2015\\12\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        String linia;

        while((linia = sr.ReadLine()) is not null)
        {
            this.JSON.Add(linia);
        }

        sr.Close(); fs.Close();
    }

    [GeneratedRegex(@"[+-]?(\d+)\1*")]
    private static partial Regex MyRegex();
    public void RozwiazanieZadania()
    {
        this.Suma = new ();
        Regex wzor = MyRegex();
        MatchCollection dopasowania;

        foreach(string linia in this.JSON)
        {
            dopasowania = wzor.Matches(linia);

            this.Suma.Add(dopasowania.Select(m => Convert.ToInt64(m.Value)).Sum());
        }
    }

    public string PokazRozwiazanie()
    {
        return string.Join("\r\n", this.Suma);
    }
}