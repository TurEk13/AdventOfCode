using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D16Z01 : IZadanie
{
    private List<char> _Wynik;
    List<Baza> _BazaOperacji;

    [GeneratedRegex(@"(((?'s's(?'s1'\d{1,}))|(?'x'x(?'x1'\d{1,})\/(?'x2'\d{1,}))|(?'p'p(?'p1'[a-p]{1})\/(?'p2'[a-p]{1})))(?:,|$))")]
    private static partial Regex WzorOperacji();

    private record Baza(char Operacja);
    private record Spin(char Operacja, int przesun) : Baza(Operacja);
    private record Exchange(char Operacja, int Pierwszy, int Drugi) : Baza(Operacja);
    private record Partner(char Operacja, char Pierwszy, char Drugi) : Baza(Operacja);

    public D16Z01(bool daneTestowe = false)
    {
        this._BazaOperacji = [];
        this._Wynik = [.. "abcde".ToCharArray().Select(c => c)];
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\16\\proba.txt" : ".\\Dane\\2017\\16\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = WzorOperacji().Matches(sr.ReadToEnd());

        sr.Close();

        foreach(Match m in mc)
        {
            if (m.Groups["s"].Success)
            {
                this._BazaOperacji.Add(new Spin('s', Convert.ToInt32(m.Groups["s1"].Value)));
            }
            else if (m.Groups["x"].Success)
            {
                this._BazaOperacji.Add(new Exchange('x', Convert.ToInt32(m.Groups["x1"].Value), Convert.ToInt32(m.Groups["x2"].Value)));
            }
            else if (m.Groups["p"].Success)
            {
                this._BazaOperacji.Add(new Partner('p', m.Groups["p1"].Value[0], m.Groups["p2"].Value[0]));
            }
        }
    }

    public void RozwiazanieZadania()
    {
        char tmp;
        Spin s;
        Exchange e;
        Partner p;

        foreach (Baza b in this._BazaOperacji)
        {
            switch (b.Operacja)
            {
                case 's':
                    s = (Spin)b;
                    for (int j = 0; j < s.przesun; j++)
                    {
                        this._Wynik.Insert(0, this._Wynik[^1]);
                        this._Wynik.RemoveAt(this._Wynik.Count - 1);
                    }
                    break;
                case 'x':
                    e = (Exchange)b;
                    tmp = this._Wynik[e.Pierwszy];
                    this._Wynik[e.Pierwszy] = this._Wynik[e.Drugi];
                    this._Wynik[e.Drugi] = tmp;
                    break;
                case 'p':
                    p = (Partner)b;
                    this._Wynik[this._Wynik.IndexOf(p.Pierwszy)] = 'x';
                    this._Wynik[this._Wynik.IndexOf(p.Drugi)] = p.Pierwszy;
                    this._Wynik[this._Wynik.IndexOf('x')] = p.Drugi;
                    break;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return new string([.. this._Wynik.Select(c => c)]);
    }
}