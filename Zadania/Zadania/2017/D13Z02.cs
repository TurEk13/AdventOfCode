using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D13Z02 : IZadanie
{
    [GeneratedRegex(@"(?'poziom'\d{1,})(?:: )((?'zasieg'\d{1,})(?:\r|$))")]
    private static partial Regex WzorLinii();

    private Dictionary<int, int> _Zapora;
    private int _Opoznienie;

    public D13Z02(bool daneTestowe = false)
    {
        this._Zapora = [];
        this._Opoznienie = 0;
        Regex r = WzorLinii();
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\13\\proba.txt" : ".\\Dane\\2017\\13\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = r.Matches(sr.ReadToEnd());

        sr.Close();

        foreach(Match m in mc)
        {
            this._Zapora.TryAdd(Convert.ToInt32(m.Groups["poziom"].Value), Convert.ToInt32(m.Groups["zasieg"].Value));
        }
    }

    public void RozwiazanieZadania()
    {
        int maks = this._Zapora.Keys.Max();
        this._Opoznienie = 0;
        bool pracuj;

        do
        {
            pracuj = false;

            for(int skanerId = 0; skanerId <= maks; skanerId++)
            {
                if(this._Zapora.TryGetValue(skanerId, out int zasieg) && this.BlokadaPola(this._Opoznienie + skanerId, zasieg))
                {
                    pracuj = true;
                    skanerId = maks + 1;
                    this._Opoznienie++;
                }
            }
        }
        while(pracuj);
    }

    private bool BlokadaPola(int pikosekunda,  int zasieg)
    {
        if(pikosekunda == 0)
        {
            return true;
        }

        int szerokosc = zasieg - 1;
        int runda = (pikosekunda / szerokosc) - 1;

        if(runda < 1)
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
        return this._Opoznienie.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}