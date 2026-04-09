using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D10Z01 : IZadanie
{
    private SortedDictionary<int, int> _Wyjscia;
    private List<string> _Instrukcje;
    private SortedDictionary<int, Bot> _Boty;
    private int _Wynik;
    private int _H;
    private int _L;

    public D10Z01(bool daneTestowe = false)
    {
        this._Wynik = -1;
        this._Boty = new ();
        this._Instrukcje = new ();
        this._Wyjscia = new ();
        this._H = 61;
        this._L = 17;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\10\\proba.txt" : ".\\Dane\\2016\\10\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        
        this._Instrukcje = sr.ReadToEnd().Split(Environment.NewLine).ToList<string>();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {   
        do
        {
            for(int i = 0; i < this._Instrukcje.Count; i++)
            {
                if(this._Instrukcje[i].StartsWith("value"))
                {
                    this.WczytajBota(i);
                    continue;
                }

                if(!this._Instrukcje[i].StartsWith("value"))
                {
                    this.OperacjeNaBocie(i);
                }
            }
        }
        while(this._Wynik == -1);
    }

    private void WczytajBota(int liniaId)
    {
        int id = Convert.ToInt32(this._Instrukcje[liniaId].Split(' ')[^1]);
        int chip = Convert.ToInt32(this._Instrukcje[liniaId].Split(' ')[1]);

        if(!this._Boty.TryAdd(id, new Bot(id, -1, chip)))
        {
            this._Boty[id] = this.UstawBota(id, chip);
        }
    }

    private void OperacjeNaBocie(int liniaId)
    {
        int botDzielacy, botPrzyjmujacy;
        string[] linia = this._Instrukcje[liniaId].Split(" ");

        botDzielacy = Convert.ToInt32(linia[1]);

        if(!this._Boty.TryGetValue(botDzielacy, out Bot _))
        {
            this._Boty.TryAdd(botDzielacy, new Bot(botDzielacy, -1, -1));
            return;
        }

        if(!this.CzyBotMaChipy(botDzielacy))
        {
            return;
        }

        if(this.SprawdzBota(botDzielacy))
        {
            return;
        }

        switch(linia[5])
        {
            case "bot":
                botPrzyjmujacy = Convert.ToInt32(linia[6]);

                if(!this._Boty.TryAdd(botPrzyjmujacy, new Bot(botPrzyjmujacy, 0, this._Boty[botDzielacy].L)))
                {
                    this._Boty[botPrzyjmujacy] = this.UstawBota(botPrzyjmujacy, this._Boty[botDzielacy].L);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { L = 0};
                }
                break;
            case "output":
                this._Wyjscia.TryAdd(Convert.ToInt32(linia[6]), this._Boty[botDzielacy].L);
                this._Boty[botDzielacy] = this._Boty[botDzielacy] with { L = 0};
                break;
        }

        switch(linia[^2])
        {
            case "bot":
                botPrzyjmujacy = Convert.ToInt32(linia[^1]);

                if(!this._Boty.TryAdd(botPrzyjmujacy, new Bot(botPrzyjmujacy, 0, this._Boty[botDzielacy].H)))
                {
                    this._Boty[botPrzyjmujacy] = this.UstawBota(botPrzyjmujacy, this._Boty[botDzielacy].H);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = 0};
                }

                break;
            case "output":
                this._Wyjscia.TryAdd(Convert.ToInt32(linia[^1]), this._Boty[botDzielacy].H);
                this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = 0};
                break;
        }
    }

    private bool CzyBotMaChipy(int id)
    {
        return this._Boty[id].H != -1 && this._Boty[id].L != -1;
    }

    private Bot UstawBota(int id, int wartosc)
    {
        Bot b = this._Boty[id];

        if(b.H == wartosc || b.L == wartosc)
        {
            return b;
        }

        return wartosc > b.H ? new Bot(id, b.H, wartosc) : new Bot(id, wartosc, b.H);
    }

    private bool SprawdzBota(int id)
    {
        if(this._Boty[id].H == this._H && this._Boty[id].L == this._L)
        {
            this._Wynik = id;
            return true;
        }

        return false;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Bot(int Id, int L, int H);
}