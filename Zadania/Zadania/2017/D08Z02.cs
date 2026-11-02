using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D08Z02 : IZadanie
{
    private List<Operacja> _SpisOperacji;
    private int _Wynik;

    [GeneratedRegex(@"(?'rejestrDoZmiany'\w{1,})(?: )(?'zmiana'(inc|dec))(?: )(?'wartosc'(\d{1,}|-\d{1,}))(?: if )(?'rejestrDoWarunku'\w{1,})(?: )(?'warunek'(<|<=|>|>=|==|!=))(?: )(?'wartoscDoWarunku'(\d{1,}|-\d{1,}))(?:\n?)")]
    private static partial Regex WzorLinii();
    
    public D08Z02(bool daneTestowe = false)
    {
        this._SpisOperacji = [];
        this._Wynik = 0;
        Regex linia = WzorLinii();

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\08\\proba.txt" : ".\\Dane\\2017\\08\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = linia.Matches(sr.ReadToEnd());

        sr.Close();

        foreach(Match m in mc)
        {
            this._SpisOperacji.Add(new Operacja(m.Groups["rejestrDoZmiany"].Value, m.Groups["zmiana"].Value, Convert.ToInt32(m.Groups["wartosc"].Value), m.Groups["rejestrDoWarunku"].Value, m.Groups["warunek"].Value, Convert.ToInt32(m.Groups["wartoscDoWarunku"].Value)));
        }
    }

    public void RozwiazanieZadania()
    {
        Dictionary<string, int> rejestry = [];
        string rejestrDoZmiany, rejestrDoWarunku;

        foreach(Operacja o in this._SpisOperacji)
        {
            rejestrDoZmiany = o.RejestrDoZmiany;
            rejestrDoWarunku = o.RejestrDoWarunku;

            rejestry.TryAdd(rejestrDoZmiany, 0);
            rejestry.TryAdd(rejestrDoWarunku, 0);

            switch(o.Warunek)
            {
                case "<":
                    if (rejestry[rejestrDoWarunku] < o.WartoscDoWarunku)
                    {
                        rejestry[rejestrDoZmiany] = o.Zmiana == "inc" ? rejestry[rejestrDoZmiany] + o.Wartosc : rejestry[rejestrDoZmiany] - o.Wartosc;
                    }
                    break;
                case "<=":
                    if (rejestry[rejestrDoWarunku] <= o.WartoscDoWarunku)
                    {
                        rejestry[rejestrDoZmiany] = o.Zmiana == "inc" ? rejestry[rejestrDoZmiany] + o.Wartosc : rejestry[rejestrDoZmiany] - o.Wartosc;
                    }
                    break;
                case ">":
                    if (rejestry[rejestrDoWarunku] > o.WartoscDoWarunku)
                    {
                        rejestry[rejestrDoZmiany] = o.Zmiana == "inc" ? rejestry[rejestrDoZmiany] + o.Wartosc : rejestry[rejestrDoZmiany] - o.Wartosc;
                    }
                    break;
                case ">=":
                    if (rejestry[rejestrDoWarunku] >= o.WartoscDoWarunku)
                    {
                        rejestry[rejestrDoZmiany] = o.Zmiana == "inc" ? rejestry[rejestrDoZmiany] + o.Wartosc : rejestry[rejestrDoZmiany] - o.Wartosc;
                    }
                    break;
                case "==":
                    if (rejestry[rejestrDoWarunku] == o.WartoscDoWarunku)
                    {
                        rejestry[rejestrDoZmiany] = o.Zmiana == "inc" ? rejestry[rejestrDoZmiany] + o.Wartosc : rejestry[rejestrDoZmiany] - o.Wartosc;
                    }
                    break;
                case "!=":
                    if (rejestry[rejestrDoWarunku] != o.WartoscDoWarunku)
                    {
                        rejestry[rejestrDoZmiany] = o.Zmiana == "inc" ? rejestry[rejestrDoZmiany] + o.Wartosc : rejestry[rejestrDoZmiany] - o.Wartosc;
                    }
                    break;
            }

            this._Wynik = Math.Max(this._Wynik, rejestry.Max(r => r.Value));
        }
    }

    public record Operacja(string RejestrDoZmiany, string Zmiana, int Wartosc, string RejestrDoWarunku, string Warunek, int WartoscDoWarunku);

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}