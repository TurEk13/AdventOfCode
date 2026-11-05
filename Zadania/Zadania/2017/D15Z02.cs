using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D15Z02 : IZadanie
{
    [GeneratedRegex(@"(?'start'\d{1,})")]
    private static partial Regex StartGeneratora();
    private int _Wynik;
    private Generator _GeneratorA;
    private Generator _GeneratorB;

    public D15Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        Regex r = StartGeneratora();
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\15\\proba.txt" : ".\\Dane\\2017\\15\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = r.Matches(sr.ReadToEnd());

        sr.Close();

        this._GeneratorA = new Generator(Convert.ToUInt64(mc[0].Groups["start"].Value), 16_807, 4);
        this._GeneratorB = new Generator(Convert.ToUInt64(mc[1].Groups["start"].Value), 48_271, 8);
    }

    public void RozwiazanieZadania()
    {
        int ilePar = 5_000_000;
        this._GeneratorA.Dzialaj(ilePar);
        this._GeneratorB.Dzialaj(ilePar);

        for(int i = 0; i < this._GeneratorA.Dlugosc; i++)
        {
            if((this._GeneratorA[i] & 0xFFFF) == (this._GeneratorB[i] & 0xFFFF))
            {
                this._Wynik++;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Generator
    {
        private readonly UInt64 _Wspolczynnik;
        private readonly UInt64 _Dzielnik;
        private List<UInt64> _Wyniki;
        private UInt64 _ObecnyWynik;
        public int Dlugosc { get { return this._Wyniki.Count; } }

        public UInt64 this[int i]
        {
            get
            {
                if(i >= 0 && i < this.Dlugosc)
                {
                    return this._Wyniki[i];
                }

                return 0;
            }
        }

        public Generator(UInt64 start, UInt64 wspolczynnik, UInt64 dzielnik) => (start, wspolczynnik, dzielnik, _) = (this._ObecnyWynik = start, this._Wspolczynnik = wspolczynnik, this._Dzielnik = dzielnik, this._Wyniki = []);

        public void Dzialaj(int ilePowtorzeń)
        {
            for(int i = 0; this.Dlugosc < ilePowtorzeń; i++)
            {
                do
                {
                    this._ObecnyWynik *= this._Wspolczynnik;
                    this._ObecnyWynik %= 2_147_483_647;
                }while(this._ObecnyWynik % this._Dzielnik != 0);

                this._Wyniki.Add(this._ObecnyWynik);
            }
        }
    }
}