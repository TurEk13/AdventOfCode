using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D12Z02 : IZadanie
{
    private string _JSON;
    private Int64 _Suma;
    public D12Z02(bool daneTestowe = false)
    {
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\12\\proba.txt" : ".\\Dane\\2015\\12\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        
        this._JSON = sr.ReadToEnd();
        
        sr.Close(); fs.Close();
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex SzukajLiczb();

    [GeneratedRegex(@"{[^{}]*((?'a'{[^{}]*)+(?'-a'}[^{}]*)+)*(?(a)(?!)):""red""[^{}]*((?'a'{[^{}]*)+(?'-a'}[^{}]*)+)*(?(a)(?!))[^{}]*}")]
    private static partial Regex SzukajRed();

    public void RozwiazanieZadania()
    {
        this._Suma = SzukajLiczb().Matches(SzukajRed().Replace(this._JSON, "")).Cast<Match>().Sum(m => int.Parse(m.Value));
    }

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}