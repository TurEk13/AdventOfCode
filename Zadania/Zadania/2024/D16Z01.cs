using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D16Z01 : IZadanie
{
    private readonly Dictionary<Punkt, char> _MapaPunkty;
    private readonly Punkt _Start;
    private readonly Punkt _Stop;
    private readonly Mapa _Mapa;
    private int _Szerokosc;
    private int _Wysokosc;
    private Int32 _Wynik;

    public D16Z01(bool daneTestowe = false)
    {
        this._Mapa = new Mapa();
        this._MapaPunkty = new Dictionary<Punkt, char>();
        
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2024\\16\\proba.txt" : ".\\Dane\\2024\\16\\dane.txt", FileMode.Open, FileAccess.Read));
        string[] daneProgramu = sr.ReadToEnd().Split(Environment.NewLine);
        sr.Close();

        this._Wysokosc = daneProgramu.Length;
        this._Szerokosc = daneProgramu[0].Length;

        for(int y = 0; y < daneProgramu.Length; y++)
        {
            if(daneProgramu[y].Contains('S'))
            {
                this._Start = new Punkt(daneProgramu[y].IndexOf('S'), y);
            }

            if(daneProgramu[y].Contains('E'))
            {
                this._Stop = new Punkt(daneProgramu[y].IndexOf('E'), y);
            }

            for(int x = 0; x < daneProgramu[y].Length; x++)
            {
                this._Mapa.Wezly.Add(new Wezel(x, y));
                this._MapaPunkty.TryAdd(new Punkt(x, y), daneProgramu[y][x]);
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

    private void AStar()
    {
        Wezel WezelPoczatkowy = new Wezel(this._Start.X, this._Start.Y)
        {
            MojKierunek = 'E',
            NadrzednyKierunek = '\0',
            Odleglosc = 0
        };

        List<Wezel> doSprawdzenia = new List<Wezel> { WezelPoczatkowy };
        HashSet<Wezel> sprawdzone = new HashSet<Wezel>();

        Dictionary<Punkt, int> znanyKoszt = new Dictionary<Punkt, int> { [new Punkt(this._Start.X, this._Start.Y)] = 0 };
        Dictionary<Punkt, int> manhattanKoszt = new Dictionary<Punkt, int> { [new Punkt(this._Start.X, this._Start.Y)] = this.Manhattan(this._Start, this._Stop) };
        Dictionary<Punkt, Wezel> mapaNadrzedna = new Dictionary<Punkt, Wezel>();

        while(doSprawdzenia.Count > 0)
        {
            Wezel obecny = doSprawdzenia.OrderBy(w => znanyKoszt[w.Wspolrzedne] + manhattanKoszt[w.Wspolrzedne]).First();

            if(obecny.Wspolrzedne == this._Stop)
            {
                this._Wynik = obecny.Odleglosc;
                return;
            }

            doSprawdzenia.Remove(obecny);
            sprawdzone.Add(obecny);

            this.ZnajdzSasiadow(obecny);

            foreach(Punkt obecnyPunkt in obecny.Sasiedzi.Keys)
            {
                Wezel obecnyWezel = _Mapa.ZwrocWezelPoLokalizacji(obecnyPunkt);

                if(obecnyWezel is null || sprawdzone.Contains(obecnyWezel))
                {
                    continue;
                }

                int odleglosc = znanyKoszt[obecny.Wspolrzedne] + obecny.Sasiedzi[obecnyWezel.Wspolrzedne];

                if(!znanyKoszt.TryGetValue(obecnyWezel.Wspolrzedne, out int v) || odleglosc < v)
                {
                    znanyKoszt[obecnyWezel.Wspolrzedne] = odleglosc;
                    manhattanKoszt[obecnyWezel.Wspolrzedne] = Manhattan(obecnyWezel.Wspolrzedne, this._Stop);

                    mapaNadrzedna[obecnyWezel.Wspolrzedne] = obecny;

                    if(!doSprawdzenia.Contains(obecnyWezel))
                    {
                        doSprawdzenia.Add(obecnyWezel);
                    }
                }
            }
        }
    }
    
    private void ZnajdzSasiadow(Wezel wezel)
    {
        int odleglosc = this.ObliczWartoscPrzejscia(wezel.MojKierunek, 'W');
        Wezel nowyWezel = this._Mapa.ZwrocWezelPoLokalizacji(new Punkt(wezel.Wspolrzedne.X - 1, wezel.Wspolrzedne.Y));
        if(nowyWezel.Wspolrzedne.X >= 0 && !this._MapaPunkty[nowyWezel.Wspolrzedne].Equals('#'))
        {
            wezel.Sasiedzi.Add(nowyWezel.Wspolrzedne, odleglosc);
            nowyWezel.Odleglosc = wezel.Odleglosc + odleglosc;
            nowyWezel.MojKierunek = 'W';
            nowyWezel.NadrzednyKierunek = wezel.MojKierunek;
        }

        odleglosc = this.ObliczWartoscPrzejscia(wezel.MojKierunek, 'E');
        nowyWezel = this._Mapa.ZwrocWezelPoLokalizacji(new Punkt(wezel.Wspolrzedne.X + 1, wezel.Wspolrzedne.Y));
        if(nowyWezel.Wspolrzedne.X < this._Szerokosc && !this._MapaPunkty[nowyWezel.Wspolrzedne].Equals('#'))
        {
            wezel.Sasiedzi.Add(nowyWezel.Wspolrzedne, odleglosc);
            nowyWezel.Odleglosc = wezel.Odleglosc + odleglosc;
            nowyWezel.MojKierunek = 'E';
            nowyWezel.NadrzednyKierunek = wezel.MojKierunek;
        }

        odleglosc = this.ObliczWartoscPrzejscia(wezel.MojKierunek, 'N');
        nowyWezel = this._Mapa.ZwrocWezelPoLokalizacji(new Punkt(wezel.Wspolrzedne.X, wezel.Wspolrzedne.Y - 1));
        if(nowyWezel.Wspolrzedne.Y >= 0 && !this._MapaPunkty[nowyWezel.Wspolrzedne].Equals('#'))
        {
            wezel.Sasiedzi.Add(nowyWezel.Wspolrzedne, odleglosc);
            nowyWezel.Odleglosc = wezel.Odleglosc + odleglosc;
            nowyWezel.MojKierunek = 'N';
            nowyWezel.NadrzednyKierunek = wezel.MojKierunek;
        }

        odleglosc = this.ObliczWartoscPrzejscia(wezel.MojKierunek, 'S');
        nowyWezel = this._Mapa.ZwrocWezelPoLokalizacji(new Punkt(wezel.Wspolrzedne.X, wezel.Wspolrzedne.Y + 1));
        if(nowyWezel.Wspolrzedne.Y < this._Wysokosc && !this._MapaPunkty[nowyWezel.Wspolrzedne].Equals('#'))
        {
            wezel.Sasiedzi.Add(nowyWezel.Wspolrzedne, odleglosc);
            nowyWezel.Odleglosc = wezel.Odleglosc + odleglosc;
            nowyWezel.MojKierunek = 'S';
            nowyWezel.NadrzednyKierunek = wezel.MojKierunek;
        }
    }

    private int ObliczWartoscPrzejscia(char staryKierunek, char nowyKierunek)
    {
        if((staryKierunek.Equals('E') || staryKierunek.Equals('W')) && (nowyKierunek.Equals('N') || nowyKierunek.Equals('S')))
        {
            return 1001;
        }

        if((staryKierunek.Equals('N') || staryKierunek.Equals('S')) && (nowyKierunek.Equals('W') || nowyKierunek.Equals('E')))
        {
            return 1001;
        }

        return 1;
    }

    private Int32 Manhattan(Punkt obecnyPunkt, Punkt cel)
    {
        return Math.Abs(obecnyPunkt.X - cel.X) + Math.Abs(obecnyPunkt.Y - cel.Y);
    }

    private record Punkt(int X, int Y);
    
    private class Wezel
    {
        public Punkt Wspolrzedne;
        public Dictionary<Punkt, int> Sasiedzi;
        public char MojKierunek;
        public char NadrzednyKierunek;
        public int Odleglosc;

        public Wezel(int x, int y)
        {
            this.Wspolrzedne = new Punkt(x, y);
            this.Sasiedzi = new Dictionary<Punkt, int>();
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