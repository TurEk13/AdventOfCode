using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D16Z01 : IZadanie
{
    private List<Ciocia> _ciocie;
    private Int64 _Id;

    public D16Z01()
    {
        this._ciocie = new();
        this._Id = 0;

        FileStream fs = new(".\\Dane\\2015\\16\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] liniaa;

        while((linia = sr.ReadLine()) is not null)
        {
            liniaa = linia[(linia.IndexOf(':') + 2)..].Replace(", ", " ").Replace(": ", " ").Split(' ');

            this._ciocie.Add(new());
            this._ciocie[^1].id = this._ciocie.Count;

            for(int i = 0; i < liniaa.Length; i += 2)
            {
                switch(liniaa[i])
                {
                    case "children":
                        this._ciocie[^1].children = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "cats":
                        this._ciocie[^1].cats = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "samoyeds":
                        this._ciocie[^1].samoyeds = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "pomeranians":
                        this._ciocie[^1].pomeranians = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "akitas":
                        this._ciocie[^1].akitas = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "vizslas":
                        this._ciocie[^1].vizslas = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "goldfish":
                        this._ciocie[^1].goldfish = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "trees":
                        this._ciocie[^1].trees = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "cars":
                        this._ciocie[^1].cars = Convert.ToInt32(liniaa[i + 1]);
                        break;
                    case "perfumes":
                        this._ciocie[^1].perfumes = Convert.ToInt32(liniaa[i + 1]);
                        break;
                }
            }
        }
    }

    public void RozwiazanieZadania()
    {
        this._Id = this._ciocie.Where(c => (c.children == 7 || c.children == 0) && (c.cats == 7 || c.cats == 0) && (c.samoyeds == 2 || c.samoyeds == 0) && (c.pomeranians == 3 || c.pomeranians == 0) && c.akitas == 0 && c.vizslas == 0 && (c.goldfish == 5 || c.goldfish == 0) && (c.trees == 3 || c.trees == 0) && (c.cars == 2 || c.cars == 2) && (c.perfumes == 1 || c.perfumes == 0)).ToList<Ciocia>()[0].id;
    }

    public string PokazRozwiazanie()
    {
        return this._Id.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    class Ciocia
    {
        public int id;
        public int children;
        public int cats;
        public int samoyeds;
        public int pomeranians;
        public int akitas;
        public int vizslas;
        public int goldfish;
        public int trees;
        public int cars;
        public int perfumes;

        public Ciocia()
        {
            this.children = 0;
            this.cats = 0;
            this.samoyeds = 0;
            this.pomeranians = 0;
            this.akitas = 0;
            this.vizslas = 0;
            this.goldfish = 0;
            this.trees = 0;
            this.cars = 0;
            this.perfumes = 0;
        }
    }
}