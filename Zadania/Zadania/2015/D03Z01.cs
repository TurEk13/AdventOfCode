using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D03Z01 : IZadanie
{
    private string Sciezka;
    private List<Punkt> OdwiedzoneLokalizacje;

    public D03Z01()
    {
        FileStream fs = new(".\\Dane\\2015\\03\\dane.txt", FileMode.Open, FileAccess.Read);
        this.OdwiedzoneLokalizacje = new();
        StreamReader sr = new(fs);

        this.Sciezka = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        int miejsce = 0;
        Punkt ObecneMiejsce = new Punkt(0, 0);
        this.OdwiedzoneLokalizacje.Add(ObecneMiejsce);

        while (miejsce < this.Sciezka.Length)
        {
            ObecneMiejsce = ObecneMiejsce.Przesun(this.Sciezka[miejsce]);

            if (this.OdwiedzoneLokalizacje.FindAll(ol => ol.X == ObecneMiejsce.X && ol.Y == ObecneMiejsce.Y).Count == 0)
            {
                this.OdwiedzoneLokalizacje.Add(ObecneMiejsce);
            }
            miejsce++;
        }
    }

    public string PokazRozwiazanie()
    {
        return this.OdwiedzoneLokalizacje.Count.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    class Punkt
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Punkt(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Punkt Przesun(char x)
        {
            return x switch
            {
                '>' => new Punkt(this.X + 1, this.Y),
                '<' => new Punkt(this.X - 1, this.Y),
                '^' => new Punkt(this.X, this.Y + 1),
                'v' => new Punkt(this.X, this.Y - 1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}