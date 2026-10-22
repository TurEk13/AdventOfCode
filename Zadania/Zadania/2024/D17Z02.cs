using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public partial class D17Z02 : IZadanie
{
    [GeneratedRegex(@"Program: (?<daneProgramu>(\d,?){1,})")]
    private static partial Regex DaneProgramu();
    private Chronospatial _Chronospatial;
    private int _DlugoscProgramu;

    public D17Z02(bool daneTestowe = false)
    {
        StreamReader sr = new StreamReader(new FileStream(daneTestowe ? ".\\Dane\\2024\\17\\proba.txt" : ".\\Dane\\2024\\17\\dane.txt", FileMode.Open, FileAccess.Read));
        string daneProgramu = sr.ReadToEnd();
        sr.Close();

        Regex KodProgramu = DaneProgramu();
        Match kod = KodProgramu.Match(daneProgramu);
        string tmp = kod.Value[(kod.Value.IndexOf(' ') + 1) .. ].Replace(",", "");
        this._DlugoscProgramu = tmp.Length;

        this._Chronospatial = new Chronospatial(kod.Groups["daneProgramu"].Value.Split(',').Select(dp => Convert.ToInt32(dp)).ToList<int>(), tmp);
    }

    public void RozwiazanieZadania()
    {
        this._Chronospatial.Rozwiaz(0UL, this._DlugoscProgramu - 1);
    }

    public string PokazRozwiazanie()
    {
        return this._Chronospatial.WynikA.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }

    private record Instrukcja(int Opcode, UInt64 Operand);

    private class Chronospatial
    {
        private UInt64 RejestrA;
        private UInt64 RejestrB;
        private UInt64 RejestrC;
        private readonly List<Instrukcja> SpisInstrukcji;
        private int ObecnaInstrukcja;
        public UInt64 WynikA { get; private set; }
        private List<int> _WynikI;
        private string _oczekiwanyWynik;

        public Chronospatial(List<int> spisInstrukcji, string wynik)
        {
            this.WynikA = UInt64.MaxValue;
            this._oczekiwanyWynik = wynik;

            this.SpisInstrukcji = new List<Instrukcja>();
            for(int i = 0; i < spisInstrukcji.Count; i += 2)
            {
                this.SpisInstrukcji.Add(new Instrukcja(spisInstrukcji[i], Convert.ToUInt64(spisInstrukcji[i + 1])));
            }
        }

        public void Rozwiaz(UInt64 obecneA, int indeks)
        {
            UInt64 noweA;

            if (indeks == -1)
            {
                this.WynikA = Math.Min(this.WynikA, obecneA);
                return;
            }

            for (UInt64 i = 0; i < 8; i++)
            {
                noweA = obecneA * 8 + i;
                this.PrzetworzProgram(noweA);

                if (this.Sprawdz(indeks))
                {
                    this.Rozwiaz(noweA, indeks - 1);
                }
            }
        }

        private bool Sprawdz(int indeksStart)
        {
            for(int i = 0; i < this._WynikI.Count; i++)
            {
                if(this._oczekiwanyWynik[indeksStart + i] - '0' != this._WynikI[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void PrzetworzProgram(UInt64 a)
        {
            this.RejestrA = a;
            this.RejestrB = 0UL;
            this.RejestrC = 0UL;
            this.ObecnaInstrukcja = 0;
            this._WynikI = new List<int>();
            
            while(this.SprawdzWskaznik())
            {
                switch (this.SpisInstrukcji[ObecnaInstrukcja].Opcode)
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
            this.RejestrA /= Convert.ToUInt64(Math.Pow(2, this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand)));
            PrzesunWskaznik();
        }

        private void BXL()
        {
            this.RejestrB ^= Convert.ToUInt64(this.SpisInstrukcji[this.ObecnaInstrukcja].Operand);
            this.PrzesunWskaznik();
        }

        private void BST()
        {
            this.RejestrB = Convert.ToUInt64(this.ZwrocCombo(this.SpisInstrukcji[this.ObecnaInstrukcja].Operand) % 8);
            this.PrzesunWskaznik();
        }

        private void JNZ()
        {
            if(this.RejestrA == 0)
            {
                this.PrzesunWskaznik();
                return;
            }

            this.PrzesunWskaznik(Convert.ToInt32(this.SpisInstrukcji[this.ObecnaInstrukcja].Operand));
        }

        private void BXC()
        {
            this.RejestrB ^= this.RejestrC;
            this.PrzesunWskaznik();
        }

        private void OOUT()
        {
            this._WynikI.Add(Convert.ToInt32(this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand) % 8));
            this.PrzesunWskaznik();
        }

        private void BDV()
        {
            this.RejestrB = this.RejestrA / Convert.ToUInt64(Math.Pow(2, this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand)));
            this.PrzesunWskaznik();
        }

        private void CDV()
        {
            this.RejestrC = this.RejestrA / Convert.ToUInt64(Math.Pow(2, this.ZwrocCombo(this.SpisInstrukcji[ObecnaInstrukcja].Operand)));
            this.PrzesunWskaznik();
        }

        private UInt64 ZwrocCombo(UInt64 i) => i switch
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