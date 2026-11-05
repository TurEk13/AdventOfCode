using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zadania._2017;

public partial class D16Z01 : IZadanie
{
    private string _Operacje;
    private List<char> _Wynik;

    public D16Z01(bool daneTestowe = false)
    {
        this._Wynik = [.. "abcdefghijklmnop".ToCharArray().Select(c => c)];
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\16\\proba.txt" : ".\\Dane\\2017\\16\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Operacje = sr.ReadToEnd();

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        int x;
        int[] y;
        char tmp;
        char[] tmp1;
        string s;

        while(this._Operacje.Length > 0)
        {
            s = this._Operacje.Contains(',') ? this._Operacje[.. this._Operacje.IndexOf(',')] : this._Operacje;

            this._Operacje = this._Operacje.Length != s.Length ? this._Operacje[(s.Length + 1)..] : "";

            switch(s[0])
            {
                case 'p':
                    tmp1 = [.. s[1 ..].Split('/').Select(c => c[0])];
                    this._Wynik[this._Wynik.IndexOf(tmp1[0])] = 'x';
                    this._Wynik[this._Wynik.IndexOf(tmp1[1])] = tmp1[0];
                    this._Wynik[this._Wynik.IndexOf('x')] = tmp1[1];
                    break;
                case 's':
                    x = Convert.ToInt32(s[1..]);
                    for(int i = 0; i < x; i++)
                    {
                        this._Wynik.Insert(0, this._Wynik[^1]);
                        this._Wynik.RemoveAt(this._Wynik.Count - 1);
                    }
                    break;
                case 'x':
                    y = [.. s[1 ..].Split('/').Select(s => Convert.ToInt32(s))];
                    tmp = this._Wynik[y[0]];
                    this._Wynik[y[0]] = this._Wynik[y[1]];
                    this._Wynik[y[1]] = tmp;
                    break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return new string([.. this._Wynik.Select(c => c)]);
    }
}