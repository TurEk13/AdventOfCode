using System;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public class D09Z01 : IZadanie
{
    private Int64 _Wynik;
    private string _Kompresja;
    public D09Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\09\\proba.txt" : ".\\Dane\\2016\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);

        this._Kompresja = sr.ReadToEnd();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        int ileZnakow, ilePowtorzen, koniec;

        for(int i = 0; i < this._Kompresja.Length; i++)
        {
            if(this._Kompresja[i].Equals('('))
            {
                (ileZnakow, ilePowtorzen, koniec) = this.Kompresja(i);
                this._Wynik += this.ObliczDlugosc(ileZnakow, ilePowtorzen);
                i = koniec + ileZnakow;
            }
            else
            {
                this._Wynik++;
            }
        }
    }

    private int ObliczDlugosc(int ileZnakow, int ilePowtorzen)
    {
        return ileZnakow * ilePowtorzen;
    }

    private (int ileZnakow, int ilePowtorzen, int dalszaPozycja) Kompresja(int pozycja)
    {
        int poczatek = pozycja += 1;
        int koniec = poczatek;
        int ileZnakow, ilePowtorzen;
        string kompresja;
        
        while(!this._Kompresja[koniec].Equals(')'))
        {
            koniec++;
        }

        kompresja = this._Kompresja[poczatek .. koniec];

        ileZnakow = Convert.ToInt32(kompresja[.. kompresja.IndexOf('x')]);
        ilePowtorzen = Convert.ToInt32(kompresja[(kompresja.IndexOf('x') + 1) ..]);
        
        return (ileZnakow, ilePowtorzen, koniec);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}