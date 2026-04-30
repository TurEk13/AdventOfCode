using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D13Z01 : IZadanie
{
    private readonly int _UlubionaLiczba;
    private Punkt _Start;
    private Punkt _Cel;
    private UInt32 _Wynik;
    private int _MaksX;
    private int _MaksY;
    public D13Z01(bool daneTestowe = false)
    {
        this._Start = new Punkt(1, 1);
        this._Cel = daneTestowe ? new Punkt(7, 4) : new Punkt(31, 39);
        this._MaksX = daneTestowe ? 10 : 50;
        this._MaksY = daneTestowe ? 7 : 50;
        this._Wynik = UInt32.MaxValue;
        FileStream fs = new (daneTestowe ? ".\\Dane\\2016\\13\\proba.txt" : ".\\Dane\\2016\\13\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);

        this._UlubionaLiczba = Convert.ToInt32(sr.ReadToEnd());

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        this.PrzesunPunkt(this._Start, new List<Punkt>(), 0);
    }

    private void PrzesunPunkt(Punkt ObecneMiejsce, List<Punkt> PoprzednieMiejsca, UInt32 DlugoscSciezki)
    {
        Punkt NoweMiejsce;

        if (ObecneMiejsce.X < 0 || ObecneMiejsce.X >= this._MaksX || ObecneMiejsce.Y < 0 || ObecneMiejsce.Y >= this._MaksY || !this.WolnyPunkt(this.Etap2(this.Etap1(ObecneMiejsce.X, ObecneMiejsce.Y))))
        {
            return;
        }

        if (ObecneMiejsce.X == this._Cel.X && ObecneMiejsce.Y == this._Cel.Y)
        {
            if (DlugoscSciezki < this._Wynik)
            {
                this._Wynik = DlugoscSciezki;
            }

            return;
        }

        NoweMiejsce = ObecneMiejsce with { X = ObecneMiejsce.X + 1 };
        if (!PoprzednieMiejsca.Contains(NoweMiejsce))
        {
            PoprzednieMiejsca.Add(ObecneMiejsce);
            this.PrzesunPunkt(NoweMiejsce, PoprzednieMiejsca, DlugoscSciezki + 1);
            PoprzednieMiejsca.RemoveAt(PoprzednieMiejsca.Count - 1);
        }

        NoweMiejsce = ObecneMiejsce with { X = ObecneMiejsce.X - 1 };
        if (!PoprzednieMiejsca.Contains(NoweMiejsce))
        {
            PoprzednieMiejsca.Add(ObecneMiejsce);
            this.PrzesunPunkt(NoweMiejsce, PoprzednieMiejsca, DlugoscSciezki + 1);
            PoprzednieMiejsca.RemoveAt(PoprzednieMiejsca.Count - 1);
        }

        NoweMiejsce = ObecneMiejsce with { Y = ObecneMiejsce.Y + 1 };
        if (!PoprzednieMiejsca.Contains(NoweMiejsce))
        {
            PoprzednieMiejsca.Add(ObecneMiejsce);
            this.PrzesunPunkt(NoweMiejsce, PoprzednieMiejsca, DlugoscSciezki + 1);
            PoprzednieMiejsca.RemoveAt(PoprzednieMiejsca.Count - 1);
        }

        NoweMiejsce = ObecneMiejsce with { Y = ObecneMiejsce.Y - 1 };
        if (!PoprzednieMiejsca.Contains(NoweMiejsce))
        {
            PoprzednieMiejsca.Add(ObecneMiejsce);
            this.PrzesunPunkt(NoweMiejsce, PoprzednieMiejsca, DlugoscSciezki + 1);
            PoprzednieMiejsca.RemoveAt(PoprzednieMiejsca.Count - 1);
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private int Etap1(int X, int Y)
    {
        return X * X + 3 * X + 2 * X * Y + Y + Y * Y;
    }

    private int Etap2(int Etap1)
    {
        return Etap1 + this._UlubionaLiczba;
    }
    
    private bool WolnyPunkt(int Etap2)
    {
        return new BitArray([Etap2]).Cast<bool>().Where(b => b).ToArray().Length % 2 == 0;
    }

    private record Punkt(int X, int Y);
}