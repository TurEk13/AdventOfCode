using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public class D19Z02 : IZadanie
{
    private List<string> _Klucze;
    private List<string> _Wartosci;
    private int _Wynik;
    private string _MolekulaDocelowa;

    public D19Z02(bool daneTestowe = false)
    {
        this._Klucze = new ();
        this._Wartosci = new ();
        this._Wynik = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\19\\proba.txt" : ".\\Dane\\2015\\19\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] liniaA;
        
        while((linia = sr.ReadLine()) != string.Empty)
        {
            liniaA = [.. linia.Split("=>").Select(l => l.Trim())];
            this._Klucze.Add(liniaA[0]);
            this._Wartosci.Add(liniaA[1]);
        }

        this._MolekulaDocelowa = sr.ReadToEnd().Trim();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        Regex r;

        while (!this._MolekulaDocelowa.Equals("e"))
        {
            for (int i = 0; i < this._Klucze.Count; i++)
            {
                if (this._MolekulaDocelowa.Contains(this._Wartosci[i]))
                {
                    r = new Regex(this._Wartosci[i]);
                    this._MolekulaDocelowa = r.Replace(this._MolekulaDocelowa, this._Klucze[i], 1);
                    this._Wynik++;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}