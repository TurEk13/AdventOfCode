using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D23Z01 : IZadanie
{
    private List<Polecenie> _Program;
    private UInt128 _A;
    private UInt128 _B;
    private int _WierszProgramu;
    public D23Z01(bool daneTestowe = false)
    {
        this._Program = new ();
        this._A = 1;
        this._B = 0;
        this._WierszProgramu = 0;
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\23\\proba.txt" : ".\\Dane\\2015\\23\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;
        string[] tmp;

        while((linia = sr.ReadLine()) is not null)
        {
            if(!linia.Contains(','))
            {
                this._Program.Add(new(linia.Split(' ')));
            }

            if(linia.Contains(','))
            {
                tmp = linia.Split(',');
                this._Program.Add(new([..tmp[0].Split(' '), tmp[1].Trim()]));
            }
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        while(this._WierszProgramu < this._Program.Count)
        {
            switch(this._Program[this._WierszProgramu].Komenda)
            {
                case "hlf":
                    this.HLF(this._Program[this._WierszProgramu]);
                    break;
                case "tpl":
                    this.TPL(this._Program[this._WierszProgramu]);
                    break;
                case "inc":
                    this.INC(this._Program[this._WierszProgramu]);
                    break;
                case "jmp":
                    this.JMP(this._Program[this._WierszProgramu]);
                    break;
                case "jie":
                    this.JIE(this._Program[this._WierszProgramu]);
                    break;
                case "jio":
                    this.JIO(this._Program[this._WierszProgramu]);
                    break;
            }
        }
    }

    private void HLF(Polecenie p)
    {
        switch(p.Rejestr)
        {
            case "a":
                this._A /= 2;
                break;
            case "b":
                this._B /= 2;
                break;
        }
        this._WierszProgramu++;
    }

    private void TPL(Polecenie p)
    {
        switch(p.Rejestr)
        {
            case "a":
                this._A *= 3;
                break;
            case "b":
                this._B *= 3;
                break;
        }
        this._WierszProgramu++;
    }

    private void INC(Polecenie p)
    {
        switch(p.Rejestr)
        {
            case "a":
                this._A += 1;
                break;
            case "b":
                this._B += 1;
                break;
        }
        this._WierszProgramu++;
    }

    private void JMP(Polecenie p)
    {
        this._WierszProgramu += p.Skok;
    }

    private void JIE(Polecenie p)
    {
        switch(p.Rejestr)
        {
            case "a":
                if(this._A % 2 == 0)
                {
                    this._WierszProgramu += p.Skok;
                }
                else
                {
                    this._WierszProgramu++;
                }
                break;
            case "b":
                if(this._B % 2 == 0)
                {
                    this._WierszProgramu += p.Skok;
                }
                else
                {
                    this._WierszProgramu++;
                }
                break;
        }
    }

    private void JIO(Polecenie p)
    {
        switch(p.Rejestr)
        {
            case "a":
                if(this._A == 1)
                {
                    this._WierszProgramu += p.Skok;
                }
                else
                {
                    this._WierszProgramu++;
                }
                break;
            case "b":
                if(this._B == 1)
                {
                    this._WierszProgramu += p.Skok;
                }
                else
                {
                    this._WierszProgramu++;
                }
                break;
        }
    }

    public string PokazRozwiazanie()
    {
        return string.Format("\r\nRejestr A: {0}\r\nRejestr B: {1}", this._A.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL")), this._B.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL")));
    }

    record Polecenie
    {
        public string Komenda { get; private set; }
        public string Rejestr { get; private set; }
        public int Skok { get; private set; }

        public Polecenie(string[] parametry)
        {
            if(parametry.Length == 2 && !parametry[0].Equals("jmp"))
            {
                this.Komenda = parametry[0];
                this.Rejestr = parametry[1];
            }

            if(parametry.Length == 2 && parametry[0].Equals("jmp"))
            {
                this.Komenda = parametry[0];
                this.Skok = parametry[1][0].Equals("+") ? Convert.ToInt32(parametry[1][1..]) : Convert.ToInt32(parametry[1]);
            }

            if(parametry.Length == 3)
            {
                this.Komenda = parametry[0];
                this.Rejestr = parametry[1];
                this.Skok = parametry[2][0].Equals("+") ? Convert.ToInt32(parametry[2][1..]) : Convert.ToInt32(parametry[2]);
            }
        }
    }
}