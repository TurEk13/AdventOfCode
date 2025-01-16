using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D05Z02 : IZadanie
{
    private int suma;
    private List<Zasady> zasady;
    private List<List<int>> aktualizacje;
    public D05Z02(bool daneTestowe = false)
    {
        this.zasady = new();
        this.aktualizacje = new();
        this.suma = 0;
        string[] plik = File.ReadAllLines(daneTestowe ? ".\\Dane\\2024\\05\\proba.txt" : ".\\Dane\\2024\\05\\dane.txt");

        string[] tmp = null;
        int i = 0;

        while (plik[i].Contains('|'))
        {
            tmp = plik[i].Split('|');
            this.zasady.Add(new(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1])));
            i++;
        }

        i++;

        while (i < plik.Length)
        {
            this.aktualizacje.Add(plik[i].Split(',').Select(x => Convert.ToInt32(x)).ToList());
            i++;
        }
    }

    public void RozwiazanieZadania()
    {
        List<int> ciag;
        Zasady zasada;
        int indeks;
        bool liczyc;

        //Sprawdzanie każdego wiersza aktualizacji
        for (int i = 0; i < this.aktualizacje.Count; i++)
        {
            ciag = new(this.aktualizacje[i]);
            liczyc = false;
            //Pobranie lewego elementu
            for (int lewy = 0; lewy < ciag.Count; lewy++)
            {
                //Pobranie prawego elementu
                for (int prawy = lewy + 1; prawy < ciag.Count; prawy++)
                {
                    //Sprawdzenie czy oba elementy są w tej samej zasadzie
                    zasada = lewy < prawy ? this.zasady.SingleOrDefault((x => x.lewo == ciag[prawy] && x.prawo == ciag[lewy]), new Zasady()) : this.zasady.SingleOrDefault((x => x.lewo == ciag[lewy] && x.prawo == ciag[prawy]), new Zasady());

                    if (zasada.lewo != -1 && zasada.prawo != -1)
                    {
                        liczyc = true;
                        this.ZmienKolejnosc(ref ciag, lewy, prawy);
                    }
                }
            }


            if (liczyc)
            {
                indeks = ciag.Count / 2;
                this.suma += ciag[indeks];
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Zasady(int lewo = -1, int prawo = -1);

    private void ZmienKolejnosc(ref List<int> lista, int elementLewy, int elementPrawy)
    {
        int x = lista[elementLewy];
        lista[elementLewy] = lista[elementPrawy];
        lista[elementPrawy] = x;
    }
}