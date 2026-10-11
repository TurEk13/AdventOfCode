using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D10Z01 : IZadanie
{
    private List<char[]> _mapa;
    private Stack<Punkt> _poczatki;
    private List<int> _wynik;

    private record Punkt(int X, int Y);

    public D10Z01(bool daneTestowe = false)
    {
        this._mapa = new List<char[]>();
        this._poczatki = new();
        this._wynik = new List<int>();

        FileStream fs = new FileStream(".\\Dane\\2024\\10\\dane.txt", FileMode.Open, FileAccess.Read);
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
        Punkt nowy;
        Queue<Punkt> sciezka = new Queue<Punkt>();
        List<Punkt> odwiedzone;

        while (this._poczatki.TryPop(out Punkt obecny))
        {
            odwiedzone = new List<Punkt>();
            sciezka.Enqueue(obecny);
            this._wynik.Add(0);

            while (sciezka.Count > 0)
            {
                obecny = sciezka.Dequeue();
                if (this._mapa[obecny.X][obecny.Y] - '0' == 9)
                {
                    this._wynik[^1]++;
                    continue;
                }

                nowy = obecny with { X = obecny.X - 1 };
                if (obecny.X - 1 >= 0 && !odwiedzone.Contains(nowy))
                {
                    if (this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
                    {
                        odwiedzone.Add(nowy);
                        sciezka.Enqueue(nowy);
                    }
                }

                nowy = obecny with { X = obecny.X + 1 };
                if (obecny.X + 1 < this._mapa.Count && !odwiedzone.Contains(nowy))
                {
                    if (this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
                    {
                        odwiedzone.Add(nowy);
                        sciezka.Enqueue(nowy);
                    }
                }

                nowy = obecny with { Y = obecny.Y - 1 };
                if (obecny.Y - 1 >= 0 && !odwiedzone.Contains(nowy))
                {
                    if (this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
                    {
                        odwiedzone.Add(nowy);
                        sciezka.Enqueue(nowy);
                    }
                }

                nowy = obecny with { Y = obecny.Y + 1 };
                if (obecny.Y + 1 < this._mapa[0].Length && !odwiedzone.Contains(nowy))
                {
                    if (this._mapa[obecny.X][obecny.Y] - '0' + 1 == this._mapa[nowy.X][nowy.Y] - '0')
                    {
                        odwiedzone.Add(nowy);
                        sciezka.Enqueue(nowy);
                    }
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._wynik.Sum().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}