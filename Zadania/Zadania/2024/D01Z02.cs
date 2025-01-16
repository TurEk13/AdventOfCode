using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public class D01Z02 : IZadanie
{
    private List<int> lewa;
    private List<int> prawa;
    private int suma;

    public D01Z02(bool daneTestowe = false)
    {
        this.lewa = new();
        this.prawa = new();
        this.suma = 0;

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\01\\proba.txt" : ".\\Dane\\2024\\01\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);

        string linia;
        Regex liczby = new(@"\d{1,9}");
        MatchCollection liczbyMC;

        while ((linia = sr.ReadLine()) != null)
        {
            liczbyMC = liczby.Matches(linia);
            this.lewa.Add(Convert.ToInt32(liczbyMC[0].Value));
            this.prawa.Add(Convert.ToInt32(liczbyMC[1].Value));
        }
    }

    public void RozwiazanieZadania()
    {
        for (int i = 0; i < this.lewa.Count; i++)
        {
            this.suma += this.lewa[i] * this.prawa.FindAll((p) => p == this.lewa[i]).ToList().Count;
        }
    }

    public string PokazRozwiazanie()
    {
        return this.suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}