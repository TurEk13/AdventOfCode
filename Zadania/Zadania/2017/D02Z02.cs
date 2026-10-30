using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D02Z02 : IZadanie
{
    private UInt32 _Wynik;
    private List<UInt32[]> _Arkusz;

    [GeneratedRegex(@"(\d){1,}")]
    private static partial Regex WzorLiczb();
    
    public D02Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Arkusz = [];
        Regex Liczby = WzorLiczb();
        MatchCollection mc;

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\02\\proba.txt" : ".\\Dane\\2017\\02\\dane.txt", FileMode.Open, FileAccess.Read));

        string[] tmp = sr.ReadToEnd().Split(Environment.NewLine);

        sr.Close();

        foreach(string s in tmp)
        {
            mc = Liczby.Matches(s);
            this._Arkusz.Add([.. mc.Select(l => Convert.ToUInt32(l.Value))]);
        }
    }

    public void RozwiazanieZadania()
    {
        foreach(UInt32[] l in this._Arkusz)
        {
            for(int i = 0; i < l.Length; i++)
            {
                for(int j = i + 1; j < l.Length; j++)
                {                    
                    if(l[i] % l[j] == 0)
                    {
                        this._Wynik += l[i] / l[j];
                        j = l.Length;
                        i = l.Length;
                        continue;
                    }

                    if(l[j] % l[i] == 0)
                    {
                        this._Wynik += l[j] / l[i];
                        j = l.Length;
                        i = l.Length;
                    }
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}