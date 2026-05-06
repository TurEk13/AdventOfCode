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
    private Pozycja _Start;
    private Pozycja _Cel;
    private char[] _DrzwiOtwarte;
    private string _NajkrotszaDroga;
    
    public D17Z01(bool daneTestowe = false)
    {
        this._Mapa = new ();
        this._DrzwiOtwarte = ['b', 'c', 'd', 'e', 'f'];
        this._Cel = new (7, 7);
        
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
                this._Start = new (linia.IndexOf('S'), wysokosc);
            }
            wysokosc++;
        }

        this._Hash = sr.ReadLine();
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        this.ZnajdzDroge(this.ObliczHash(""), "", new Pozycja(this._Start.X, this._Start.Y));
    }

    private void ZnajdzDroge(string hash, string przebytaDroga, Pozycja pozycja)
    {
        if(przebytaDroga.Length > this._NajkrotszaDroga.Length)
        {
            return;
        }

        if(pozycja == this._Cel)
        {
            this.SprawdzDlugosc(przebytaDroga);
            return;
        }

        if(this._DrzwiOtwarte.Contains(hash[0]))
        {
            this.PrzetworzPunkt('U', przebytaDroga, pozycja);
        }

        if(this._DrzwiOtwarte.Contains(hash[1]))
        {
            this.PrzetworzPunkt('D', przebytaDroga, pozycja);
        }

        if(this._DrzwiOtwarte.Contains(hash[2]))
        {
            
            this.PrzetworzPunkt('L', przebytaDroga, pozycja);
        }

        if(this._DrzwiOtwarte.Contains(hash[3]))
        {
            this.PrzetworzPunkt('R', przebytaDroga, pozycja);
        }
    }

    private void PrzetworzPunkt(char kierunek, string przebytaDroga, Pozycja pozycja)
    {
        Pozycja nowaPozycja = null;

        switch(kierunek)
        {
            case 'U':
                nowaPozycja = pozycja with { Y = pozycja.Y - 2 };
                break;
            case 'D':
                nowaPozycja = pozycja with { Y = pozycja.Y + 2 };
                break;
            case 'L':
                nowaPozycja = pozycja with { X = pozycja.X - 2 };
                break;
            case 'R':
                nowaPozycja = pozycja with { X = pozycja.X + 2 };
                break;
        }

        if(this.SprawdzPrzejscie(nowaPozycja))
        {
            this.ZnajdzDroge(this.ObliczHash($"{przebytaDroga}{kierunek}"), new string($"{przebytaDroga}{kierunek}"), nowaPozycja);
        }
    }

    private bool SprawdzPrzejscie(Pozycja nowyPunkt)
    {
        return nowyPunkt.Y >= 0 && nowyPunkt.Y < this._Mapa.Count && nowyPunkt.X >= 0 && nowyPunkt.X < this._Mapa[0].Length;
    }

    private string ObliczHash(string przebytaDroga)
    {
        return string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{this._Hash}{przebytaDroga}")).Select(o => o.ToString("x2"))).ToLower()[.. 4];
    }

    private void SprawdzDlugosc(string przebytaDroga)
    {
        if(this._NajkrotszaDroga.Length > przebytaDroga.Length)
        {
            this._NajkrotszaDroga = przebytaDroga;
        }
    }
    
    public string PokazRozwiazanie()
    {
        return this._NajkrotszaDroga;
    }

    private record Pozycja(int X, int Y);
}