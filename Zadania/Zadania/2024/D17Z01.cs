using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public partial class D17Z01 : IZadanie
{
    [GeneratedRegex(@"Register [A-C]{1}: (\d{1,})")]
    private static partial Regex Rejestry();

    [GeneratedRegex(@"Program: (?<daneProgramu>(\d,?){1,})")]
    private static partial Regex DaneProgramu();

    private Chronospatial _Chronospatial;

    public D17Z01(bool daneTestowe = false)
    {
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2024\\17\\proba.txt" : ".\\Dane\\2024\\17\\dane.txt", FileMode.Open, FileAccess.Read));
        string daneProgramu = sr.ReadToEnd();
        sr.Close();

        Regex DaneRejestrow = Rejestry();
        Regex KodProgramu = DaneProgramu();

        MatchCollection rejestry = DaneRejestrow.Matches(daneProgramu);
        Match kod = KodProgramu.Match(daneProgramu);

        this._Chronospatial = new Chronospatial(
            Convert.ToInt32(rejestry[0].Groups[1].Value),
            Convert.ToInt32(rejestry[1].Groups[1].Value),
            Convert.ToInt32(rejestry[2].Groups[1].Value),
            kod.Groups["daneProgramu"].Value.Split(',').Select(dp => Convert.ToInt32(dp)).ToList<int>()
        );
    }

    public void RozwiazanieZadania()
    {
        this._Chronospatial.Pracuj();
    }

    public string PokazRozwiazanie()
    {
        return string.Join(",", this._Chronospatial.Wynik);
    }

    private record Instrukcja(int Opcode, int Operand);

    private class Chronospatial
    {
        private int RejestrA;
        private int RejestrB;
        private int RejestrC;
        private readonly List<Instrukcja> SpisInstrukcji;
        private int ObecnaInstrukcja;
        public List<int> Wynik { get; private set; }

        public Chronospatial(int rejestrA, int rejestrB, int rejestrC, List<int> spisInstrukcji)
        {
            this.RejestrA = rejestrA;
            this.RejestrB = rejestrB;
            this.RejestrC = rejestrC;

            this.SpisInstrukcji = new List<Instrukcja>();
            for(int i = 0; i < spisInstrukcji.Count; i += 2)
            {
                this.SpisInstrukcji.Add(new Instrukcja(spisInstrukcji[i], spisInstrukcji[i + 1]));
            }

            this.Wynik = new List<int>();
            this.ObecnaInstrukcja = 0;
        }

        public void Pracuj()
        {
            int opcode;
            while(this.SprawdzWskaznik())
            {
                opcode = this.SpisInstrukcji[ObecnaInstrukcja].Opcode;
                switch (opcode)
                {
                    case 0:
                        this.ADV();
                        break;
                    case 1:
                        this.BXL();
                        break;
                    case 2:
                        this.BST();
                        break;
                    case 3:
                        this.JNZ();
                        break;
                    case 4:
                        this.BXC();
                        break;
                    case 5:
                        this.OOUT();
                        break;
                    case 6:
                        this.BDV();
                        break;
                    case 7:
                        this.CDV();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Opcode must be beetween 0 and 7.");
                }
            }
        }

        private void ADV()
        {
            this.RejestrA /= Convert.ToInt32(Math.Pow(2, this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand)));
            PrzesunWskaznik();
        }

        private void BXL()
        {
            this.RejestrB ^= this.SpisInstrukcji[this.ObecnaInstrukcja].Operand;
            this.PrzesunWskaznik();
        }

        private void BST()
        {
            this.RejestrB = this.ZwrocCombo(this.SpisInstrukcji[this.ObecnaInstrukcja].Operand) % 8;
            this.PrzesunWskaznik();
        }

        private void JNZ()
        {
            if(this.RejestrA == 0)
            {
                this.PrzesunWskaznik();
                return;
            }

            this.PrzesunWskaznik(this.SpisInstrukcji[this.ObecnaInstrukcja].Operand);
        }

        private void BXC()
        {
            this.RejestrB ^= this.RejestrC;
            this.PrzesunWskaznik();
        }

        private void OOUT()
        {
            this.Wynik.Add(this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand) % 8);
            this.PrzesunWskaznik();
        }

        private void BDV()
        {
            this.RejestrB = this.RejestrA / Convert.ToInt32(Math.Pow(2, this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand)));
            this.PrzesunWskaznik();
        }

        private void CDV()
        {
            this.RejestrC = this.RejestrA / Convert.ToInt32(Math.Pow(2, this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand)));
            this.PrzesunWskaznik();
        }

        private int ZwrocCombo(int i) => i switch
        {
            0 or 1 or 2 or 3 => i,
            4 => RejestrA,
            5 => RejestrB,
            6 => RejestrC,
            _ => throw new ArgumentOutOfRangeException(nameof(i), "Index must be beetween 0 and 6.")
        };

        private void PrzesunWskaznik(int i = -1)
        {
            this.ObecnaInstrukcja = i == -1 ? this.ObecnaInstrukcja + 1 : i / 2;
        }

        private bool SprawdzWskaznik()
        {
            return this.ObecnaInstrukcja < this.SpisInstrukcji.Count;
        }
    }
}