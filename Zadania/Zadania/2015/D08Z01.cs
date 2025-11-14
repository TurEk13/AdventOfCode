using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D08Z01 : IZadanie
{
    private List<string> _wiersz;
	private (Int64,Int64,Int64) _liczby;

    public D08Z01(bool daneTestowe = false)
    {
        this._wiersz = new();
        
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\08\\proba.txt" : ".\\Dane\\2015\\08\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while((linia = sr.ReadLine()) is not null)
        {
            this._wiersz.Add(linia);
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
		this._liczby = new();
		int dlugoscSlowa;

		for(int i = 0; i < this._wiersz.Count; i++)
        {
			dlugoscSlowa = 0;

			for(int j = 0; j < this._wiersz[i].Length; j++)
			{
            	switch(this._wiersz[i][j])
                {
                    case '\"':
						break;
					case '\\':
						j = this._wiersz[i][j+1] == 'x' ? j += 3 : j += 1;
						dlugoscSlowa++;
						break;
					default:
						dlugoscSlowa++;
						break;
                }
			}

			this._liczby.Item1 += this._wiersz[i].Length;
			this._liczby.Item2 += dlugoscSlowa;
        }

		this._liczby.Item3 = this._liczby.Item1 - this._liczby.Item2;
    }
	
    public string PokazRozwiazanie()
    {
		return $"\r\nCałkowita dłuść słowa: {(this._liczby.Item1).ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}\r\nDrukowana długość słowa: {(this._liczby.Item2).ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}\r\nRóżnica: {(this._liczby.Item1 - this._liczby.Item2).ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}";
    }
}