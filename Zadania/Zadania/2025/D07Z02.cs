using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D07Z02 : IZadanie
{
    private List<char[]> _Mapa;
    private Int64[] _Liczniki;
    private Int64 _Wynik;
    public D07Z02(bool daneTestowe = false)
    {
        this._Mapa = new List<char[]>();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\07\\proba.txt" : ".\\Dane\\2025\\07\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Mapa.Add(linia.ToCharArray());
        }

        sr.Close(); fs!.Close();

        this._Liczniki = new Int64[this._Mapa[0].Length];
        Array.Fill(this._Liczniki, 0);
        this._Liczniki[new string(this._Mapa[0]).IndexOf('S')]++;
    }

    public void RozwiazanieZadania()
    {
        int szerokosc = this._Mapa[0].Length;
        int wysokosc = this._Mapa.Count;

        this._Mapa[1][new string(this._Mapa[0]).IndexOf('S')] = '|';

        for (int w = 2; w < wysokosc; w += 2)
        {
            for (int s = 0; s < szerokosc; s++)
            {
                if (this._Mapa[w][s].Equals('.') && this._Mapa[w - 1][s].Equals('|'))
                {
                    this._Mapa[w + 1][s] = '|';
                }

                if (this._Mapa[w][s].Equals('^') && this._Mapa[w - 1][s].Equals('|') && this._Liczniki[s] > 0)
                {
                    this._Liczniki[s - 1] += this._Liczniki[s];
                    this._Liczniki[s + 1] += this._Liczniki[s];
                    this._Liczniki[s] = 0;
                    this._Mapa[w + 1][s - 1] = '|';
                    this._Mapa[w + 1][s + 1] = '|';
                }
            }
        }

        this._Wynik = this._Liczniki.Sum();
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}