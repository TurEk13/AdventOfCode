using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D16Z02 : IZadanie
{
    private char[] _SumaKontrolna;
    private int _WielkoscDysku;
    private int _ObecnaDlugosc;

    public D16Z02(bool daneTestowe = false)
    {
        this._WielkoscDysku = 35_651_584;
        this._SumaKontrolna = new char[this._WielkoscDysku];

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\16\\proba.txt" : ".\\Dane\\2016\\16\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;

		StreamReader sr = new(fs);
        linia = sr.ReadToEnd();

        sr.Close(); fs!.Close();

        this._ObecnaDlugosc = linia.Length;
        linia.ToCharArray().CopyTo(this._SumaKontrolna, 0);
    }

    public void RozwiazanieZadania()
    {
        this.ZnajdzCiag();
        this.ObliczSumeKontrolna();
    }

    private void ZnajdzCiag()
    {
        char[] tmp;
        while(this._ObecnaDlugosc <= this._WielkoscDysku)
        {
            this._SumaKontrolna[this._ObecnaDlugosc] = '0';
            tmp = this._SumaKontrolna.Take(this.
            _ObecnaDlugosc).Reverse().ToArray();

            for(int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = tmp[i].Equals('1') ? '0' : '1';
            }

            if(this._ObecnaDlugosc * 2 + 1 > this._WielkoscDysku)
            {
                foreach(char c in tmp)
                {
                    this._SumaKontrolna[this._ObecnaDlugosc + 1] = c;
                    this._ObecnaDlugosc++;
                    if(this._ObecnaDlugosc == this._WielkoscDysku - 1)
                    {
                        this._ObecnaDlugosc = this._WielkoscDysku + 1;
                        break;
                    }
                }
            }

            if(this._ObecnaDlugosc * 2 + 1 <= this._WielkoscDysku)
            {
                tmp.CopyTo(this._SumaKontrolna, this._ObecnaDlugosc + 1);
                this._ObecnaDlugosc = this._ObecnaDlugosc * 2 + 1;
            }
        }
    }

    private void ObliczSumeKontrolna()
    {
        List<char> tmp = new ();
        
        while(this._SumaKontrolna.Length % 2 != 1)
        {
            for(int i = 0; i < this._SumaKontrolna.Length; i += 2)
            {
                tmp.Add(this.SprawdzPare(i));
            }

            this._SumaKontrolna = (char[])tmp.ToArray().Clone();
            tmp.Clear();
        }
    }

    private char SprawdzPare(int i)
    {
        if(this._SumaKontrolna[i].Equals(this._SumaKontrolna[i + 1]))
        {
            return '1';
        }

        return '0';
    }

    public string PokazRozwiazanie()
    {
        return new string(this._SumaKontrolna);
    }
}