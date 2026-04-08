using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        this._H = 61;
        this._L = 17;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\10\\proba.txt" : ".\\Dane\\2016\\10\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        
        this._Instrukcje = sr.ReadToEnd().Split(Environment.NewLine).ToList<string>();

        sr.Close(); fs!.Close();
    }

    private void SprawdzBota(int id)
    {
        if(this._Boty[id].H == this._H && this._Boty[id].L == this._L)
        {
            this._Wynik = id;
        }

        if(this._Boty[id].H == this._H || this._Boty[id].H == this._L || this._Boty[id].L == this._H || this._Boty[id].L == this._L)
        {
            Debug.WriteLine($"id: {id} - L: {this._Boty[id].L} | H: {this._Boty[id].H}");
        }
    }

    public void RozwiazanieZadania()
    {
        this.WczytajBoty();
        int LL, HH;
        
        while((HH = this._Boty.Count(b => b.Value.H > 0)) + (LL = this._Boty.Count(b => b.Value.L > 0)) > 0)
        {
            Debug.WriteLine($"HH + LL = {HH + LL}");

            this.OperacjeNaBotach();

            Debug.WriteLine("---------------------");

            if(this._Wynik != -1)
            {
                break;
            }
        }

        Debug.WriteLine("Koniec");
    }

    private void OperacjeNaBotach()
    {
        int botDzielacy, botPrzyjmujacy;
        foreach(string[] linia in this._Instrukcje.FindAll(i => i.StartsWith("bot")).Select(l => l.Split(" ")).ToArray())
        {
            botDzielacy = Convert.ToInt32(linia[1]);

            switch(linia[5])
            {
                case "bot":
                    botPrzyjmujacy = Convert.ToInt32(linia[6]);

                    if(!this._Boty.TryAdd(botPrzyjmujacy, new Bot(botPrzyjmujacy, 0, this._Boty[botDzielacy].L)))
                    {
                        this._Boty[botPrzyjmujacy] = this.UstawBota(botPrzyjmujacy, this._Boty[botDzielacy].L);
                        this.SprawdzBota(botPrzyjmujacy);
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
                        this.SprawdzBota(botPrzyjmujacy);
                        this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = 0};
                    }

                    break;
                case "output":
                    this._Wyjscia.TryAdd(Convert.ToInt32(linia[^1]), this._Boty[botDzielacy].H);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = 0};
                    break;
            }
        }
    }

    private Bot UstawBota(int id, int wartosc)
    {
        Bot b = this._Boty[id];

        return wartosc > b.H ? new Bot(id, b.H, wartosc) : new Bot(id, wartosc, b.H);
    }

    private void WczytajBoty()
    {
        int id, chip, maks;

        foreach(string linia in this._Instrukcje.FindAll(i => i.StartsWith("value")))
        {
            id = Convert.ToInt32(linia.Split(' ')[^1]);
            chip = Convert.ToInt32(linia.Split(' ')[1]);

            if(!this._Boty.TryAdd(id, new Bot(id, 0, chip)))
            {
                this._Boty.TryGetValue(id, out Bot bot);
                this._Boty[bot.Id] = chip > bot.H ? new Bot(id, bot.H, chip) : new Bot(id, chip, bot.H);
                this.SprawdzBota(id);
            }
        }

        maks = maks = this._Instrukcje.FindAll(i => i.StartsWith("bot")).Select(l => l.Split(' ')).ToList().Max(i => Convert.ToInt32(i[1]));

        this._Wyjscia = new ();

        for(int i = 0; i < maks + 1; i++)
        {
            this._Boty.TryAdd(i, new Bot(i, 0, 0));
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Bot(int Id, int L, int H);
}