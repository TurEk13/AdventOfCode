using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D07Z01 : IZadanie
{
    private List<string> Komendy;
	private List<Operacja> Operacje;
    private SortedDictionary<string, UInt16> Przewody;

    public D07Z01(bool daneTestowe = false)
    {
        this.Komendy = new();
        this.Przewody = new();
		this.Operacje = new();
        
        FileStream fs = new(daneTestowe ? ".\\Dane\\2015\\07\\proba.txt" : ".\\Dane\\2015\\07\\dane.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new(fs);
        string linia;

        while((linia = sr.ReadLine()) is not null)
        {
            this.Komendy.Add(linia);
        }

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        this.WczytajPrzewody();

		this.Wypelnij();
    }

    private void WczytajPrzewody()
    {
		string[] przewod;
		string[] dzialanie;

		foreach(string komenda in this.Komendy)
		{
			przewod = komenda.Split("->").Select(p => p.Trim()).ToArray();

			if (!this.Przewody.ContainsKey(przewod[1]) && UInt16.TryParse(przewod[0], out UInt16 wartosc))
			{
				this.Przewody.Add(przewod[1], wartosc);
				continue;
			}

			dzialanie = przewod[0].Split(' ');

			switch(dzialanie.Length)
            {
                case 1:
					this.Operacje.Add(new(dzialanie[0], przewod[1], string.Empty, string.Empty));
					break;
				case 2:
					this.Operacje.Add(new(dzialanie[0], dzialanie[1], przewod[1], string.Empty));
					break;
				case 3:
					this.Operacje.Add(new(dzialanie[0], dzialanie[1], dzialanie[2], przewod[1]));
					break;
            }
		}
    }
	
    public string PokazRozwiazanie()
    {
        return this.Przewody["a"].ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private void Wypelnij()
	{
		Operacja operacja;
		UInt16 lewa = 0, prawa = 0;
		bool lewaB, prawaB;

		while(this.Operacje.Count > 0)
		{
			for(int wierszI = 0; wierszI < this.Operacje.Count; wierszI++)
            {
				lewaB = prawaB = false;

				operacja = this.Operacje[wierszI];

				// Sprawdzenie operacji NOT
				if(!operacja.A.Equals("NOT") && operacja.C.Equals(string.Empty))
                {
                    if(this.Przewody.ContainsKey(operacja.A) && UInt16.TryParse(operacja.B, out _))
                    {
                        this.Przewody.Add(operacja.B, Convert.ToUInt16(operacja.A));
						this.Operacje.RemoveAt(wierszI);
						wierszI--;
                    }

					if(this.Operacje.Count == 1)
                    {
                        this.Przewody.Add(operacja.B, Convert.ToUInt16(this.Przewody[operacja.A]));
						this.Operacje.RemoveAt(0);
                    }
                }

                // Sprawdzenie operacji NOT
				if(operacja.A.Equals("NOT"))
                {
                    if(this.Przewody.ContainsKey(operacja.B))
                    {
                        this.Przewody.Add(operacja.C, Convert.ToUInt16(UInt16.MaxValue - Convert.ToUInt16(this.Przewody[operacja.B])));
						this.Operacje.RemoveAt(wierszI);
						wierszI--;
                    }
                }

				// Sprawdzenie operacji LSHIFT, RSHIFT, OR, AND
				if(operacja.B.Contains("LSHIFT") ||operacja.B.Contains("RSHIFT") || operacja.B.Contains("OR") || operacja.B.Contains("AND"))
                {
                    //Wczytanie wartości po lewej i prawej operatora
					if(this.Przewody.ContainsKey(operacja.A))
					{
						lewa = this.Przewody[operacja.A];
						lewaB = true;
					}

					if(!lewaB &&UInt16.TryParse(operacja.A, out lewa))
					{
						lewaB = true;
					}

					if(this.Przewody.ContainsKey(operacja.C))
					{
						prawa = this.Przewody[operacja.C];
						prawaB = true;
					}

					if(!prawaB && UInt16.TryParse(operacja.C, out prawa))
					{
						prawaB = true;
					}

					switch(operacja.B, lewaB, prawaB)
                    {
                        case ("LSHIFT", true, true):
							this.Przewody.Add(operacja.Wynik, Convert.ToUInt16(lewa << prawa));
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							break;
						case ("RSHIFT", true, true):
							this.Przewody.Add(operacja.Wynik, Convert.ToUInt16(lewa >> prawa));
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							break;
						case ("OR", true, true):
							this.Przewody.Add(operacja.Wynik, Convert.ToUInt16(lewa | prawa));
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							break;
						case ("AND", true, true):
							this.Przewody.Add(operacja.Wynik, Convert.ToUInt16(lewa & prawa));
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							break;
                    }
                }
            }
		}
	}

	private record Operacja(string A, string B, string C, string Wynik);
}