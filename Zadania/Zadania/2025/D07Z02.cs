using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public partial class D07Z02 : IZadanie
{
    private List<char[]> _Mapa;
    private Int128 _Licznik;
    public D07Z02(bool daneTestowe = false)
    {
        this._Licznik = 0;
        this._Mapa = new List<char[]>();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\07\\proba.txt" : ".\\Dane\\2025\\07\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Mapa.Add(linia.ToCharArray());
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        this.Rysuj();
        this.ZnajdzSciezki(2, new string(this._Mapa[0]).IndexOf('S'));
    }

    private void Rysuj()
    {
        int start = new string(this._Mapa[0]).IndexOf('S');
        int szerokosc = this._Mapa[0].Length;
        int wysokosc = this._Mapa.Count;

        this._Mapa[1][start] = '|';

        for (int w = 2; w < wysokosc; w++)
        {
            for (int s = 0; s < szerokosc; s++)
            {
                if (this._Mapa[w][s].Equals('.') && this._Mapa[w - 1][s].Equals('|'))
                {
                    this._Mapa[w][s] = '|';
                }

                if (this._Mapa[w][s].Equals('^') && this._Mapa[w - 1][s].Equals('|'))
                {
                    this._Mapa[w][s - 1] = '|';
                    this._Mapa[w][s + 1] = '|';
                }
            }
        }
    }

    private void ZnajdzSciezki(int wiersz, int kolumna)
    {
        if (wiersz == this._Mapa.Count)
        {
            this._Licznik++;
            if (this._Licznik % 100_000_000 == 0)
            {
                Debug.WriteLine($"{this._Licznik:N0}");
            }
            return;
        }

        if (this._Mapa[wiersz][kolumna].Equals('|'))
        {
            this.ZnajdzSciezki(wiersz + 2, kolumna);
            return;
        }

        if (this._Mapa[wiersz][kolumna].Equals('^'))
        {
            this.ZnajdzSciezki(wiersz + 2, kolumna - 1);
            this.ZnajdzSciezki(wiersz + 2, kolumna + 1);
            return;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Licznik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}