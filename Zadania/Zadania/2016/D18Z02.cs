using System;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public class D18Z02 : IZadanie
{
    private int _Wysokosc;
    private char[] _MapaZrodlo;
    private char[] _MapaCel;
    private UInt64 _Wynik;
    public D18Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Wysokosc = 400_000;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\18\\proba.txt" : ".\\Dane\\2016\\18\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia = sr.ReadToEnd();
        sr.Close(); fs!.Close();
        this._MapaZrodlo = new char[linia.Length + 2];
        this._MapaCel = new char[linia.Length + 2];

        for(int i = 1; i <= linia.Length; i++)
        {
            this._MapaZrodlo[i] = linia[i - 1];
            if(this._MapaZrodlo[i].Equals('.'))
            {
                this._Wynik++;
            }
        }

        this._MapaZrodlo[0] = '.';
        this._MapaZrodlo[linia.Length + 1] = '.';
    }

    public void RozwiazanieZadania()
    {
        for(int i = 0; i < this._Wysokosc - 1; i++)
        {
            for(int szerokosc = 1; szerokosc < this._MapaZrodlo.Length - 1; szerokosc++)
            {
                if(this.BezpiecznePole(szerokosc))
                {
                    this._MapaCel[szerokosc] = '.';
                    this._Wynik++;
                    continue;
                }

                if(!this.BezpiecznePole(szerokosc))
                {
                    this._MapaCel[szerokosc] = '^';
                }
            }

            this._MapaCel[0] = '.';
            this._MapaCel[^1] = '.';

            this._MapaZrodlo = (char[])this._MapaCel.Clone();
        }
    }

    private bool BezpiecznePole(int X)
    {
        if(this._MapaZrodlo[X - 1].Equals('^') && this._MapaZrodlo[X].Equals('^') && this._MapaZrodlo[X + 1].Equals('.'))
        {
            return false;
        }

        if(this._MapaZrodlo[X - 1].Equals('.') && this._MapaZrodlo[X].Equals('^') && this._MapaZrodlo[X + 1].Equals('^'))
        {
            return false;
        }

        if(this._MapaZrodlo[X - 1].Equals('^') && this._MapaZrodlo[X].Equals('.') && this._MapaZrodlo[X + 1].Equals('.'))
        {
            return false;
        }

        if(this._MapaZrodlo[X - 1].Equals('.') && this._MapaZrodlo[X].Equals('.') && this._MapaZrodlo[X + 1].Equals('^'))
        {
            return false;
        }

        return true;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}