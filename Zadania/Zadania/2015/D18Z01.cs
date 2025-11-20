using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Zadania._2015;

public class D18Z01 : IZadanie
{
    private Int64 _wlaczoneSwiatla;

    private char[,] _swiatla;
    private char[,] _noweSwiatla;

    public D18Z01(bool daneTestowe = false)
    {
        this._wlaczoneSwiatla = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\18\\proba.txt" : ".\\Dane\\2015\\18\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        List<string> mapa = new ();
        string linia;
        int ileWierszy = 0;
        
        while((linia = sr.ReadLine()) is not null)
        {
            mapa.Add(linia);
            ileWierszy++;
        }

        this._swiatla = new char[ileWierszy, mapa[0].Length];

        for(int w  = 0; w < ileWierszy; w++)
        {
            for(int k = 0; k < mapa[0].Length; k++)
            {
                this._swiatla[w, k] = mapa[w][k];
            }
        }
    }

    public void RozwiazanieZadania()
    {
        //909+
        StringBuilder sb = new();
        int ileWierszy = this._swiatla.GetLength(0);
        int ileKolumn = this._swiatla.GetLength(1);
        

        int punkt;

        for(int i = 0; i < 100; i++)
        {
            this._noweSwiatla = new char[ileWierszy, ileKolumn];
            sb.AppendLine($"\r\n{i + 1}:");

            this.Rogi(ileWierszy, ileKolumn);
            this.PierwszyOstatniWiersz(ileWierszy, ileKolumn);
            this.PierwszaOstatniaKolumna(ileWierszy, ileKolumn);

            for(int w = 1; w < ileWierszy - 1; w++)
            {
                for(int k = 1; k < ileKolumn - 1; k++)
                {
                    punkt = 0;
                    if(this._swiatla[w, k].Equals('#'))
                    {
                        punkt = this.CzyWlaczone(w - 1, k - 1) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w - 1, k) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w - 1, k + 1) ? punkt + 1 : punkt;

                        punkt = this.CzyWlaczone(w, k - 1) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w, k + 1) ? punkt + 1 : punkt;
                        
                        punkt = this.CzyWlaczone(w + 1, k - 1) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w + 1, k) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w + 1, k + 1) ? punkt + 1 : punkt;

