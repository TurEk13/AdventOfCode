using System;
using System.Globalization;
using System.IO;

namespace Zadania._2024;

public class D09Z01 : IZadanie
{
    private string tablicaDysku;
    private int[] wielkoscPlikow;
    private int[] dysk;
	private Int128 sumaKontrolna;

    public D09Z01(bool daneTestowe = false)
    {
        this.tablicaDysku = File.ReadAllText(daneTestowe ? ".\\Dane\\2024\\09\\proba.txt" : ".\\Dane\\2024\\09\\dane.txt");
    }

    public void RozwiazanieZadania()
    {
        int nazwaPliku = 0, wielkoscDysku = 0, pozycja = 0, wskaznik = 0;
        bool CzyPlik = true;

        this.wielkoscPlikow = new int[this.tablicaDysku.Length];

        foreach (char c in this.tablicaDysku)
        {
            this.wielkoscPlikow[wskaznik] = c - '0';
            wielkoscDysku += this.wielkoscPlikow[wskaznik];
            wskaznik++;
        }

        this.dysk = new int[wielkoscDysku];

        for (int i = 0; i < this.wielkoscPlikow.Length; i++)
        {
            if (CzyPlik)
            {
                for (int j = 0; j < this.wielkoscPlikow[i]; j++, pozycja++)
                {
                    this.dysk[pozycja] = nazwaPliku;
                }
                nazwaPliku++;
            }

            if (!CzyPlik)
            {
                for (int j = 0; j < this.wielkoscPlikow[i]; j++, pozycja++)
                {
                    this.dysk[pozycja] = -1;
                }
            }

            CzyPlik = !CzyPlik;
        }

        int Czytaj, Zapisz;

        for (Zapisz = 0, Czytaj = this.dysk.Length - 1; Czytaj > Zapisz;)
        {
            while (this.dysk[Zapisz] != -1)
            {
                Zapisz++;
            }

            while (this.dysk[Czytaj] == -1)
            {
                Czytaj--;
            }

            if (Czytaj < Zapisz)
            {
                break;
            }

            this.dysk[Zapisz] = this.dysk[Czytaj];
            this.dysk[Czytaj] = -1;
        }
		
		this.ObliczSumeKontrolna();
    }

    public string PokazRozwiazanie()
    {
        return this.sumaKontrolna.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private void ObliczSumeKontrolna()
    {
        this.sumaKontrolna = 0;

        for (int i = 0; this.dysk[i] != -1; i++)
        {
            this.sumaKontrolna += i * this.dysk[i];
        }
    }
}