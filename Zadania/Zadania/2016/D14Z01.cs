using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D14Z01 : IZadanie
{
    private readonly string _Ziarno;
    private int _Klucz;
    public D14Z01(bool daneTestowe = false)
    {
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\14\\proba.txt" : ".\\Dane\\2016\\14\\dane.txt", FileMode.Open, FileAccess.Read);

		StreamReader sr = new(fs);
        this._Ziarno = sr.ReadToEnd();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        string obrobka;
        char znak;
        bool dalej;
        int j = 0;

        for(int i = 0; i < 64; i++)
        {
            dalej = true;
            while(dalej)
            {
                obrobka = this.ObliczHash($"{this._Ziarno}{j}");

                if(!(znak = this.PotrojnaLitera(obrobka)).Equals('-'))
                {
                    if(this.PieciokrotnaLitera(znak, j))
                    {
                        dalej = false;
                        this._Klucz = j;
                    }
                }
                j++;
            }
        }
    }

    private char PotrojnaLitera(string sprawdz)
    {
        for(int i = 2; i < sprawdz.Length; i++)
        {
            if(sprawdz[i - 2].Equals(sprawdz[i - 1]) && sprawdz[i].Equals(sprawdz[i - 1]))
            {
                return sprawdz[i];
            }
        }

        return '-';
    }

    private bool PieciokrotnaLitera(char litera, int start)
    {
        Regex piecZnakow = new (@"(?:" + litera + "){5}");
        string obrobka;
        for(int i = start + 1; i < start + 1 + 1000; i++)
        {
            obrobka = this.ObliczHash($"{this._Ziarno}{i}");

            if(piecZnakow.Match(obrobka).Success)
            {
                return true;
            }
        }

        return false;
    }

    private string ObliczHash(string baza)
    {
        return string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{baza}")).Select(o => o.ToString("x2")));
    }

    public string PokazRozwiazanie()
    {
        return this._Klucz.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));

    }
}