using System;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public partial class D19Z01 : IZadanie
{
    private Elfy _ListaElfow;

    public D19Z01(bool daneTestowe = false)
    {
        this._ListaElfow = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\19\\proba.txt" : ".\\Dane\\2016\\19\\dane.txt", FileMode.Open, FileAccess.Read);       
		StreamReader sr = new(fs);

        int IleElfow = Convert.ToInt32(sr.ReadToEnd());

        for(Int32 i = 0; i < IleElfow;  i++)
        {
            this._ListaElfow.Dodaj(new Elf()
            {
                Id = i + 1,
                IlePrezentow = 1
            });
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        while(this._ListaElfow.Usun(this._ListaElfow.ElfObecny) is null);
    }

    public string PokazRozwiazanie()
    {
        return this._ListaElfow.ElfObecny.Id.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Elfy
    {
        public static int IleElementow { get; private set; }
        public Elf ElfObecny;
        public Elf ElfPoczatkowy;
        public Elf ElfKoncowy;

        static Elfy()
        {
            IleElementow = 0;
        }

        public void Dodaj(Elf elf)
        {
            if(IleElementow == 0)
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
            }

            IleElementow++;
        }

        public Elf Usun(Elf elf)
        {
            if(elf.ElfNastepny == elf.ElfPoprzedni)
            {
                elf.IlePrezentow += elf.ElfNastepny.IlePrezentow;
                ElfObecny = elf;
                return elf;
            }

            elf.IlePrezentow += elf.ElfNastepny.IlePrezentow;
            elf.ElfNastepny.IlePrezentow = 0;

            elf.ElfNastepny.ElfNastepny.ElfPoprzedni = elf;
            elf.ElfNastepny = elf.ElfNastepny.ElfNastepny;
            IleElementow--;

            ElfObecny = elf.ElfNastepny;

            return null;
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