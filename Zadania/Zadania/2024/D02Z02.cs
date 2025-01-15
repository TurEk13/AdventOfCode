using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D02Z02 : IZadanie
{
    private int suma;
    private List<int[]> liniaInt;

    public D02Z02(bool daneTestowe = false)
    {
        this.suma = 0;
        string linia;
        this.liniaInt = new ();
        FileStream fs = daneTestowe ? new(".\\Dane\\2024\\02\\proba.txt", FileMode.Open, FileAccess.Read) : new(".\\Dane\\2024\\02\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);

        while ((linia = sr.ReadLine()) != null)
        {
            this.liniaInt.Add(linia.Split(' ').Select((x) => Convert.ToInt32(x)).ToArray());
        }
    }

    public void RozwiazanieZadania()
    {
        List<int> listaRoznic, nowaLinia;
        
        for (int i = 0; i < this.liniaInt.Count; i++)
        {
            listaRoznic = Roznica(this.liniaInt[i]);
            if (Bezpieczny(listaRoznic, 1, 3) || Bezpieczny(listaRoznic, -3, -1))
            {
                suma++;
            }
            else
            {
                for(int j = 0; j < this.liniaInt[i].Length; j++)
                {
                    nowaLinia = new(this.liniaInt[i]);
                    nowaLinia.RemoveAt(j);

                    listaRoznic = Roznica(nowaLinia.ToArray());
                    if (Bezpieczny(listaRoznic, 1, 3) || Bezpieczny(listaRoznic, -3, -1))
                    {
                        suma++;
                        break;
                    }
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private List<int> Roznica(int[] ciag)
    {
        List<int> zwroc = new();

        for (int i = 0; i < ciag.Length - 1; i++)
        {
            zwroc.Add(ciag[i] - ciag[i + 1]);
        }

        return zwroc;
    }

    private bool Bezpieczny(List<int> ciag, int dolnyZakres, int gornyZakres)
    {
        return ciag.FindAll((x) => x < dolnyZakres || gornyZakres < x).Count() > 0 ? false : true;
    }
}