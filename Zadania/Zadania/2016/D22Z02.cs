using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D22Z02 : IZadanie
{
    private UInt64 _IleRuchow;
    private readonly int _maxX;
    private readonly int _maxY;
    private readonly string _celZeroID;
    private readonly string _celDocelowyID;
    private readonly Punkt _lokalizacjaKoncowa;
    private readonly Punkt _lokalizacjaPoczatkowa;
    private readonly Punkt _lokalizacjaPosrednia;
    private Dictionary<Punkt, Wezel> _SpisWezlow;

    [GeneratedRegex(@"x(\d{1,2})-y(\d{1,2})(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})")]
    private static partial Regex wezel();
    
    public D22Z02(bool daneTestowe = false)
    {
        this._IleRuchow = UInt64.MaxValue;
        this._SpisWezlow = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\22\\proba.txt" : ".\\Dane\\2016\\22\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;

        Regex daneWezla = wezel();
        Match m;

        while((linia = sr.ReadLine()) is not null)
        {
            m = daneWezla.Match(linia);

            if(m.Success)
            {
                this._SpisWezlow.Add(new Punkt(Convert.ToInt32(m.Groups[1].Value), Convert.ToInt32(m.Groups[2].Value)), new Wezel($"{m.Groups[1].Value}{m.Groups[2].Value}", new Punkt(Convert.ToInt32(m.Groups[1].Value), Convert.ToInt32(m.Groups[2].Value)), Convert.ToInt32(m.Groups[3].Value), Convert.ToInt32(m.Groups[4].Value), Convert.ToInt32(m.Groups[5].Value), Convert.ToInt32(m.Groups[6].Value)));
            }
        }
        
        sr.Close(); fs!.Close();

        this._maxX = this._SpisWezlow.Max(p => p.Key.X);
        this._maxY = this._SpisWezlow.Max(p => p.Key.Y);
        this._lokalizacjaKoncowa = new (0, 0);
        this._lokalizacjaPoczatkowa = this._SpisWezlow.First(sw => sw.Value.Uzyte == 0).Key;
        this._lokalizacjaPosrednia = new (this._maxX, 0);

        this._celZeroID = this._SpisWezlow[this._lokalizacjaPoczatkowa].Id;
        this._celDocelowyID = this._SpisWezlow[this._lokalizacjaPosrednia].Id;
    }

    public void RozwiazanieZadania()
    {
        UInt64 tmp;
        List<Punkt> Odwiedzone = new List<Punkt>();

        this.DFS(this._lokalizacjaPoczatkowa, this._lokalizacjaPosrednia, 0, null, Odwiedzone);
        Debug.WriteLine(this._IleRuchow);
        tmp = this._IleRuchow;

        this._IleRuchow = UInt64.MaxValue;
        Odwiedzone.Clear();
        this.DFS(this._lokalizacjaPosrednia, this._lokalizacjaKoncowa, 0, null, Odwiedzone);
        Debug.WriteLine(this._IleRuchow);
        this._IleRuchow += tmp;
    }

    private void DFS(Punkt obecny, Punkt koncowy, UInt64 odleglosc, Punkt rodzic, List<Punkt> odwiedzone)
    {
        if(odwiedzone.Any(o => o.X == obecny.X && o.Y == obecny.Y))
        {
            return;
        }

        if(rodzic is not null && obecny.X == rodzic.X && obecny.Y == rodzic.Y)
        {
            return;
        }
        
        if(obecny.X == koncowy.X && obecny.Y == koncowy.Y)
        {
            if(odleglosc < this._IleRuchow)
            {
                this._IleRuchow = odleglosc;
                return;
            }
        }

        odwiedzone.Add(obecny);
        
        if(obecny.X == 0 && obecny.Y == 0)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X + 1 }))
            {
                DFS(obecny with { X = obecny.X + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y + 1 }))
            {
                DFS(obecny with { Y = obecny.Y + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X == 0 && obecny.Y > 0 && obecny.Y < this._maxY)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X + 1 }))
            {
                DFS(obecny with { X = obecny.X + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y + 1 }))
            {
                DFS(obecny with { Y = obecny.Y + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y - 1 }))
            {
                DFS(obecny with { Y = obecny.Y - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X == 0 && obecny.Y == this._maxY)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X + 1 }))
            {
                DFS(obecny with { X = obecny.X + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y - 1 }))
            {
                DFS(obecny with { Y = obecny.Y - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X > 0 && obecny.X < this._maxX && obecny.Y == 0)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X + 1 }))
            {
                DFS(obecny with { X = obecny.X + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }
            
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X - 1 }))
            {
                DFS(obecny with { X = obecny.X - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y + 1 }))
            {
                DFS(obecny with { Y = obecny.Y + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X == this._maxX && obecny.Y == 0)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X - 1 }))
            {
                DFS(obecny with { X = obecny.X - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y + 1 }))
            {
                DFS(obecny with { Y = obecny.Y + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X == this._maxX && obecny.Y > 0 && obecny.Y < this._maxY)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X - 1 }))
            {
                DFS(obecny with { X = obecny.X - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y + 1 }))
            {
                DFS(obecny with { Y = obecny.Y + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y - 1 }))
            {
                DFS(obecny with { Y = obecny.Y - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X == this._maxX && obecny.Y== this._maxY)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X - 1 }))
            {
                DFS(obecny with { X = obecny.X - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y - 1 }))
            {
                DFS(obecny with { Y = obecny.Y - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X > 0 && obecny.X < this._maxX && obecny.Y == this._maxY)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X + 1 }))
            {
                DFS(obecny with { X = obecny.X + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X - 1 }))
            {
                DFS(obecny with { X = obecny.X - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y - 1 }))
            {
                DFS(obecny with { Y = obecny.Y - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            return;
        }
        
        if(obecny.X > 0 && obecny.X < this._maxX && obecny.Y > 0 && obecny.Y < this._maxY)
        {
            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X + 1 }))
            {
                DFS(obecny with { X = obecny.X + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { X = obecny.X - 1 }))
            {
                DFS(obecny with { X = obecny.X - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y + 1 }))
            {
                DFS(obecny with { Y = obecny.Y + 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }

            if(this.ZamienMiejscami(obecny, obecny with { Y = obecny.Y - 1 }))
            {
                DFS(obecny with { Y = obecny.Y - 1 }, koncowy, odleglosc + 1, obecny, new List<Punkt>(odwiedzone));
            }
        }
    }

    private bool ZamienMiejscami(Punkt gdziePrzeniesc, Punkt skadPrzeniesc)
    {
        if(this._SpisWezlow[gdziePrzeniesc].Wolne < this._SpisWezlow[skadPrzeniesc].Uzyte)
        {
            return false;
        }

        Wezel tmp = this._SpisWezlow[gdziePrzeniesc] with { Id = this._SpisWezlow[gdziePrzeniesc].Id, Punkt = this._SpisWezlow[gdziePrzeniesc].Punkt, Rozmiar = this._SpisWezlow[gdziePrzeniesc].Rozmiar, Uzyte = this._SpisWezlow[gdziePrzeniesc].Uzyte, Wolne = this._SpisWezlow[gdziePrzeniesc].Wolne, UzyteProcent = this._SpisWezlow[gdziePrzeniesc].UzyteProcent };

        this._SpisWezlow[gdziePrzeniesc] = this._SpisWezlow[gdziePrzeniesc] with { Id = this._SpisWezlow[skadPrzeniesc].Id, Rozmiar = this._SpisWezlow[skadPrzeniesc].Rozmiar, Uzyte = this._SpisWezlow[skadPrzeniesc].Uzyte, Wolne = this._SpisWezlow[skadPrzeniesc].Wolne, UzyteProcent = this._SpisWezlow[skadPrzeniesc].UzyteProcent };
        
        this._SpisWezlow[skadPrzeniesc] = this._SpisWezlow[skadPrzeniesc] with { Id = tmp.Id, Rozmiar = tmp.Rozmiar, Uzyte = tmp.Uzyte, Wolne = tmp.Wolne, UzyteProcent = tmp.UzyteProcent };

        return true;
    }

    public string PokazRozwiazanie()
    {
        return this._IleRuchow.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Wezel(string Id, Punkt Punkt, int Rozmiar, int Uzyte, int Wolne, int UzyteProcent);
    private record Punkt(int X, int Y);
}