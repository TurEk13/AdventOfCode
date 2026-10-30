using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D02Z01 : IZadanie
{
    private UInt32 _Wynik;
    private List<UInt32[]> _Arkusz;

    [GeneratedRegex(@"(\d){1,}")]
    private static partial Regex WzorLiczb();
    
    public D02Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Arkusz = [];
        Regex Liczby = WzorLiczb();
        MatchCollection mc;

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\02\\proba.txt" : ".\\Dane\\2017\\02\\dane.txt", FileMode.Open, FileAccess.Read));

        string[] tmp = sr.ReadToEnd().Split(Environment.NewLine);

        sr.Close();

        foreach(string s in tmp)
        {
            mc = Liczby.Matches(s);
            this._Arkusz.Add([.. mc.Select(l => Convert.ToUInt32(l.Value))]);
        }
    }

    public void RozwiazanieZadania()
    {
        foreach(UInt32[] l in this._Arkusz)
        {
            this._Wynik += l.Max() - l.Min();
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}