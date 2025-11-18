using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D11Z01 : IZadanie
{
    private readonly string Haslo;
    private string NoweHaslo;

    public D11Z01(bool daneTestowe = false)
    {
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\11\\proba.txt" : ".\\Dane\\2015\\11\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);

        this.Haslo = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        bool DobreHaslo = false;
        this.NoweHaslo = this.Haslo;

        while(!DobreHaslo)
        {
            this.ZmienLitere(this.NoweHaslo.Length - 1);
            DobreHaslo = this.SprawdzWarunkiHasla();
        }
    }

    private void ZmienLitere(int indeks)
    {
        StringBuilder sb = new(this.NoweHaslo);
        char znak = Convert.ToChar(sb[indeks] + 1);
        
        if(znak > 'z' && indeks > 0)
        {
            sb[indeks] = 'a';
            this.NoweHaslo = sb.ToString();
            this.ZmienLitere(indeks - 1);
            return;
        }

        sb[indeks] = znak;
        this.NoweHaslo = sb.ToString();
        sb.Clear();
    }

    private bool SprawdzWarunkiHasla()
    {
        return this.Kolejne3Znaki() && this.Min2PowtarzajaceLitery() && !this.BrakIOL();
    }

    private bool Kolejne3Znaki()
    {
        for(int i = 0; i < this.NoweHaslo.Length - 2; i++)
        {
            if(this.NoweHaslo[i] + 1 == this.NoweHaslo[i + 1] && this.NoweHaslo[i + 1] + 1 == this.NoweHaslo[i + 2])
            {
                return true;
            }
        }

        return false;
    }

    private bool BrakIOL()
    {
        char[] ZleLitery = ['i', 'o', 'l'];

        if(this.NoweHaslo.Contains(ZleLitery[0]) || this.NoweHaslo.Contains(ZleLitery[1]) || this.NoweHaslo.Contains(ZleLitery[2]))
        {
            return true;
        }

        return false;
    }

    [GeneratedRegex(@"(\w)\1*")]
    private static partial Regex MyRegex();
    private bool Min2PowtarzajaceLitery()
    {
        Regex wzor = MyRegex();
        MatchCollection mc = wzor.Matches(this.NoweHaslo);

        int i = mc.Where(m => m.Length > 1).ToList().Count;

        return i > 1;
    }

    public string PokazRozwiazanie()
    {
        return $"\r\nStare hasło: {this.Haslo}\r\nNowe hasło: {this.NoweHaslo}";
    }
}