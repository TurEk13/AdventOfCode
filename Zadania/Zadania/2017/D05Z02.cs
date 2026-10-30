using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2017;

public partial class D05Z02 : IZadanie
{
    private UInt32 _Wynik;
    private int[] _Program;
    
    public D05Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        this._Program = [];

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\05\\proba.txt" : ".\\Dane\\2017\\05\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Program = [..sr.ReadToEnd().Split(Environment.NewLine).Select(l => Convert.ToInt32(l))];

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        int obecnaPozycja = 0, staraPozycja;

        while(obecnaPozycja < this._Program.Length)
        {
            staraPozycja = obecnaPozycja;
            obecnaPozycja += this._Program[obecnaPozycja];

            if(this._Program[staraPozycja] > 2)
            {
                this._Program[staraPozycja]--;
            }
            else
            {
                this._Program[staraPozycja]++;
            }
            
            this._Wynik++;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}