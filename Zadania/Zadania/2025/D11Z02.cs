using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D11Z02 : IZadanie
{
    private Dictionary<string, string[]> _Stacje;

    public D11Z02(bool daneTestowe = false)
    {
        this._Stacje = new Dictionary<string, string[]>();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\11\\proba.txt" : ".\\Dane\\2025\\11\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Stacje.Add(linia[..3], linia[5..].Split(' '));
        }
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        // 14 976, 56 070 144-
        string Svr = "svr", Out = "out", Dac = "dac", Fft = "fft";

        int sd = this.ZnajdzSciezke(Svr, Dac);
        int df = this.ZnajdzSciezke(Dac, Fft);
        int fo = this.ZnajdzSciezke(Fft, Out);

        int sf = this.ZnajdzSciezke(Svr, Fft);
        int fd = this.ZnajdzSciezke(Fft, Dac);
        int od = this.ZnajdzSciezke(Dac, Out);

        int x = sd * df * fo + sf * fd * od;
        int y = sd * df * fo * sf * fd * od;
    }

    private int ZnajdzSciezke(string poczatek, string koniec)
    {
        foreach(KeyValuePair<string, string[]> kvp in this._Stacje.Where(s => s.Key.Equals(poczatek)))
        {
            foreach (string s in kvp.Value)
            {
                if(s.Equals(koniec))
                {
                    return 1;
                }

                return this.ZnajdzSciezke(s, koniec) + 1;
            }
        }

        return 0;
    }

    public string PokazRozwiazanie()
    {
        return 0.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}