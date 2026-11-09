using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2017;

public partial class D04Z02 : IZadanie
{
    private UInt32 _Wynik;
    private List<string[]> _Passphrase;
    
    public D04Z02(bool daneTestowe = false)
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
        List<List<List<string>>> UporzadkowaneDane = [.. this._Passphrase.Select(a => a.GroupBy(c => c.Length).Select(d => a.Where(p => p.Length == d.Key).ToList<string>().Select(a => new string([.. a.ToCharArray().OrderBy(c => c)])).ToList<string>()).ToList<List<string>>())];

        List<string> tmp;
        bool prawidlowy;

        foreach(List<List<string>> l in UporzadkowaneDane)
        {
            prawidlowy = true;

            foreach(List<string> m in l)
            {
                if(m.Count < 2)
                {
                    continue;
                }

                tmp = [.. m.Distinct()];

                if(m.Count != tmp.Count)
                {
                    prawidlowy = false;
                }
            }

            if(prawidlowy)
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