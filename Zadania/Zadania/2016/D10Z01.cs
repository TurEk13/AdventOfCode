using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D10Z01 : IZadanie
{
    private Dictionary<int, Wyjscie> _Wyjscia;
    private List<string> _Instrukcje;
    private Dictionary<int, Bot> _Boty;
    private int _Wynik;
    private int _H;
    private int _L;

    public D10Z01(bool daneTestowe = false)
    {
        this._Wynik = -1;
        this._Boty = new ();
        this._Instrukcje = new ();
        this._Wyjscia = new ();

        this._L = daneTestowe ? 2 : 17;
        this._H = daneTestowe ? 5 : 61;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\10\\proba.txt" : ".\\Dane\\2016\\10\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        
        this._Instrukcje = sr.ReadToEnd().Split(Environment.NewLine).ToList<string>();

        sr.Close(); fs!.Close();

        int maksL = this._Instrukcje.Select(i => i.Split(' ')).Where(i => i[0].Equals("bot") && i[5].Equals("output")).Max(i => Convert.ToInt32(i[6]));
        int maksH = this._Instrukcje.Select(i => i.Split(' ')).Where(i => i[0].Equals("bot") && i[10].Equals("output")).Max(i => Convert.ToInt32(i[11]));
    }

    public void RozwiazanieZadania()
    {
        do
        {
            for(int i = 0; i < this._Instrukcje.Count; i++)
            {
                switch(this._Instrukcje[i].StartsWith("value"))
                {
                    case false:
                        this.PodzielChipy(i);
                        break;
                    case true:
                        this.WczytajBota(i);
                        break;        
                }
            }
        }
        while(this._Wynik == -1);
    }

    private void WczytajBota(int liniaId)
    {
        int botId = Convert.ToInt32(this._Instrukcje[liniaId].Split(' ')[^1]);
        int chip = Convert.ToInt32(this._Instrukcje[liniaId].Split(' ')[1]);

        if(!this._Boty.TryAdd(botId, new Bot(botId, -1, chip)))
        {
            this._Boty[botId] = this.UstawBota(botId, chip);
        }
    }

    private void PodzielChipy(int liniaId)
    {
        int botId, botNiskiChip, botWysokiChip;
        string[] linia = this._Instrukcje[liniaId].Split(" ");

        botId = Convert.ToInt32(linia[1]);

        if(!this._Boty.TryGetValue(botId, out Bot _))
        {
            this._Boty.TryAdd(botId, new Bot(botId, -1, -1));
            return;
        }

        if(!this.CzyBotMaChipy(botId))
        {
            return;
        }

        if(this.SprawdzBota(botId))
        {
            return;
        }

        switch(linia[5], linia[10])
        {
            case ("bot", "output"):
                botNiskiChip = Convert.ToInt32(linia[6]);
                if(!this._Boty.TryAdd(botNiskiChip, new Bot(botNiskiChip, 0, this._Boty[botId].L)))
                {
                    this._Boty[botNiskiChip] = this.UstawBota(botNiskiChip, this._Boty[botId].L);
                    this._Boty[botId] = this._Boty[botId] with { L = -1 };
                }

                this._Wyjscia.TryAdd(Convert.ToInt32(linia[11]), new Wyjscie(Convert.ToInt32(linia[11]), this._Boty[botId].H));
                this._Boty[botId] = this._Boty[botId] with { H = -1 };
                break;
            case ("output", "bot"):
                this._Wyjscia.TryAdd(Convert.ToInt32(linia[6]), new Wyjscie(Convert.ToInt32(linia[6]), this._Boty[botId].L));
                this._Boty[botId] = this._Boty[botId] with { L = -1 };

                botWysokiChip = Convert.ToInt32(linia[11]);
                if(!this._Boty.TryAdd(botWysokiChip, new Bot(botWysokiChip, 0, this._Boty[botId].H)))
                {
                    this._Boty[botWysokiChip] = this.UstawBota(botWysokiChip, this._Boty[botId].H);
                    this._Boty[botId] = this._Boty[botId] with { H = -1 };
                }
                break;
            case ("output", "output"):
                this._Wyjscia.TryAdd(Convert.ToInt32(linia[6]), new Wyjscie(Convert.ToInt32(linia[6]), this._Boty[botId].L));
                this._Boty[botId] = this._Boty[botId] with { L = -1 };

                this._Wyjscia.TryAdd(Convert.ToInt32(linia[11]), new Wyjscie(Convert.ToInt32(linia[11]), this._Boty[botId].H));
                this._Boty[botId] = this._Boty[botId] with { H = -1 };
                break;
            case ("bot", "bot"):
                botNiskiChip = Convert.ToInt32(linia[6]);
                botWysokiChip = Convert.ToInt32(linia[11]);

                if(!this._Boty.TryAdd(botNiskiChip, new Bot(botNiskiChip, 0, this._Boty[botId].L)))
                {
                    this._Boty[botNiskiChip] = this.UstawBota(botNiskiChip, this._Boty[botId].L);
                    this._Boty[botId] = this._Boty[botId] with { L = -1 };
                }

                if(!this._Boty.TryAdd(botWysokiChip, new Bot(botWysokiChip, 0, this._Boty[botId].H)))
                {
                    this._Boty[botWysokiChip] = this.UstawBota(botWysokiChip, this._Boty[botId].H);
                    this._Boty[botId] = this._Boty[botId] with { H = -1 };
                }
                break;
        }
    }

    private bool CzyBotMaChipy(int Id)
    {
        return this._Boty[Id].H != -1 && this._Boty[Id].L != -1;
    }

    private Bot UstawBota(int id, int wartosc)
    {
        return wartosc > this._Boty[id].H ? new Bot(id, this._Boty[id].H, wartosc) : new Bot(id, wartosc, this._Boty[id].H);
    }

    private bool SprawdzBota(int Id)
    {
        if(this._Boty[Id].H == this._H && this._Boty[Id].L == this._L)
        {
            this._Wynik = this._Boty[Id].Id;
            return true;
        }

        return false;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Bot(int Id, int L, int H);
    private record Wyjscie(int Id, int Wartosc);
}