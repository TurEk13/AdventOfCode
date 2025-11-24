using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Zadania._2015;

public class D20Z01 : IZadanie
{
    private Int64 _numerDomu;
    private SortedDictionary<int, int> _obdarowaneDomy;
    private SortedDictionary<int, int> _pomijaneDomy;

    public D20Z01(bool daneTestowe = false)
    {
        this._numerDomu = 0;
        this._obdarowaneDomy = new ();
        this._pomijaneDomy = new ();
    }

    public void RozwiazanieZadania()
    {
        int ilePrezentow = 34_000_000;
        int maksIloscElfow = ilePrezentow / 10;
        
        int prezentyElfa;
        for (int numerElfa = 1; numerElfa <= maksIloscElfow; numerElfa++)
        {
            prezentyElfa = numerElfa * 10;

            for (int numerDomu = numerElfa; numerDomu <= maksIloscElfow; numerDomu += numerElfa)
            {
                if (this._obdarowaneDomy.ContainsKey(numerDomu))
                {
                    this._obdarowaneDomy[numerDomu] += prezentyElfa;

                    if (this._obdarowaneDomy[numerDomu] > ilePrezentow)
                    {
                        this._pomijaneDomy.Add(numerDomu, this._obdarowaneDomy[numerDomu]);
                        this._obdarowaneDomy.Remove(numerDomu);
                    }
                }

                if (!this._pomijaneDomy.ContainsKey(numerDomu) && !this._obdarowaneDomy.ContainsKey(numerDomu))
                {
                    this._obdarowaneDomy.Add(numerDomu, prezentyElfa);
                }
            }
        }

        int numerDomuOdbarownego = this._obdarowaneDomy.FirstOrDefault(od => od.Value >= ilePrezentow).Key;
        int numerDomuPomijanego = this._pomijaneDomy.FirstOrDefault(pd => pd.Value >= ilePrezentow).Key;

        this._numerDomu = numerDomuOdbarownego > numerDomuPomijanego ? numerDomuPomijanego : numerDomuOdbarownego;
    }

    public string PokazRozwiazanie()
    {
        return this._numerDomu.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}