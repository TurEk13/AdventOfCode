using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public class D10Z02 : IZadanie
{
    private Dictionary<int, Wyjscie> _Wyjscia;
    private List<string> _Instrukcje;
    private Dictionary<int, Bot> _Boty;
    private int _Wynik;
    private int _H;
    private int _L;

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
        
        this._Instrukcje = sr.ReadToEnd().Split(Environment.NewLine).ToList<string>();

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        // 267 173, 86 459+
        do
        {
            for(int i = 0; i < this._Instrukcje.Count; i++)
            {
                switch(this._Instrukcje[i].StartsWith("value"))
                {
                    case false:
                        this.OperacjeNaBocie(i);
                        break;
                    case true:
                        this.WczytajBota(i);
                        break;        
                }
            }
        }
        while(this.Sprawdz3Wyjscia());
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
        int botDzielacy, botPrzyjmujacyL, botPrzyjmujacyH;
        string[] linia = this._Instrukcje[liniaId].Split(" ");

        botDzielacy = Convert.ToInt32(linia[1]);

        if(!this._Boty.TryGetValue(botDzielacy, out Bot botIstniejacy))
        {
            this._Boty.TryAdd(botDzielacy, new Bot(botDzielacy, -1, -1));
            return;
        }

        if(!this.CzyBotMaChipy(botIstniejacy))
        {
            return;
        }

        if(this.SprawdzBota(botIstniejacy))
        {
            return;
        }

        switch(linia[5], linia[10])
        {
            case ("bot", "output"):
                botPrzyjmujacyL = Convert.ToInt32(linia[6]);
                if(!this._Boty.TryAdd(botPrzyjmujacyL, new Bot(botPrzyjmujacyL, 0, this._Boty[botDzielacy].L)))
                {
                    this._Boty[botPrzyjmujacyL] = this.UstawBota(botPrzyjmujacyL, this._Boty[botDzielacy].L);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { L = -1 };
                }

                this._Wyjscia.TryAdd(Convert.ToInt32(linia[11]), new Wyjscie(Convert.ToInt32(linia[11]), this._Boty[botDzielacy].H));
                this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = -1 };
                break;
            case ("output", "bot"):
                this._Wyjscia.TryAdd(Convert.ToInt32(linia[6]), new Wyjscie(Convert.ToInt32(linia[6]), this._Boty[botDzielacy].L));
                this._Boty[botDzielacy] = this._Boty[botDzielacy] with { L = -1 };

                botPrzyjmujacyH = Convert.ToInt32(linia[11]);
                if(!this._Boty.TryAdd(botPrzyjmujacyH, new Bot(botPrzyjmujacyH, 0, this._Boty[botDzielacy].H)))
                {
                    this._Boty[botPrzyjmujacyH] = this.UstawBota(botPrzyjmujacyH, this._Boty[botDzielacy].H);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = -1 };
                }
                break;
            case ("output", "output"):
                this._Wyjscia.TryAdd(Convert.ToInt32(linia[6]), new Wyjscie(Convert.ToInt32(linia[6]), this._Boty[botDzielacy].L));
                this._Boty[botDzielacy] = this._Boty[botDzielacy] with { L = -1 };

                this._Wyjscia.TryAdd(Convert.ToInt32(linia[11]), new Wyjscie(Convert.ToInt32(linia[11]), this._Boty[botDzielacy].H));
                this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = -1 };
                break;
            case ("bot", "bot"):
                botPrzyjmujacyL = Convert.ToInt32(linia[6]);
                botPrzyjmujacyH = Convert.ToInt32(linia[11]);

                if(!this._Boty.TryAdd(botPrzyjmujacyL, new Bot(botPrzyjmujacyL, 0, this._Boty[botDzielacy].L)))
                {
                    this._Boty[botPrzyjmujacyL] = this.UstawBota(botPrzyjmujacyL, this._Boty[botDzielacy].L);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { L = -1 };
                }

                if(!this._Boty.TryAdd(botPrzyjmujacyH, new Bot(botPrzyjmujacyH, 0, this._Boty[botDzielacy].H)))
                {
                    this._Boty[botPrzyjmujacyH] = this.UstawBota(botPrzyjmujacyH, this._Boty[botDzielacy].H);
                    this._Boty[botDzielacy] = this._Boty[botDzielacy] with { H = -1 };
                }
                break;
        }
    }

    private bool CzyBotMaChipy(Bot bot)
    {
        return bot.H != -1 && bot.L != -1;
    }

    private Bot UstawBota(int id, int wartosc)
    {
        Bot b = this._Boty[id];

        /*if(b.H == wartosc || b.L == wartosc)
        {
            return b;
        }*/

        return wartosc > b.H ? new Bot(id, b.H, wartosc) : new Bot(id, wartosc, b.H);
    }

    private bool SprawdzBota(Bot bot)
    {
        if(bot.H == this._H && bot.L == this._L)
        {
            //this._Wynik = bot.Id;
            return true;
        }

        return false;
    }

    private bool Sprawdz3Wyjscia()
    {
        if(this._Wyjscia.TryGetValue(0, out Wyjscie w1) && this._Wyjscia.TryGetValue(1, out Wyjscie w2) && this._Wyjscia.TryGetValue(2, out Wyjscie w3))
        {
            this._Wynik = w1.Wartosc * w2.Wartosc * w3.Wartosc;
            return false;
        }

        return true;
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Bot(int Id, int L, int H);
    private record Wyjscie(int Id, int Wartosc);
}