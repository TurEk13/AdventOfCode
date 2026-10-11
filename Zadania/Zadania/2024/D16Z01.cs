using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Zadania._2024;

public class D16Z01 : IZadanie
{
    private readonly List<char[]> _mapa;
    private Punkt _obecnaPozycja;
    private (int DlugoscSciezki, Int64 WartoscSciezki) _wynik;

    
    private record Punkt(int X, int Y, int DlugoscSciezki, Int64 WartoscSciezki, char Kierunek);
    
    public D16Z01(bool daneTestowe = false)
    {
        this._mapa = new List<char[]>();
        this._wynik = (0, Int16.MaxValue);

        FileStream fs = new FileStream(daneTestowe ? ".\\Dane\\2024\\16\\proba.txt" : ".\\Dane\\2024\\16\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        string s;

        while ((s = sr.ReadLine()) is not null)
        {
            this._mapa.Add(s.ToCharArray());

            if (this._mapa[^1].Contains('S'))
            {
                this._obecnaPozycja = new Punkt(this._mapa.Count - 1, Array.IndexOf(this._mapa[^1], 'S'), 0, 0, 'E');
            }
        }
    }

    public void RozwiazanieZadania()
    {
        List<Punkt> odwiedzone = new List<Punkt>([this._obecnaPozycja]);
        this.DFS(this._obecnaPozycja, odwiedzone);
        //this.BFS(this._obecnaPozycja);
    }

    private void BFS(Punkt poczatek)
    {
        Punkt nowaPozycja;
        Stack<Punkt> punktyDoSprawdzenia = new Stack<Punkt>();
        punktyDoSprawdzenia.Push(poczatek);

        List<Punkt> odwiedzonePunkty = new List<Punkt>();
        odwiedzonePunkty.Add(poczatek);

        (int Skret, int Prosto) Dodaj;

        while (punktyDoSprawdzenia.TryPop(out Punkt obecnyPunkt))
        {
            if((obecnyPunkt.X == 1 && obecnyPunkt.Y == 12) || (obecnyPunkt.X == 2 && obecnyPunkt.Y == 13))
            {
                Debug.WriteLine(obecnyPunkt);
            }

            odwiedzonePunkty.Add(obecnyPunkt);
            Debug.WriteLine($"[{obecnyPunkt.X}, {obecnyPunkt.Y}]: Punkty do sprawdzenia: {punktyDoSprawdzenia.Count}, Punkty sprawdzone: {odwiedzonePunkty.Count}, Długość ścieżki: {obecnyPunkt.DlugoscSciezki}");
            if(this.CzyKoniec(obecnyPunkt))
            {
                if(this._wynik.DlugoscSciezki > obecnyPunkt.DlugoscSciezki)
                {
                    this._wynik = (obecnyPunkt.DlugoscSciezki, obecnyPunkt.WartoscSciezki);
                    Debug.WriteLine("Długość ścieżki: " + obecnyPunkt.DlugoscSciezki + ")");
                }
                continue;
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'N');
            nowaPozycja = obecnyPunkt with { X = obecnyPunkt.X - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, Kierunek = 'N' };
            if(this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'S');
            nowaPozycja = obecnyPunkt with { X = obecnyPunkt.X + 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, Kierunek = 'S' };
            if (this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'W');
            nowaPozycja = obecnyPunkt with { Y = obecnyPunkt.Y - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, Kierunek = 'W' };
            if (this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }

            Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'E');
            nowaPozycja = obecnyPunkt with { Y = obecnyPunkt.Y + 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, Kierunek = 'E' };
            if (this.DostepnePole(nowaPozycja) && !odwiedzonePunkty.Any(p => p.X == nowaPozycja.X && p.Y == nowaPozycja.Y))
            {
                punktyDoSprawdzenia.Push(nowaPozycja);
            }
        
            if(punktyDoSprawdzenia.Count == 0)
            {
                Debug.WriteLine(0);
            }
        }
    }

    private void DFS(Punkt obecnyPunkt, List<Punkt> punktySprawdzone)
    {
        (int Skret, int Prosto) Dodaj = this.DodajWartosc(obecnyPunkt.Kierunek, 'N');
        Punkt nowyPunkt = obecnyPunkt with { X = obecnyPunkt.X - 1, DlugoscSciezki = obecnyPunkt.DlugoscSciezki + 1, WartoscSciezki = obecnyPunkt.WartoscSciezki + Dodaj.Prosto + Dodaj.Skret, Kierunek = 'N' };

        if(this.CzyKoniec(nowyPunkt))
        {
            if(this._wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                Debug.WriteLine("Długość ścieżki: " + nowyPunkt.DlugoscSciezki + ")");
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
            if (this._wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                Debug.WriteLine("Długość ścieżki: " + nowyPunkt.DlugoscSciezki + ")");
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
            if (this._wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                Debug.WriteLine("Długość ścieżki: " + nowyPunkt.DlugoscSciezki + ")");
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
            if (this._wynik.WartoscSciezki > nowyPunkt.WartoscSciezki)
            {
                this._wynik = (nowyPunkt.DlugoscSciezki, nowyPunkt.WartoscSciezki);
                Debug.WriteLine("Długość ścieżki: " + nowyPunkt.DlugoscSciezki + ")");
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
        return this._mapa[p.X][p.Y].Equals('E');
    }

    private bool DostepnePole(Punkt p)
    {
        return !this._mapa[p.X][p.Y].Equals('#');
    }

    public string PokazRozwiazanie()
    {
        return $"Wartość ścieżki(7036): {this._wynik.WartoscSciezki}";//.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}