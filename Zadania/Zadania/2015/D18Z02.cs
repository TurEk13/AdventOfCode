using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Zadania._2015;

public class D18Z02 : IZadanie
{
    private Int64 _wlaczoneSwiatla;

    private char[,] _swiatla;
    private char[,] _noweSwiatla;

    public D18Z02(bool daneTestowe = false)
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
        int ileWierszy = this._swiatla.GetLength(0);
        int ileKolumn = this._swiatla.GetLength(1);

        this._swiatla[0, 0] = '#';
        this._swiatla[0, ileKolumn - 1] = '#';
        this._swiatla[ileWierszy - 1, 0] = '#';
        this._swiatla[ileWierszy - 1, ileKolumn - 1] = '#';

        int wartoscPunktu;

        for(int i = 0; i < 100; i++)
        {
            this._noweSwiatla = new char[ileWierszy, ileKolumn];
            
            this.WstawRogi(ileWierszy, ileKolumn);
            this.WstawBoki(ileWierszy, ileKolumn);
            this.WstawKrance(ileWierszy, ileKolumn);

            for(int w = 1; w < ileWierszy - 1; w++)
            {
                for(int k = 1; k < ileKolumn - 1; k++)
                {
                    wartoscPunktu = 0;

                    wartoscPunktu = this.CzyWlaczone(w - 1, k - 1) ? wartoscPunktu + 1 : wartoscPunktu;
                    wartoscPunktu = this.CzyWlaczone(w - 1, k) ? wartoscPunktu + 1 : wartoscPunktu;
                    wartoscPunktu = this.CzyWlaczone(w - 1, k + 1) ? wartoscPunktu + 1 : wartoscPunktu;

                    wartoscPunktu = this.CzyWlaczone(w, k - 1) ? wartoscPunktu + 1 : wartoscPunktu;
                    wartoscPunktu = this.CzyWlaczone(w, k + 1) ? wartoscPunktu + 1 : wartoscPunktu;

                    wartoscPunktu = this.CzyWlaczone(w + 1, k - 1) ? wartoscPunktu + 1 : wartoscPunktu;
                    wartoscPunktu = this.CzyWlaczone(w + 1, k) ? wartoscPunktu + 1 : wartoscPunktu;
                    wartoscPunktu = this.CzyWlaczone(w + 1, k + 1) ? wartoscPunktu + 1 : wartoscPunktu;

                    if (this._swiatla[w, k].Equals('#'))
                    {
                        this._noweSwiatla[w, k] = (wartoscPunktu == 2 || wartoscPunktu == 3) ? '#' : '.';
                    }

                    if (this._swiatla[w, k].Equals('.'))
                    {
                        this._noweSwiatla[w, k] = wartoscPunktu == 3 ? '#' : '.';
                    }
                }
            }

            this._swiatla = this._noweSwiatla;
        }

        for (int w = 0; w < ileWierszy; w++)
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

    private void WstawBoki(int ileWierszy, int ileKolumn)
    {
        int wartoscPunktuL, wartoscPunktuP;

        for(int w = 1; w < ileWierszy - 1; w++)
        {
            wartoscPunktuL = wartoscPunktuP = 0;

            wartoscPunktuL = this.CzyWlaczone(w - 1, 0) ? wartoscPunktuL + 1 : wartoscPunktuL;
            wartoscPunktuL = this.CzyWlaczone(w + 1, 0) ? wartoscPunktuL + 1 : wartoscPunktuL;

            wartoscPunktuL = this.CzyWlaczone(w - 1, 1) ? wartoscPunktuL + 1 : wartoscPunktuL;
            wartoscPunktuL = this.CzyWlaczone(w, 1) ? wartoscPunktuL + 1 : wartoscPunktuL;
            wartoscPunktuL = this.CzyWlaczone(w + 1, 1) ? wartoscPunktuL + 1 : wartoscPunktuL;

            if (this._swiatla[w, 0].Equals('#'))
            {
                this._noweSwiatla[w, 0] = (wartoscPunktuL == 2 || wartoscPunktuL == 3) ? '#' : '.';
            }

            if (this._swiatla[w, 0].Equals('.'))
            {
                this._noweSwiatla[w, 0] = wartoscPunktuL == 3 ? '#' : '.';
            }

            wartoscPunktuP = this.CzyWlaczone(w - 1, ileKolumn - 1) ? wartoscPunktuP + 1 : wartoscPunktuP;
            wartoscPunktuP = this.CzyWlaczone(w + 1, ileKolumn - 1) ? wartoscPunktuP + 1 : wartoscPunktuP;

            wartoscPunktuP = this.CzyWlaczone(w - 1, ileKolumn - 2) ? wartoscPunktuP + 1 : wartoscPunktuP;
            wartoscPunktuP = this.CzyWlaczone(w, ileKolumn - 2) ? wartoscPunktuP + 1 : wartoscPunktuP;
            wartoscPunktuP = this.CzyWlaczone(w + 1, ileKolumn - 2) ? wartoscPunktuP + 1 : wartoscPunktuP;

            if (this._swiatla[w, ileKolumn - 1].Equals('#'))
            {
                this._noweSwiatla[w, ileKolumn - 1] = (wartoscPunktuP == 2 || wartoscPunktuP == 3) ? '#' : '.';
            }

            if (this._swiatla[w, ileKolumn - 1].Equals('.'))
            {
                this._noweSwiatla[w, ileKolumn - 1] = wartoscPunktuP == 3 ? '#' : '.';
            }
        }
    }