                        this._noweSwiatla[w, k] = punkt == 2 || punkt == 3 ? '#' : '.';
                    }

                    punkt = 0;
                    if(this._swiatla[w, k].Equals('.'))
                    {
                        punkt = this.CzyWlaczone(w - 1, k - 1) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w - 1, k) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w - 1, k + 1) ? punkt + 1 : punkt;

                        punkt = this.CzyWlaczone(w, k - 1) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w, k + 1) ? punkt + 1 : punkt;
                        
                        punkt = this.CzyWlaczone(w + 1, k - 1) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w + 1, k) ? punkt + 1 : punkt;
                        punkt = this.CzyWlaczone(w + 1, k + 1) ? punkt + 1 : punkt;

                        this._noweSwiatla[w, k] = punkt == 3 ? '#' : '.';
                    }
                }
            }

            for(int w = 0; w < ileWierszy; w++)
            {
                for(int k = 0; k < ileKolumn; k++)
                {
                    sb.Append(this._noweSwiatla[w, k]);
                }
                sb.AppendLine();
            }

            this._swiatla = this._noweSwiatla;

            File.AppendAllText("181.txt", sb.ToString());
            sb.Clear();
        }

        for(int w = 0; w < ileWierszy; w++)
        {
            for(int k = 0; k < ileKolumn; k++)
            {
                if(this._swiatla[w, k].Equals('#'))
                {
                    this._wlaczoneSwiatla++;
                }
            }
        }
    }

    private void Rogi(int ileWierszy, int ileKolumn)
    {
        int punkt = 0;
        // Włączone
        // Górne rogi
        // Lewo góra
        if(this._swiatla[0, 0].Equals('#'))
        {
            punkt = this.CzyWlaczone(0, 1) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(1, 1) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(1, 0) ? punkt + 1 : punkt;

            this._noweSwiatla[0, 0] = punkt == 2 || punkt == 3 ? '#' : '.';
        }

        punkt = 0;
        // Prawo góra
        if(this._swiatla[0, ileKolumn - 1].Equals('#'))
        {
            punkt = this.CzyWlaczone(0, ileKolumn - 2) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(1, ileKolumn - 2) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(1, ileKolumn - 1) ? punkt + 1 : punkt;

            this._noweSwiatla[0, ileKolumn - 1] = punkt == 2 || punkt == 3 ? '#' : '.';
        }

        // Wyłączone
        // Górne rogi
        // Lewo góra
        if(this._swiatla[0, 0].Equals('.'))
        {
            punkt = this.CzyWlaczone(0, 1) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(1, 1) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(1, 0) ? punkt + 1 : punkt;

            this._noweSwiatla[0, 0] = punkt == 3 ? '#' : '.';
        }

        punkt = 0;
        // Prawo góra
        if(this._swiatla[0, ileKolumn - 1].Equals('.'))
        {
            punkt = this.CzyWlaczone(0, ileKolumn - 2) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(1, ileKolumn - 2) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(1, ileKolumn - 1) ? punkt + 1 : punkt;

            this._noweSwiatla[0, ileKolumn - 1] = punkt == 3 ? '#' : '.';
        }

        punkt = 0;
        // Włączone
        // Dolne rogi
        // Lewo dół
        if(this._swiatla[ileWierszy - 1, 0].Equals('#'))
        {
            punkt = this.CzyWlaczone(ileWierszy - 2, 0) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(ileWierszy - 2, 1) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(ileWierszy - 1, 1) ? punkt + 1 : punkt;
            
            this._noweSwiatla[ileWierszy - 1, 0] = punkt == 2 || punkt == 3 ? '#' : '.';
        }

        punkt = 0;
        // Prawo dół
        if(this._swiatla[ileWierszy - 1, ileKolumn - 1].Equals('#'))
        {
            punkt = this.CzyWlaczone(ileWierszy - 2, ileKolumn - 2) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(ileWierszy - 2, ileKolumn - 1) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(ileWierszy - 1, ileKolumn - 2) ? punkt + 1 : punkt;

            this._noweSwiatla[ileWierszy - 1, ileKolumn - 1] = punkt == 2 || punkt == 3 ? '#' : '.';
        }

        // Wyłączone
        // Dolne rogi
        // Lewo dół
        if(this._swiatla[ileWierszy - 1, 0].Equals('.'))
        {
            punkt = this.CzyWlaczone(ileWierszy - 2, 0) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(ileWierszy - 2, 1) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(ileWierszy - 1, 1) ? punkt + 1 : punkt;

            this._noweSwiatla[ileWierszy - 1, 0] = punkt == 3 ? '#' : '.';
        }

        punkt = 0;
        // Prawo dół
        if(this._swiatla[ileWierszy - 1, ileKolumn - 1].Equals('.'))
        {
            punkt = this.CzyWlaczone(ileWierszy - 2, ileKolumn - 2) ? punkt + 1 : punkt;
            punkt = this.CzyWlaczone(ileWierszy - 2, ileKolumn - 1) ? punkt + 1 : punkt;

            punkt = this.CzyWlaczone(ileWierszy - 1, ileKolumn - 2) ? punkt + 1 : punkt;

            this._noweSwiatla[ileWierszy - 1, ileKolumn - 1] = punkt == 3 ? '#' : '.';
        }
    }

    private void PierwszyOstatniWiersz(int ileWierszy, int ileKolumn)
    {
        int punkt;

        for(int k = 1; k < ileKolumn - 1; k++)
        {
            punkt = 0;
            if(this._swiatla[0, k].Equals('#'))
            {
                punkt = this.CzyWlaczone(0, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(0, k + 1) ? punkt + 1 : punkt;

                punkt = this.CzyWlaczone(1, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(1, k) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(1, k + 1) ? punkt + 1 : punkt;

                this._noweSwiatla[0, k] = punkt == 2 || punkt == 3 ? '#' : '.';
            }

            punkt = 0;
            if(this._swiatla[0, k].Equals('.'))
            {
                punkt = this.CzyWlaczone(0, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(0, k + 1) ? punkt + 1 : punkt;

                punkt = this.CzyWlaczone(1, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(1, k) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(1, k + 1) ? punkt + 1 : punkt;

                this._noweSwiatla[0, k] = punkt == 3 ? '#' : '.';
            }

            punkt = 0;
            if(this._swiatla[ileWierszy - 1, k].Equals('#'))
            {
                punkt = this.CzyWlaczone(ileWierszy - 2, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(ileWierszy - 2, k) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(ileWierszy - 2, k + 1) ? punkt + 1 : punkt;

                punkt = this.CzyWlaczone(ileWierszy - 1, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(ileWierszy - 1, k + 1) ? punkt + 1 : punkt;

                this._noweSwiatla[ileWierszy - 1, k] = punkt == 2 || punkt == 3 ? '#' : '.';
            }

            punkt = 0;
            if(this._swiatla[ileWierszy - 1, k].Equals('.'))
            {
                punkt = this.CzyWlaczone(ileWierszy - 2, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(ileWierszy - 2, k) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(ileWierszy - 2, k + 1) ? punkt + 1 : punkt;

                punkt = this.CzyWlaczone(ileWierszy - 1, k - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(ileWierszy - 1, k + 1) ? punkt + 1 : punkt;

                this._noweSwiatla[ileWierszy - 1, k] = punkt == 3 ? '#' : '.';
            }
        }
    }

    private void PierwszaOstatniaKolumna(int ileWierszy, int ileKolumn)
    {
        int punkt;

        for(int w = 1; w < ileWierszy - 1; w++)
        {
            punkt = 0;
            if(this._swiatla[w, 0].Equals('#'))
            {
                punkt = this.CzyWlaczone(w - 1, 0) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, 0) ? punkt + 1 : punkt;

                punkt = this.CzyWlaczone(w - 1, 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w, 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, 1) ? punkt + 1 : punkt;

                this._noweSwiatla[w, 0] = punkt == 2 || punkt == 3 ? '#' : '.';
            }

            punkt = 0;
            if(this._swiatla[w, 0].Equals('.'))
            {
                punkt = this.CzyWlaczone(w - 1, 0) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, 0) ? punkt + 1 : punkt;
                
                punkt = this.CzyWlaczone(w - 1, 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w, 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, 1) ? punkt + 1 : punkt;

                this._noweSwiatla[w, 0] = punkt == 3 ? '#' : '.';
            }

            punkt = 0;
            if(this._swiatla[w, ileKolumn - 1].Equals('#'))
            {
                punkt = this.CzyWlaczone(w - 1, ileKolumn - 2) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w, ileKolumn - 2) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, ileKolumn - 2) ? punkt + 1 : punkt;

                punkt = this.CzyWlaczone(w - 1, ileKolumn - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, ileKolumn - 1) ? punkt + 1 : punkt;

                this._noweSwiatla[w, ileKolumn - 1] = punkt == 2 || punkt == 3 ? '#' : '.';
            }

            punkt = 0;
            if(this._swiatla[w, ileKolumn - 1].Equals('.'))
            {
                punkt = this.CzyWlaczone(w - 1, ileKolumn - 2) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w, ileKolumn - 2) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, ileKolumn - 2) ? punkt + 1 : punkt;
                
                punkt = this.CzyWlaczone(w - 1, ileKolumn - 1) ? punkt + 1 : punkt;
                punkt = this.CzyWlaczone(w + 1, ileKolumn - 1) ? punkt + 1 : punkt;

                this._noweSwiatla[w, ileKolumn - 1] = punkt == 3 ? '#' : '.';
            }
        }
    }

    private bool CzyWlaczone(int wiersz, int kolumna)
    {
        return this._swiatla[wiersz, kolumna].Equals('#');
    }

    public string PokazRozwiazanie()
    {
        return this._wlaczoneSwiatla.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}