using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2024;

public class D09Z02 : IZadanie
{
    private string tablicaDysku;
    private Int64 sumaKontrolna;

    public D09Z02(bool daneTestowe = false)
    {
        this.sumaKontrolna = 0;
        this.tablicaDysku = File.ReadAllText(daneTestowe ? ".\\Dane\\2024\\09\\proba.txt" : ".\\Dane\\2024\\09\\dane.txt");
    }

    public void RozwiazanieZadania()
    {
        List<Plik> listaPlikow = new();
        List<Plik> listaPrzeniesionychPlikow = new();
        List<Plik> wszystkiePliki;
        List<WolneMiejsce> listaWolnegoMiejsca = new();
        Int64 nazwaPliku = 0, pozycja = 0;
        bool CzyPlik = true;

        for (int i = 0; i < tablicaDysku.Length; i++)
        {
            int dlugosc = tablicaDysku[i] - '0';
            if (CzyPlik)
            {
                listaPlikow.Add(new Plik(nazwaPliku, pozycja, dlugosc));
                nazwaPliku++;
            }
            else
            {
                listaWolnegoMiejsca.Add(new WolneMiejsce(pozycja, dlugosc));
            }
            pozycja += dlugosc;
            CzyPlik = !CzyPlik;
        }

        for (int indeksPliku = listaPlikow.Count - 1; indeksPliku >= 0; indeksPliku--)
        {
            Plik plikDoPrzeniesienia = listaPlikow[indeksPliku];

            for (int indeksWolnegoMiejsca = 0; indeksWolnegoMiejsca < listaWolnegoMiejsca.Count; indeksWolnegoMiejsca++)
            {
                WolneMiejsce wolneMiejsceDoSprawdzenia = listaWolnegoMiejsca[indeksWolnegoMiejsca];

                if (plikDoPrzeniesienia.dlugoscPliku <= wolneMiejsceDoSprawdzenia.dlugoscWolnegoMiejsca && plikDoPrzeniesienia.poczatekPliku > wolneMiejsceDoSprawdzenia.poczatekWolnegoMiejsca)
                {
                    listaPrzeniesionychPlikow.Add(new Plik(plikDoPrzeniesienia.nazwaPliku, wolneMiejsceDoSprawdzenia.poczatekWolnegoMiejsca, plikDoPrzeniesienia.dlugoscPliku));
                    listaPlikow.RemoveAt(indeksPliku);

                    if (listaWolnegoMiejsca[indeksWolnegoMiejsca].dlugoscWolnegoMiejsca == 0)
                    {
                        listaWolnegoMiejsca.RemoveAt(indeksWolnegoMiejsca);
                        break;
                    }

                    listaWolnegoMiejsca[indeksWolnegoMiejsca] = new WolneMiejsce(wolneMiejsceDoSprawdzenia.poczatekWolnegoMiejsca + plikDoPrzeniesienia.dlugoscPliku, wolneMiejsceDoSprawdzenia.dlugoscWolnegoMiejsca - plikDoPrzeniesienia.dlugoscPliku);
                    break;
                }
            }
        }

        wszystkiePliki = listaPlikow.Concat(listaPrzeniesionychPlikow).OrderBy(p => p.poczatekPliku).ToList();

        foreach (Plik p in wszystkiePliki)
        {
            for (int przesuniecie = 0; przesuniecie < p.dlugoscPliku; przesuniecie++)
            {
                this.sumaKontrolna += (p.poczatekPliku + przesuniecie) * p.nazwaPliku;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.sumaKontrolna.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private record Plik(Int64 nazwaPliku, Int64 poczatekPliku, int dlugoscPliku);
    private record WolneMiejsce(Int64 poczatekWolnegoMiejsca, int dlugoscWolnegoMiejsca);
}