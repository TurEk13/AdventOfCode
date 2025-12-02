using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D01Z02 : IZadanie
{
    private Ruch[] _Instrukcje;
    private int _Odleglosc;
    private Zwrot _ObecnyPunkt;
    private int _ObecnyKierunek;
    private Kierunek _Kierunek;
    private List<Zwrot> _OdwiedzonePunkty;
    public D01Z02(bool daneTestowe = false)
    {
        this._ObecnyPunkt = new (0, 0);
        this._ObecnyKierunek = 0;
        this._Kierunek = new Kierunek();
        this._OdwiedzonePunkty = [this._ObecnyPunkt];
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\01\\proba.txt" : ".\\Dane\\2016\\01\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        this._Instrukcje = [.. sr.ReadToEnd().Split(',').Select(t => t.Trim()).Select(j => new Ruch(j[0], Convert.ToInt32(j[1..])))];

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        foreach(Ruch r in this._Instrukcje)
        {
            switch(r.Kierunek)
            {
                case 'L':
                    this._ObecnyKierunek--;
                    if(this._ObecnyKierunek < 0)
                    {
                        this._ObecnyKierunek += 4;
                    }
                    break;
                case 'R':
                    this._ObecnyKierunek++;
                    if(this._ObecnyKierunek > 3)
                    {
                        this._ObecnyKierunek -= 4;
                    }
                    break;
            }

            if(this._ObecnyKierunek == 0 || this._ObecnyKierunek == 2)
            {
                this._ObecnyPunkt.Y += r.Odleglosc * this._Kierunek.Zwrot[this._ObecnyKierunek].Y;
            }

            if (this._ObecnyKierunek == 1 || this._ObecnyKierunek == 3)
            {
                this._ObecnyPunkt.X += r.Odleglosc * this._Kierunek.Zwrot[this._ObecnyKierunek].X;
            }

            if(this._OdwiedzonePunkty.FirstOrDefault(op => op.X == this._ObecnyPunkt.X && op.Y == this._ObecnyPunkt.Y) is null)
            {
                this._OdwiedzonePunkty.Add(this._ObecnyPunkt);
            }

            if (this._OdwiedzonePunkty.FirstOrDefault(op => op.X == this._ObecnyPunkt.X && op.Y == this._ObecnyPunkt.Y) is not null)
            {
                this._Odleglosc = Math.Abs(this._ObecnyPunkt.X) + Math.Abs(this._ObecnyPunkt.Y);
                return;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Odleglosc.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Ruch(char Kierunek, int Odleglosc);
    
    record Zwrot
    {
        public int X;
        public int Y;

        public Zwrot(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class Kierunek
    {
        public readonly Zwrot[] Zwrot = [new(0, 1), new(1, 0), new(0, -1), new(-1, 0)];
    }
}