using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D20Z02 : IZadanie
{
    private UInt32 _Adres;
    private UInt32 _Suma;
    private List<Przedzial> _Przedzialy;
    public D20Z02(bool daneTestowe = false)
    {
        this._Przedzialy = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\20\\proba.txt" : ".\\Dane\\2016\\20\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;
        
        while((linia = sr.ReadLine()) is not null)
        {
            this._Przedzialy.Add(new (Convert.ToUInt32(linia[.. linia.IndexOf('-')]), Convert.ToUInt32(linia[(linia.IndexOf('-') + 1) ..])));
        }

        sr.Close(); fs!.Close();
        this._Przedzialy = this._Przedzialy.OrderBy(p => p.Poczatek).OrderBy(p=> p.Koniec).ToList<Przedzial>();
    }

    public void RozwiazanieZadania()
    {
        // 875 832 870 +
        this._Adres = this._Przedzialy[0].Koniec + 1;
        this._Suma = 0;

        for(int i = 1; i < this._Przedzialy.Count; i++)
        {
            if(this._Adres > this._Przedzialy[i - 1].Koniec && this._Adres < this._Przedzialy[i].Poczatek)
            {
                this._Suma += this._Przedzialy[i].Poczatek - this._Przedzialy[i - 1].Koniec - 1;
            }

            this._Adres = this._Przedzialy[i].Koniec + 1;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Przedzial(UInt32 Poczatek, UInt32 Koniec);
}