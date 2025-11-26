using System;
using System.Globalization;
using System.IO;
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

    [GeneratedRegex(@"[+-]?(\d+)\1*")]
    private static partial Regex SzukajLiczb();
    [GeneratedRegex(@"{(?>[^{}:]+|[^{}]+?|({(?>[^{}]+|'1')*}))*:""red(?>[^{}]+|({(?>[^{}]+|'2')*}))*}")]
    private static partial Regex SzukajRed();
    public void RozwiazanieZadania()
    {
        // 141962 +
        Regex wzorRed = SzukajRed();
        MatchCollection dopasowaniaRed = wzorRed.Matches(this.JSON);

        Regex wzorLiczb = SzukajLiczb();
        MatchCollection dopasowaniaLiczb = wzorLiczb.Matches(this.JSON);

        int red = 0, liczba = 0;
        Int64 wynik = 0;
        while(red < dopasowaniaRed.Count)
        {
            if(dopasowaniaLiczb[liczba].Index < dopasowaniaRed[red].Index)
            {
                wynik += Convert.ToInt64(dopasowaniaLiczb[liczba].Value);
                liczba++;
                continue;
            }

            if(dopasowaniaLiczb[liczba].Index < dopasowaniaRed[red].Index + dopasowaniaRed[red].Length)
            {
                liczba++;
                continue;
            }

            if(dopasowaniaLiczb[liczba].Index > dopasowaniaRed[red].Index + dopasowaniaRed[red].Length)
            {
                red++;
            }
        }

        while(liczba < dopasowaniaLiczb.Count)
        {
            wynik += Convert.ToInt64(dopasowaniaLiczb[liczba].Value);
            liczba++;
        }

        this.Suma = wynik;
    }

    public string PokazRozwiazanie()
    {
        return this.Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}