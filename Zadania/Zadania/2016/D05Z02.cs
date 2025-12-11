using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Zadania._2016;

public class D05Z02 : IZadanie
{
    private string _Wynik;
    private string _Poczatek;

    public D05Z02(bool daneTestowe = false)
    {
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\05\\proba.txt" : ".\\Dane\\2016\\05\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);

        this._Poczatek = sr.ReadToEnd();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        string obrobka = this._Poczatek;
        char[] haslo = new char[8];

        int i = 0, indeks;
        while (haslo.Contains('\0'))
        {
            do
            {
                obrobka = string.Join("", MD5.HashData(Encoding.UTF8.GetBytes($"{this._Poczatek}{i}")).Select(o => o.ToString("x2")));
                i++;
            }
            while (!obrobka[..5].Equals("00000"));

            indeks = obrobka[5] - '0';
            
            if (indeks >= 0 && indeks < haslo.Length)
            {
                if (haslo[indeks].Equals('\0'))
                {
                    haslo[indeks] = obrobka[6];
                }
            }
        }

        this._Wynik = new string(haslo);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik;
    }
}