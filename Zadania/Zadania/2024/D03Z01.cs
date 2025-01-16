using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public class D03Z01 : IZadanie
{
    private int suma;
    private string tekst;

    public D03Z01(bool daneTestowe = false)
    {
        this.suma = 0;
        this.tekst = daneTestowe ? File.ReadAllText(".\\Dane\\2024\\03\\proba1.txt") : File.ReadAllText(".\\Dane\\2024\\03\\dane.txt");
    }

    public void RozwiazanieZadania()
    {
        Regex mnozenie = new Regex(@"mul\(\d{1,3},\d{1,3}\)", RegexOptions.IgnoreCase);
        MatchCollection mnozenieMC = mnozenie.Matches(tekst);

        foreach (Match m in mnozenieMC)
        {
            suma += Mnoz(m.Value);
        }
    }

    public string PokazRozwiazanie()
    {
        return this.suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private static int Mnoz(string x)
    {
        Regex liczby = new(@"\d{1,3}");
        MatchCollection l = liczby.Matches(x);
        return Convert.ToInt32(l[0].Value) * Convert.ToInt32(l[1].Value);
    }
}