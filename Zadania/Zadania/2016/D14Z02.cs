using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2016;

public partial class D14Z02 : IZadanie
{
    private readonly string _Ziarno;
    private int _Klucz;
    public D14Z02(bool daneTestowe = false)
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

        for(int i = 0, j = 0; i < 64; i++)
        {
            dalej = true;
            while(dalej)
            {
                obrobka = this.ObliczHash($"{this._Ziarno}{j}");
                obrobka = this.Hash2016(obrobka);

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

    [GeneratedRegex(@"(\w)\1{2}")]
    private static partial Regex PotrojnyZnak();
    private char PotrojnaLitera(string sprawdz)
    {
        Match m = PotrojnyZnak().Match(sprawdz);

        if(m.Success)
        {
            return m.Groups[0].Value[0];
        }

        return '-';
    }

    private bool PieciokrotnaLitera(char litera, int start)
    {
        Regex piecZnakow = new (@"(" + litera + @")\1{4}", RegexOptions.Compiled);
        string obrobka;

        for(int i = start + 1; i < start + 1 + 1000; i++)
        {
            obrobka = this.ObliczHash($"{this._Ziarno}{i}");
            obrobka = this.Hash2016(obrobka);

            if(piecZnakow.IsMatch(obrobka))
            {
                return true;
            }
        }

        return false;
    }

    private string Hash2016(string hash)
    {
        for(int i = 0; i < 2016; i++)
        {
            hash = this.ObliczHash(hash);
        }

        return hash;
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