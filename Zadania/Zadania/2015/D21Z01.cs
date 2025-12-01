using System.Diagnostics;
using System.Globalization;

namespace Zadania._2015;

public class D21Z01 : IZadanie
{
    private StatystykiPostaci Gracz;
    private StatystykiPostaci Boss;
    private StatystykiBroni[] SpisBroni;
    private StatystykiZbroi[] SpisZbroi;
    private StatystykiPierscieniAtaku[] SpisPierscieni;
    private int ZuzyteZloto;
    public D21Z01(bool daneTestowe = false)
    {
        this.SpisBroni = [new (8, 4, 0), new (10, 5, 0), new (25, 6, 0), new (40, 7, 0), new (74, 8, 0)];
        this.SpisZbroi = [new (0, 0, 0), new (13, 0, 1), new (31, 0, 2), new (53, 0, 3), new (75, 0, 4), new (102, 0, 5)];
        this.SpisPierscieni = [new (0, 0, 0), new (0, 0, 0), new (25, 1, 0), new (50, 2, 0), new (100, 0, 3), new (20, 0, 1), new (40, 0, 2), new (80, 0, 3)];
        this.ZuzyteZloto = int.MaxValue;
    }

    public void RozwiazanieZadania()
    {
        int uzyteZloto;

        for(int b = 0; b < this.SpisBroni.Length; b++)
        {
            for(int z = 0; z < this.SpisZbroi.Length; z++)
            {
                for(int p1 = 0; p1 < this.SpisPierscieni.Length; p1++)
                {
                    for(int p2 = 0; p2 < SpisPierscieni.Length; p2++)
                    {
                        if(p1 == p2)
                        {
                            continue;
                        }

                        this.Boss = new (103, 9, 2);

                        this.Gracz = new (100, this.SpisBroni[b].Obrazenia + this.SpisPierscieni[p1].Obrazenia + this.SpisPierscieni[p2].Obrazenia, this.SpisZbroi[z].Obrona + this.SpisPierscieni[p1].Obrona + this.SpisPierscieni[p2].Obrona);

                        uzyteZloto = this.SpisBroni[b].Koszt + this.SpisZbroi[z].Koszt + this.SpisPierscieni[p1].Koszt + this.SpisPierscieni[p2].Koszt;

                        while(this.Gracz.PunktyZycia > 0 && this.Boss.PunktyZycia > 0)
                        {
                            this.WykonajTure();
                        }

                        if(this.Gracz.PunktyZycia > 0)
                        {
                            Debug.WriteLine($"Gracz wygrał: {this.ZuzyteZloto}");
                        }

                        if(this.Boss.PunktyZycia > 0)
                        {
                            Debug.WriteLine("Boss wygrał");
                        }

                        if(this.Gracz.PunktyZycia > 0 && this.Boss.PunktyZycia < 0 &&this.ZuzyteZloto > uzyteZloto)
                        {
                            this.ZuzyteZloto = uzyteZloto;
                        }
                    }
                }
            }
        }
    }

    private void WykonajTure()
    {
        int obrazeniaBoss = this.Gracz.Obrazenia - this.Boss.Obrona;
        this.Boss = obrazeniaBoss == 0 ? this.Boss with { PunktyZycia = this.Boss.PunktyZycia - 1 } : this.Boss with { PunktyZycia = this.Boss.PunktyZycia - obrazeniaBoss };
        if(this.Boss.PunktyZycia < 1)
        {
            return;
        }

        int obrazeniaGracz = this.Boss.Obrazenia - this.Gracz.Obrona;
        this.Gracz = obrazeniaGracz == 0 ? this.Gracz with { PunktyZycia = this.Gracz.PunktyZycia - 1 } : this.Gracz with { PunktyZycia = this.Gracz.PunktyZycia - obrazeniaGracz };
    }

    public string PokazRozwiazanie()
    {
        return this.ZuzyteZloto.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record StatystykiPostaci(int PunktyZycia, int Obrazenia, int Obrona);
    record StatystykiBroni(int Koszt, int Obrazenia, int Obrona);
    record StatystykiZbroi(int Koszt, int Obrazenia, int Obrona);
    record StatystykiPierscieniObrony(int Koszt, int Obrazenia, int Obrona);
    record StatystykiPierscieniAtaku(int Koszt, int Obrazenia, int Obrona = 0);
}