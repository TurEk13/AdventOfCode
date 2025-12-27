using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D12Z02 : IZadanie
{
    private string[] _Instrukcje;
    private Komputer _Komputer;
    public D12Z02(bool daneTestowe = false)
    {
        this._Komputer = new Komputer();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\12\\proba.txt" : ".\\Dane\\2016\\12\\dane.txt", FileMode.Open, FileAccess.Read);
        
		StreamReader sr = new(fs);

        this._Instrukcje = sr.ReadToEnd().Split("\r\n").ToArray();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        for (int i = 0; i < this._Instrukcje.Length;)
        {
            switch (this._Instrukcje[i][0 .. 3])
            {
                case "inc":
                    this._Komputer.ZwiekszRejestr(this._Instrukcje[i][4]);
                    i++;
                    break;
                case "dec":
                    this._Komputer.ZmniejszRejestr(this._Instrukcje[i][4]);
                    i++;
                    break;
                case "jnz":
                    if (!char.IsDigit(this._Instrukcje[i][4]) && !this._Komputer.CzyZero(this._Instrukcje[i][4]))
                    {
                        i += Convert.ToInt32(this._Instrukcje[i][6..]);
                        break;
                    }

                    if (char.IsDigit(this._Instrukcje[i][4]) && !this._Instrukcje[i][4].Equals('0'))
                    {
                        i += Convert.ToInt32(this._Instrukcje[i][6..]);
                        break;
                    }

                    i++;
                    break;
                case "cpy":
                    if (!char.IsDigit(this._Instrukcje[i][4]))
                    {
                        this._Komputer.Kopiuj(this._Instrukcje[i][4], null, this._Instrukcje[i][6]);
                        i++;
                        break;
                    }

                    this._Komputer.Kopiuj(null, Convert.ToInt32(this._Instrukcje[i][4..this._Instrukcje[i].LastIndexOf(' ')]), this._Instrukcje[i][this._Instrukcje[i].LastIndexOf(' ') + 1]);
                    i++;
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
        private Dictionary<char, Int64> _Rejestry;

        public Int64 this[char Indeks]
        {
            get { return this._Rejestry[Indeks]; }
        }

        public Komputer()
        {
            this._Rejestry = new Dictionary<char, Int64>();
            this._Rejestry.Add('a', 0);
            this._Rejestry.Add('b', 0);
            this._Rejestry.Add('c', 1);
            this._Rejestry.Add('d', 0);
        }

        public void ZwiekszRejestr(char rejestr)
        {
            this._Rejestry[rejestr]++;
        }

        public void ZmniejszRejestr(char rejestr)
        {
            this._Rejestry[rejestr]--;
        }

        public bool CzyZero(char rejestr)
        {
            return this._Rejestry[rejestr] == 0;
        }

        public void Kopiuj(char? rejestrZrodlowy, int? wartosc, char rejestrDocelowy)
        {
            if(rejestrZrodlowy is not null)
            {
                this._Rejestry[rejestrDocelowy] = this._Rejestry[rejestrZrodlowy.Value];
            }

            if(rejestrZrodlowy is null)
            {
                this._Rejestry[rejestrDocelowy] = wartosc.Value;
            }
        }
    }
}