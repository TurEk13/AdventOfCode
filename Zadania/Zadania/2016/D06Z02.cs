using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D06Z02 : IZadanie
{
    private string _Wynik;
    private Dictionary<char, int> _IloscLiter;
    private List<string> _Ciagi;

    public D06Z02(bool daneTestowe = false)
    {
        this._IloscLiter = new();
        this._Ciagi = new();
        this._Wynik = string.Empty;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\06\\proba.txt" : ".\\Dane\\2016\\06\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string wiersz;

        while((wiersz = sr.ReadLine()) is not null)
        {
            this._Ciagi.Add(wiersz);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for (int kolumna = 0; kolumna < this._Ciagi[0].Length; kolumna++)
        {
            for (int wiersz = 0; wiersz < this._Ciagi.Count; wiersz++)
            {
                if (this._IloscLiter.TryGetValue(this._Ciagi[wiersz][kolumna], out _))
                {
                    this._IloscLiter[this._Ciagi[wiersz][kolumna]]++;
                }

                _IloscLiter.TryAdd(this._Ciagi[wiersz][kolumna], 1);
            }

            KeyValuePair<char, int> NajczestrzaLitera = this._IloscLiter.OrderBy(il => il.Value).ToList()[0];
            this._IloscLiter = new ();
            this._Wynik += NajczestrzaLitera.Key;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik;
    }
}