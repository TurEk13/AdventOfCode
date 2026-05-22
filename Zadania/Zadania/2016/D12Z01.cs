using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D12Z01 : IZadanie
{
    private List<string[]> _Instrukcje;
    private Komputer _Komputer;
    public D12Z01(bool daneTestowe = false)
    {
        this._Komputer = new Komputer();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\12\\proba.txt" : ".\\Dane\\2016\\12\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);
        this._Instrukcje = sr.ReadToEnd().Split("\r\n").Select(i => i.Split(' ')).ToList();
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for (int krok = 0; krok < this._Instrukcje.Count;)
        {
            switch (this._Instrukcje[krok][0])
            {
                case "inc":
                    this._Komputer.ZwiekszRejestr(this._Instrukcje[krok][1][0]);
                    krok++;
                    break;
                case "dec":
                    this._Komputer.ZmniejszRejestr(this._Instrukcje[krok][1][0]);
                    krok++;
                    break;
                case "jnz":
                    switch(Int32.TryParse(this._Instrukcje[krok][1], out int wartoscJNZ_1), Int32.TryParse(this._Instrukcje[krok][2], out int wartoscJNZ_2))
                    {
                        case (false, true):
                            krok = this._Komputer.CzyRejestJestZero(this._Instrukcje[krok][1][0]) ? krok + 1 : krok += wartoscJNZ_2;
                            break;
                        case (true, false):
                            krok = wartoscJNZ_1 == 0 ? krok + 1 : krok + this._Komputer[this._Instrukcje[krok][2][0]];
                            break;
                        case (true, true):
                            if(wartoscJNZ_1 == 0 && wartoscJNZ_2 == 0)
                            {
                                krok++;
                                break;
                            }
                            krok = wartoscJNZ_1 == 0 ? krok + 1 : krok + wartoscJNZ_2;
                            break;
                    }
                    break;
                case "cpy":
                    if (Int32.TryParse(this._Instrukcje[krok][1], out int wartoscCPY))
                    {
                        this._Komputer.Kopiuj(wartoscCPY, this._Instrukcje[krok][2][0]);
                        krok++;
                        break;
                    }
                    this._Komputer.Kopiuj(this._Komputer[this._Instrukcje[krok][1][0]], this._Instrukcje[krok][2][0]);
                    krok++;
                    break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Komputer['a'].ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Komputer
    {
        private Dictionary<char, Int32> _Rejestry;

        public Int32 this[char Indeks]
        {
            get { return this._Rejestry[Indeks]; }
        }

        public Komputer()
        {
            this._Rejestry = new Dictionary<char, Int32>
            {
                { 'a', 0 },
                { 'b', 0 },
                { 'c', 0 },
                { 'd', 0 }
            };
        }

        public void ZwiekszRejestr(char rejestr)
        {
            this._Rejestry[rejestr]++;
        }

        public void ZmniejszRejestr(char rejestr)
        {
            this._Rejestry[rejestr]--;
        }

        public bool CzyRejestJestZero(char rejestr)
        {
            return this._Rejestry[rejestr] == 0;
        }

        public void Kopiuj(int wartosc, char rejestrDocelowy)
        {
            this._Rejestry[rejestrDocelowy] = wartosc;
        }
    }
}