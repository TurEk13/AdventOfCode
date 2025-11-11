using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2015;

public class D05Z02 : IZadanie
{
    private List<string> ListaSlow;
    private int DobreSlowo;

    public D05Z02()
    {
        FileStream fs = new(".\\Dane\\2015\\05\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        this.ListaSlow = new();
        this.DobreSlowo = 0;

        while((linia = sr.ReadLine()) is not null)
        {
            this.ListaSlow.Add(linia);
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        bool PodwojoneCzastki, PodwojneLitery;

        foreach(string slowo in this.ListaSlow)
        {
            PodwojoneCzastki = this.PodwojoneCzastki(slowo);

            PodwojneLitery = this.PodwojoneLitery(slowo);

            if(PodwojoneCzastki && PodwojneLitery)
            {
                this.DobreSlowo++;
            }
        }
    }

    private bool PodwojoneCzastki(string slowo)
    {
        Regex wzor;
        MatchCollection dopasowania;
        HashSet<string> UzyteWzory = new();
        int ileDopasowan = 0;

        for(int i = 0; i < slowo.Length - 1; i++)
        {
            if(!UzyteWzory.Contains($"{slowo[i]}{slowo[i + 1]}"))
            {
                UzyteWzory.Add($"{slowo[i]}{slowo[i + 1]}");
                wzor = new($"{slowo[i]}{slowo[i + 1]}");
                dopasowania = wzor.Matches(slowo);

                if (dopasowania.Count > 1)
                {
                    ileDopasowan++;
                }
            }
        }

        return ileDopasowan > 0;
    }

    private bool PodwojoneLitery(string slowo)
    {
        for (int i = 0; i < slowo.Length - 2; i++)
        {
            if (slowo[i] == slowo[i + 2])
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