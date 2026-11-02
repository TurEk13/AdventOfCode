using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D07Z02 : IZadanie
{
    private Dictionary<Wierzcholek, List<Wierzcholek>> _Wierzcholek;
    private Dictionary<string, int> _SpisWierzcholkow;
    private string _Szczyt;
    private int _Wynik;

    [GeneratedRegex(@"(((?'nazwa'\w{1,})(?: \()(?'waga'\d{1,})(?:(\) -> )))((?'dysk'\w{1,})(?:(, )?)){1,})|((?'nazwa'\w{1,})(?: \()(?'waga'\d{1,})(?:((\)\n))|(\))))")]
    private static partial Regex WzorLinii();
    
    public D07Z02(bool daneTestowe = false)
    {
        this._Wynik = -1;
        this._Wierzcholek = [];
        this._SpisWierzcholkow = [];
        Regex linia = WzorLinii();
        List<Wierzcholek> sameWierzcholki = [];

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\07\\proba.txt" : ".\\Dane\\2017\\07\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = linia.Matches(sr.ReadToEnd());

        sr.Close();

        foreach(Match m in mc)
        {
            sameWierzcholki.Add(new Wierzcholek(m.Groups["nazwa"].Value, Convert.ToInt32(m.Groups["waga"].Value)));
            this._SpisWierzcholkow.Add(m.Groups["nazwa"].Value, Convert.ToInt32(m.Groups["waga"].Value));
        }

        foreach(Match m in mc)
        {
            if(m.Groups["dysk"].Success)
            {
                this._Wierzcholek.TryAdd(sameWierzcholki.First(sw => sw.Nazwa.Equals(m.Groups["nazwa"].Value)), [.. m.Groups["dysk"].Captures.Select(s => sameWierzcholki.First(sw => sw.Nazwa.Equals(s.Value)))]);
            }
        }

        this._Szczyt = this.ZnajdzSzczyt();
    }

    private string ZnajdzSzczyt()
    {
        List<Krawedz> doUzycia = [];
        Krawedz tmp, tmp2;
        bool znaleziono = true;

        foreach(Wierzcholek wn in this._Wierzcholek.Keys)
        {
            if(this._Wierzcholek[wn] is null)
            {
                continue;
            }

            foreach(Wierzcholek wp in this._Wierzcholek[wn])
            {
                doUzycia.Add(new Krawedz(wn.Nazwa, wp.Nazwa));
            }
        }

        tmp = doUzycia[0];

        while(znaleziono)
        {
            if((tmp2 = doUzycia.FirstOrDefault(du => du.Koniec.Equals(tmp.Poczatek))) is null)
            {
                return tmp.Poczatek;
            }
            
            foreach (Krawedz k in doUzycia.Where(du => du.Poczatek.Equals(tmp.Koniec)).ToArray())
            {
                doUzycia.Remove(k);
            }

            tmp = tmp2;
        }

        return null;
    }

    public void RozwiazanieZadania()
    {
        _ = ZnajdzRoznice(this._Szczyt);
    }

    private int ZnajdzRoznice(string nazwaWierzcholka)
    {
        List<int> podSumy = [];
        int indeks = 0, roznica = 0;

        if(this._Wierzcholek.TryGetValue(new Wierzcholek(nazwaWierzcholka, this._SpisWierzcholkow[nazwaWierzcholka]), out List<Wierzcholek> wierzcholek))
        {
            foreach(Wierzcholek w in wierzcholek)
            {
                podSumy.Add(this.ZnajdzRoznice(w.Nazwa));
            }

            if(this._Wynik == -1 & podSumy.Min() != podSumy.Max())
            {
                // Dodać różnicę, po jest za mało, różnica jest dodatnia
                if(podSumy.Count(ps => ps == podSumy.Min()) == 1)
                {
                    indeks = podSumy.IndexOf(podSumy.Min());
                    roznica = podSumy.Max() - podSumy.Min();
                }

                // Odjąć różnicę, po jest za dużo, różnica jest ujemno
                if(podSumy.Count(ps => ps == podSumy.Max()) == 1)
                {
                    indeks = podSumy.IndexOf(podSumy.Max());
                    roznica = podSumy.Min() - podSumy.Max();
                }

                this._Wynik = this._SpisWierzcholkow[wierzcholek[indeks].Nazwa] + roznica;
            }
        }

        return this._SpisWierzcholkow[nazwaWierzcholka] + podSumy.Sum();
    }

    public record Krawedz(string Poczatek, string Koniec);
    public record Wierzcholek(string Nazwa, int Waga);

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}