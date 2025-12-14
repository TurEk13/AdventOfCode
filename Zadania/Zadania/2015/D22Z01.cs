using System.Diagnostics;
using System.Globalization;

namespace Zadania._2015;

public class D22Z01 : IZadanie
{
    private Gracz _Gracz;
    private Boss _Boss;
    private Czary _Czary;

    public D22Z01(bool daneTestowe = false)
    {
        this._Gracz = new (10, 250);
        this._Boss = new (13, 8);
        this._Czary = new ();
    }

    public void RozwiazanieZadania()
    {
        while(this._Gracz.CzyZyje)
        {
            this._Czary.RzucCzar(ref this._Gracz);
            this._Czary.UzyjCzar(ref this._Gracz, ref this._Boss);

            if(!this._Boss.CzyZyje)
            {
                Debug.WriteLine("Boss przegrał");
                return;
            }

            this._Gracz.OtrzymaneObrazenia(this._Boss.Obrazenia);
            this._Czary.UzyjCzar(ref this._Gracz, ref this._Boss);
            this._Czary.RzucCzar(ref this._Gracz);

            if (!this._Boss.CzyZyje)
            {
                Debug.WriteLine("Boss przegrał");
                return;
            }
        }

        if (!this._Gracz.CzyZyje)
        {
            Debug.WriteLine("Boss wygrał");
        }
    }

    public string PokazRozwiazanie()
    {
        return 0.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Gracz
    {
        public int PunktyZycia { get; private set; }
        public int Mana {  get; private set; }
        public int Obrona { get; private set; }
        public bool CzyZyje { get { return this.PunktyZycia > 0; } }

        public Gracz(int punktyZycia, int Mana)
        {
            this.PunktyZycia = punktyZycia;
            this.Mana = Mana;
            this.Obrona = 0;
        }

        public void OdejmijMane(int Koszt)
        {
            this.Mana -= Koszt;
        }

        public void DodajMane(int Ilosc)
        {
            this.Mana += Ilosc;
        }

        public void OtrzymaneObrazenia(int Obrazenia)
        {
            this.PunktyZycia -= Obrazenia;
        }

        public void DodajPunktyZycia(int Ilosc)
        {
            this.PunktyZycia += Ilosc;
        }

        public void DodajObrone(int Ilosc)
        {
            this.Obrona += Ilosc;
        }

        public void ZerujObrone()
        {
            this.Obrona = 0;
        }
    }

    private class Boss
    {
        public int PunktyZycia { get; private set; }
        public int Obrazenia { get; private set; }
        public bool CzyZyje { get { return this.PunktyZycia > 0; } }

        public Boss(int punktyZycia, int obrazenia)
        {
            this.PunktyZycia = punktyZycia;
            this.Obrazenia = obrazenia;
        }

        public void OtrzymaneObrazenia(int obrazenia)
        {
            this.PunktyZycia -= obrazenia;
        }
    }

#nullable enable
    private class Czary
    {
        private MagicMissile? _magicMissile;
        private Drain? _Drain;
        private Shield? _Shield;
        private Poison? _Poison;
        private Recharge? _Recharge;

        public void RzucCzar(ref Gracz gracz)
        {
            if (this._Poison is null)
            {
                this._Poison = new();
                gracz.OdejmijMane(this._Poison.KosztMany);
                return;
            }

            if (this._magicMissile is null)
            {
                this._magicMissile = new ();
                gracz.OdejmijMane(this._magicMissile.KosztMany);
                return;
            }

            if(this._Drain is null)
            {
                this._Drain = new ();
                gracz.OdejmijMane(this._Drain.KosztMany);
                return;
            }

            if(this._Shield is null)
            {
                this._Shield = new ();
                gracz.OdejmijMane (this._Shield.KosztMany);
                return;
            }

            if(this._Recharge is null)
            {
                this._Recharge = new ();
                gracz.OdejmijMane(this._Recharge.KosztMany);
                return;
            }
        }

        public void UzyjCzar(ref Gracz gracz, ref Boss boss)
        {
            if(this._magicMissile is not null)
            {
                boss.OtrzymaneObrazenia(this._magicMissile.Obrazenia);
                this._magicMissile = null;
            }

            if(this._Drain is not null)
            {
                boss.OtrzymaneObrazenia(this._Drain.Obrazenia);
                gracz.DodajPunktyZycia(this._Drain.DodanieZycia);
                this._Drain = null;
            }

            if(this._Shield is not null)
            {
                gracz.DodajObrone(this._Shield.Obrona);
                this._Shield = this._Shield with { Tura = this._Shield.Tura + 1 };

                if(this._Shield.Trwanie == this._Shield.Tura)
                {
                    this._Shield = null;
                }
            }

            if(this._Poison is not null)
            {
                boss.OtrzymaneObrazenia(this._Poison.Obrazenia);
                this._Poison = this._Poison with { Tura = this._Poison.Tura + 1 };

                if(this._Poison.Trwanie == this._Poison.Tura)
                {
                    this._Poison = null;
                }
            }

            if (this._Recharge is not null)
            {
                gracz.DodajMane(this._Recharge.DodanieMany);
                this._Recharge = this._Recharge with { Tura = this._Recharge.Tura + 1 };

                if (this._Recharge.Trwanie == this._Recharge.Tura)
                {
                    this._Recharge = null;
                }
            }
        }

        private record MagicMissile(int KosztMany = 53, int Obrazenia = 4);
        private record Drain(int KosztMany = 73, int Obrazenia = 2, int DodanieZycia = 2);
        private record Shield(int KosztMany = 113, int Trwanie = 6, int Obrona = 7, int Tura = 0);
        private record Poison(int KosztMany = 173, int Obrazenia = 3, int Trwanie = 6, int Tura = 0);
        private record Recharge(int KosztMany = 229, int Trwanie = 5, int DodanieMany = 101, int Tura = 0);
    }
#nullable disable
}