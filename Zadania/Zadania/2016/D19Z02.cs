using System;
using System.Diagnostics;
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
        // 26 583 -
        int i = 0;

        while (this._ListaElfow.Usun(this._ListaElfow.ElfObecny, Elfy.IleElementow / 2) is null)
        {
            i++;
            if(i == 10_000)
            {
                Debug.WriteLine(Elfy.IleElementow.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL")));
                i = 0;
            }
        }
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

        public Elf Usun(Elf elf, int OdKtorego)
        {
            Elf Kolejny = elf;
            int i = 0;

            if(elf.ElfNastepny == elf.ElfPoprzedni)
            {
                elf.IlePrezentow += elf.ElfNastepny.IlePrezentow;
                elf.ElfNastepny.IlePrezentow = 0;
                ElfObecny = elf;
                return elf;
            }

            while(i < OdKtorego)
            {
                Kolejny = Kolejny.ElfNastepny;
                i++;
            }

            elf.IlePrezentow += Kolejny.IlePrezentow;
            Kolejny.IlePrezentow = 0;

            Kolejny.ElfPoprzedni.ElfNastepny = Kolejny.ElfNastepny;
            Kolejny.ElfNastepny.ElfPoprzedni = Kolejny.ElfPoprzedni;
            IleElementow--;

            ElfObecny = elf.ElfNastepny;

            //Debug.WriteLine($"Ile elementów: {IleElementow}, Usunięty elf: {Kolejny.Id}");

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