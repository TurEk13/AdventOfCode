using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D07Z01 : IZadanie
{
    private Dictionary<string, string[]?> _Wierzcholek;
    private Krawedz _Wynik;

    [GeneratedRegex(@"(((?'nazwa'\w{1,})(?: \()(?'waga'\d{1,})(?:(\) -> )))((?'dysk'\w{1,})(?:(, )?)){1,})|((?'nazwa'\w{1,})(?: \()(?'waga'\d{1,})(?:((\)\n))|(\))))")]
    private static partial Regex WzorLinii();
    
    public D07Z01(bool daneTestowe = false)
    {
        this._Wierzcholek = [];
        Regex linia = WzorLinii();

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\07\\proba.txt" : ".\\Dane\\2017\\07\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = linia.Matches(sr.ReadToEnd());

        sr.Close();

        foreach(Match m in mc)
        {
            if(m.Groups["dysk"].Success)
            {
                this._Wierzcholek.TryAdd(m.Groups["nazwa"].Value, [.. m.Groups["dysk"].Captures.Select(s => s.Value)]);
            }
        }
    }

    public void RozwiazanieZadania()
    {
        List<Krawedz> doUzycia = [];
        Krawedz tmp;
        bool znaleziono = true;

        foreach(string p in this._Wierzcholek.Keys)
        {
            if(this._Wierzcholek[p] is null)
            {
                continue;
            }

            foreach(string w in this._Wierzcholek[p])
            {
                doUzycia.Add(new Krawedz(p, w));
            }
        }

        this._Wynik = doUzycia[0];

        while(znaleziono)
        {
            if((tmp = doUzycia.FirstOrDefault(du => du.Koniec.Equals(this._Wynik.Poczatek))) is null)
            {
                znaleziono = false;
                return;
            }
            
            foreach (Krawedz k in doUzycia.Where(du => du.Poczatek.Equals(tmp.Koniec)).ToArray())
            {
                doUzycia.Remove(k);
            }

            this._Wynik = tmp;
        }
    }

    public record Krawedz(string Poczatek, string Koniec);

    public string PokazRozwiazanie()
    {
        return this._Wynik.Poczatek;
    }
}