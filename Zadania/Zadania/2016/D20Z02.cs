using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D20Z02 : IZadanie
{
    private UInt32 _Suma;
    private List<Przedzial> _Przedzialy;
    public D20Z02(bool daneTestowe = false)
    {
        this._Przedzialy = new ();
        this._Suma = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\20\\proba.txt" : ".\\Dane\\2016\\20\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;
        
        while((linia = sr.ReadLine()) is not null)
        {
            this._Przedzialy.Add(new (Convert.ToUInt32(linia[.. linia.IndexOf('-')]), Convert.ToUInt32(linia[(linia.IndexOf('-') + 1) ..])));
        }

        sr.Close(); fs!.Close();
        this._Przedzialy = this._Przedzialy.OrderBy(p => p.Poczatek).ToList<Przedzial>();
    }

    public void RozwiazanieZadania()
    {
        bool jestPrzedzial;
        int indexId;

        for (UInt64 adresIp = 0; adresIp < UInt32.MaxValue; adresIp++)
        {
            jestPrzedzial = false;
            for (indexId = 0; indexId < this._Przedzialy.Count && !jestPrzedzial; indexId++)
            {
                if (adresIp >= this._Przedzialy[indexId].Poczatek && adresIp <= this._Przedzialy[indexId].Koniec)
                {
                    jestPrzedzial = true;
                    adresIp = this._Przedzialy[indexId].Koniec;
                }
            }
            
            if (!jestPrzedzial)
            {
                this._Suma++;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Przedzial(UInt32 Poczatek, UInt32 Koniec);
}