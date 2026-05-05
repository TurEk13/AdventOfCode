using System;
using System.Collections.Generic;
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
    
    public D17Z01(bool daneTestowe = false)
    {
        this._Mapa = new ();
        this._DrzwiOtwarte = ['b', 'c', 'd', 'e', 'f'];
        
        StringBuilder sb = new StringBuilder(5 * byte.MaxValue);
        while(sb.Length < sb.Capacity)
        {
            sb.Append('a');
        }

        this._NajkrotszaDroga = sb.ToString();
        sb.Clear();

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
        this.ZnajdzDroge(this.ObliczHash(""), "", new Pozycja(this._Pozycja.X, this._Pozycja.Y));
    }

    private void ZnajdzDroge(string hash, string przebytaDroga, Pozycja pozycja)
    {
        Pozycja nowyPunkt;

        if(przebytaDroga.Length > this._NajkrotszaDroga.Length)
        {
            return;
        }

        if (pozycja.X == 7 && pozycja.Y == 7)
        {
            this.SprawdzDlugosc(przebytaDroga);
            return;
        }

        if (this._DrzwiOtwarte.Contains(hash[0]))
        {
            nowyPunkt = pozycja with { Y = pozycja.Y - 2 };

            if(this.SprawdzPrzejscie(nowyPunkt))
            {
                this.ZnajdzDroge(this.ObliczHash($"{przebytaDroga}U"), new string($"{przebytaDroga}U"), nowyPunkt);
            }
        }

        if(this._DrzwiOtwarte.Contains(hash[1]))
        {
            nowyPunkt = pozycja with { Y = pozycja.Y + 2 };

            if (this.SprawdzPrzejscie(nowyPunkt))
            {
                this.ZnajdzDroge(this.ObliczHash($"{przebytaDroga}D"), new string($"{przebytaDroga}D"), nowyPunkt);
            }
        }

        if(this._DrzwiOtwarte.Contains(hash[2]))
        {
            nowyPunkt = pozycja with { X = pozycja.X - 2 };

            if (this.SprawdzPrzejscie(nowyPunkt))
            {
                this.ZnajdzDroge(this.ObliczHash($"{przebytaDroga}L"), new string($"{przebytaDroga}L"), nowyPunkt);
            }
        }

        if(this._DrzwiOtwarte.Contains(hash[3]))
        {
            nowyPunkt = pozycja with { X = pozycja.X + 2 };

            if (this.SprawdzPrzejscie(nowyPunkt))
            {
                this.ZnajdzDroge(this.ObliczHash($"{przebytaDroga}R"), new string($"{przebytaDroga}R"), nowyPunkt);
            }
        }
    }

    private bool SprawdzPrzejscie(Pozycja nowyPunkt)
    {
        return nowyPunkt.Y >= 0 && nowyPunkt.Y < this._Mapa.Count && nowyPunkt.X >= 0 && nowyPunkt.X < this._Mapa[0].Length;
    }

    private string ObliczHash(string reszta)
    {
        return string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{this._Hash}{reszta}")).Select(o => o.ToString("x2")))[.. 4];
    }

    private void SprawdzDlugosc(string ciag)
    {
        if(this._NajkrotszaDroga.Length > ciag.Length)
        {
            this._NajkrotszaDroga = ciag;
        }
    }
    public string PokazRozwiazanie()
    {
        return "DDRLRRUDDR: " + this._NajkrotszaDroga;
    }

    private record Pozycja(int X, int Y);
}