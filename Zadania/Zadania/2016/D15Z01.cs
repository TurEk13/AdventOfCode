using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D15Z01 : IZadanie
{
    private int _Czas;
    private List<Dysk> _Dyski;

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex WzorLiczb();
    public D15Z01(bool daneTestowe = false)
    {
        this._Dyski = new ();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\15\\proba.txt" : ".\\Dane\\2016\\15\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;
        Regex liczby = WzorLiczb();
        MatchCollection mc;

        while((linia = sr.ReadLine()) is not null)
        {
            mc = liczby.Matches(linia);
            this._Dyski.Add(new (Convert.ToInt32(mc[1].Value), Convert.ToInt32(mc[3].Value)));
        }
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        bool kontynuuj = true;
        int czas;
        for(this._Czas = 0; kontynuuj; this._Czas++)
        {
            czas = this._Czas;
            foreach(Dysk d in this._Dyski)
            {
                czas++;
                d.Przeszlo = d.CzyOtwarte(czas);
            }

            if(this._Dyski.Where(d => d.Przeszlo.Equals(false)).ToArray().Length == 0)
            {
                break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Czas.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Dysk
    {
        private int _IlePozycji;
        private int _PozycjaStartowa;
        public bool Przeszlo;

        public Dysk(int ilePozycji, int pozycjaStartowa, bool przeszlo = false) => (ilePozycji, pozycjaStartowa, przeszlo) = (this._IlePozycji = ilePozycji, this._PozycjaStartowa = pozycjaStartowa, this.Przeszlo = przeszlo);

        public bool CzyOtwarte(int czas)
        {
            return (this._PozycjaStartowa + czas) % this._IlePozycji == 0;
        }
    }
}