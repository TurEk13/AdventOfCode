using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D06Z02 : IZadanie
{
    private int[] _PamiecOperacje;

    private List<PamiecProba> _ZapamietaneProba;
    private List<PamiecDane> _ZapamietaneDane;
    private Int32 _Wynik;

    [GeneratedRegex(@"(\d){1,}")]
    private static partial Regex WzorLiczb();
    
    public D06Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        Regex liczby = WzorLiczb();
        this._ZapamietaneProba = [];
        this._ZapamietaneDane = [];

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\06\\proba.txt" : ".\\Dane\\2017\\06\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = liczby.Matches(sr.ReadToEnd());

        sr.Close();

        this._PamiecOperacje = [.. mc.Select(l => Convert.ToInt32(l.Value))];
    }

    public void RozwiazanieZadania()
    {
        if(this._PamiecOperacje.Length == 4)
        {
            this.Proba();
        }
        else
        {
            this.Dane();
        }
    }

    private void Proba()
    {
        int maks, indeks;

        PamiecProba pamiecProba = this.WczytajDoPamieciProba(this._PamiecOperacje);

        while (!this._ZapamietaneProba.Contains(pamiecProba))
        {
            this._ZapamietaneProba.Add(pamiecProba);
            maks = this._PamiecOperacje.Max();
            indeks = this._PamiecOperacje.IndexOf(maks);
            this._PamiecOperacje[indeks] = 0;
            indeks++;

            for (int i = 0; i < maks; i++)
            {
                this._PamiecOperacje[(indeks + i) % this._PamiecOperacje.Length]++;
            }

            pamiecProba = this.WczytajDoPamieciProba(this._PamiecOperacje);
        }

        this._Wynik = this._ZapamietaneProba.Count - this._ZapamietaneProba.IndexOf(pamiecProba);
    }

    private void Dane()
    {
        int maks, indeks;

        PamiecDane pamiecDane = this.WczytajDoPamieciDane(this._PamiecOperacje);

        while (!this._ZapamietaneDane.Contains(pamiecDane))
        {
            this._ZapamietaneDane.Add(pamiecDane);
            maks = this._PamiecOperacje.Max();
            indeks = this._PamiecOperacje.IndexOf(maks);
            this._PamiecOperacje[indeks] = 0;
            indeks++;

            for (int i = 0; i < maks; i++)
            {
                this._PamiecOperacje[(indeks + i) % this._PamiecOperacje.Length]++;
            }

            pamiecDane = this.WczytajDoPamieciDane(this._PamiecOperacje);
        }

        this._Wynik = this._ZapamietaneDane.Count - this._ZapamietaneDane.IndexOf(pamiecDane);
    }

    private PamiecProba WczytajDoPamieciProba(int[] pamiec)
    {
        return new PamiecProba(pamiec[0], pamiec[1], pamiec[2], pamiec[3]);
    }

    private PamiecDane WczytajDoPamieciDane(int[] pamiec)
    {
        return new PamiecDane(pamiec[0], pamiec[1], pamiec[2], pamiec[3], pamiec[4], pamiec[5], pamiec[6], pamiec[7], pamiec[8], pamiec[9], pamiec[10], pamiec[11], pamiec[12], pamiec[13], pamiec[14], pamiec[14]);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record PamiecProba(int A, int B, int C, int D);
    private record PamiecDane(int A, int B, int C, int D, int E, int F, int G, int H, int I, int J, int K, int L, int M, int N, int O, int P);
}