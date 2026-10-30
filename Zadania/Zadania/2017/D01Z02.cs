using System;
using System.Globalization;
using System.IO;

namespace Zadania._2017;

public class D01Z02 : IZadanie
{
    private UInt32 _Wynik;
    private char[] _Captcha;
    public D01Z02(bool daneTestowe = false)
    {
        this._Wynik = 0;
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\01\\proba.txt" : ".\\Dane\\2017\\01\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Captcha = sr.ReadToEnd().ToCharArray();

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        int przesuniecie = this._Captcha.Length / 2;

        for(int i = 0; i < this._Captcha.Length; i++)
        {
            if(this._Captcha[i].Equals(this._Captcha[(i + przesuniecie) % this._Captcha.Length]))
            {
                this._Wynik += Convert.ToUInt32(this._Captcha[i] - '0');
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}