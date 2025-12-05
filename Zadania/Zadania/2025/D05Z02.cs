using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public class D05Z02 : IZadanie
{
    private List<Przedzial> _DobreSkladniki;
    private UInt64 _WszystkieDobreSkladniki;

    public D05Z02(bool daneTestowe = false)
    {
        this._DobreSkladniki = new ();
        this._WszystkieDobreSkladniki = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\05\\proba.txt" : ".\\Dane\\2025\\05\\dane.txt", FileMode.Open, FileAccess.Read);
        string linia;
        string[] liniaT;

        StreamReader sr = new(fs);

        while((linia = sr.ReadLine()) != string.Empty)
        {
            liniaT = linia.Split('-');
            this._DobreSkladniki.Add(new Przedzial(Convert.ToUInt64(liniaT[0]), Convert.ToUInt64(liniaT[1])));
        }

        sr.Close(); fs!.Close();

        this._DobreSkladniki = this._DobreSkladniki.OrderBy(ds => ds.Min).ToList<Przedzial>();
    }

    public void RozwiazanieZadania()
    {
        List<Przedzial> calosc = [this._DobreSkladniki[0]];

        foreach(Przedzial p in this._DobreSkladniki)
        {
            for(int j = 0; j < calosc.Count; j++)
            {
                if(p.Min < calosc[j].Min && p.Maks == calosc[j].Min)
                {
                    calosc.Add(new (p.Min, calosc[j].Maks));
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }

                if(p.Min == calosc[j].Maks && p.Maks > calosc[j].Maks)
                {
                    calosc.Add(new (calosc[j].Min, p.Maks));
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }
                
                if((p.Min == calosc[j].Min && p.Maks < calosc[j].Maks) || (p.Min > calosc[j].Min && p.Maks == calosc[j].Maks))
                {
                    j = calosc.Count;
                    continue;
                }

                if(p.Min < calosc[j].Min && p.Maks < calosc[j].Maks && p.Maks > calosc[j].Min)
                {
                    calosc.Add(new (p.Min, calosc[j].Maks));
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }

                if(p.Min > calosc[j].Min && p.Maks > calosc[j].Maks && p.Min < calosc[j].Maks)
                {
                    calosc.Add(new (calosc[j].Min, p.Maks));
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }
                
                if(p.Min >= calosc[j].Min && p.Maks <= calosc[j].Maks)
                {
                    j = calosc.Count;
                    continue;
                }

                if(p.Maks > calosc[j].Maks && p.Min == calosc[j].Min)
                {
                    calosc.Add(p);
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }

                if(p.Min < calosc[j].Min && p.Maks == calosc[j].Maks)
                {
                    calosc.Add(p);
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }

                if(j == calosc.Count - 1 && (p.Maks < calosc[j].Min || p.Min > calosc[j].Min))
                {
                    calosc.Add(p);
                    j = calosc.Count;
                    continue;
                }

                if(p.Min < calosc[j].Min && p.Maks > calosc[j].Maks)
                {
                    calosc.Add(p);
                    calosc.RemoveAt(j);
                    j = calosc.Count;
                    continue;
                }

                if((p.Min == p.Maks) && (p.Min == calosc[j].Min || p.Maks == calosc[j].Maks))
                {
                    j = calosc.Count;
                    continue;
                }
            }
        }

        foreach(Przedzial p in calosc)
        {
            this._WszystkieDobreSkladniki = this._WszystkieDobreSkladniki + (p.Maks - p.Min) + 1;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._WszystkieDobreSkladniki.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Przedzial(UInt64 Min, UInt64 Maks);
}