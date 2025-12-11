using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D11Z01 : IZadanie
{
    private List<Polaczenie> _Polaczenia;
    private List<Obwod> _Obwody;
    public D11Z01(bool daneTestowe = false)
    {
        this._Polaczenia = new ();
        this._Obwody = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\11\\proba.txt" : ".\\Dane\\2025\\11\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] Koniec;

        while ((linia = sr.ReadLine()) is not null)
        {
            Koniec = linia[5 ..].Split(' ');
            foreach(string s in Koniec)
            {
                this._Polaczenia.Add(new (linia[.. 3], s));
            }
        }
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        foreach(Polaczenie p in this._Polaczenia.FindAll(pl => pl.Poczatek.Equals("you")))
        {
            List<string> ls = [p.Poczatek, p.Koniec];
            this.ZnajdzObwod(new (ls));
        }
    }

    private void ZnajdzObwod(List<string> ls)
    {
        foreach(Polaczenie p in this._Polaczenia.FindAll(pl => pl.Poczatek.Equals(ls[^1])))
        {
            ls.Add(p.Koniec);

            if(p.Koniec.Equals("out"))
            {
                this._Obwody.Add(new (ls));
                return;
            }

            this.ZnajdzObwod(new (ls));
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Obwody.Count().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Polaczenie(string Poczatek, string Koniec);
    private record Obwod(List<string> Stacje);
}