using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public class D03Z02 : IZadanie
{
	private Int64 _Suma;
    private List<string> _Instrukcje;
	
    public D03Z02(bool daneTestowe = false)
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
			this._Suma += this.ZnajdzMaksLiczbe(this._Instrukcje[i]);
		}
    }
	
	private Int64 ZnajdzMaksLiczbe(string bank)
	{
        int indeks = -1;
        char[] liczba = new char[12];
        
        for(int i = 0; i < 12; i++)
        {
            indeks = this.ZnajdzMaksCyfre(bank, i, indeks + 1);
            liczba[i] = bank[indeks];
        }
		
		return Convert.ToInt64(new string(liczba));
	}

    private Int32 ZnajdzMaksCyfre(ReadOnlySpan<char> bank, int poziom, int indeks)
    {
        int start = indeks > poziom ? indeks : poziom;
        int maks = start, stare = bank[maks] - '0', nowe;

        for(int i = start; i < bank.Length - (11 - poziom); i++)
        {
            nowe = bank[i] - '0';
            if(nowe > stare)
            {
                maks = i;
                stare = bank[maks] - '0';
            }
        }

        return maks;
    }

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}