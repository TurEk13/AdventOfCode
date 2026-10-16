using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D10Z02 : IZadanie
{
    private List<char[]> _mapa;
    private Stack<Punkt> _poczatki;
    private List<int> _wynik;

    private record Punkt(int X, int Y);

    public D10Z02(bool daneTestowe = false)
    {
        this._mapa = new List<char[]>();
        this._poczatki = new();
        this._wynik = new List<int>();

        FileStream fs = new FileStream(daneTestowe ? ".\\Dane\\2024\\10\\proba.txt" : ".\\Dane\\2024\\10\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        string s;
        int x = 0, y;

        while ((s = sr.ReadLine()) is not null)
        {
            this._mapa.Add(s.ToCharArray());

            if (this._mapa[^1].Contains('0'))
            {
                y = 0;
                while (y < this._mapa[x].Length)
                {
                    if (this._mapa[x][y].Equals('0'))
                    {
                        this._poczatki.Push(new Punkt(x, y));
                    }
                    y++;
                }
            }
            x++;
        }
    }

    public void RozwiazanieZadania()
    {
        Stack<Punkt> odwiedzone;

        foreach(Punkt p in this._poczatki)
        {
            odwiedzone = new Stack<Punkt>();
            this._wynik.Add(0);
            odwiedzone.Push(p);

            this.DFS(p, odwiedzone);
        }
    }

    private void DFS(Punkt obecny, Stack<Punkt> odwiedzone)
    {
        Punkt nowy;

        if (this._mapa[obecny.X][obecny.Y] - '0' == 9)
        {
            this._wynik[^1]++;
            return;
        }

        nowy = obecny with { X = obecny.X - 1 };
        if (nowy.X >= 0 && this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
        {
            if(!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                odwiedzone.Pop();
            }
        }

        nowy = obecny with { X = obecny.X + 1 };
        if (nowy.X < this._mapa.Count && this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
        {
            if (!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                odwiedzone.Pop();
            }
        }

        nowy = obecny with { Y = obecny.Y - 1 };
        if (nowy.Y >= 0 && this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
        {
            if (!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                odwiedzone.Pop();
            }
        }

        nowy = obecny with { Y = obecny.Y + 1 };
        if (nowy.Y < this._mapa[0].Length && this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
        {
            if (!odwiedzone.Contains(nowy))
            {
                odwiedzone.Push(nowy);
                this.DFS(nowy, odwiedzone);
                odwiedzone.Pop();
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._wynik.Sum().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}