    private void WstawKrance(int ileWierszy, int ileKolumn)
    {
        int wartoscPunktuG, wartoscPunktuD;

        for(int k = 1; k < ileKolumn - 1; k++)
        {
            wartoscPunktuG = wartoscPunktuD = 0;

            wartoscPunktuG = this.CzyWlaczone(0, k - 1) ? wartoscPunktuG + 1 : wartoscPunktuG;
            wartoscPunktuG = this.CzyWlaczone(0, k + 1) ? wartoscPunktuG + 1 : wartoscPunktuG;

            wartoscPunktuG = this.CzyWlaczone(1, k - 1) ? wartoscPunktuG + 1 : wartoscPunktuG;
            wartoscPunktuG = this.CzyWlaczone(1, k) ? wartoscPunktuG + 1 : wartoscPunktuG;
            wartoscPunktuG = this.CzyWlaczone(1, k + 1) ? wartoscPunktuG + 1 : wartoscPunktuG;

            if (this._swiatla[0, k].Equals('#'))
            {
                this._noweSwiatla[0, k] = (wartoscPunktuG == 2 || wartoscPunktuG == 3) ? '#' : '.';
            }

            if (this._swiatla[0, k].Equals('.'))
            {
                this._noweSwiatla[0, k] = wartoscPunktuG == 3 ? '#' : '.';
            }

            wartoscPunktuD = this.CzyWlaczone(ileWierszy - 1, k - 1) ? wartoscPunktuD + 1 : wartoscPunktuD;
            wartoscPunktuD = this.CzyWlaczone(ileWierszy - 1, k + 1) ? wartoscPunktuD + 1 : wartoscPunktuD;

            wartoscPunktuD = this.CzyWlaczone(ileWierszy - 2, k - 1) ? wartoscPunktuD + 1 : wartoscPunktuD;
            wartoscPunktuD = this.CzyWlaczone(ileWierszy - 2, k) ? wartoscPunktuD + 1 : wartoscPunktuD;
            wartoscPunktuD = this.CzyWlaczone(ileWierszy - 2, k + 1) ? wartoscPunktuD + 1 : wartoscPunktuD;

            if (this._swiatla[ileWierszy - 1, k].Equals('#'))
            {
                this._noweSwiatla[ileWierszy - 1, k] = (wartoscPunktuD == 2 || wartoscPunktuD == 3) ? '#' : '.';
            }

            if (this._swiatla[ileWierszy - 1, k].Equals('.'))
            {
                this._noweSwiatla[ileWierszy - 1, k] = wartoscPunktuD == 3 ? '#' : '.';
            }
        }
    }

    private void WstawRogi(int ileWierszy, int ileKolumn)
    {
        this._noweSwiatla[0, 0] = '#';
        this._noweSwiatla[0, ileKolumn - 1] = '#';
        this._noweSwiatla[ileWierszy - 1, 0] = '#';
        this._noweSwiatla[ileWierszy - 1, ileKolumn - 1] = '#';
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