using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2017;

public partial class D12Z01 : IZadanie
{
    [GeneratedRegex(@"(?'wezelNadrzedny'\d{1,})(?: <-> )((?'wezelPodrzedny'\d{1,})(?:, |\r|$)){1,}")]
    private static partial Regex WzorLinii();

    private Dictionary<int, int[]> _Programy;
    private int _Wynik;

    public D12Z01(bool daneTestowe = false)
    {
        this._Programy = [];
        this._Wynik = 0;
        Regex r = WzorLinii();
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\12\\proba.txt" : ".\\Dane\\2017\\12\\dane.txt", FileMode.Open, FileAccess.Read));

        MatchCollection mc = r.Matches(sr.ReadToEnd());

        foreach(Match m in mc)
        {
            this._Programy.TryAdd(Convert.ToInt32(m.Groups["wezelNadrzedny"].Value), [.. m.Groups["wezelPodrzedny"].Captures.Select(s => Convert.ToInt32(s.Value))]);
        }

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        Stack<int> DoSprawdzenia = [];
        List<int> Odwiedzone = [];
        DoSprawdzenia.Push(this._Programy[0][0]);

        while(DoSprawdzenia.Count > 0)
        {
            DoSprawdzenia.TryPop(out int obecny);
            foreach(int i in this._Programy[obecny])
            {
                if(!Odwiedzone.Contains(i))
                {
                    Odwiedzone.Add(i);
                    DoSprawdzenia.Push(i);
                }
            }
        }

        this._Wynik = Odwiedzone.Count;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}