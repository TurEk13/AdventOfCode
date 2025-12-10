using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public partial class D13Z01 : IZadanie
{
    private List<Maszyna> _Maszyny;
    private Int64[] _Wyniki;
    private int _KosztA;
    private int _KosztB;
    [GeneratedRegex("\\d+")]
    private static partial Regex _Liczby();
    
    public D13Z01(bool daneTestowe = false)
    {
        this._KosztA = 3;
        this._KosztB = 1;
        this._Maszyny = new();
        FileStream fs = new (daneTestowe ? ".\\Dane\\2024\\13\\proba.txt" : ".\\Dane\\2024\\13\\dane.txt", FileMode.OpenOrCreate, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        string linia;
        Regex liczby = _Liczby();
        MatchCollection mc;
        int[] A, B, P;
        while((linia = sr.ReadLine()) is not null)
        {
            //linia = sr.ReadLine();
            mc = liczby.Matches(linia);
            A = [.. mc.Select(m => Convert.ToInt32(m.Value))];

            linia = sr.ReadLine();
            mc = liczby.Matches(linia);
            B = [..mc.Select(m => Convert.ToInt32(m.Value))];

            linia = sr.ReadLine();
            mc = liczby.Matches(linia);
            P = [..mc.Select(m => Convert.ToInt32(m.Value))];

            this._Maszyny.Add(new Maszyna(this._Maszyny.Count, A[0], A[1], B[0], B[1], P[0], P[1]));

            sr.ReadLine();
        }

        sr.Close(); fs!.Close();

        this._Wyniki = new Int64[this._Maszyny.Count];
    }

    public void RozwiazanieZadania()
    {
        int MinKoszt, Koszt;

        int PX, PY;

        foreach (Maszyna m in this._Maszyny)
        {
            this._Wyniki[m.Id] = int.MaxValue;

            for (int a = 0; a <= 100; a++)
            {
                MinKoszt = int.MaxValue;
                for (int b = 0; b <= 100; b++)
                {
                    PX = a * m.AX + b * m.BX;
                    PY = a * m.AY + b * m.BY;

                    if(PX > m.PX && PY > m.PY)
                    {
                        b = 100;
                    }

                    if (PX == m.PX && PY == m.PY)
                    {
                        Koszt = a * this._KosztA + b * this._KosztB;

                        if (this._Wyniki[m.Id] > Koszt)
                        {
                            MinKoszt = Koszt;
                        }
                    }
                }

                if (this._Wyniki[m.Id] > MinKoszt)
                {
                    this._Wyniki[m.Id] = MinKoszt;
                }
            }

            if (this._Wyniki[m.Id] == int.MaxValue)
            {
                this._Wyniki[m.Id] = 0;
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this._Wyniki.Sum().ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    record Maszyna(int Id, int AX, int AY, int BX, int BY, int PX, int PY);
}