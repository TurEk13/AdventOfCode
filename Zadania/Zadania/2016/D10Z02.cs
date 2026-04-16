using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D10Z02 : IZadanie
{
    private Dictionary<int, Wyjscie> _Wyjscia;
    private List<string[]> _Instrukcje;
    private Dictionary<int, Bot> _Boty;
    private int _Wynik;
    private int _H;
    private int _L;
    private bool KoniecPetli
    {
        get
        {
           if(this._Wyjscia.TryGetValue(0, out Wyjscie w1) && this._Wyjscia.TryGetValue(1, out Wyjscie w2) && this._Wyjscia.TryGetValue(2, out Wyjscie w3))
            {
                this._Wynik = w1.Chip * w2.Chip * w3.Chip;
                return true;
            }

            return false; 
        }
    }

    public D10Z02(bool daneTestowe = false)
    {
        this._Wynik = -1;
        this._Boty = new ();
        this._Instrukcje = new ();
        this._Wyjscia = new ();

        this._L = daneTestowe ? 2 : 17;
        this._H = daneTestowe ? 5 : 61;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\10\\proba.txt" : ".\\Dane\\2016\\10\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        
        this._Instrukcje = sr.ReadToEnd().Split(Environment.NewLine).OrderBy(i => i).Select(wiersz => wiersz.Split(' ')).ToList<string[]>();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        //this._Instrukcje = this._Instrukcje.OrderBy(i => i).ToList();
        // 8 113+
        // 7 847
        do
        {
            
            for(int i = 0; i < this._Instrukcje.Count; i++)
            {
                switch(this._Instrukcje[i][0])
                {
                    case "value":
                        this.DodajWartoscDoBota(Id: Convert.ToInt32(this._Instrukcje[i][^1]), wartosc: Convert.ToInt32(this._Instrukcje[i][1]));
                        break;   
                    case "bot":
                    default:
                        this.PodzielWartosci(i);
                        break;
                }
            }
        }
        while(!this.KoniecPetli);
    }

    private bool IdBota(int Id)
    {
        if(this._Boty[Id].Chipy.Min() == this._L && this._Boty[Id].Chipy.Max() == this._H)
        {
            return true;
        }

        return false;
    }

    private void PodzielWartosci(int liniaId)
    {
        int botId = Convert.ToInt32(this._Instrukcje[liniaId][1]);
        int celLow, celHigh;

        if(!this._Boty.TryGetValue(botId, out _))
        {
            this._Boty.Add(botId, new Bot(botId, []));
            return;
        }

        if(this.IdBota(botId))
        {
            Debug.WriteLine(botId);
        }

        if(this._Boty[botId].Chipy.Count < 2)
        {
            return;
        }

        celLow = Convert.ToInt32(this._Instrukcje[liniaId][6]);
        celHigh = Convert.ToInt32(this._Instrukcje[liniaId][^1]);

        switch(this._Instrukcje[liniaId][5], this._Instrukcje[liniaId][^2])
        {
            case ("output", "output"):
                this.DodajWartoscDoWyjscia(Id: celLow, wartosc: this._Boty[botId].Chipy.Min());
                this.DodajWartoscDoWyjscia(Id: celHigh, wartosc: this._Boty[botId].Chipy.Max());
                this._Boty[botId].Chipy.Clear();
                break;
            case("output", "bot"):
                this.DodajWartoscDoWyjscia(Id: celLow, wartosc: this._Boty[botId].Chipy.Min());
                this.DodajWartoscDoBota(Id: celHigh, wartosc: this._Boty[botId].Chipy.Max());
                this._Boty[botId].Chipy.Clear();
                break;
            case("bot", "output"):
                this.DodajWartoscDoBota(Id: celLow, wartosc: this._Boty[botId].Chipy.Min());
                this.DodajWartoscDoWyjscia(Id: celHigh, wartosc: this._Boty[botId].Chipy.Max());
                this._Boty[botId].Chipy.Clear();
                break;
            case("bot", "bot"):
                this.DodajWartoscDoBota(Id: celLow, wartosc: this._Boty[botId].Chipy.Min());
                this.DodajWartoscDoBota(Id: celHigh, wartosc: this._Boty[botId].Chipy.Max());
                this._Boty[botId].Chipy.Clear();
                break;
        }
    }

    private void DodajWartoscDoBota(int Id, int wartosc)
    {
        if(!this._Boty.TryAdd(Id, new Bot(Id, new List<int>())))
        {
            if(this._Boty[Id].Chipy.Count == 2)
            {
                if(this._Boty[Id].Chipy.Max() > wartosc)
                {
                    this._Boty[Id].Chipy.Remove(this._Boty[Id].Chipy.Min());
                }
                else
                {
                    this._Boty[Id].Chipy.Remove(this._Boty[Id].Chipy.Max());
                }
            }
        }

        this._Boty[Id].Chipy.Add(wartosc); 
    }

    private void DodajWartoscDoWyjscia(int Id, int wartosc)
    {
        if(!this._Wyjscia.TryAdd(Id, new (Id, wartosc)))
        {
            this._Wyjscia[Id] = new (Id, wartosc);
        }
    }

    public string PokazRozwiazanie()
    {
        return $"7 849: {this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"))}";
    }

    private record Bot(int Id, List<int> Chipy);
    private record Wyjscie(int Id, int Chip);
}