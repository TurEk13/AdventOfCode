using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D22Z02 : IZadanie
{
    private UInt64 _IleRuchow;
    private readonly int _maxX;
    private readonly int _maxY;
    private readonly string _celZeroID;
    private readonly string _celDocelowyID;
    private readonly Punkt _lokalizacjaDocelowa;
    private readonly Punkt _lokalizacjaPunktuZero;
    private readonly Punkt _lokalizacjaPoczatkowa;
    private Dictionary<Punkt, Wezel> _SpisWezlow;

    [GeneratedRegex(@"x(\d{1,2})-y(\d{1,2})(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})T(?: {0,10})(\d{1,3})")]
    private static partial Regex wezel();
    
    public D22Z02(bool daneTestowe = false)
    {
        this._IleRuchow = int.MaxValue;
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
        this._lokalizacjaDocelowa = new (0, 0);
        this._lokalizacjaPunktuZero = this._SpisWezlow.First(sw => sw.Value.Uzyte == 0).Key;
        this._celZeroID = this._SpisWezlow[this._lokalizacjaPunktuZero].Id;
        this._lokalizacjaPoczatkowa = new (this._maxX, 0);
        this._celDocelowyID = this._SpisWezlow[this._lokalizacjaPoczatkowa].Id;
    }

    public void RozwiazanieZadania()
    {
        UInt64 OdZeroDoGory;

        this.SprawdzWezel(this._lokalizacjaPunktuZero, 0, this._lokalizacjaPoczatkowa, false);
        
        OdZeroDoGory = this._IleRuchow;
        this._IleRuchow = int.MaxValue;
        this.SprawdzWezel(this._lokalizacjaPoczatkowa, 0, this._lokalizacjaDocelowa, true);
        this._IleRuchow += OdZeroDoGory;
    }

    private void SprawdzWezel(Punkt obecnaLokalizacja, UInt64 ruch, Punkt docelowaLokalizacja, bool koniec)
    {
        Debug.WriteLine($"X: {obecnaLokalizacja.X}, Y: {obecnaLokalizacja.Y}, ObecnyId: {this._SpisWezlow[obecnaLokalizacja].Id}, KoniecID: {this._SpisWezlow[docelowaLokalizacja].Id}");
        
        if(!koniec && this._SpisWezlow[docelowaLokalizacja].Id.Equals(this._celZeroID))
        {
            if(this._IleRuchow > ruch)
            {
                this._IleRuchow = ruch;
                Debug.WriteLine($"####### Ile ruchów: {this._IleRuchow} #######");
            }
            return;
        }

        if(koniec && this._SpisWezlow[docelowaLokalizacja].Id.Equals(this._celDocelowyID))
        {
            if(this._IleRuchow > ruch)
            {
                this._IleRuchow = ruch;
                Debug.WriteLine($"####### Ile ruchów: {this._IleRuchow} #######");
            }
            return;
        }

        // 2 kierunki
        if(obecnaLokalizacja.X == 0 && obecnaLokalizacja.Y == 0)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        if(obecnaLokalizacja.X == 0 && obecnaLokalizacja.Y == this._maxY)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        if(obecnaLokalizacja.X == this._maxX && obecnaLokalizacja.Y == 0)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        if(obecnaLokalizacja.X == this._maxX && obecnaLokalizacja.Y == this._maxY)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        // 3 kierunki
        if(obecnaLokalizacja.X == 0 && obecnaLokalizacja.Y > 0 && obecnaLokalizacja.Y < this._maxY)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        if(obecnaLokalizacja.X == this._maxX && obecnaLokalizacja.Y > 0 && obecnaLokalizacja.Y < this._maxY)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        if(obecnaLokalizacja.X > 0 && obecnaLokalizacja.X < this._maxX && obecnaLokalizacja.Y == 0)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        if(obecnaLokalizacja.X > 0 && obecnaLokalizacja.X < this._maxX && obecnaLokalizacja.Y == this._maxY)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }

        // 4 kierunki
        if(obecnaLokalizacja.X > 0 && obecnaLokalizacja.X < this._maxX && obecnaLokalizacja.Y > 0 && obecnaLokalizacja.Y < this._maxY)
        {
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y + 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { X = obecnaLokalizacja.X - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            this.ZamienMiejscami(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, obecnaLokalizacja);
            this.SprawdzWezel(obecnaLokalizacja with { Y = obecnaLokalizacja.Y - 1 }, ruch + 1, docelowaLokalizacja, koniec);
            return;
        }
    }

    private void ZamienMiejscami(Punkt gdziePrzeniesc, Punkt skadPrzeniesc)
    {
        if(this._SpisWezlow[gdziePrzeniesc].Wolne < this._SpisWezlow[skadPrzeniesc].Uzyte)
        {
            return;
        }

        Wezel tmp = this._SpisWezlow[gdziePrzeniesc] with { Id = this._SpisWezlow[gdziePrzeniesc].Id, Punkt = this._SpisWezlow[gdziePrzeniesc].Punkt, Rozmiar = this._SpisWezlow[gdziePrzeniesc].Rozmiar, Uzyte = this._SpisWezlow[gdziePrzeniesc].Uzyte, Wolne = this._SpisWezlow[gdziePrzeniesc].Wolne, UzyteProcent = this._SpisWezlow[gdziePrzeniesc].UzyteProcent };

        this._SpisWezlow[gdziePrzeniesc] = this._SpisWezlow[gdziePrzeniesc] with { Id = this._SpisWezlow[skadPrzeniesc].Id, Rozmiar = this._SpisWezlow[skadPrzeniesc].Rozmiar, Uzyte = this._SpisWezlow[skadPrzeniesc].Uzyte, Wolne = this._SpisWezlow[skadPrzeniesc].Wolne, UzyteProcent = this._SpisWezlow[skadPrzeniesc].UzyteProcent };
        
        this._SpisWezlow[skadPrzeniesc] = this._SpisWezlow[skadPrzeniesc] with { Id = tmp.Id, Rozmiar = tmp.Rozmiar, Uzyte = tmp.Uzyte, Wolne = tmp.Wolne, UzyteProcent = tmp.UzyteProcent };
    }

    public string PokazRozwiazanie()
    {
        return this._IleRuchow.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Wezel(string Id, Punkt Punkt, int Rozmiar, int Uzyte, int Wolne, int UzyteProcent);
    private record Punkt(int X, int Y);
}