using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Zadania._2016;

public partial class D17Z01 : IZadanie
{
    private List<char[]> _Mapa;
    private string _Hash;
    private Pozycja _Pozycja;
    private char[] _DrzwiOtwarte;
    private string _NajkrotszaDroga;

    //private const int MAKS = 1000;

    public D17Z01(bool daneTestowe = false)
    {
        this._Mapa = new ();
        this._DrzwiOtwarte = ['b', 'c', 'd', 'e', 'f'];

        StringBuilder sb = new ();
        /*for (int i = 0; i < MAKS; i++)
        {
            sb.Append('a');
        }*/

        this._NajkrotszaDroga = string.Empty;// sb.ToString();
        //sb.Clear();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\17\\proba.txt" : ".\\Dane\\2016\\17\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;

        int wysokosc = 0;

        while((linia = sr.ReadLine()) != string.Empty)
        {
            this._Mapa.Add(linia.ToCharArray());

            if(linia.Contains('S'))
            {
                this._Pozycja = new (linia.IndexOf('S'), wysokosc);
            }
            wysokosc++;
        }

        this._Hash = sr.ReadLine();
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        this.ZnajdzDroge(this.ObliczHash(this._Hash), "", new Pozycja(this._Pozycja.X, this._Pozycja.Y));
    }

    private void ZnajdzDroge(string hash, string przebytaDroga, Pozycja pozycja)
    {
        Przejscie p;

        if(!this._NajkrotszaDroga.Equals(string.Empty) && przebytaDroga.Length > this._NajkrotszaDroga.Length)
        {
            return;
        }

        // GÓRA
        if(this._DrzwiOtwarte.Contains(hash[0]))
        {
            p = this.SprawdzPrzejscie(new (pozycja.Y - 1, pozycja.X));
            
            switch(p)
            {
                case Przejscie.KONIEC:
                    this.SprawdzDlugosc(przebytaDroga);
                    return;
                case Przejscie.OTWARTE:
                    this.ZnajdzDroge(this.ObliczHash($"{this._Hash}{przebytaDroga}U"), new string($"{przebytaDroga}U"), pozycja with { Y = pozycja.Y - 2 });
                    break;
                default:
                    break;
            }
        }

        // DÓŁ
        if(this._DrzwiOtwarte.Contains(hash[1]))
        {
            p = this.SprawdzPrzejscie(new (pozycja.Y + 1, pozycja.X));

            switch(p)
            {
                case Przejscie.KONIEC:
                    this.SprawdzDlugosc(przebytaDroga);
                    return;
                case Przejscie.OTWARTE:
                    this.ZnajdzDroge(this.ObliczHash($"{this._Hash}{przebytaDroga}D"), new string($"{przebytaDroga}D"), pozycja with { Y = pozycja.Y + 2 });
                    break;
                default:
                    break;
            }
        }

        // LEWO
        if(this._DrzwiOtwarte.Contains(hash[2]))
        {
            p = this.SprawdzPrzejscie(new (pozycja.Y, pozycja.X - 1));

            switch(p)
            {
                case Przejscie.KONIEC:
                    this.SprawdzDlugosc(przebytaDroga);
                    return;
                case Przejscie.OTWARTE:
                    this.ZnajdzDroge(this.ObliczHash($"{this._Hash}{przebytaDroga}L"), new string($"{przebytaDroga}L"), pozycja with { X = pozycja.X - 2 });
                    break;
                default:
                    break;
            }
        }

        // PRAWO
        if(this._DrzwiOtwarte.Contains(hash[3]))
        {
            p = this.SprawdzPrzejscie(new (pozycja.Y, pozycja.X + 1));

            switch(p)
            {
                case Przejscie.KONIEC:
                    this.SprawdzDlugosc(przebytaDroga);
                    return;
                case Przejscie.OTWARTE:
                    this.ZnajdzDroge(this.ObliczHash($"{this._Hash}{przebytaDroga}R"), new string($"{przebytaDroga}R"), pozycja with { X = pozycja.X + 2 });
                    break;
                default:
                    break;
            }
        }
    }

    private Przejscie SprawdzPrzejscie(Pozycja nowyPunkt)
    {
        if(this._Mapa[nowyPunkt.Y][nowyPunkt.X].Equals('#'))
        {
            return Przejscie.SCIANA;
        }

        if(this._Mapa[nowyPunkt.Y][nowyPunkt.X].Equals(' '))
        {
            return Przejscie.KONIEC;
        }

        return Przejscie.OTWARTE;
    }

    private string ObliczHash(string baza)
    {
        return string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{baza}")).Select(o => o.ToString("x2")))[.. 4];
    }

    private void SprawdzDlugosc(string ciag)
    {
        if(this._NajkrotszaDroga.Equals(string.Empty))
        {
            this._NajkrotszaDroga = ciag;
        }

        if(this._NajkrotszaDroga.Length > ciag.Length)
        {
            this._NajkrotszaDroga = ciag;
        }
    }
    public string PokazRozwiazanie()
    {
        return this._NajkrotszaDroga;
    }

    private record Pozycja(int X, int Y);

    [Flags]
    public enum Przejscie: int
    {
        SCIANA = 1,
        OTWARTE = 2,
        KONIEC = 4
    }
}