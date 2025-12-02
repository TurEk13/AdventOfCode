using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2025;

public partial class D02Z02 : IZadanie
{
    private string _Kody;
    private List<Int64> _ZleNumery;
    public D02Z02(bool daneTestowe = false)
    {
        this._ZleNumery = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\02\\proba.txt" : ".\\Dane\\2025\\02\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        
        this._Kody = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    [GeneratedRegex(@"(\d+)-(\d+)")]
    private static partial Regex MyRegex();
    public void RozwiazanieZadania()
    {
        Int64 start, stop;
        Regex wzor = MyRegex();
        MatchCollection mc = wzor.Matches(this._Kody);

        foreach(Match m in mc)
        {
            start = Convert.ToInt64(m.Groups[1].Value);
            stop = Convert.ToInt64(m.Groups[2].Value);

            for(Int64 i = start; i <= stop; i++)
            {
                if(this.ZnajdzPare(i) || this.ZnajdzPowtorzeniaX(i))
                {
                    this._ZleNumery.Add(i);
                }
            }
        }
    }

    private bool ZnajdzPare(Int64 numer)
    {
        string liczba = numer.ToString();

        return liczba[..(liczba.Length / 2)] == liczba[(liczba.Length / 2)..];
    }

    private bool ZnajdzPowtorzeniaX(Int64 numer)
    {
        string liczba = numer.ToString();
        Regex wzor;
        MatchCollection mc;

        for(int i = 1; i < liczba.Length / 2 + 1; i++)
        {
            wzor = new Regex(liczba[..i]);
            mc = wzor.Matches(liczba);

            if(mc.Sum(m => m.Length) == liczba.Length && mc.Count > 2)
            {
                return true;
            }
        }

        return false;
    }

    public string PokazRozwiazanie()
    {
        return this._ZleNumery.Sum().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}