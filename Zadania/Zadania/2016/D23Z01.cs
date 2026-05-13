using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D23Z01 : IZadanie
{
    private List<string[]> _Instrukcje;
    private Komputer _Komputer;

    public D23Z01(bool daneTestowe = false)
    {
        this._Komputer = new Komputer();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\23\\proba.txt" : ".\\Dane\\2016\\23\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        this._Instrukcje = sr.ReadToEnd().Split("\r\n").Select(i => i.Split(' ')).ToArray().ToList();
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
                    if (Int32.TryParse(this._Instrukcje[krok][1], out int wartoscJNZ))
                    {
                        krok = wartoscJNZ == 0 ? krok + 1 : krok + this._Komputer[this._Instrukcje[krok][2][0]];
                        break;
                    }

                    krok = this._Komputer.CzyRejestJestZero(this._Instrukcje[krok][1][0]) ? krok + 1 : krok += Convert.ToInt32(this._Instrukcje[krok][2]);
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
                case "tgl":
                    if(!Int32.TryParse(this._Instrukcje[krok][1], out int wartoscTGL))
                    {
                        wartoscTGL = this._Komputer[this._Instrukcje[krok][1][0]];
                    }

                    if (krok + wartoscTGL < this._Instrukcje.Count)
                    {
                        switch (this._Instrukcje[krok + wartoscTGL][0])
                        {
                            case "inc":
                                this._Instrukcje[krok + wartoscTGL][0] = "dec";
                                break;
                            case "dec":
                            case "tgl":
                                this._Instrukcje[krok + wartoscTGL][0] = "inc";
                                break;
                            case "cpy":
                                this._Instrukcje[krok + wartoscTGL][0] = "jnz";
                                break;
                            case "jnz":
                                this._Instrukcje[krok + wartoscTGL][0] = "cpy";
                                break;
                        }
                    }
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
                { 'a', 7 },
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