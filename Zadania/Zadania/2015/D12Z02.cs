using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D12Z02 : IZadanie
{
    private string JSON;
    private Int64 Suma;
    public D12Z02(bool daneTestowe = false)
    {
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\12\\proba.txt" : ".\\Dane\\2015\\12\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        
        this.JSON = sr.ReadToEnd();
        
        sr.Close(); fs.Close();
    }

    [GeneratedRegex(@"[+-]?([0-9]{1,})")]
    private static partial Regex SzukajLiczb();
    [GeneratedRegex(@"{(?>[^{}:]+|[^{}]+?|({(?>[^{}]+|'1')*}))*:""red(?>[^{}]+|({(?>[^{}]+|'2')*}))*}")]
    private static partial Regex SzukajRed();
    public void RozwiazanieZadania()
    {
        // 141 962 +
        Regex wzorRed = SzukajRed();
        Regex wzorLiczb = SzukajLiczb();

        Int64 sumaRed = wzorRed.Matches(this.JSON).Sum(m => wzorLiczb.Matches(m.Value).Sum(l => Convert.ToInt64(l.Value)));
        Int64 sumaCalkowita = wzorLiczb.Matches(this.JSON).Sum(m => Convert.ToInt64(m.Value));  

        this.Suma = sumaCalkowita - sumaRed;
    }

    public string PokazRozwiazanie()
    {
        return this.Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}