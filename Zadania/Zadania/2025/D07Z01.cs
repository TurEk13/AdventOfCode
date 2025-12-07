using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public partial class D07Z01 : IZadanie
{
    private List<char[]> _Mapa;
    private int _Licznik;

    public D07Z01(bool daneTestowe = false)
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

                if (this._Mapa[w][s].Equals('^') && this._Mapa[w - 1][s].Equals('|'))
                {
                    this._Mapa[w + 1][s - 1] = '|';
                    this._Mapa[w + 1][s + 1] = '|';
                    this._Licznik++;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Licznik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}