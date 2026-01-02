using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D23Z01 : IZadanie
{
    private string[] _Instrukcje;
    private Komputer _Komputer;

    public D23Z01(bool daneTestowe = false)
    {
        this._Komputer = new Komputer();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\23\\proba.txt" : ".\\Dane\\2016\\23\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        this._Instrukcje = sr.ReadToEnd().Split("\r\n").ToArray();
        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        int przesuniecie;

        for (int krok = 0; krok < this._Instrukcje.Length;)
        {
            switch (this._Instrukcje[krok][0 .. 3])
            {
                case "inc":
                    this._Komputer.ZwiekszRejestr(this._Instrukcje[krok][4]);
                    krok++;
                    break;
                case "dec":
                    this._Komputer.ZmniejszRejestr(this._Instrukcje[krok][4]);
                    krok++;
                    break;
                case "jnz":
                    if (!char.IsDigit(this._Instrukcje[krok][4]) && !this._Komputer.CzyZero(this._Instrukcje[krok][4]))
                    {
                        krok += Convert.ToInt32(this._Instrukcje[krok][6..]);
                        break;
                    }

                    if (char.IsDigit(this._Instrukcje[krok][4]) && !this._Instrukcje[krok][4].Equals('0'))
                    {
                        przesuniecie = !char.IsDigit(this._Instrukcje[krok][^1]) ? this._Komputer[this._Instrukcje[krok][^1]] : Convert.ToInt32(this._Instrukcje[krok][(this._Instrukcje[krok].IndexOf(' ') + 1) ..]);
                        krok += przesuniecie;
                        break;
                    }

                    krok++;
                    break;
                case "cpy":
                    if (!char.IsDigit(this._Instrukcje[krok][4]))
                    {
                        this._Komputer.Kopiuj(this._Instrukcje[krok][4], null, this._Instrukcje[krok][6]);
                        krok++;
                        break;
                    }

                    this._Komputer.Kopiuj(null, Convert.ToInt32(this._Instrukcje[krok][4..this._Instrukcje[krok].LastIndexOf(' ')]), this._Instrukcje[krok][this._Instrukcje[krok].LastIndexOf(' ') + 1]);
                    krok++;
                    break;
                case "tgl":
                    char rejestr = this._Instrukcje[krok][^1];
                    przesuniecie = Convert.ToInt32(this._Komputer[rejestr]);
                    if(krok + przesuniecie < 0 || krok + przesuniecie > this._Instrukcje.Length)
                    {
                        break;
                    }

                    if (this._Instrukcje[krok + przesuniecie][.. 3].Equals("inc"))
                    {
                        this._Instrukcje[krok + przesuniecie] = "dec" + this._Instrukcje[krok + przesuniecie][3..];
                        krok++;
                        break;
                    }

                    if (this._Instrukcje[krok + przesuniecie][.. 3].Equals("dec") || this._Instrukcje[krok + przesuniecie][.. 3].Equals("tgl"))
                    {
                        this._Instrukcje[krok + przesuniecie] = "inc" + this._Instrukcje[krok + przesuniecie][3 ..];
                        krok++;
                        break;
                    }

                    if (this._Instrukcje[krok + przesuniecie][.. 3].Equals("jnz"))
                    {
                        this._Instrukcje[krok + przesuniecie] = "cpy" + this._Instrukcje[krok + przesuniecie][3..];
                        krok++;
                        break;
                    }

                    if (this._Instrukcje[krok + przesuniecie][.. 3].Equals("cpy"))
                    {
                        this._Instrukcje[krok + przesuniecie] = "jnz" + this._Instrukcje[krok + przesuniecie][3 ..];
                        krok++;
                        break;
                    }

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
            this._Rejestry = new Dictionary<char, Int32>();
            this._Rejestry.Add('a', 0);
            this._Rejestry.Add('b', 0);
            this._Rejestry.Add('c', 0);
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