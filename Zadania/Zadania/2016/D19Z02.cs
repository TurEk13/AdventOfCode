using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Zadania._2016;

public partial class D19Z02 : IZadanie
{
    private List<Elf> _ListaElfow;

    public D19Z02(bool daneTestowe = false)
    {
        this._ListaElfow = new ();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\19\\proba.txt" : ".\\Dane\\2016\\19\\dane.txt", FileMode.Open, FileAccess.Read);       
		StreamReader sr = new(fs);

        int IleElfow = Convert.ToInt32(sr.ReadToEnd());

        for(Int32 i = 0; i < IleElfow;  i++)
        {
            this._ListaElfow.Add(new Elf()
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
        int IleElfow;
        int ElfDocelowy;
        
        while(this._ListaElfow.Count > 1)
        {
            for(int i = 0; i < this._ListaElfow.Count; i++)
            {
                IleElfow = this._ListaElfow.Count;
                ElfDocelowy = IleElfow / 2;

                this._ListaElfow[i].IlePrezentow += this._ListaElfow[(i + ElfDocelowy) % IleElfow].IlePrezentow;
                this._ListaElfow.RemoveAt((i + ElfDocelowy) % IleElfow);
            }
            Debug.WriteLine(this._ListaElfow.Count);
        }
    }

    public string PokazRozwiazanie()
    {
        return this._ListaElfow[0].Id.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Elf
    {
        public int Id;
        public int IlePrezentow;
    }
}