using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zadania._2015;

public class D07Z01 : IZadanie
{
    private Int32 ZapaloneZarowki;
    private List<string> Komendy;
	private List<List<string>> Operacje;
    private SortedDictionary<string, UInt16> Przewody;

    public D07Z01(bool daneTestowe = false)
    {
        this.Komendy = new();
        this.Przewody = new();
		this.Operacje = new();
        
        this.ZapaloneZarowki = 0;
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

		foreach(string komenda in this.Komendy)
		{
			przewod = komenda.Split("->").Select(p => p.Trim()).ToArray();

			if (!this.Przewody.ContainsKey(przewod[1]) && UInt16.TryParse(przewod[0], out UInt16 wartosc))
			{
				this.Przewody.Add(przewod[1], wartosc);
				continue;
			}

			this.Operacje.Add([.. przewod[0].Split(' '), przewod[1]]);
		}
    }
	
    public string PokazRozwiazanie()
    {
        return this.ZapaloneZarowki.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private void Wypelnij()
	{
		bool dodanoElement = true;
		bool wartoscB = false, wynikB = false, wartoscLB = false, wartoscPB = false;
		UInt16 wartosc = 0, wynik = 0, wartoscL = 0,  wartoscP = 0;
		List<string> operacja; 

		while (dodanoElement)
		{
			dodanoElement = false;
			for(int wierszI = 0; wierszI < this.Operacje.Count; wierszI++)
			{
				wartoscLB = wartoscPB = wartoscB = wynikB = false;

				operacja = this.Operacje[wierszI];

				if (operacja[0] == "NOT")
				{
					//NOT x[1] => x[2] -> NOT wartosc => wynik

					if (this.Przewody.ContainsKey(operacja[1]))
					{
						this.Przewody.TryGetValue(operacja[1], out wartosc);
						wartoscB = true;
						wynikB = false;
					}

					if (this.Przewody.ContainsKey(operacja[2]))
					{
						this.Przewody.TryGetValue(operacja[2], out wynik);
						wartoscB = false;
						wynikB = true;
					}

					switch (operacja[0], wartoscB, wynikB)
					{
						case ("NOT", true, false):
							this.Przewody.Add(operacja[2], Convert.ToUInt16(UInt16.MaxValue - wartosc));
							dodanoElement = true;
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
						case ("NOT", false, true):
							this.Przewody.Add(operacja[1], Convert.ToUInt16(UInt16.MaxValue - wynik));
							dodanoElement = true;
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
					}
				}

				if (this.Przewody.ContainsKey(operacja[0]))
				{
					this.Przewody.TryGetValue(operacja[0], out wartoscL);
					wartoscLB = true;
				}

				if (this.Przewody.ContainsKey(operacja[2]))
				{
					this.Przewody.TryGetValue(operacja[2], out wartoscP);
					wartoscPB = true;
				}

				if (this.Przewody.ContainsKey(operacja[3]))
				{
					this.Przewody.TryGetValue(operacja[3], out wynik);
					wynikB = true;
				}
				
				switch(operacja[1], wartoscLB, wartoscPB, wynikB)
				{
					case ("LSHIFT", true, false, false):
						wynik = Convert.ToUInt16(wartoscL << Convert.ToUInt16(operacja[2]));
						this.Przewody.Add(operacja[3], wynik);
						this.Operacje.RemoveAt(wierszI);
						wierszI--;
						continue;
					case ("RSHIFT", true, false, false):
						wynik = Convert.ToUInt16(wartoscL >> Convert.ToUInt16(operacja[2]));
						this.Przewody.Add(operacja[3], wynik);
						this.Operacje.RemoveAt(wierszI);
						wierszI--;
						continue;
					case("AND", true, true, false):
						wynik = Convert.ToUInt16(wartoscL & wartoscP);
						this.Przewody.Add(operacja[3], wynik);
						this.Operacje.RemoveAt(wierszI);
						wierszI--;
						continue;
					case("OR", true, true, false):
						wynik = Convert.ToUInt16(wartoscL | wartoscP);
						this.Przewody.Add(operacja[3], wynik);
						this.Operacje.RemoveAt(wierszI);
						wierszI--;
						continue;
				}
			}
		}
		
		int aa = 3 + 4;
	}
}