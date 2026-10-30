using System;
using System.Globalization;
using System.IO;

namespace Zadania._2017;

public class D01Z01 : IZadanie
{
    private UInt32 _Wynik;
    private char[] _Captcha;
    public D01Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\01\\proba.txt" : ".\\Dane\\2017\\01\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Captcha = sr.ReadToEnd().ToCharArray();

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        for(int i = 1; i < this._Captcha.Length; i++)
        {
            if(this._Captcha[i - 1].Equals(this._Captcha[i]))
            {
                this._Wynik += Convert.ToUInt32(this._Captcha[i] - '0');
            }
        }

        if(this._Captcha[0].Equals(this._Captcha[this._Captcha.Length - 1]))
        {
            this._Wynik += Convert.ToUInt32(this._Captcha[0] - '0');
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}