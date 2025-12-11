using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2025;

public partial class D11Z02 : IZadanie
{
    private List<Polaczenie> _Polaczenia;
    private List<Obwod> _Obwody;
    public D11Z02(bool daneTestowe = false)
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
            Obwod o = new (new ());
            o.Stacje.Add("you");
            o.Stacje.Add(p.Koniec);
            this.ZnajdzObwod(o);
        }
    }

    private void ZnajdzObwod(Obwod o)
    {
        foreach(Polaczenie p in this._Polaczenia.FindAll(pl => pl.Poczatek.Equals(o.Stacje[^1])))
        {
            o.Stacje.Add(p.Koniec);

            if(p.Koniec.Equals("out"))
            {
                o.Stacje.Add(p.Koniec);
                this._Obwody.Add(o);
            }

            this.ZnajdzObwod(o);
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Obwody.Count().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Polaczenie(string Poczatek, string Koniec);
    private record Obwod(List<string> Stacje);
}