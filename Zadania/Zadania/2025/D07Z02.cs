using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Zadania._2025;

public partial class D07Z02 : IZadanie
{
    private List<char[]> _Mapa;
    private Int64[] _Liczniki;
    private Int64 _Wynik;
    public D07Z02(bool daneTestowe = false)
    {
        this._Mapa = new List<char[]>();
        FileStream fs = new(daneTestowe ? ".\\Dane\\2025\\07\\proba.txt" : ".\\Dane\\2025\\07\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while ((linia = sr.ReadLine()) is not null)
        {
            this._Mapa.Add(linia.ToCharArray());
        }

        sr.Close(); fs!.Close();

        this._Liczniki = new Int64[this._Mapa[0].Length];
        Array.Fill(this._Liczniki, 0);
        this._Liczniki[new string(this._Mapa[0]).IndexOf('S')]++;
    }

    public void RozwiazanieZadania()
    {
    }

    {
        {
            {
                {
            }
        }
    }

    {
        }
    }

    public string PokazRozwiazanie()
    {
    }
}