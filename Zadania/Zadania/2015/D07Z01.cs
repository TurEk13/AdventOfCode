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
	private List<Operacja> Operacje;
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
		string[] dzialanie;

		foreach(string komenda in this.Komendy)
		{
			przewod = komenda.Split("->").Select(p => p.Trim()).ToArray();

			if (!this.Przewody.ContainsKey(przewod[1]) && UInt16.TryParse(przewod[0], out UInt16 wartosc))
			{
				this.Przewody.Add(przewod[1], wartosc);
				continue;
			}

            if ((dzialanie = przewod[0].Split(' ')).Length == 1)
            {
                this.Operacje.Add(new(dzialanie[0], przewod[1], string.Empty, string.Empty));
				continue;
            }

            if ((dzialanie = przewod[0].Split(' ')).Length == 2)
			{
				this.Operacje.Add(new(dzialanie[0], dzialanie[1], przewod[1], string.Empty));
				continue;
			}

            if ((dzialanie = przewod[0].Split(' ')).Length == 3)
            {
                this.Operacje.Add(new(dzialanie[0], dzialanie[1], dzialanie[2], przewod[1]));
            }
		}
    }
	
    public string PokazRozwiazanie()
    {
        return this.ZapaloneZarowki.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
	
	private void Wypelnij()
	{
		bool wartoscB = false, wynikB = false, wartoscLB = false, wartoscPB = false;
		UInt16 wartosc = 0, wynik = 0, wartoscL = 0,  wartoscP = 0;
		Operacja operacja;

		while(this.Operacje.Count > 0)
		{
			for(int wierszI = 0; wierszI < this.Operacje.Count; wierszI++)
			{
				wartoscLB = wartoscPB = wartoscB = wynikB = false;

				operacja = this.Operacje[wierszI];

				if(operacja.c.Equals(string.Empty))
				{
					if(UInt16.TryParse(operacja.a, out wynik))
					{
						this.Przewody.Add(operacja.b, wynik);
					}

					continue;
				}

				if (operacja.a.Equals("NOT"))
				{
					if (this.Przewody.ContainsKey(operacja.b))
					{
						this.Przewody.TryGetValue(operacja.b, out wartosc);
						wartoscB = true;
						wynikB = false;
					}

					if (this.Przewody.ContainsKey(operacja.c))
					{
						this.Przewody.TryGetValue(operacja.c, out wynik);
						wartoscB = false;
						wynikB = true;
					}

					switch (operacja.a, wartoscB, wynikB)
					{
						case ("NOT", true, false):
							this.Przewody.Add(operacja.c, Convert.ToUInt16(UInt16.MaxValue - wartosc));
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
						case ("NOT", false, true):
							this.Przewody.Add(operacja.b, Convert.ToUInt16(UInt16.MaxValue - wynik));
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
					}

					continue;
				}

				if (!operacja.a.Equals("NOT"))
				{
					if (this.Przewody.ContainsKey(operacja.a))
					{
						this.Przewody.TryGetValue(operacja.a, out wartoscL);
						wartoscLB = true;
					}

					if (this.Przewody.ContainsKey(operacja.c))
					{
						this.Przewody.TryGetValue(operacja.c, out wartoscP);
						wartoscPB = true;
					}

					if (this.Przewody.ContainsKey(operacja.wynik))
					{
						this.Przewody.TryGetValue(operacja.wynik, out wynik);
						wynikB = true;
					}

					switch (operacja.b, wartoscLB, wartoscPB, wynikB)
					{
						case ("LSHIFT", true, false, false):
							wynik = Convert.ToUInt16(wartoscL << Convert.ToUInt16(operacja.c));
							this.Przewody.Add(operacja.wynik, wynik);
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
						case ("RSHIFT", true, false, false):
							wynik = Convert.ToUInt16(wartoscL >> Convert.ToUInt16(operacja.c));
							this.Przewody.Add(operacja.wynik, wynik);
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
						case ("AND", true, true, false):
							wynik = Convert.ToUInt16(wartoscL & wartoscP);
							this.Przewody.Add(operacja.wynik, wynik);
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
						case ("OR", true, true, false):
							wynik = Convert.ToUInt16(wartoscL | wartoscP);
							this.Przewody.Add(operacja.wynik, wynik);
							this.Operacje.RemoveAt(wierszI);
							wierszI--;
							continue;
					}
				}
			}
		}
	}

	private record Operacja(string a, string b, string c, string wynik);
}