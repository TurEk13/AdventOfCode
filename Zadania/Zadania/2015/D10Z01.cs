using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public partial class D10Z01 : IZadanie
{
    private List<string> _sekwencjaT;
    private List<string> _wynikT;
    private string _sekwencja;
    private int _wynik;
    private bool daneTestowe;

    public D10Z01(bool daneTestowe = false)
    {
        this.daneTestowe = daneTestowe;
        this._sekwencjaT = new ();
        this._wynikT = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\10\\proba.txt" : ".\\Dane\\2015\\10\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia = string.Empty;

        if(daneTestowe)
        {
            while((linia = sr.ReadLine()) is not null)
            {
                this._sekwencjaT.Add(linia);
            }
        }

        if(!daneTestowe)
        {
            this._sekwencja = sr.ReadToEnd();
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        if(daneTestowe)
        {
            this.ZamienCiagT();
        }

        if(!daneTestowe)
        {
            this.ZamienCiag();
        }
    }

    [GeneratedRegex(@"(\d)\1*")]
    private static partial Regex MyRegex();
    private void ZamienCiagT()
    {
        StringBuilder sb = new ();
        Regex wzor = MyRegex();

        foreach(string s in this._sekwencjaT)
        {    
            MatchCollection mc = wzor.Matches(s);

            foreach(Match m in mc)
            {
                sb.Append($"{m.Length}{m.Value[0]}");
            }

            this._wynikT.Add(sb.ToString());
            sb.Clear();
        }
    }

    private void ZamienCiag()
    {
        string wartoscPosrednia = this._sekwencja;
        StringBuilder sb = new ();
        Regex wzor = MyRegex();

        for(int i = 0; i < 50; i++)
        {    
            MatchCollection mc = wzor.Matches(wartoscPosrednia);

            foreach(Match m in mc)
            {
                sb.Append($"{m.Length}{m.Value[0]}");
            }

            wartoscPosrednia = sb.ToString();
            sb.Clear();
        }

        this._wynik = wartoscPosrednia.Length;
    }

    public string PokazRozwiazanie()
    {
        if(daneTestowe)
        {
            StringBuilder sb = new();

            for(int i = 0; i < this._wynikT.Count; i++)
            {
                sb.Append($"\r\n{this._sekwencjaT[i]} -> {this._wynikT[i]}");
            }
            return sb.ToString();
        }

        return $"\r\n{this._sekwencja} -> Dlugość: {this._wynik}";
    }
}