using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public class D03Z01 : IZadanie
{
	private int _Suma;
    private List<string> _Instrukcje;
	
    public D03Z01(bool daneTestowe = false)
    {
        this._Suma = 0;
        this._Instrukcje = new ();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\03\\proba.txt" : ".\\Dane\\2025\\03\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;

        StreamReader sr = new(fs);

        while((linia = sr.ReadLine()) is not null)
        {
            this._Instrukcje.Add(linia);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {		
        for(int i = 0; i < this._Instrukcje.Count; i++)
		{
			this._Suma += this.ZnajdzMaks(this._Instrukcje[i]);
		}
    }
	
	private int ZnajdzMaks(ReadOnlySpan<char> bank)
	{
		int maks = 0, tmpI;
		
		for(int i = 0; i < bank.Length; i++)
		{
			for(int j = i + 1; j < bank.Length; j++)
            {
				if((tmpI = (bank[i] - '0') * 10 + bank[j] - '0') > maks)
                {
                    maks = tmpI;
                }
            }
		}
		
		return maks;
	}

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}