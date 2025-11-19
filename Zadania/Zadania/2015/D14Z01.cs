using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D14Z01 : IZadanie
{
    private Dictionary<string, (int Predkosc, int CzasLotu, int CzasPrzerwy)> _renifery;
    private List<String> _imiona;
    private Int64 _maksymalnapPrzebytaDroga;

    public D14Z01(bool daneTestowe = false)
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
        Int64[] przebytaDroga = new Int64[this._imiona.Count];
        int czasWyscigu = 2503;
        Int64 iloscCykli, iloscPozostalegoCzasu;

        for(int i = 0; i < przebytaDroga.Length; i++)
        {
            iloscCykli = czasWyscigu / (this._renifery[this._imiona[i]].CzasLotu + this._renifery[this._imiona[i]].CzasPrzerwy);

            iloscPozostalegoCzasu = czasWyscigu - iloscCykli * (this._renifery[this._imiona[i]].CzasLotu + this._renifery[this._imiona[i]].CzasPrzerwy);

            przebytaDroga[i] = iloscCykli * (this._renifery[this._imiona[i]].Predkosc * this._renifery[this._imiona[i]].CzasLotu);
            
            if(iloscPozostalegoCzasu <= this._renifery[this._imiona[i]].CzasLotu)
            {
                przebytaDroga[i] += iloscPozostalegoCzasu * this._renifery[this._imiona[i]].Predkosc;
            }

            if(iloscPozostalegoCzasu > this._renifery[this._imiona[i]].CzasLotu)
            {
                przebytaDroga[i] += this._renifery[this._imiona[i]].CzasLotu * this._renifery[this._imiona[i]].Predkosc;
            }
        }

        this._maksymalnapPrzebytaDroga = przebytaDroga.Max();

        //1120 1056
    }

    public string PokazRozwiazanie()
    {
        return this._maksymalnapPrzebytaDroga.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}