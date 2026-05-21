using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D24Z02 : IZadanie
{
    private readonly List<char[]> _MapaZnaki;
    private readonly MapaWartosci _MapaWartosci;
    private List<Punkt> _PunktyDoOdwiedzenia;
    private List<Odleglosc> _OdleglosciPunktow;
    private List<int> _PunktyOdwiedzoneTSP;
    private int _Wynik;
    
    public D24Z02(bool daneTestowe = false)
    {
        this._MapaZnaki = new ();
        this._MapaWartosci = new ();
        this._PunktyDoOdwiedzenia = new ();
        this._PunktyOdwiedzoneTSP = new ();
        this._OdleglosciPunktow = new ();
        this._Wynik = int.MaxValue;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\24\\proba.txt" : ".\\Dane\\2016\\24\\dane.txt", FileMode.Open, FileAccess.Read);
		StreamReader sr = new(fs);
        string linia;
        int wysokosc = 0;

        while((linia = sr.ReadLine()) is not null)
        {
            this._MapaZnaki.Add(linia.ToCharArray());

            for(int szerokosc = 0; szerokosc < linia.Length; szerokosc++)
            {
                if(char.IsDigit(linia[szerokosc]))
                {
                    this._PunktyDoOdwiedzenia.Add(new (szerokosc, wysokosc, linia[szerokosc] - '0'));
                    this._MapaWartosci.Punkty.Add(new (szerokosc, wysokosc, linia[szerokosc] - '0'));
                    continue;
                }
                this._MapaWartosci.Punkty.Add(new (szerokosc, wysokosc));
            }
            wysokosc++;
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        int tmp;

        for(int punktPoczatkowy = 0; punktPoczatkowy < this._PunktyDoOdwiedzenia.Count; punktPoczatkowy++)
        {
            for(int punktKoncowy = 0; punktKoncowy < this._PunktyDoOdwiedzenia.Count; punktKoncowy++)
            {
                if (punktPoczatkowy != punktKoncowy && !(this._OdleglosciPunktow.Where(op => (op.IdStart == punktPoczatkowy && op.IdStop == punktKoncowy) || (op.IdStart == punktKoncowy && op.IdStop == punktPoczatkowy)).Count() == 2))
                {
                    tmp = this.A_Gwiazdka(this._PunktyDoOdwiedzenia.First(p => p.PunktNazwa == punktPoczatkowy), this._PunktyDoOdwiedzenia.First(p => p.PunktNazwa == punktKoncowy));
                    this._OdleglosciPunktow.Add(new (punktPoczatkowy, punktKoncowy, tmp));
                    this._OdleglosciPunktow.Add(new (punktKoncowy, punktPoczatkowy, tmp));
                }
            }
        }

        this.TSP(0, 0);
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private int A_Gwiazdka(Punkt punktPoczatkowy, Punkt punktKoncowy)
    {
        Punkt punktObecny, sasiad;

        double fScoreTMP;

        List<Punkt> punktyDoSprawdzenia = [ punktPoczatkowy ];
        HashSet<Punkt> punktySprawdzone = new ();

        Dictionary<int, double> gScore = new () { [punktPoczatkowy.PunktId] = 0 };
        Dictionary<int, double> hScore = new () { [punktPoczatkowy.PunktId] = Manhattan(punktPoczatkowy, punktKoncowy) };
        Dictionary<int, Punkt> nadrzednaMapaPunktow = new ();

        while (punktyDoSprawdzenia.Count > 0)
        {
            punktObecny = punktyDoSprawdzenia.OrderBy(p => gScore[p.PunktId] + hScore[p.PunktId]).First();

            if(punktObecny.PunktNazwa == punktKoncowy.PunktNazwa)
            {
                return ZwrocSciezke(nadrzednaMapaPunktow, punktObecny);
            }

            punktyDoSprawdzenia.Remove(punktObecny);
            punktySprawdzone.Add(punktObecny);

            this.ZnajdzSasiadow(punktObecny);

            foreach (int punktId in punktObecny.Sasiedzi.Keys)
            {
                sasiad = this._MapaWartosci.PobierzPunktPoId(punktId);
                if (sasiad is null || punktySprawdzone.Contains(sasiad))
                {
                    continue;
                }

                fScoreTMP = gScore[punktObecny.PunktId] + punktObecny.Sasiedzi[punktId];

                if (!gScore.TryGetValue(sasiad.PunktId, out _) || fScoreTMP < gScore[sasiad.PunktId])
                {
                    gScore[sasiad.PunktId] = fScoreTMP;
                    hScore[sasiad.PunktId] = Manhattan(sasiad, punktKoncowy);

                    nadrzednaMapaPunktow[sasiad.PunktId] = punktObecny;

                    if (!punktyDoSprawdzenia.Contains(sasiad))
                    {
                        punktyDoSprawdzenia.Add(sasiad);
                    }
                }
            }
        }

        return 0;
    }

    private void ZnajdzSasiadow(Punkt obecnyPunkt)
    {
        if(!this._MapaZnaki[obecnyPunkt.Y - 1][obecnyPunkt.X].Equals('#'))
        {
            obecnyPunkt.Sasiedzi.TryAdd(1000 * (obecnyPunkt.X + 1) + 10*(obecnyPunkt.Y - 1 + 1), 10);
        }

        if(!this._MapaZnaki[obecnyPunkt.Y + 1][obecnyPunkt.X].Equals('#'))
        {
            obecnyPunkt.Sasiedzi.TryAdd(1000 * (obecnyPunkt.X + 1) + 10*(obecnyPunkt.Y + 1 + 1), 10);
        }

        if(!this._MapaZnaki[obecnyPunkt.Y][obecnyPunkt.X - 1].Equals('#'))
        {
            obecnyPunkt.Sasiedzi.TryAdd(1000 * (obecnyPunkt.X - 1 + 1) + 10*(obecnyPunkt.Y + 1), 10);
        }

        if(!this._MapaZnaki[obecnyPunkt.Y][obecnyPunkt.X + 1].Equals('#'))
        {
            obecnyPunkt.Sasiedzi.TryAdd(1000 * (obecnyPunkt.X + 1 + 1) + 10*(obecnyPunkt.Y + 1), 10);
        }
    }

    private double Manhattan(Punkt obecnyPunkt, Punkt koncowyPunkt)
    {
        return Math.Abs(obecnyPunkt.X - koncowyPunkt.X) + Math.Abs(obecnyPunkt.Y - koncowyPunkt.Y);
    }

    private int ZwrocSciezke(Dictionary<int, Punkt> nadrzednaMapaPunktow, Punkt obecny)
    {
        List<Punkt> sciezka = [];

        while (nadrzednaMapaPunktow.ContainsKey(obecny.PunktId))
        {
            obecny = nadrzednaMapaPunktow[obecny.PunktId];
            sciezka.Add(obecny);
        }

        return sciezka.Count;
    }

    private void TSP(int punktId, int odleglosc)
    {
        if(odleglosc > this._Wynik)
        {
            return;
        }

        List<Odleglosc> odleglosciOdPunktId = this._OdleglosciPunktow.Where(op => op.IdStart == punktId).OrderBy(op => op.OdlegloscPunktow).ToList<Odleglosc>();
        odleglosciOdPunktId.RemoveAll(kds => this._PunktyOdwiedzoneTSP.Select(oi => oi).Contains(kds.IdStop));

        if (odleglosciOdPunktId.Count == 0)
        {
            for(int i = 1; i < this._PunktyDoOdwiedzenia.Count - 1; i++)
            {
                this._Wynik = Math.Min(odleglosc + this._OdleglosciPunktow.Where(op => op.IdStart == punktId && op.IdStop == 0).ToArray<Odleglosc>()[0].OdlegloscPunktow, this._Wynik);
            }
            return;
        }
        
        foreach(Odleglosc op in odleglosciOdPunktId)
        {
            this._PunktyOdwiedzoneTSP.Add(op.IdStart);
            this.TSP(op.IdStop, odleglosc + op.OdlegloscPunktow);
            this._PunktyOdwiedzoneTSP.Remove(op.IdStart);
        }
    }

    private class Punkt
    {
        //public string PunktId { get; private set; }
        public int PunktId { get; private set; }
        public int PunktNazwa { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Dictionary<int, double> Sasiedzi { get; set; } // PunktId, Odleglosc

        public Punkt(int x, int y, int nazwaPunktu = -1)
        {
            //this.PunktId = $"X{x}Y{y}";
            this.PunktId = 1000 * (x + 1) + 10*(y + 1);
            this.X = x;
            this.Y = y;
            this.PunktNazwa = nazwaPunktu;
            this.Sasiedzi = [];
        }
    }

    private class MapaWartosci
    {
        public HashSet<Punkt> Punkty { get; set; } = [];

        // Retrieve a node by ID
        public Punkt PobierzPunktPoId(int nodeId)
        {
            return Punkty.FirstOrDefault(node => node.PunktId == nodeId);
        }
    }

    private record Odleglosc(int IdStart, int IdStop, int OdlegloscPunktow);
}