using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Zadania._2015;

public class D04Z02 : IZadanie
{
    private int _i;
    private string _wynik;
    private string _poczatek;
    public D04Z02()
    {
        this._i = 0;
        this._wynik = string.Empty;

        FileStream fs = new(".\\Dane\\2015\\04\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);

        this._poczatek = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        string obrobka = this._poczatek;

        while(obrobka[..6] != "000000")
        {
            obrobka = string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"{this._poczatek}{this._i}")).Select(o => o.ToString("x2")));
            this._i++;
        }

        this._i--;

        this._wynik = $"\r\nPoczątek: {this._poczatek}\r\nWynik: {obrobka}\r\nIteracji: {this._i.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}";

    }

    public string PokazRozwiazanie()
    {
        return this._wynik;
    }
}