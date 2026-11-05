using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D13Z01 : IZadanie
{
    [GeneratedRegex(@"(?'poziom'\d{1,})(?:: )((?'zasieg'\d{1,})(?:\r|$))")]
    private static partial Regex WzorLinii();

    private Dictionary<int, int> _Zapora;
    private int _Wynik;

    public D13Z01(bool daneTestowe = false)
    {
        this._Zapora = [];
        this._Wynik = 0;
        Regex r = WzorLinii();
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\13\\proba.txt" : ".\\Dane\\2017\\13\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = r.Matches(sr.ReadToEnd());

        foreach(Match m in mc)
        {
            this._Zapora.TryAdd(Convert.ToInt32(m.Groups["poziom"].Value), Convert.ToInt32(m.Groups["zasieg"].Value));
        }

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        int maks = this._Zapora.Keys.Max();

        for(int i = 0; i <= maks; i++)
        {
            if(this._Zapora.TryGetValue(i, out int zasieg) && this.BlokadaPola(i, zasieg))
            {
                this._Wynik += i * zasieg;
            }
        }
    }

    private bool BlokadaPola(int pikosekunda, int zasieg)
    {
        int szerokosc = zasieg - 1;
        int runda = (pikosekunda / szerokosc) - 1;

        if(pikosekunda == 0)
        {
            return true;
        }

        if(pikosekunda > 0 && runda < 1)
        {
            return false;
        }

        if(runda % 2 == 1)
        {
            return pikosekunda % szerokosc == 0;
        }
        
        return false;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}