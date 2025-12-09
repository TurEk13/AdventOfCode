using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public partial class D09Z02 : IZadanie
{
    private Int64 _Wynik;
    private List<Punkt> _Punkty;
    private char[,] _Tablica;

    public D09Z02(bool daneTestowe = false)
    {
        this._Punkty = new ();
        this._Tablica = new char[9, 14];
        this._Wynik = Int64.MinValue;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\09\\proba.txt" : ".\\Dane\\2025\\09\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Punkty.Add(new (linia.Split(',')));
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for(int i = 0; i < this._Tablica.GetLength(0); i++)
        {
            for(int j = 0; j < this._Tablica.GetLength(1); j++)
            {
                this._Tablica[i, j] = '.';
            }
        }

        foreach(Punkt p in this._Punkty)
        {
            this._Tablica[p.Y, p.X] = '#';
        }

        for(int i = 0; i < this._Tablica.GetLength(0); i++)
        {
            for(int j = 1; j < this._Tablica.GetLength(1); j++)
            {
                //
            }
        }

        for(int i = 0; i < this._Tablica.GetLength(0); i++)
        {
            for(int j = 0; j < this._Tablica.GetLength(1); j++)
            {
                Debug.Write(this._Tablica[i, j]);
            }
            Debug.WriteLine("");
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Punkt
    {
        public Int64 X { get; private set; }
        public Int64 Y { get; private set; }

        public Punkt(string[] punkty)
        {
            this.X = Convert.ToInt64(punkty[0]);
            this.Y = Convert.ToInt64(punkty[1]);
        }
    }
}