using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2016;

public partial class D19Z01 : IZadanie
{
    private List<Elf> _Elfy;
    private Int32 _IleElfow;

    public D19Z01(bool daneTestowe = false)
    {
        this._Elfy = new();

        FileStream fs = new(daneTestowe ? ".\\Dane\\2016\\19\\proba.txt" : ".\\Dane\\2016\\19\\dane.txt", FileMode.Open, FileAccess.Read);       
		StreamReader sr = new(fs);

        this._IleElfow = Convert.ToInt32(sr.ReadToEnd());

        for(Int32 i = 0; i < this._IleElfow;  i++)
        {
            this._Elfy.Add(new(i + 1, 1));
        }

        sr.Close(); fs!.Close();
    }

    public void RozwiazanieZadania()
    {
        // 2 796 203 +
        bool parzysta = true;

        while (this._Elfy.Count > 1)
        {
            if (parzysta)
            {
                this._Elfy.RemoveAll(e => e.Lp % 2 == 0);
            }

            if(!parzysta)
            {
                this._Elfy.RemoveAll(e => e.Lp % 2 == 1);
            }

            parzysta = !parzysta;

            this.ZmienLP();

            Debug.WriteLine(this._Elfy.Count);
        }
    }

    private void ZmienLP()
    {
        this._Elfy = this._Elfy.OrderBy(e => e.Lp).ToList().ToList<Elf>();

        for (int j = 0; j < this._Elfy.Count; j++)
        {
            this._Elfy[j].Lp = j + 1;
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Elfy[0].Id.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private class Elf
    {
        public Int32 Lp { get; set; }
        public Int32 Id { get; private set; }
        public Int64 IlePrezentow { get; private set; }

        public Elf(Int32 id, Int64 ilePrezentow) => (id, id, ilePrezentow) = (this.Lp = id, this.Id = id, this.IlePrezentow = ilePrezentow);

        public void DodajPrezenty(Int64 ilePrezentow)
        {
            this.IlePrezentow += ilePrezentow;
        }
    }
}