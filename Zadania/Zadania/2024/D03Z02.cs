using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public class D03Z02 : IZadanie
{
    private int suma;
    private string tekst;

    public D03Z02(bool daneTestowe = false)
    {
        this.suma = 0;
        
        this.tekst = File.ReadAllText(daneTestowe ? ".\\Dane\\2024\\03\\proba2.txt" : ".\\Dane\\2024\\03\\dane.txt");
    }

    public void RozwiazanieZadania()
    {
        this.tekst += "don't()";
        Regex doo = new Regex(@"do\(\)");
        Regex dont = new Regex(@"don't\(\)");
        Regex mnozenie = new Regex(@"mul\(\d{1,3},\d{1,3}\)", RegexOptions.IgnoreCase);

        //poczatek
        Match poczatekM = dont.Match(this.tekst);
        MatchCollection mnozenieMC = mnozenie.Matches(tekst.Substring(0, poczatekM.Index + 7));

        foreach (Match m in mnozenieMC)
        {
            suma += Mnoz(m.Value);
        }

        tekst = tekst.Substring(poczatekM.Index + 7);

        //Reszta
        Match doM;
        Match dontM;
        MatchCollection mulMC;
        string tekstTMP;

        while ((doM = doo.Match(tekst)).Length != 0)
        {
            tekst = tekst.Substring(doM.Index + "do()".Length);
            dontM = dont.Match(tekst);

            tekstTMP = tekst.Substring(0, dontM.Index + 7);

            mulMC = mnozenie.Matches(tekstTMP);

            foreach (Match m in mulMC)
            {
                suma += Mnoz(m.Value);
            }

            tekst = tekst.Substring(dontM.Index + "don't()".Length);
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