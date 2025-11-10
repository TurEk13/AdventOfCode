using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D05Z02 : IZadanie
{
    private List<string> ListaSlow;
    private int DobreSlowo;
    private char[] Samogloski;

    public D05Z02()
    {
        FileStream fs = new(".\\Dane\\2015\\05\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        this.ListaSlow = new();
        this.DobreSlowo = 0;
        this.Samogloski = ['a', 'e', 'i', 'o', 'u'];

        while((linia = sr.ReadLine()) is not null)
        {
            this.ListaSlow.Add(linia);
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        bool ZlaCzastka, IleSamoglosek, PodwojneLitery;

        foreach(string slowo in this.ListaSlow)
        {
            ZlaCzastka = this.ZlaCzastka(slowo);

            IleSamoglosek = this.IleSamoglosek(slowo);

            PodwojneLitery = this.PodwojoneLitery(slowo);

            if(!ZlaCzastka && IleSamoglosek && PodwojneLitery)
            {
                this.DobreSlowo++;
            }
        }
    }

    private bool ZlaCzastka(string slowo)
    {
        if (slowo.Contains("ab") || slowo.Contains("cd") || slowo.Contains("pq") || slowo.Contains("xy"))
        {
            return true;
        }

        return false;
    }

    private bool IleSamoglosek(string slowo)
    {
        int ileSamoglosek = 0;

        foreach (char litera in slowo)
        {
            if (this.Samogloski.Contains(litera))
            {
                ileSamoglosek++;
            }
        }

        return ileSamoglosek > 2 ? true : false;
    }

    private bool PodwojoneLitery(string slowo)
    {
        for (int i = 0; i < slowo.Length - 1; i++)
        {
            if (slowo[i] == slowo[i + 1])
            {
                return true;
            }
        }

        return false;
    }

    public string PokazRozwiazanie()
    {
        return this.DobreSlowo.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}