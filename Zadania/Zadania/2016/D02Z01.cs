using System.Collections.Generic;
using System.IO;

namespace Zadania._2016;

public class D02Z01 : IZadanie
{
    private Klawiatura _Klawiatura;
    private string _Kod;
    private List<string> _Instrukcje;
    public D02Z01(bool daneTestowe = false)
    {
        this._Klawiatura = new Klawiatura();
        this._Instrukcje = new ();
        string linia;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\02\\proba.txt" : ".\\Dane\\2016\\02\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        
        while((linia = sr.ReadLine()) is not null)
        {
            this._Instrukcje.Add (linia);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        foreach(string linia in this._Instrukcje)
        {
            foreach(char znak in linia)
            {
                switch(znak)
                {
                    case 'U':
                        this._Klawiatura.PrzesunY(-1);
                        break;
                    case 'D':
                        this._Klawiatura.PrzesunY(1);
                        break;
                    case 'L':
                        this._Klawiatura.PrzesunX(-1);
                        break;
                    case 'R':
                        this._Klawiatura.PrzesunX(1);
                        break;
                }
            }

            this._Kod += this._Klawiatura.ZwrocNumerPrzycisku;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Kod;
    }

    class Klawiatura
    {
        private readonly int[,] _Klawiatura;
        private int X;
        private int Y;

        public Klawiatura()
        {
            this._Klawiatura = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            this.X = 1;
            this.Y = 1;
        }

        public int ZwrocNumerPrzycisku { get { return this._Klawiatura[this.Y, this.X]; } }

        public void PrzesunX(int przesuniecie)
        {
            this.X = this.X + przesuniecie > 2 ? 2 : this.X + przesuniecie < 0 ? 0 : this.X + przesuniecie;
        }

        public void PrzesunY(int przesuniecie)
        {
            this.Y = this.Y + przesuniecie > 2 ? 2 : this.Y + przesuniecie < 0 ? 0 : this.Y + przesuniecie;
        }
    }
}