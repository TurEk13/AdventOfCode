using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Zadania._2024;

public class D16Z01 : IZadanie
{
    private readonly List<char[]> _Mapa;
    private Punkt _ObecnaPozycja;
    private (int DlugoscSciezki, Int64 WartoscSciezki) _Wynik;

    
    private record Punkt(int X, int Y, int DlugoscSciezki, Int64 WartoscSciezki, char Kierunek);
    
    public D16Z01(bool daneTestowe = false)
    {
        this._Mapa = new List<char[]>();
        this._Wynik = (0, Int64.MaxValue);

        FileStream fs = new FileStream(daneTestowe ? ".\\Dane\\2024\\16\\proba.txt" : ".\\Dane\\2024\\16\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        string s;

        while ((s = sr.ReadLine()) is not null)
        {
            this._Mapa.Add(s.ToCharArray());

            if (this._Mapa[^1].Contains('S'))
            {
                this._ObecnaPozycja = new Punkt(this._Mapa.Count - 1, Array.IndexOf(this._Mapa[^1], 'S'), 0, 0, 'E');
            }
        }
    }

    public void RozwiazanieZadania()
    {
        List<Punkt> odwiedzone = new List<Punkt>([this._ObecnaPozycja]);
        //this.DFS(this._ObecnaPozycja, odwiedzone);
        this.BFS(this._ObecnaPozycja);
    }

    private void BFS(Punkt poczatek)
    {
        Punkt nowaPozycja;
        Stack<Punkt> punktyDoSprawdzenia = new Stack<Punkt>();
        punktyDoSprawdzenia.Push(poczatek);

        List<Punkt> odwiedzonePunkty = new List<Punkt>();

        (int Skret, int Prosto) Dodaj;

        while (punktyDoSprawdzenia.TryPop(out Punkt obecnyPunkt))
        {
            odwiedzonePunkty.Add(obecnyPunkt);

            if(obecnyPunkt.WartoscSciezki >= this._Wynik.WartoscSciezki)
            {
                continue;
            }

            if(this.CzyKoniec(obecnyPunkt))
            {
                if(this._Wynik.WartoscSciezki > obecnyPunkt.WartoscSciezki)
                {
                    this._Wynik = (obecnyPunkt.DlugoscSciezki, obecnyPunkt.WartoscSciezki);
                }
                continue;
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'N');
            nowaPozycja = obecnyPunkt with { X = obecnyPunkt.X - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'N' };
            if(this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'S');
            nowaPozycja = obecnyPunkt with { X = obecnyPunkt.X + 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'S' };
            if (this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'W');
            nowaPozycja = obecnyPunkt with { Y = obecnyPunkt.Y - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'W' };
            if (this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'E');
            nowaPozycja = obecnyPunkt with { Y = obecnyPunkt.Y + 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'E' };
            if (this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }
        }

        List<char[]> kopia = [.. this._Mapa];

        foreach(Punkt p in odwiedzonePunkty)
        {
            this._Mapa[p.X][p.Y] = 'X';
        }

        File.WriteAllText("D16Z01_r.txt", this._Wynik.WartoscSciezki.ToString() + Environment.NewLine);

        foreach(char[] x in kopia)
        {
            File.AppendAllText("D16Z01_r.txt", new string(x) + Environment.NewLine);
        }
    }

    private void DFS(Punkt obecnyPunkt, List<Punkt> punktySprawdzone)
    {
        if(obecnyPunkt.WartoscSciezki > this._Wynik.WartoscSciezki)
        {
            return;
        }

        (int Skret, int Prosto) Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'N');
        Punkt nowyPunkt = obecnyPunkt with { X = obecnyPunkt.X - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'N' };

        if(this.CzyKoniec(nowyPunkt))
        {
            if(this._Wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._Wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                return;
            }
        }

        if(this.DostepnePole(nowyPunkt) && !punktySprawdzone.Any(p => p.X == nowyPunkt.X && p.Y == nowyPunkt.Y))
        {
            punktySprawdzone.Add(nowyPunkt);
            this.DFS(nowyPunkt, punktySprawdzone);
            punktySprawdzone.Remove(nowyPunkt);
        }

        Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'S');
        nowyPunkt = obecnyPunkt with { X = obecnyPunkt.X + 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'S' };

        if (this.CzyKoniec(nowyPunkt))
        {
            if(this._Wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._Wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                return;
            }
        }

        if (this.DostepnePole(nowyPunkt) && !punktySprawdzone.Any(p => p.X == nowyPunkt.X && p.Y == nowyPunkt.Y))
        {
            punktySprawdzone.Add(nowyPunkt);
            this.DFS(nowyPunkt, punktySprawdzone);
            punktySprawdzone.Remove(nowyPunkt);
        }

        Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'W');
        nowyPunkt = obecnyPunkt with { Y = obecnyPunkt.Y - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'W' };

        if (this.CzyKoniec(nowyPunkt))
        {
            if(this._Wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._Wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                return;
            }
        }

        if (this.DostepnePole(nowyPunkt) && !punktySprawdzone.Any(p => p.X == nowyPunkt.X && p.Y == nowyPunkt.Y))
        {
            punktySprawdzone.Add(nowyPunkt);
            this.DFS(nowyPunkt, punktySprawdzone);
            punktySprawdzone.Remove(nowyPunkt);
        }

        Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'E');
        nowyPunkt = obecnyPunkt with { Y = obecnyPunkt.Y + 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'E' };

        if (this.CzyKoniec(nowyPunkt))
        {
            if(this._Wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._Wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                return;
            }
        }

        if (this.DostepnePole(nowyPunkt) && !punktySprawdzone.Any(p => p.X == nowyPunkt.X && p.Y == nowyPunkt.Y))
        {
            punktySprawdzone.Add(nowyPunkt);
            this.DFS(nowyPunkt, punktySprawdzone);
            punktySprawdzone.Remove(nowyPunkt);
        }
    }

    private (int Skret, int Prosto) DodajWartosc(char kierunekStary, char kierunekNowy)
    {
        if (kierunekStary.Equals(kierunekNowy))
        {
            return (0, 1);
        }

        if (kierunekStary.Equals('N') || kierunekStary.Equals('S'))
        {
            if (kierunekNowy.Equals('W') || kierunekNowy.Equals('E'))
                return (1000, 1);
        }

        if (kierunekStary.Equals('W') || kierunekStary.Equals('E'))
        {
            if (kierunekNowy.Equals('N') || kierunekNowy.Equals('S'))
                return (1000, 1);
        }

        return (0, 0);
    }

    private bool CzyKoniec(Punkt p)
    {
        return this._Mapa[p.X][p.Y].Equals('E');
    }

    private bool DostepnePole(Punkt p)
    {
        return !this._Mapa[p.X][p.Y].Equals('#');
    }

    public string PokazRozwiazanie()
    {
        return $"Wartość ścieżki(7036 / 11048): {this._Wynik.WartoscSciezki}";//.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}