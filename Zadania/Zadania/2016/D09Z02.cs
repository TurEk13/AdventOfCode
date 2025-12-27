using System;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public class D09Z02 : IZadanie
{
    private Int64 _Wynik;
    private readonly string _Kompresja;
    public D09Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\09\\proba.txt" : ".\\Dane\\2016\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);

        this._Kompresja = sr.ReadToEnd();
        // "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN" - 445

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        // 66 492 940 886 618 064 808 914 347 218 040 634 778 +
        int ileZnakow, ilePowtorzen, nawiasZamykajacy;

        for(int i = 0; i < this._Kompresja.Length; i++)
        {
            if(this._Kompresja[i].Equals('('))
            {
                (ileZnakow, ilePowtorzen, nawiasZamykajacy) = this.ZnajdzZnaczniki(i);
                this._Wynik += this.ObliczDlugosc(ileZnakow, ilePowtorzen, nawiasZamykajacy);
                i = nawiasZamykajacy + ileZnakow;
            }
            else
            {
                this._Wynik++;
            }
        }
    }

    private Int64 ObliczDlugosc(int ileZnakow, int ilePowtorzen, int nawiasZamykajacy)
    {
        int ileZnakow2, ilePowtorzen2, nawiasZamykajacy2, indeks;

        indeks = nawiasZamykajacy + 1;

        if (indeks < this._Kompresja.Length && this._Kompresja[indeks].Equals('('))
        {
            (ileZnakow2, ilePowtorzen2, nawiasZamykajacy2) = this.ZnajdzZnaczniki(indeks);
            return Convert.ToInt64(ilePowtorzen) *  this.ObliczDlugosc(ileZnakow2, ilePowtorzen2, nawiasZamykajacy2);
        }

        indeks += ileZnakow;

        if (indeks < this._Kompresja.Length && this._Kompresja[indeks].Equals('('))
        {
            (ileZnakow2, ilePowtorzen2, nawiasZamykajacy2) = this.ZnajdzZnaczniki(indeks);
            return Convert.ToInt64(ileZnakow * ilePowtorzen) + this.ObliczDlugosc(ileZnakow2, ilePowtorzen2, nawiasZamykajacy2);
        }

        return Convert.ToInt64(ileZnakow * ilePowtorzen);
    }

    private (int ileZnakow, int ilePowtorzen, int nawiasZamykajacy) ZnajdzZnaczniki(int nawiasOtwierajacy)
    {
        int nawiasZamykajacy = nawiasOtwierajacy + 1;
        int ileZnakow, ilePowtorzen;
        string kompresja;
        
        while(!this._Kompresja[nawiasZamykajacy].Equals(')'))
        {
            nawiasZamykajacy++;
        }

        kompresja = this._Kompresja[(nawiasOtwierajacy + 1)..nawiasZamykajacy];

        ileZnakow = Convert.ToInt32(kompresja[.. kompresja.IndexOf('x')]);
        ilePowtorzen = Convert.ToInt32(kompresja[(kompresja.IndexOf('x') + 1) ..]);
        
        return (ileZnakow, ilePowtorzen, nawiasZamykajacy);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}