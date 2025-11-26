using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D12Z01 : IZadanie
{
    private string JSON;
    private Int64 Suma;
    public D12Z01(bool daneTestowe = false)
    {
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\12\\proba.txt" : ".\\Dane\\2015\\12\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        
        this.JSON = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    [GeneratedRegex(@"[+-]?(\d+)\1*")]
    private static partial Regex MyRegex();
    public void RozwiazanieZadania()
    {
        Regex wzor = MyRegex();

        this.Suma = wzor.Matches(this.JSON).Select(m => Convert.ToInt64(m.Value)).Sum();
    }

    public string PokazRozwiazanie()
    {
        return this.Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}