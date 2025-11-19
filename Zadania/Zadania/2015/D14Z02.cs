using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D14Z02 : IZadanie
{
    private Dictionary<string, (int Predkosc, int CzasLotu, int CzasPrzerwy)> _renifery;
    private List<String> _imiona;
    private Int64 _maksymalnaIloscPunktow;

    public D14Z02(bool daneTestowe = false)
    {
        this._renifery = new Dictionary<string, (int Predkosc, int CzasLotu, int CzasPrzerwy)>();
        this._imiona = new ();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\14\\proba.txt" : ".\\Dane\\2015\\14\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] liniaa;

        while((linia = sr.ReadLine()) is not null)
        {
            liniaa = linia.Split(' ');
            this._renifery.Add(liniaa[0], new(Convert.ToInt32(liniaa[3]), Convert.ToInt32(liniaa[6]), Convert.ToInt32(liniaa[^2])));
            this._imiona.Add(liniaa[0]);
        }
    }

    public void RozwiazanieZadania()
    {
        Int64 przebytaDroga;
        int czasWyscigu = 2503;
        List<int> prowadzaceRenifery = new ();
        Int64 iloscCykli, iloscPozostalegoCzasu;
        Int64 najwiekszyDystans;
        Int64[] punktacja = new Int64[this._imiona.Count];

        for(int t = 1; t <= czasWyscigu; t++)
        {
            najwiekszyDystans = 0;

            for(int i = 0; i < this._imiona.Count; i++)
            {
                // Obliczenie przebytej drogi po t czasu
                iloscCykli = t / (this._renifery[this._imiona[i]].CzasLotu + this._renifery[this._imiona[i]].CzasPrzerwy);

                iloscPozostalegoCzasu = t - iloscCykli * (this._renifery[this._imiona[i]].CzasLotu + this._renifery[this._imiona[i]].CzasPrzerwy);

                przebytaDroga = iloscCykli * (this._renifery[this._imiona[i]].Predkosc * this._renifery[this._imiona[i]].CzasLotu);
                
                if(iloscPozostalegoCzasu <= this._renifery[this._imiona[i]].CzasLotu)
                {
                    przebytaDroga += iloscPozostalegoCzasu * this._renifery[this._imiona[i]].Predkosc;
                }

                if(iloscPozostalegoCzasu > this._renifery[this._imiona[i]].CzasLotu)
                {
                    przebytaDroga += this._renifery[this._imiona[i]].CzasLotu * this._renifery[this._imiona[i]].Predkosc;
                }

                // Sprawdzenie największego przybytego dystansu w dotychczasowym okresie
                if(przebytaDroga != 0 && przebytaDroga > najwiekszyDystans)
                {
                    prowadzaceRenifery.Clear();
                    najwiekszyDystans = przebytaDroga;
                }

                if(przebytaDroga != 0 && przebytaDroga == najwiekszyDystans)
                {
                    prowadzaceRenifery.Add(i);
                }
            }

            for(int p = 0; p < prowadzaceRenifery.Count; p++)
            {
                punktacja[prowadzaceRenifery[p]]++;
            }
        }

        this._maksymalnaIloscPunktow = punktacja.Max();

        //312 689
    }

    public string PokazRozwiazanie()
    {
        return this._maksymalnaIloscPunktow.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}