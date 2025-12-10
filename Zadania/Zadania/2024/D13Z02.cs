using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public partial class D13Z02 : IZadanie
{
    private List<Maszyna> _Maszyny;
    private Int64[] _Wyniki;
    private int _KosztA;
    private int _KosztB;
    [GeneratedRegex("\\d+")]
    private static partial Regex _Liczby();
    
    public D13Z02(bool daneTestowe = false)
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
        Int64 blad = 10_000_000_000_000;
        while ((linia = sr.ReadLine()) is not null)
        {
            mc = liczby.Matches(linia);
            A = [.. mc.Select(m => Convert.ToInt32(m.Value))];

            linia = sr.ReadLine();
            mc = liczby.Matches(linia);
            B = [..mc.Select(m => Convert.ToInt32(m.Value))];

            linia = sr.ReadLine();
            mc = liczby.Matches(linia);
            P = [..mc.Select(m => Convert.ToInt32(m.Value))];

            this._Maszyny.Add(new Maszyna(this._Maszyny.Count, A[0], A[1], B[0], B[1], P[0] + blad, P[1] + blad));

            sr.ReadLine();
        }

        sr.Close(); fs!.Close();

        this._Wyniki = new Int64[this._Maszyny.Count];
    }

    public void RozwiazanieZadania()
    {
        Int64 MinKoszt, Koszt, MaxA, MaxB;

        Int64 PX = 0, PY = 0;

        foreach (Maszyna m in this._Maszyny)
        {
            this._Wyniki[m.Id] = int.MaxValue;
            MaxA = Math.Max(m.PX / Convert.ToInt64(m.AX), m.PY / Convert.ToInt64(m.AY));
            MaxB = Math.Max(m.PX / Convert.ToInt64(m.BX), m.PY / Convert.ToInt64(m.BY));

            for (Int64 a = 0; a <= MaxA + 1; a++)
            {
                MinKoszt = int.MaxValue;

                for (Int64 b = 0; b <= MaxB + 1; b++)
                {
                    PX = a * m.AX + b * m.BX;
                    PY = a * m.AY + b * m.BY;

                    if(PX > m.PX && PY > m.PY)
                    {
                        break;
                    }

                    if (PX == m.PX && PY == m.PY)
                    {
                        Koszt = a * this._KosztA + b * this._KosztB;

                        if (this._Wyniki[m.Id] > Koszt)
                        {
                            MinKoszt = Koszt;
                        }
                    }
                    
                    if(b % 1_000_000_000 == 0)
                    {
                        Debug.WriteLine($"{b:N0}: {PX:N0} - {m.PX:N0} / {PY:N0} - {m.PY:N0}");
                    }
                }

                if (this._Wyniki[m.Id] > MinKoszt)
                {
                    this._Wyniki[m.Id] = MinKoszt;
                }

                if (PX > m.PX && PY > m.PY)
                {
                    break;
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

    record Maszyna(int Id, int AX, int AY, int BX, int BY, Int64 PX, Int64 PY);
}