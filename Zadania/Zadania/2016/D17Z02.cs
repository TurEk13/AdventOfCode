using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Zadania._2016;

public partial class D17Z02 : IZadanie
{
    private List<char[]> _Mapa;
    private string _Hash;
    private Punkt _Start;
    private Punkt _Cel;
    private char[] _DrzwiOtwarte;
    private int _NajdluzszaDroga;
    
    public D17Z02(bool daneTestowe = false)
    {
        this._Mapa = new ();
        this._DrzwiOtwarte = ['b', 'c', 'd', 'e', 'f'];
        this._NajdluzszaDroga = 0;
        this._Cel = new ("", new (7, 7));
        
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\17\\proba.txt" : ".\\Dane\\2016\\17\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;

        int wysokosc = 0;

        while((linia = sr.ReadLine()) != string.Empty)
        {
            this._Mapa.Add(linia.ToCharArray());

            if(linia.Contains('S'))
            {
                this._Start = new ("", new (linia.IndexOf('S'), wysokosc));
            }
            wysokosc++;
        }

        this._Hash = sr.ReadLine();
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        Punkt obecnyElement;
        Punkt nowaPozycja;
        string hash;
        Queue<Punkt> punktyDoSprawdzenia = new ();
        punktyDoSprawdzenia.Enqueue(this._Start);

        do
        {
            obecnyElement = punktyDoSprawdzenia.Dequeue();
            if(obecnyElement.Pozycja == this._Cel.Pozycja)
            {
                this._NajdluzszaDroga = Math.Max(this._NajdluzszaDroga, obecnyElement.PrzebytaSciezka.Length);
                continue;
            }

            hash = this.ObliczHash(obecnyElement.PrzebytaSciezka);

            if(this._DrzwiOtwarte.Contains(hash[0]))
            {
                nowaPozycja = obecnyElement with { PrzebytaSciezka = obecnyElement.PrzebytaSciezka + 'U', Pozycja = obecnyElement.Pozycja with { Y = obecnyElement.Pozycja.Y - 2}};

                if(this.SprawdzPrzejscie(nowaPozycja.Pozycja))
                {
                   punktyDoSprawdzenia.Enqueue(nowaPozycja);
                }
            }

            if(this._DrzwiOtwarte.Contains(hash[1]))
            {
                nowaPozycja = obecnyElement with { PrzebytaSciezka = obecnyElement.PrzebytaSciezka + 'D', Pozycja = obecnyElement.Pozycja with { Y = obecnyElement.Pozycja.Y + 2}};

                if(this.SprawdzPrzejscie(nowaPozycja.Pozycja))
                {
                   punktyDoSprawdzenia.Enqueue(nowaPozycja);
                }
            }

            if(this._DrzwiOtwarte.Contains(hash[2]))
            {
                nowaPozycja = obecnyElement with { PrzebytaSciezka = obecnyElement.PrzebytaSciezka + 'L', Pozycja = obecnyElement.Pozycja with { X = obecnyElement.Pozycja.X - 2}};
                
                if(this.SprawdzPrzejscie(nowaPozycja.Pozycja))
                {
                   punktyDoSprawdzenia.Enqueue(nowaPozycja);
                }
            }

            if(this._DrzwiOtwarte.Contains(hash[3]))
            {
                nowaPozycja = obecnyElement with { PrzebytaSciezka = obecnyElement.PrzebytaSciezka + 'R', Pozycja = obecnyElement.Pozycja with { X = obecnyElement.Pozycja.X + 2}};
                
                if(this.SprawdzPrzejscie(nowaPozycja.Pozycja))
                {
                   punktyDoSprawdzenia.Enqueue(nowaPozycja);
                }
            }

        }while(punktyDoSprawdzenia.Count > 0);
    }

    private string ObliczHash(string reszta)
    {
        return string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{this._Hash}{reszta}")).Select(o => o.ToString("x2"))).ToLower()[.. 4];
    }

    private bool SprawdzPrzejscie(Pozycja nowyPunkt)
    {
        return nowyPunkt.Y >= 0 && nowyPunkt.X >= 0 && nowyPunkt.Y < this._Mapa.Count && nowyPunkt.X < this._Mapa[0].Length;
    }
    
    public string PokazRozwiazanie()
    {
        return this._NajdluzszaDroga.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Pozycja(int X, int Y);
    private record Punkt(string PrzebytaSciezka, Pozycja Pozycja);
}