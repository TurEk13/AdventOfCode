using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Zadania._2024;

public partial class D20Z01 : IZadanie
{
    private readonly Dictionary<Punkt, char> _MapaPunkty;
    private readonly Punkt _Start;
    private readonly Punkt _Stop;
    private List<Punkt> _Sciezka;
    private List<Skroty> _Skroty;
    private Int32 _Wynik;

    public D20Z01(bool daneTestowe = false)
    {
        this._Sciezka = new List<Punkt>();
        this._Skroty = new List<Skroty>();
        this._MapaPunkty = new Dictionary<Punkt, char>();
        this._Wynik = 0;

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2024\\20\\proba.txt" : ".\\Dane\\2024\\20\\dane.txt", FileMode.Open, FileAccess.Read));
        string[] daneProgramu = sr.ReadToEnd().Split(Environment.NewLine);
        sr.Close();

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
                this._MapaPunkty.TryAdd(new Punkt(x, y), daneProgramu[y][x]);
            }
        }
    }

    public void RozwiazanieZadania()
    {
        this.DFS();

        this.ZnajdzSkroty();

        this.PoliczDlugosc();
    }

    private void PoliczDlugosc()
    {
        int start, stop;

        foreach(Skroty s in this._Skroty)
        {
            start = this._Sciezka.IndexOf(s.PunktStart);
            stop = this._Sciezka.IndexOf(s.PunktStop);

            if(stop - start - 2 >= 100)
            {
                this._Wynik++;
            }
        }
    }

    private void ZnajdzSkroty()
    {
        Punkt tmpSrodek, tmpKoniec;
        foreach(Punkt obecny in this._Sciezka)
        {
            tmpKoniec = obecny with { X = obecny.X - 2 };
            tmpSrodek = obecny with { X = obecny.X - 1 };
            if(this._Sciezka.Contains(tmpKoniec) && !this._Sciezka.Contains(tmpSrodek) && !this._Skroty.Contains(new Skroty(tmpKoniec, obecny)))
            {
                this._Skroty.Add(new Skroty(obecny, tmpKoniec));
            }

            tmpKoniec = obecny with { X = obecny.X + 2 };
            tmpSrodek = obecny with { X = obecny.X + 1 };
            if(this._Sciezka.Contains(tmpKoniec) && !this._Sciezka.Contains(tmpSrodek) && !this._Skroty.Contains(new Skroty(tmpKoniec, obecny)))
            {
                this._Skroty.Add(new Skroty(obecny, tmpKoniec));
            }

            tmpKoniec = obecny with { Y = obecny.Y - 2 };
            tmpSrodek = obecny with { Y = obecny.Y - 1 };
            if(this._Sciezka.Contains(tmpKoniec) && !this._Sciezka.Contains(tmpSrodek) && !this._Skroty.Contains(new Skroty(tmpKoniec, obecny)))
            {
                this._Skroty.Add(new Skroty(obecny, tmpKoniec));
            }

            tmpKoniec = obecny with { Y = obecny.Y + 2 };
            tmpSrodek = obecny with { Y = obecny.Y + 1 };
            if(this._Sciezka.Contains(tmpKoniec) && !this._Sciezka.Contains(tmpSrodek) && !this._Skroty.Contains(new Skroty(tmpKoniec, obecny)))
            {
                this._Skroty.Add(new Skroty(obecny, tmpKoniec));
            }
        }
    }

    private void DFS()
    {
        Stack<Punkt> odwiedzone = new Stack<Punkt>();
        Punkt nowy, obecny = this._Start;
        odwiedzone.Push(obecny);

        while(!obecny.Equals(this._Stop))
        {
            nowy = obecny with { X = obecny.X - 1 };
            if (!odwiedzone.Contains(nowy) && !this._MapaPunkty[nowy].Equals('#'))
            {
                odwiedzone.Push(nowy);
                obecny = nowy;
                continue;
            }

            nowy = obecny with { X = obecny.X + 1 };
            if (!odwiedzone.Contains(nowy) && !this._MapaPunkty[nowy].Equals('#'))
            {
                odwiedzone.Push(nowy);
                obecny = nowy;
                continue;
            }

            nowy = obecny with { Y = obecny.Y - 1 };
            if (!odwiedzone.Contains(nowy) && !this._MapaPunkty[nowy].Equals('#'))
            {
                odwiedzone.Push(nowy);
                obecny = nowy;
                continue;
            }

            nowy = obecny with { Y = obecny.Y + 1 };
            if (!odwiedzone.Contains(nowy) && !this._MapaPunkty[nowy].Equals('#'))
            {
                odwiedzone.Push(nowy);
                obecny = nowy;
                continue;
            }
        }

        while(odwiedzone.TryPop(out Punkt p))
        {
            this._Sciezka.Add(p);
        }
        this._Sciezka.Reverse();

        /*
        nowy = obecny with { X = obecny.X - 1 };
        if (!this._MapaPunkty[nowy].Equals('#') && !odwiedzone.Contains(nowy))
        {
            if(!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                return;
            }
        }

        nowy = obecny with { X = obecny.X + 1 };
        if (!this._MapaPunkty[nowy].Equals('#') && !odwiedzone.Contains(nowy))
        {
            if (!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                return;
            }
        }

        nowy = obecny with { Y = obecny.Y - 1 };
        if (!this._MapaPunkty[nowy].Equals('#') && !odwiedzone.Contains(nowy))
        {
            if (!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                return;
            }
        }

        nowy = obecny with { Y = obecny.Y + 1 };
        if (!this._MapaPunkty[nowy].Equals('#') && !odwiedzone.Contains(nowy))
        {
            if (!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                return;
            }
        }
        */
    }

    private record Punkt(int X, int Y);
    private record Skroty(Punkt PunktStart, Punkt PunktStop);

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}