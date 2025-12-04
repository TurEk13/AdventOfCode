using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public class D04Z01 : IZadanie
{
    private List<char[]> _Mapa;
    private Int64 _DostepneRolki;
    public D04Z01(bool daneTestowe = false)
    {
        this._Mapa = new ();
        this._DostepneRolki = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\04\\proba.txt" : ".\\Dane\\2025\\04\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;

        StreamReader sr = new(fs);

        while((linia = sr.ReadLine()) is not null)
        {
            this._Mapa.Add(linia.ToCharArray());
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        int szerokosc = this._Mapa[0].Length;
        int wysokosc = this._Mapa.Count;

        this.SprawdzRogi();

        this.SprawdzKrawedzie();

        for(int y = 1; y < wysokosc - 1; y++)
        {
            for(int x = 1; x < szerokosc - 1; x++)
            {
                if(this._Mapa[y][x].Equals('@') && this.SprawdzPunkt(x, y))
                {
                    this._DostepneRolki++;
                }
            }
        }
    }

    private void SprawdzKrawedzie()
    {
        int szerokosc = this._Mapa[0].Length;
        int wysokosc = this._Mapa.Count;
        int wartoscPunktu1 = 0, wartoscPunktu2 = 0;

        for(int x = 1; x < szerokosc - 1; x++)
        {
            if(this._Mapa[0][x].Equals('@'))
            {
                if(this._Mapa[0][x - 1].Equals('@')) { wartoscPunktu1++; }
                if(this._Mapa[0][x + 1].Equals('@')) { wartoscPunktu1++; }

                if(this._Mapa[1][x - 1].Equals('@')) { wartoscPunktu1++; }
                if(this._Mapa[1][x].Equals('@')) { wartoscPunktu1++; }
                if(this._Mapa[1][x + 1].Equals('@')) { wartoscPunktu1++; }

                if(wartoscPunktu1 < 4)
                {
                    this._DostepneRolki++;
                }

                wartoscPunktu1 = 0;
            }

            if(this._Mapa[^1][x].Equals('@'))
            {
                if(this._Mapa[^1][x - 1].Equals('@')) { wartoscPunktu2++; }
                if(this._Mapa[^1][x + 1].Equals('@')) { wartoscPunktu2++; }

                if(this._Mapa[^2][x - 1].Equals('@')) { wartoscPunktu2++; }
                if(this._Mapa[^2][x].Equals('@')) { wartoscPunktu2++; }
                if(this._Mapa[^2][x + 1].Equals('@')) { wartoscPunktu2++; }

                if(wartoscPunktu2 < 4)
                {
                    this._DostepneRolki++;
                }

                wartoscPunktu2 = 0;
            }
        }

        for(int y = 1; y < wysokosc - 1; y++)
        {
            if(this._Mapa[y][0].Equals('@'))
            {
                if(this._Mapa[y - 1][0].Equals('@')) { wartoscPunktu1++; }
                if(this._Mapa[y + 1][0].Equals('@')) { wartoscPunktu1++; }

                if(this._Mapa[y - 1][1].Equals('@')) { wartoscPunktu1++; }
                if(this._Mapa[y][1].Equals('@')) { wartoscPunktu1++; }
                if(this._Mapa[y + 1][1].Equals('@')) { wartoscPunktu1++; }

                if(wartoscPunktu1 < 4)
                {
                    this._DostepneRolki++;
                }

                wartoscPunktu1 = 0;
            }

            if(this._Mapa[y][^1].Equals('@'))
            {
                if(this._Mapa[y - 1][^1].Equals('@')) { wartoscPunktu2++; }
                if(this._Mapa[y + 1][^1].Equals('@')) { wartoscPunktu2++; }

                if(this._Mapa[y - 1][^2].Equals('@')) { wartoscPunktu2++; }
                if(this._Mapa[y][^2].Equals('@')) { wartoscPunktu2++; }
                if(this._Mapa[y + 1][^2].Equals('@')) { wartoscPunktu2++; }

                if(wartoscPunktu2 < 4)
                {
                    this._DostepneRolki++;
                }

                wartoscPunktu2 = 0;
            }
        }
    }

    private void SprawdzRogi()
    {
        int szerokosc = this._Mapa[0].Length;
        int wysokosc = this._Mapa.Count;
        int wartoscPunktu = 0;

        if(this._Mapa[0][0].Equals('@'))
        {
            if(this._Mapa[0][1].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[1][0].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[1][1].Equals('@')) { wartoscPunktu++; }

            if(wartoscPunktu < 4)
            {
                this._DostepneRolki++;
            }

            wartoscPunktu = 0;
        }

        if(this._Mapa[0][szerokosc - 1].Equals('@'))
        {
            if(this._Mapa[0][szerokosc - 2].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[1][szerokosc - 2].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[1][szerokosc - 1].Equals('@')) { wartoscPunktu++; }

            if(wartoscPunktu < 4)
            {
                this._DostepneRolki++;
            }

            wartoscPunktu = 0;
        }

        if(this._Mapa[wysokosc - 1][0].Equals('@'))
        {
            if(this._Mapa[wysokosc - 2][0].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[wysokosc - 2][1].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[wysokosc - 1][1].Equals('@')) { wartoscPunktu++; }

            if(wartoscPunktu < 4)
            {
                this._DostepneRolki++;
            }

            wartoscPunktu = 0;
        }

        if(this._Mapa[wysokosc - 1][szerokosc - 1].Equals('@'))
        {
            if(this._Mapa[wysokosc - 2][szerokosc - 2].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[wysokosc - 2][szerokosc - 1].Equals('@')) { wartoscPunktu++; }
            if(this._Mapa[wysokosc - 1][szerokosc - 2].Equals('@')) { wartoscPunktu++; }

            if(wartoscPunktu < 4)
            {
                this._DostepneRolki++;
            }
        }
    }

    private bool SprawdzPunkt(int x, int y)
    {
        if(this._Mapa[y][x].Equals('.'))
        {
            return false;
        }

        int wartoscPunktu = 0;

        if(this._Mapa[y - 1][x - 1].Equals('@')) { wartoscPunktu++; }
        if(this._Mapa[y - 1][x].Equals('@')) { wartoscPunktu++; }
        if(this._Mapa[y - 1][x + 1].Equals('@')) { wartoscPunktu++; }

        if(this._Mapa[y][x - 1].Equals('@')) { wartoscPunktu++; }
        if(this._Mapa[y][x + 1].Equals('@')) { wartoscPunktu++; }

        if(this._Mapa[y + 1][x - 1].Equals('@')) { wartoscPunktu++; }
        if(this._Mapa[y + 1][x].Equals('@')) { wartoscPunktu++; }
        if(this._Mapa[y + 1][x + 1].Equals('@')) { wartoscPunktu++; }

        return wartoscPunktu < 4;
    }

    public string PokazRozwiazanie()
    {
        return this._DostepneRolki.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}