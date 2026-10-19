using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public partial class D18Z01 : IZadanie
{
    [GeneratedRegex(@"(?<x>\d{1,2}),(?<y>\d{1,2})")]
    private static partial Regex Wspolrzedne();
    private Int32 _Wynik;
    private readonly List<Punkt> BledneLikalizacje;
    private readonly int Szerokosc;
    private readonly int Wysokosc;
    private readonly int Bytes;
    private readonly Wezel Start;
    private readonly Wezel Stop;
    private readonly Mapa mapa;

    public D18Z01(bool daneTestowe = false)
    {
        this.Szerokosc = 71;
        this.Wysokosc = 71;
        this.Bytes = 1024;

        this._Wynik = Int32.MaxValue;
        this.Start = new Wezel(0, 0);
        this.Stop = new Wezel(this.Wysokosc - 1, this.Szerokosc - 1);
        this.BledneLikalizacje = new List<Punkt>();
        this.mapa = new Mapa();
        
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2024\\18\\proba.txt" : ".\\Dane\\2024\\18\\dane.txt", FileMode.Open, FileAccess.Read));
        string daneProgramu = sr.ReadToEnd();
        sr.Close();

        Regex BledneMiejsca = Wspolrzedne();

        MatchCollection BledneMiejscaWspolrzedne = BledneMiejsca.Matches(daneProgramu);

        foreach(Match m in BledneMiejscaWspolrzedne)
        {
            BledneLikalizacje.Add(new Punkt(Convert.ToInt32(m.Groups["x"].Value), Convert.ToInt32(m.Groups["y"].Value)));
        }

        for(int i = 0; i < this.Wysokosc; i++)
        {
            for(int j = 0; j < this.Szerokosc; j++)
            {
                this.mapa.Wezly.Add(new Wezel(i, j));
            }
        }
    }

    public void RozwiazanieZadania()
    {
        this.AStar();
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private List<Wezel> AStar()
    {
        List<Wezel> doSprawdzenia = new List<Wezel> { this.Start };
        HashSet<Wezel> sprawdzone = new HashSet<Wezel>();

        Dictionary<Punkt, int> znanyKoszt = new Dictionary<Punkt, int> { [this.Start.Wspolrzedne] = 0 };
        Dictionary<Punkt, int> manhattanKoszt = new Dictionary<Punkt, int> { [this.Start.Wspolrzedne] = this.Manhattan(this.Start, this.Stop) };
        Dictionary<Punkt, Wezel> mapaNadrzedna = new Dictionary<Punkt, Wezel>();

        while(doSprawdzenia.Count > 0)
        {
            Wezel obecny = doSprawdzenia.OrderBy(w => znanyKoszt[w.Wspolrzedne] + manhattanKoszt[w.Wspolrzedne]).First();

            if(obecny.Wspolrzedne == this.Stop.Wspolrzedne)
            {
                this._Wynik = Math.Min(this._Wynik, ObliczDlugosc(mapaNadrzedna, obecny).Count - 1);
            }

            doSprawdzenia.Remove(obecny);
            sprawdzone.Add(obecny);

            this.ZnajdzSasiadow(obecny);

            foreach(Punkt p in obecny.Sasiedzi.Keys)
            {
                Wezel w = mapa.ZwrocWezelPoLokalizacji(p);

                if(w is null || sprawdzone.Contains(w))
                {
                    continue;
                }

                int odleglosc = znanyKoszt[obecny.Wspolrzedne] + obecny.Sasiedzi[w.Wspolrzedne];

                if(!znanyKoszt.TryGetValue(w.Wspolrzedne, out int value) || odleglosc < value)
                {
                    value = odleglosc;
                    znanyKoszt[w.Wspolrzedne] = value;
                    manhattanKoszt[w.Wspolrzedne] = Manhattan(w, this.Stop);

                    mapaNadrzedna[w.Wspolrzedne] = obecny;

                    if(!doSprawdzenia.Contains(w))
                    {
                        doSprawdzenia.Add(w);
                    }
                }
            }
        }

        return null;
    }

    private List<Wezel> ObliczDlugosc(Dictionary<Punkt,Wezel> mapa, Wezel wezel)
    {
        List<Wezel> sciezka = new List<Wezel> { wezel };

        while(mapa.ContainsKey(wezel.Wspolrzedne))
        {
            wezel = mapa[wezel.Wspolrzedne];
            sciezka.Add(wezel);
        }

        sciezka.Reverse();
        return sciezka;
    }

    private void ZnajdzSasiadow(Wezel wezel)
    {
        Punkt nowyPunkt = wezel.Wspolrzedne with { X = wezel.Wspolrzedne.X - 1 };
        if(nowyPunkt.X >= 0 && !this.BledneLikalizacje[0 .. this.Bytes].Contains(nowyPunkt))
        {
            wezel.Sasiedzi.Add(nowyPunkt, wezel.Odleglosc + 1);
        }

        nowyPunkt = wezel.Wspolrzedne with { X = wezel.Wspolrzedne.X + 1 };
        if(nowyPunkt.X < this.Szerokosc && !this.BledneLikalizacje[0 .. this.Bytes].Contains(nowyPunkt))
        {
            wezel.Sasiedzi.Add(nowyPunkt, wezel.Odleglosc + 1);
        }

        nowyPunkt = wezel.Wspolrzedne with { Y = wezel.Wspolrzedne.Y - 1 };
        if(nowyPunkt.Y >= 0 && !this.BledneLikalizacje[0 .. this.Bytes].Contains(nowyPunkt))
        {
            wezel.Sasiedzi.Add(nowyPunkt, wezel.Odleglosc + 1);
        }

        nowyPunkt = wezel.Wspolrzedne with { Y = wezel.Wspolrzedne.Y + 1 };
        if(nowyPunkt.Y < this.Wysokosc && !this.BledneLikalizacje[0 .. this.Bytes].Contains(nowyPunkt))
        {
            wezel.Sasiedzi.Add(nowyPunkt, wezel.Odleglosc + 1);
        }
    }

    private Int32 Manhattan(Wezel obecnyPunkt, Wezel cel)
    {
        return Math.Abs(obecnyPunkt.Wspolrzedne.X - cel.Wspolrzedne.X) + Math.Abs(obecnyPunkt.Wspolrzedne.Y - cel.Wspolrzedne.Y);
    }

    private record Punkt(int X, int Y);
    
    private class Wezel
    {
        public Punkt Wspolrzedne;
        public Dictionary<Punkt, int> Sasiedzi;
        public Int32 Odleglosc;

        public Wezel(int x, int y)
        {
            this.Wspolrzedne = new Punkt(x, y);
            this.Sasiedzi = new Dictionary<Punkt, int>();
            this.Odleglosc = 0;
        }

        public Wezel(Punkt wspolrzedne)
        {
            this.Wspolrzedne = wspolrzedne;
            this.Sasiedzi = new Dictionary<Punkt, int>();
            this.Odleglosc = 0;
        }
    }

    private class Mapa
    {
        public HashSet<Wezel> Wezly;

        public Mapa()
        {
            this.Wezly = new HashSet<Wezel>();
        }

        public Wezel ZwrocWezelPoLokalizacji(Punkt p)
        {
            return Wezly.FirstOrDefault(w => w.Wspolrzedne.Equals(p));
        }
    }
}