using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D15Z01 : IZadanie
{
    private Dictionary<string, (int Pojemnosc, int Trwalosc, int Smak, int Tekstura, int Kalorie)> _skladniki;
    private List<string> _nazwySkladnikow;
    private Int64 _maksymalnaWartoscCiastka;

    public D15Z01(bool daneTestowe = false)
    {
        this._skladniki = new Dictionary<string, (int Pojemnosc, int Trwalosc, int Smak, int Tekstura, int Kalorie)>();
        this._nazwySkladnikow = new ();
        this._maksymalnaWartoscCiastka = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\15\\proba.txt" : ".\\Dane\\2015\\15\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] liniaa;

        while((linia = sr.ReadLine()) is not null)
        {
            liniaa = linia.Split(' ');
            this._skladniki.Add(liniaa[0][..liniaa[0].IndexOf(':')], new(Convert.ToInt32(liniaa[2][..liniaa[2].IndexOf(',')]), Convert.ToInt32(liniaa[4][..liniaa[4].IndexOf(',')]), Convert.ToInt32(liniaa[6][..liniaa[6].IndexOf(',')]), Convert.ToInt32(liniaa[8][..liniaa[8].IndexOf(',')]), Convert.ToInt32(liniaa[10])));
            this._nazwySkladnikow.Add(liniaa[0][..liniaa[0].IndexOf(':')]);
        }
    }

    public void RozwiazanieZadania()
    {
        int iloscLyzek = 100;
        Int64 wartosc;
        Int64[] dane = new Int64[5];
        int s1, s2, s3, s4;

        for(s1 = 1; s1 <= iloscLyzek; s1++)
        {
            for(s2 = 1; s2 <= iloscLyzek; s2++)
            {
                for(s3 = 1; s3 <= iloscLyzek - (s1 + s2); s3++)
                {
                    s4 = iloscLyzek - (s1 + s2 + s3);

                    dane[0] = s1 * this._skladniki[this._nazwySkladnikow[0]].Pojemnosc + s2 * this._skladniki[this._nazwySkladnikow[1]].Pojemnosc + s3 * this._skladniki[this._nazwySkladnikow[2]].Pojemnosc + s4 * this._skladniki[this._nazwySkladnikow[3]].Pojemnosc;

                    dane[1] = s1 * this._skladniki[this._nazwySkladnikow[0]].Trwalosc + s2 * this._skladniki[this._nazwySkladnikow[1]].Trwalosc + s3 * this._skladniki[this._nazwySkladnikow[2]].Trwalosc + s4 * this._skladniki[this._nazwySkladnikow[3]].Trwalosc;

                    dane[2] = s1 * this._skladniki[this._nazwySkladnikow[0]].Smak + s2 * this._skladniki[this._nazwySkladnikow[1]].Smak + s3 * this._skladniki[this._nazwySkladnikow[2]].Smak + s4 * this._skladniki[this._nazwySkladnikow[3]].Smak;

                    dane[3] = s1 * this._skladniki[this._nazwySkladnikow[0]].Tekstura + s2 * this._skladniki[this._nazwySkladnikow[1]].Tekstura + s3 * this._skladniki[this._nazwySkladnikow[2]].Tekstura + s4 * this._skladniki[this._nazwySkladnikow[3]].Tekstura;

                    if(dane[0] < 0) dane[0] = 0;
                    if(dane[1] < 0) dane[1] = 0;
                    if(dane[2] < 0) dane[2] = 0;
                    if(dane[3] < 0) dane[3] = 0;

                    wartosc = dane[0] * dane[1] * dane[2] * dane[3];

                    if(this._maksymalnaWartoscCiastka < wartosc)
                    {
                        this._maksymalnaWartoscCiastka = wartosc;
                    }
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._maksymalnaWartoscCiastka.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}