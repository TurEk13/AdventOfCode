using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private Pozycja _Pozycja;
    private char[] _DrzwiOtwarte;
    private int _NajdluzszaDroga;
    private string _PrzebytaDroga;
    private List<Pozycja> _OdwiedzonePunkty;
    
    public D17Z02(bool daneTestowe = false)
    {
        this._OdwiedzonePunkty = new ();
        this._PrzebytaDroga = "";
        this._Mapa = new ();
        this._DrzwiOtwarte = ['b', 'c', 'd', 'e', 'f'];
        this._NajdluzszaDroga = 0;
        
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
        this.ZnajdzDroge(this.ObliczHash(""), new Pozycja(this._Pozycja.X, this._Pozycja.Y));
    }

    private void ZnajdzDroge(string hash, Pozycja pozycja)
    {
        Pozycja nowaPozycja;

        if(pozycja.X == 7 && pozycja.Y == 7)
        {
            this.SprawdzDlugosc();
        }

        this._OdwiedzonePunkty.Add(pozycja);

        if (!this._DrzwiOtwarte.Contains(hash[0]) && !this._DrzwiOtwarte.Contains(hash[1]) && !this._DrzwiOtwarte.Contains(hash[2]) && !this._DrzwiOtwarte.Contains(hash[3]))
        {
            this._OdwiedzonePunkty.RemoveAt(this._OdwiedzonePunkty.Count - 1);
            return;
        }

        if(this._DrzwiOtwarte.Contains(hash[0]))
        {
            nowaPozycja = pozycja with { Y = pozycja.Y - 2 };

            if(this.SprawdzPrzejscie(nowaPozycja))
            {
                this._PrzebytaDroga += 'U';
                if (!this.PunktyOdwiedzone(pozycja, nowaPozycja))
                {
                    this.ZnajdzDroge(this.ObliczHash(this._PrzebytaDroga), nowaPozycja);
                }
                this._PrzebytaDroga = this._PrzebytaDroga[..^1];
            }
        }

        if(this._DrzwiOtwarte.Contains(hash[1]))
        {
            nowaPozycja = pozycja with { Y = pozycja.Y + 2 };
            if (this.SprawdzPrzejscie(nowaPozycja))
            {
                this._PrzebytaDroga += 'D';
                if (!this.PunktyOdwiedzone(pozycja, nowaPozycja))
                {
                    this.ZnajdzDroge(this.ObliczHash(this._PrzebytaDroga), nowaPozycja);
                }
                this._PrzebytaDroga = this._PrzebytaDroga[..^1];
            }
        }

        if(this._DrzwiOtwarte.Contains(hash[2]))
        {
            nowaPozycja = pozycja with { X = pozycja.X - 2 };
            if (this.SprawdzPrzejscie(nowaPozycja))
            {
                this._PrzebytaDroga += 'L';
                if (!this.PunktyOdwiedzone(pozycja, nowaPozycja))
                {
                    this.ZnajdzDroge(this.ObliczHash(this._PrzebytaDroga), nowaPozycja);
                }
                this._PrzebytaDroga = this._PrzebytaDroga[..^1];
            }
        }

        if(this._DrzwiOtwarte.Contains(hash[3]))
        {
            nowaPozycja = pozycja with { X = pozycja.X + 2 };
            if (this.SprawdzPrzejscie(nowaPozycja))
            {
                this._PrzebytaDroga += 'R';
                if (!this.PunktyOdwiedzone(pozycja, nowaPozycja))
                {
                    this.ZnajdzDroge(this.ObliczHash(this._PrzebytaDroga), nowaPozycja);
                }
                this._PrzebytaDroga = this._PrzebytaDroga[..^1];
            }
        }

        this._OdwiedzonePunkty.RemoveAt(this._OdwiedzonePunkty.Count - 1);
    }

    /// <summary>
    /// Sprawdza czy dwa kolejne punkty były już odwiedzane
    /// </summary>
    /// <param name="obecnaPozycja">Obecnie sprawdzany punkt</param>
    /// <param name="nastepnaPozycja">Kolejny punkt do sprawdzenia</param>
    /// <returns>TRUE jeśli oba punkty są na liście odzwiedzonych punktów</returns>
    private bool PunktyOdwiedzone(Pozycja obecnaPozycja, Pozycja nastepnaPozycja)
    {
        List<(Pozycja p, int ndx)> punkty = this._OdwiedzonePunkty.Select((pozycja, indeks) => (pozycja, indeks)).Where(op => op.pozycja == obecnaPozycja).ToList<(Pozycja p, int ndx)>();
		
		if(punkty.Count == 1 && this._OdwiedzonePunkty.IndexOf(obecnaPozycja) == this._OdwiedzonePunkty.Count - 1)
		{
			return false;
		}
		
		if(punkty.Count > 1)
		{
            for(int i = 0; i < punkty.Count; i++)
            {
                (_, int ndx) = punkty[i];

                if(ndx == this._OdwiedzonePunkty.Count - 1)
                {
                    return false;
                }

                if(ndx + 1 < this._OdwiedzonePunkty.Count && this._OdwiedzonePunkty[ndx + 1].X == nastepnaPozycja.X && this._OdwiedzonePunkty[ndx + 1].Y == nastepnaPozycja.Y)
                {
                    return true;
                }
            }
		}

        return false;
    }

    private bool SprawdzPrzejscie(Pozycja nowyPunkt)
    {
        return nowyPunkt.Y >= 0 && nowyPunkt.Y < this._Mapa.Count && nowyPunkt.X >= 0 && nowyPunkt.X < this._Mapa[0].Length;
    }

    private string ObliczHash(string reszta)
    {
        return string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{this._Hash}{reszta}")).Select(o => o.ToString("x2")))[.. 4];
    }

    private void SprawdzDlugosc()
    {
        Debug.WriteLine($"{this._PrzebytaDroga}: {this._PrzebytaDroga.Length}");
        
        if(this._NajdluzszaDroga < this._PrzebytaDroga.Length)
        {
            this._NajdluzszaDroga = this._PrzebytaDroga.Length;
            Debug.WriteLine("XX");
        }
    }
    
    public string PokazRozwiazanie()
    {
        return "370: " + this._NajdluzszaDroga.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Pozycja(int X, int Y);
}