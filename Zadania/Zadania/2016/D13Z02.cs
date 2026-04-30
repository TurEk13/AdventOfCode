using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D13Z02 : IZadanie
{
    private readonly int _UlubionaLiczba;
    private Punkt _Start;
    private int _MaksDroga;
    private int _MaksX;
    private int _MaksY;
    private HashSet<Punkt> _OdwiedzonePunkty;
    public D13Z02(bool daneTestowe = false)
    {
        this._OdwiedzonePunkty = new();
        this._Start = new Punkt(1, 1);
        this._MaksDroga = 50;
        this._MaksX = 50;
        this._MaksY = 50;
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

        this._OdwiedzonePunkty.Add(ObecneMiejsce);
        
        if (DlugoscSciezki == this._MaksDroga)
        {
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
        return this._OdwiedzonePunkty.Count.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
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