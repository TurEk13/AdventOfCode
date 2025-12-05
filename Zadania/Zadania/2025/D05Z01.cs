using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public class D05Z01 : IZadanie
{
    private List<Przedzial> _DobreSkladniki;
    private List<UInt64> _ListaSkladnikow;
    private UInt64 _IleSkladnikow;

    public D05Z01(bool daneTestowe = false)
    {
        this._DobreSkladniki = new ();
        this._ListaSkladnikow = new ();
        this._IleSkladnikow = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\05\\proba.txt" : ".\\Dane\\2025\\05\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;
        string[] liniaT;

        StreamReader sr = new(fs);

        while((linia = sr.ReadLine()) != string.Empty)
        {
            liniaT = linia.Split('-');
            this._DobreSkladniki.Add(new Przedzial(Convert.ToUInt64(liniaT[0]), Convert.ToUInt64(liniaT[1])));
        }

        while((linia = sr.ReadLine()) is not null)
        {
            this._ListaSkladnikow.Add(Convert.ToUInt64(linia));
        }

        sr.Close(); fs!.Close();

        this._DobreSkladniki = this._DobreSkladniki.OrderBy(ds => ds.Min).ToList<Przedzial>();
    }

    public void RozwiazanieZadania()
    {
        foreach(UInt64 s in this._ListaSkladnikow)
        {
            for(int i = 0; i < this._DobreSkladniki.Count; i++)
            {
                if(this._DobreSkladniki[i].Min <= s && s <= this._DobreSkladniki[i].Maks)
                {
                    this._IleSkladnikow++;
                    i = this._DobreSkladniki.Count;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._IleSkladnikow.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Przedzial(UInt64 Min, UInt64 Maks);
}