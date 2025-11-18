using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D10Z01 : IZadanie
{
    private List<string> _sekwencja;
    private List<string> _wynik;

    public D10Z01(bool daneTestowe = false)
    {
        this._sekwencja = new ();
        this._wynik = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\10\\proba.txt" : ".\\Dane\\2015\\10\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia = string.Empty;

        while((linia = sr.ReadLine()) is not null)
        {
            this._sekwencja.Add(linia);
        }
        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        this.ZamienCiag();
    }

    [GeneratedRegex(@"(\d)\1*")]
    private static partial Regex MyRegex();
    private void ZamienCiag()
    {
        StringBuilder sb = new ();
        Regex wzor = MyRegex();

        foreach(string s in this._sekwencja)
        {    
            MatchCollection mc = wzor.Matches(s);

            foreach(Match m in mc)
            {
                sb.Append($"{m.Length}{m.Value[0]}");
            }

            this._wynik.Add(sb.ToString());
            sb.Clear();
        }
    }

    public string PokazRozwiazanie()
    {
        StringBuilder sb = new();

        for(int i = 0; i < this._wynik.Count; i++)
        {
            sb.Append($"\r\n{this._sekwencja[i]} -> {this._wynik[i]}");
        }
        return sb.ToString();
    }
}