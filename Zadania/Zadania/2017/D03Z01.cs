using System;
using System.Globalization;
using System.IO;

namespace Zadania._2017;

public partial class D03Z01 : IZadanie
{
    private UInt32 _Wartosc;
    private UInt32 _Wynik;
    
    public D03Z01(bool daneTestowe = false)
    {
        this._Wynik = 0;

        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2017\\03\\proba.txt" : ".\\Dane\\2017\\03\\dane.txt", FileMode.Open, FileAccess.Read));

        this._Wartosc = Convert.ToUInt32(sr.ReadToEnd());

        sr.Close();
    }

    public void RozwiazanieZadania()
    {
        int licznik = 1, obecnaWartoscG, obecnaWartoscD, obecnaWartosc;
        Punkt G, D, tmpP;

        while(Math.Pow(licznik, 2) < this._Wartosc)
        {
            licznik++;
        }

        // Jeśli parzyste D => G
        if (licznik % 2 == 0)
        {
            obecnaWartoscG = Convert.ToInt32(Math.Pow(licznik, 2));
            G = new Punkt(-(licznik / 2 - 1), licznik / 2);

            obecnaWartoscD = Convert.ToInt32(Math.Pow(licznik - 1, 2));
            D = new Punkt((licznik - 1) / 2, -((licznik - 1) / 2));
            obecnaWartosc = obecnaWartoscD + 1;
            tmpP = D with { X = D.X + 1 };

            if (obecnaWartoscG == this._Wartosc)
            {
                this.Manhattan(G);
                return;
            }

            if(obecnaWartoscD == this._Wartosc)
            {
                this.Manhattan(D);
                return;
            }

            if(obecnaWartosc == this._Wartosc)
            {
                this.Manhattan(tmpP);
                return;
            }

            for(int i = 0; i < licznik; i++)
            {
                tmpP = tmpP with { Y = tmpP.Y + 1 };
                obecnaWartosc++;
                if (obecnaWartosc == this._Wartosc)
                {
                    this.Manhattan(tmpP);
                    return;
                }
            }

            for (int i = 0; i < licznik; i++)
            {
                tmpP = tmpP with { X = tmpP.X - 1 };
                obecnaWartosc++;
                if (obecnaWartosc == this._Wartosc)
                {
                    this.Manhattan(tmpP);
                    return;
                }
            }
        }

        // Jeśli nieparzyste G => D
        if (licznik % 2 == 1)
        {
            obecnaWartoscG = Convert.ToInt32(Math.Pow(licznik - 1, 2));
            obecnaWartosc = obecnaWartoscG + 1;
            G = new Punkt(-((licznik - 1) / 2 - 1), (licznik - 1) / 2);
            tmpP = G with { X = G.X - 1 };

            obecnaWartoscD = Convert.ToInt32(Math.Pow(licznik, 2));
            D = new Punkt(licznik / 2, -(licznik / 2));
            
            if (obecnaWartoscG == this._Wartosc)
            {
                this.Manhattan(G);
                return;
            }

            if (obecnaWartoscD == this._Wartosc)
            {
                this.Manhattan(D);
                return;
            }

            if (obecnaWartosc == this._Wartosc)
            {
                this.Manhattan(tmpP);
                return;
            }

            for (int i = 0; i < licznik - 1; i++)
            {
                tmpP = tmpP with { Y = tmpP.Y - 1 };
                obecnaWartosc++;
                if (obecnaWartosc == this._Wartosc)
                {
                    this.Manhattan(tmpP);
                    return;
                }
            }

            for (int i = 0; i < licznik - 1; i++)
            {
                tmpP = tmpP with { X = tmpP.X + 1 };
                obecnaWartosc++;
                if (obecnaWartosc == this._Wartosc)
                {
                    this.Manhattan(tmpP);
                    return;
                }
            }
        }
    }

    private void Manhattan(Punkt punkt)
    {
        this._Wynik = Convert.ToUInt32(Math.Abs(0 - punkt.X) + Math.Abs(0 - punkt.Y));
    }

    public string PokazRozwiazanie()
    {
        return this._Wynik.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Punkt(int X, int Y);
}