using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public partial class D21Z02 : IZadanie
{
    private Dictionary<int, int> _Przesuniecia;
    private List<char> _Wynik;
    private List<string[]> _Intrukcje;

    public D21Z02(bool daneTestowe = false)
    {
        this._Przesuniecia = [];
        this._Przesuniecia.Add(0, 1);
        this._Przesuniecia.Add(1, 1);
        this._Przesuniecia.Add(2, 6);
        this._Przesuniecia.Add(3, 2);
        this._Przesuniecia.Add(4, 7);
        this._Przesuniecia.Add(5, 3);
        this._Przesuniecia.Add(6, 0);
        this._Przesuniecia.Add(7, 4);

        this._Intrukcje = [];
        
		StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2016\\21\\proba.txt" : ".\\Dane\\2016\\21\\dane.txt", FileMode.Open, FileAccess.Read));
        string linia;

        while((linia = sr.ReadLine()) != string.Empty)
        {
            this._Intrukcje.Add(linia.Split(' '));
        }

        this._Intrukcje.Reverse();

        this._Wynik = [.. sr.ReadLine().Select(c => c)];
        
        sr.Close();
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
                    this.RotateLeftRight(Convert.ToInt32(instrukcja[2]), 'l');
                    break;
                case ("rotate", "right"):
                    this.RotateLeftRight(Convert.ToInt32(instrukcja[2]), 'r');
                    break;
                case ("rotate", "based"):
                    this.RotateBased(instrukcja[6][0]);
                    break;
                case ("move", _):
                    this.MovePosition(Convert.ToInt32(instrukcja[2]), Convert.ToInt32(instrukcja[5]));
                    break;
                case ("reverse", _):
                    this.Reverse(Convert.ToInt16(instrukcja[2]), Convert.ToInt32(instrukcja[4]));
                    break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return new string([.. this._Wynik.Select(c => c)]);
    }

    private void SwapPosition(int pozycjaX, int pozycjaY)
    {
        (this._Wynik[pozycjaX], this._Wynik[pozycjaY]) = (this._Wynik[pozycjaY], this._Wynik[pozycjaX]);
    }

    private void SwapLetter(char literaX, char literaY)
    {
        (this._Wynik[this._Wynik.IndexOf(literaX)], this._Wynik[this._Wynik.IndexOf(literaY)]) = (this._Wynik[this._Wynik.IndexOf(literaY)], this._Wynik[this._Wynik.IndexOf(literaX)]);
    }

    private void RotateLeftRight(int przesuniecie, char kierunek)
    {
        switch(kierunek)
        {
            case 'l':
                for(int i = 0; i < przesuniecie; i++)
                {
                    this._Wynik.Insert(0, this._Wynik[^1]);
                    this._Wynik.RemoveAt(this._Wynik.Count - 1);
                }
                break;
            case 'r':
                for(int i = 0; i < przesuniecie; i++)
                {
                    this._Wynik.Insert(this._Wynik.Count, this._Wynik[0]);
                    this._Wynik.RemoveAt(0);
                }
                break;
        }
    }

    private void RotateBased(char litera)
    {
        this.RotateLeftRight(this._Przesuniecia[this._Wynik.IndexOf(litera)], 'r');
    }

    private void MovePosition(int pozycjaStara, int pozycjaNowa)
    {
        char c = this._Wynik[pozycjaNowa];
        this._Wynik.RemoveAt(pozycjaNowa);
        this._Wynik.Insert(pozycjaStara, c);
    }

    private void Reverse(int start, int koniec)
    {
        while(start < koniec)
        {
            (this._Wynik[start], this._Wynik[koniec]) = (this._Wynik[koniec], this._Wynik[start]);
            start++;
            koniec--;
        }
    }
}