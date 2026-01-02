using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D22Z01 : IZadanie
{
    private UInt64 _IlePar;
    private int _X;
    private int _Y;
    private Dictionary<(int Y, int X), Wezel> _SpisWezlow;

    [GeneratedRegex(@"x(\d{1,2})-y(\d{1,2})(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})")]
    private static partial Regex wezel();
    
    public D22Z01()
    {
        this._IlePar = 0;
        this._SpisWezlow = new ();
        FileStream fs = new(".\\Dane\\2016\\22\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;

        Regex daneWezla = wezel();
        Match m;

        while((linia = sr.ReadLine()) is not null)
        {
            m = daneWezla.Match(linia);

            if(m.Success)
            {
                this._SpisWezlow.Add((Convert.ToUInt16(m.Groups[2].Value), Convert.ToUInt16(m.Groups[1].Value)), new (Convert.ToUInt16(m.Groups[2].Value), Convert.ToUInt16(m.Groups[1].Value), Convert.ToUInt16(m.Groups[3].Value), Convert.ToUInt16(m.Groups[4].Value), Convert.ToUInt16(m.Groups[5].Value), Convert.ToUInt16(m.Groups[6].Value)));
            }
        }
        
        sr.Close(); fs!.Close();

        this._X = this._SpisWezlow.Max(p => p.Key.X);
        this._Y = this._SpisWezlow.Max(p => p.Key.Y);
    }

    public void RozwiazanieZadania()
    {
        for(int y = 0; y <= this._Y; y++)
        {
            for(int x = 0; x <= this._X; x++)
            {
                if(this._SpisWezlow.TryGetValue((y, x), out Wezel zrodlo))
                {
                    this.SprawdzPunkt(zrodlo);
                }
            }
        }
    }

    private void SprawdzPunkt(Wezel zrodlo)
    {
        Wezel cel;
        for(int y = 0; y <= this._Y; y++)
        {
            for(int x = 0; x <= this._X; x++)
            {
                if(this._SpisWezlow.TryGetValue((y, x), out cel))
                {
                    if(this.SprawdzWezel(zrodlo, cel))
                    {
                        this._IlePar++;
                    }
                }
            }
        }
    }

    private bool SprawdzWezel(Wezel zrodlo, Wezel cel)
    {
        if(zrodlo.Uzyte == 0)
        {
            return false;
        }

        if(zrodlo.X == cel.X && zrodlo.Y == cel.Y)
        {
            return false;
        }

        if(zrodlo.Uzyte > cel.Wolne)
        {
            return false;
        }

        return true;
    }

    public string PokazRozwiazanie()
    {
        return this._IlePar.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Wezel(UInt16 X, UInt16 Y, UInt16 Rozmiar, UInt16 Uzyte, UInt16 Wolne, UInt16 UzyteProcent);
}