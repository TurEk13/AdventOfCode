using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public partial class D21Z01 : IZadanie
{
    private char[] _Wynik;
    private List<string[]> _Intrukcje;
    public D21Z01(bool daneTestowe = false)
    {
        this._Intrukcje = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\21\\proba.txt" : ".\\Dane\\2016\\21\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;

        while((linia = sr.ReadLine()) != string.Empty)
        {
            this._Intrukcje.Add(linia.Split(' '));
        }

        this._Wynik = sr.ReadLine().ToCharArray();
        
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        // dcfegbah
        char zmiana;
        char[] zmianaCA;
        string zmianaString;
        List<char> zmianaList;
        foreach(string[] instrukcja in this._Intrukcje)
        {
            switch(instrukcja[0])
            {
                case "swap":
                    switch(instrukcja[1])
                    {
                        case "position":
                            zmiana = this._Wynik[Convert.ToInt32(instrukcja[2])];
                            
                            this._Wynik[Convert.ToInt32(instrukcja[2])] = this._Wynik[Convert.ToInt32(instrukcja[5])];

                            this._Wynik[Convert.ToInt32(instrukcja[5])] = zmiana;
                            break;
                        case "letter":
                            zmianaString = new string(this._Wynik).Replace(instrukcja[2][0], '-');

                            zmianaString = zmianaString.Replace(instrukcja[5], instrukcja[2]);

                            Array.Copy(zmianaString.Replace('-', instrukcja[5][0]).ToCharArray(), this._Wynik, zmianaString.Length);
                            break;
                    }
                    break;
                case "reverse":
                    zmianaCA = [.. this._Wynik.Skip(Convert.ToInt32(Convert.ToInt32(instrukcja[2]))).Take(Convert.ToInt32(instrukcja[4]) - Convert.ToInt32(instrukcja[2]) + 1).Reverse()];

                    for(int i = 0, poczatek = Convert.ToInt32(instrukcja[2]); i < zmianaCA.Length; i++, poczatek++)
                    {
                        this._Wynik[poczatek] = zmianaCA[i];
                    }
                    break;
                case "rotate":
                    switch(instrukcja[1])
                    {
                        case "left":
                            zmianaCA = this._Wynik.Take(this._Wynik.Length).ToArray<char>();
                            Int32.TryParse(instrukcja[2], out int przesuniecie);

                            for(int i = 0; i < this._Wynik.Length; i++)
                            {
                                this._Wynik[i] = zmianaCA[(i + przesuniecie + this._Wynik.Length) % this._Wynik.Length];
                            }
                            break;
                        case "based":
                            // abdec[b] >> ecabd[d] >> decab
                            zmianaList = this._Wynik.ToList<char>();
                            int indeks = zmianaList.IndexOf(instrukcja[^1][0]);

                            zmianaList.AddRange(this._Wynik[.. (indeks + 2 > this._Wynik.Length ? indeks : indeks + 2)]);
                            zmianaList.RemoveRange(0, indeks + 2 > this._Wynik.Length ? indeks : indeks + 2);
                            this._Wynik = zmianaList.ToArray<char>();
                            break;
                    }
                    break;
                case "move":
                    zmiana = this._Wynik[Convert.ToInt32(instrukcja[2])];
                    zmianaList = this._Wynik.ToList<char>();
                    zmianaList.RemoveAt(Convert.ToInt32(instrukcja[2]));
                    zmianaList.InsertRange(Convert.ToInt32(instrukcja[5]), [zmiana]);

                    Array.Copy(zmianaList.ToArray<char>(), this._Wynik, this._Wynik.Length);
                    break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return new string(this._Wynik);
    }
}