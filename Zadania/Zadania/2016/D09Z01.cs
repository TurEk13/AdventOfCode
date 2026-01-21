using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D09Z01 : IZadanie
{
    private Int64 _Wynik;
    private string _Kompresja;
    public D09Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\09\\proba.txt" : ".\\Dane\\2016\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);

        this._Kompresja = sr.ReadToEnd();

        sr.Close(); fs!.Close();
    }

    [GeneratedRegex(@"\((?<ileZnakow>[0-9]{1,})x(?<mnoznik>[0-9]{1,})\)|(?:\w{1})\((?<ileZnakow>[0-9]{1,})x(?<mnoznik>[0-9]{1,})\)(?:[\(])")]
    private static partial Regex Zakresy();
    public void RozwiazanieZadania()
    {
        Regex r = Zakresy();
        MatchCollection mc = r.Matches(this._Kompresja);

        for (int i = 0, m = 0; i < this._Kompresja.Length; i++)
        {
            if (i == mc[m].Groups["ileZnakow"].Index - 1)
            {
                this._Wynik += Convert.ToInt32(mc[m].Groups["ileZnakow"].Value) * Convert.ToInt32(mc[m].Groups["mnoznik"].Value);
                i += Convert.ToInt32(mc[m].Groups["ileZnakow"].Value) + mc[m].Groups["ileZnakow"].Length + mc[m].Groups["mnoznik"].Length + 2;
                m++;
                
                if (m < mc.Count && !(i < mc[m].Groups["ileZnakow"].Index - 1))
                {
                    m += mc.Skip(m).Count(k => k.Groups["ileZnakow"].Index - 1 < i);

                }
                continue;
            }

            this._Wynik++;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}