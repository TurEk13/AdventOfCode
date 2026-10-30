using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2017;

public partial class D04Z01 : IZadanie
{
    private UInt32 _Wynik;
    private List<string[]> _Passphrase;
    
    public D04Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Passphrase = new List<string[]>();

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\04\\proba.txt" : ".\\Dane\\2017\\04\\dane.txt", FileMode.Open, FileAccess.Read));

        string[] tmp = sr.ReadToEnd().Split(Environment.NewLine);

        sr.Close();

        foreach(string s in tmp)
        {
            this._Passphrase.Add([.. s.Split(' ')]);
        }
    }

    public void RozwiazanieZadania()
    {
        bool poprawny;
        foreach(string[] wiersz in this._Passphrase)
        {
            poprawny = true;

            for(int i = 0; i < wiersz.Length; i++)
            {
                if(wiersz.Count(wiersz[i]) > 1)
                {
                    poprawny = false;
                    i = wiersz.Length;
                }
            }

            if(poprawny)
            {
                this._Wynik++;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}