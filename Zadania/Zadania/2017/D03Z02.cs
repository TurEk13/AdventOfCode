using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2017;

public partial class D03Z02 : IZadanie
{
    private Dictionary<Punkt, UInt32> _Wartosci;
    private readonly UInt32 _Wartosc;
    private UInt32 _Wynik;
    private bool _Dalej;
    
    public D03Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Wartosci = [];
        this._Dalej = true;

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\03\\proba.txt" : ".\\Dane\\2017\\03\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Wartosc = Convert.ToUInt32(sr.ReadToEnd());

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        Punkt staryPunkt = new Punkt(0, 0), obecnyPunkt;
        this._Wartosci.TryAdd(staryPunkt, 1);

        int licznik = 1;
        while(this._Dalej)
        {
            for(int i = 0; i < licznik; i++)
            {
                obecnyPunkt = staryPunkt with { X = staryPunkt.X + 1 };
                this._Wartosci.TryAdd(obecnyPunkt, this.PobierzWartosc(obecnyPunkt));
                staryPunkt = obecnyPunkt;

                if (this._Wartosci[staryPunkt] > this._Wartosc)
                {
                    this._Wynik = this._Wartosci[staryPunkt];
                    this._Dalej = false;
                    return;
                }
            }

            for (int i = 0; i < licznik; i++)
            {
                obecnyPunkt = staryPunkt with { Y = staryPunkt.Y + 1 };
                this._Wartosci.TryAdd(obecnyPunkt, this.PobierzWartosc(obecnyPunkt));
                staryPunkt = obecnyPunkt;

                if (this._Wartosci[staryPunkt] > this._Wartosc)
                {
                    this._Wynik = this._Wartosci[staryPunkt];
                    this._Dalej = false;
                    return;
                }
            }

            licznik++;

            for (int i = 0; i < licznik; i++)
            {
                obecnyPunkt = staryPunkt with { X = staryPunkt.X - 1 };
                this._Wartosci.TryAdd(obecnyPunkt, this.PobierzWartosc(obecnyPunkt));
                staryPunkt = obecnyPunkt;

                if (this._Wartosci[staryPunkt] > this._Wartosc)
                {
                    this._Wynik = this._Wartosci[staryPunkt];
                    this._Dalej = false;
                    return;
                }
            }

            for (int i = 0; i < licznik; i++)
            {
                obecnyPunkt = staryPunkt with { Y = staryPunkt.Y - 1 };
                this._Wartosci.TryAdd(obecnyPunkt, this.PobierzWartosc(obecnyPunkt));
                staryPunkt = obecnyPunkt;

                if (this._Wartosci[staryPunkt] > this._Wartosc)
                {
                    this._Wynik = this._Wartosci[staryPunkt];
                    this._Dalej = false;
                    return;
                }
            }

            licznik++;
        }
    }

    private UInt32 PobierzWartosc(Punkt punkt)
    {
        this._Wartosci.TryGetValue(new Punkt(punkt.X - 1, punkt.Y), out UInt32 i1);
        this._Wartosci.TryGetValue(new Punkt(punkt.X + 1, punkt.Y), out UInt32 i2);
        this._Wartosci.TryGetValue(new Punkt(punkt.X, punkt.Y - 1), out UInt32 i3);
        this._Wartosci.TryGetValue(new Punkt(punkt.X, punkt.Y + 1), out UInt32 i4);

        this._Wartosci.TryGetValue(new Punkt(punkt.X - 1, punkt.Y - 1), out UInt32 i5);
        this._Wartosci.TryGetValue(new Punkt(punkt.X - 1, punkt.Y + 1), out UInt32 i6);
        this._Wartosci.TryGetValue(new Punkt(punkt.X + 1, punkt.Y - 1), out UInt32 i7);
        this._Wartosci.TryGetValue(new Punkt(punkt.X + 1, punkt.Y + 1), out UInt32 i8);

        return i1 + i2 + i3 + i4 + i5 + i6 + i7 + i8;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Punkt(int X, int Y);
}