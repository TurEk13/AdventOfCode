using System;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public partial class D19Z02 : IZadanie
{
    private Elfy _ListaElfow;

    public D19Z02(bool daneTestowe = false)
    {
        this._ListaElfow = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\19\\proba.txt" : ".\\Dane\\2016\\19\\dane.txt", FileMode.Open, FileAccess.Read);       
		StreamReader sr = new(fs);
        bool enp;

        int IleElfow = Convert.ToInt32(sr.ReadToEnd());

        for(Int32 i = 0; i < IleElfow;  i++)
        {
            enp = i == IleElfow / 2;
            this._ListaElfow.Dodaj(new Elf()
            {
                Id = i + 1,
                IlePrezentow = 1
            }, enp);
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        while(this._ListaElfow.Usun());
    }

    public string PokazRozwiazanie()
    {
        return this._ListaElfow.Id.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Elfy
    {
        public static int IleElfow { get; private set; }
        private Elf ElfObecny;
        private Elf ElfPoczatkowy;
        private Elf ElfKoncowy;
        private Elf ElfNaPrzeciwko;

        static Elfy()
        {
            IleElfow = 0;
        }

        public void Dodaj(Elf elf, bool enp)
        {
            if(IleElfow == 0)
            {
                ElfObecny = elf;
                ElfPoczatkowy = elf;
                ElfKoncowy = elf;
                elf.ElfNastepny = elf;
                elf.ElfPoprzedni = elf;
            }
            else if(ElfPoczatkowy == ElfKoncowy)
            {
                elf.ElfNastepny = ElfPoczatkowy;
                ElfPoczatkowy.ElfNastepny = elf;
                elf.ElfPoprzedni = ElfKoncowy;
                ElfKoncowy = elf;
            }
            else
            {
                elf.ElfPoprzedni = ElfKoncowy;
                elf.ElfNastepny = ElfPoczatkowy;
                ElfKoncowy.ElfNastepny = elf;
                ElfKoncowy = elf;

                if(enp)
                {
                    ElfNaPrzeciwko = elf;
                }
            }

            IleElfow++;
        }

        public bool Usun()
        {
            if(this.ElfObecny.ElfNastepny != this.ElfObecny)
            {
                this.ElfObecny.IlePrezentow += this.ElfNaPrzeciwko.IlePrezentow;
                this.ElfNaPrzeciwko.ElfPoprzedni.ElfNastepny = this.ElfNaPrzeciwko.ElfNastepny;
                this.ElfNaPrzeciwko.ElfNastepny.ElfPoprzedni = this.ElfNaPrzeciwko.ElfPoprzedni;
                this.ElfNaPrzeciwko = (IleElfow % 2 == 1) ? this.ElfNaPrzeciwko.ElfNastepny.ElfNastepny : this.ElfNaPrzeciwko.ElfNastepny;

                this.ElfObecny = this.ElfObecny.ElfNastepny;
                IleElfow--;

                return true;
            }
            return false;
        }

        public int Id
        {
            get
            {
                return this.ElfObecny.Id;
            }
        }
    }

    private record Elf
    {
        public int Id;
        public int IlePrezentow;
        public Elf ElfPoprzedni;
        public Elf ElfNastepny;
    }
}