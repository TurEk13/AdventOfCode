using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public class D08Z01 : IZadanie
{
    private Int64 _Wynik;
	private char[,] _Lampki;
    private List<string> _Instrukcje;

    public D08Z01(bool daneTestowe = false)
    {
        this._Instrukcje = new();
		//this._Lampki = new char[7, 4];
		this._Lampki = new char[6, 50];
        this._Wynik = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\08\\proba.txt" : ".\\Dane\\2016\\08\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);
        string wiersz;

        while((wiersz = sr.ReadLine()) is not null)
        {
            this._Instrukcje.Add(wiersz);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        char[,] Zmiana;
		string[] obecnaInstrukcja;
		int szerokosc, wysokosc, przesuwanaKolumna, docelowaKolumna, przesuwanyWiersz, docelowyWiersz, przesuniecie;

		for(int w = 0; w < this._Lampki.GetLength(0); w++)
		{
			for(int sz = 0; sz < this._Lampki.GetLength(1); sz++)
			{
				this._Lampki[w, sz] = '.';
			}
		}
		
		foreach(string s in this._Instrukcje)
		{
			Zmiana = (char[,])this._Lampki.Clone();
			obecnaInstrukcja = s.Split(' ');

			if(s.Equals("rotate row y=2 by 8"))
			{
				Debug.WriteLine("D");
			}
			
			switch(obecnaInstrukcja[0])
			{
				case "rect":
					szerokosc = Convert.ToInt32(obecnaInstrukcja[1][..obecnaInstrukcja[1].IndexOf('x')]);
					wysokosc = Convert.ToInt32(obecnaInstrukcja[1][(obecnaInstrukcja[1].IndexOf('x') + 1)..]);
					
					for(int w = 0; w < wysokosc; w++)
					{
						for(int sz = 0; sz < szerokosc; sz++)
						{
							Zmiana[w, sz] = '#';
						}
					}
					break;
				case "rotate":
					switch(obecnaInstrukcja[1])
					{
						case "column":
							przesuwanaKolumna = Convert.ToInt32(obecnaInstrukcja[2][(obecnaInstrukcja[2].IndexOf('=') + 1)..]);
							przesuniecie = Convert.ToInt32(obecnaInstrukcja[4]);

							for(int i = 0; i < this._Lampki.GetLength(0); i++)
							{
								docelowyWiersz = (i + przesuniecie) % this._Lampki.GetLength(0);
								Zmiana[docelowyWiersz, przesuwanaKolumna] = this._Lampki[i, przesuwanaKolumna];
							}
							break;
						case "row": // >
							przesuwanyWiersz = Convert.ToInt32(obecnaInstrukcja[2][(obecnaInstrukcja[2].IndexOf('=') + 1)..]);
							przesuniecie = Convert.ToInt32(obecnaInstrukcja[4]);

							for(int i = 0; i < this._Lampki.GetLength(1); i++)
							{
								docelowaKolumna = (i + przesuniecie) % this._Lampki.GetLength(1);
								Zmiana[przesuwanyWiersz, docelowaKolumna] = this._Lampki[przesuwanyWiersz, i];
							}
							break;
					}
					break;
			}
			
			this._Lampki = (char[,])Zmiana.Clone();
		}

		for(int w = 0; w < this._Lampki.GetLength(0); w++)
		{
			for(int sz = 0; sz < this._Lampki.GetLength(1); sz++)
			{
				if(this._Lampki[w, sz].Equals('#'))
				{
					this._Wynik++;
				}
			}
		}
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}