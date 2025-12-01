using System;
using System.Globalization;

namespace Zadania._2015;

public class D21Z02 : IZadanie
{
    private StatystykiPostaci Gracz;
    private StatystykiPostaci Boss;
    private StatystykiBroni[] SpisBroni;
    private StatystykiZbroi[] SpisZbroi;
    private StatystykiPierscieni[] SpisPierscieni;
    private int ZuzyteZloto;
    public D21Z02()
    {
        this.SpisBroni = [new (8, 4, 0), new (10, 5, 0), new (25, 6, 0), new (40, 7, 0), new (74, 8, 0)];
        this.SpisZbroi = [new (0, 0, 0), new (13, 0, 1), new (31, 0, 2), new (53, 0, 3), new (75, 0, 4), new (102, 0, 5)];
        this.SpisPierscieni = [new (0, 0, 0), new (0, 0, 0), new (25, 1, 0), new (50, 2, 0), new (100, 0, 3), new (20, 0, 1), new (40, 0, 2), new (80, 0, 3)];
    }

    public void RozwiazanieZadania()
    {
        int uzyteZloto;
        this.ZuzyteZloto = int.MinValue;

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
                        this.UtworzGracza(b, z, p1, p2);
                        uzyteZloto = this.WydaneZloto(b, z, p1, p2);

                        while(this.Gracz.PunktyZycia > 0 && this.Boss.PunktyZycia > 0)
                        {
                            this.WykonajTure();
                        }

                        if(this.Gracz.PunktyZycia <= 0 && this.Boss.PunktyZycia > 0 && this.ZuzyteZloto < uzyteZloto)
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
        int obrazeniaBossa = Math.Max(1, this.Gracz.Obrazenia - this.Boss.Obrona);
        this.Boss = this.Boss with { PunktyZycia = this.Boss.PunktyZycia - obrazeniaBossa };

        if(this.Boss.PunktyZycia < 1)
        {
            return;
        }

        int obrazeniaGracza = Math.Max(1, this.Boss.Obrazenia - this.Gracz.Obrona);

        this.Gracz = this.Gracz with { PunktyZycia = this.Gracz.PunktyZycia - obrazeniaGracza };
    }

    private void UtworzGracza(int Bron, int Zbroja, int Pierscien1, int Pierscien2)
    {
        this.Gracz = new (
            100,
            this.SpisBroni[Bron].Obrazenia + this.SpisPierscieni[Pierscien1].Obrazenia + this.SpisPierscieni[Pierscien2].Obrazenia,
            this.SpisZbroi[Zbroja].Obrona + this.SpisPierscieni[Pierscien1].Obrona + this.SpisPierscieni[Pierscien2].Obrona
        );
    }

    private int WydaneZloto(int Bron, int Zbroja, int Pierscien1, int Pierscien2)
    {
        return this.SpisBroni[Bron].Koszt + this.SpisZbroi[Zbroja].Koszt + this.SpisPierscieni[Pierscien1].Koszt + this.SpisPierscieni[Pierscien2].Koszt;
    }

    public string PokazRozwiazanie()
    {
        return this.ZuzyteZloto.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record StatystykiPostaci(int PunktyZycia, int Obrazenia, int Obrona);
    record StatystykiBroni(int Koszt, int Obrazenia, int Obrona);
    record StatystykiZbroi(int Koszt, int Obrazenia, int Obrona);
    record StatystykiPierscieni(int Koszt, int Obrazenia, int Obrona);
}