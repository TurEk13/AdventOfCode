using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        foreach(string[] instrukcja in this._Intrukcje)
        {
            switch(instrukcja[0], instrukcja[1])
            {
                case ("swap", "position"):
                    this.SwapPosition(Convert.ToInt32(instrukcja[2]), Convert.ToInt32(instrukcja[5]));
                    break;
                case ("swap", "letter"):
                    this.SwapLetter(instrukcja[2][0], instrukcja[5][0]);
                    break;
                case ("rotate", "left"):
                    this.RotateKierunek(Convert.ToInt32(instrukcja[2]), 'l');
                    break;
                case ("rotate", "right"):
                    this.RotateKierunek(Convert.ToInt32(instrukcja[2]), 'r');
                    break;
                case ("rotate", "based"):
                    this.RotateBased(instrukcja[6][0]);
                    break;
                case ("move", _):
                    this.Move(Convert.ToInt32(instrukcja[2]), Convert.ToInt32(instrukcja[5]));
                    break;
                case ("reverse", _):
                    this.Reverse(Convert.ToInt16(instrukcja[2]), Convert.ToInt32(instrukcja[4]));
                    break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return new string(this._Wynik);
    }

    private void SwapPosition(int pozycjaX, int pozycjaY)
    {
        char tmp = this._Wynik[pozycjaY];
        this._Wynik[pozycjaY] = this._Wynik[pozycjaX];
        this._Wynik[pozycjaX] = tmp;
    }

    private void SwapLetter(char literaX, char literaY)
    {
        for(int i = 0; i < this._Wynik.Length; i++)
        {
            this._Wynik[i] = this._Wynik[i].Equals(literaX) ? literaY : this._Wynik[i].Equals(literaY) ? literaX : this._Wynik[i];
        }
    }

    private void RotateKierunek(int przesuniecie, char kierunek)
    {
        char[] tmp = new char[this._Wynik.Length];

        for(int i = 0; i < tmp.Length; i++)
        {
            tmp[i] = this._Wynik[kierunek.Equals('l') ? (i + przesuniecie) % tmp.Length : (i - przesuniecie + 2 * tmp.Length) % tmp.Length];
        }

        Array.Copy(tmp, this._Wynik, tmp.Length);
    }

    private void RotateBased(char litera)
    {
        int indeks = Array.FindIndex(this._Wynik, l => l.Equals(litera));

        int przesuniecie = indeks > 3 ? 2 + indeks : 1 + indeks;

        this.RotateKierunek(przesuniecie, 'r');
    }

    private void Move(int pozycjaStara, int pozycjaNowa)
    {
        char[] tmpA = new char[this._Wynik.Length];
        
        tmpA[pozycjaNowa] = this._Wynik[pozycjaStara];

        for(int cel = 0, zrodlo = 0; cel < tmpA.Length && zrodlo < this._Wynik.Length; cel++, zrodlo++)
        {
            if(cel == pozycjaNowa)
            {
                cel++;
            }

            if(zrodlo == pozycjaStara)
            {
                zrodlo++;
            }

            tmpA[cel] = this._Wynik[zrodlo];
        }

        Array.Copy(tmpA, this._Wynik, tmpA.Length);
    }

    private void Reverse(int start, int koniec)
    {
        char[] tmp = this._Wynik.Skip(start).Take(koniec - start + 1).Reverse().ToArray<char>();

        for(int i = start, j = 0; j < tmp.Length; i++, j++)
        {
            this._Wynik[i] = tmp[j];
        }
    }
}