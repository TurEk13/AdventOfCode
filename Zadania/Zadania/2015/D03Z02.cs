using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D03Z02 : IZadanie
{
    private string Sciezka;
    private List<Punkt> OdwiedzoneLokalizacje;

    public D03Z02()
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
        Punkt DomMikolaja = new Punkt(0, 0);
        Punkt DomRoboMikolaja = new Punkt(0, 0);

        this.OdwiedzoneLokalizacje.Add(DomMikolaja);

        while (miejsce < this.Sciezka.Length)
        {
            DomMikolaja = DomMikolaja.Przesun(this.Sciezka[miejsce]);

            if (this.OdwiedzoneLokalizacje.FindAll(ol => ol.X == DomMikolaja.X && ol.Y == DomMikolaja.Y).Count == 0)
            {
                this.OdwiedzoneLokalizacje.Add(DomMikolaja);
            }
            miejsce++;

            DomRoboMikolaja = DomRoboMikolaja.Przesun(this.Sciezka[miejsce]);

            if (this.OdwiedzoneLokalizacje.FindAll(ol => ol.X == DomRoboMikolaja.X && ol.Y == DomRoboMikolaja.Y).Count == 0)
            {
                this.OdwiedzoneLokalizacje.Add(DomRoboMikolaja);
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