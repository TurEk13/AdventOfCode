using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Zadania._2024;

public class D04Z02 : IZadanie
{
    private int suma;
    private int wysokosc;
    private int szerokosc;
    private List<char[]> litery;

    public D04Z02(bool daneTestowe = false)
    {
        this.suma = 0;
        this.litery = new();
        string linia;

        FileStream fs = daneTestowe ? new(".\\Dane\\2024\\04\\proba.txt", FileMode.Open, FileAccess.Read) : new(".\\Dane\\2024\\04\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);
        while((linia = sr.ReadLine()) != null)
        {
            this.litery.Add(linia.ToCharArray());
        }

        sr.Close(); fs.Close();

        this.wysokosc = this.litery.Count;
        this.szerokosc = this.litery[0].Length;
    }

    public void RozwiazanieZadania()
    {
        for (int i = 1; i < this.wysokosc - 1; i++)
        {
            for (int j = 1; j < this.szerokosc - 1; j++)
            {
                if (this.litery[i][j] == 'A')
                {
                    //góra
                    if (this.litery[i - 1][j - 1] == 'M' && this.litery[i - 1][j + 1] == 'M' && this.litery[i + 1][j - 1] == 'S' && this.litery[i + 1][j + 1] == 'S')
                    {
                        this.suma++;
                    }

                    //lewo
                    if (this.litery[i - 1][j - 1] == 'M' && this.litery[i - 1][j + 1] == 'S' && this.litery[i + 1][j - 1] == 'M' && this.litery[i + 1][j + 1] == 'S')
                    {
                        this.suma++;
                    }

                    //dół
                    if (this.litery[i - 1][j - 1] == 'S' && this.litery[i - 1][j + 1] == 'S' && this.litery[i + 1][j - 1] == 'M' && this.litery[i + 1][j + 1] == 'M')
                    {
                        this.suma++;
                    }

                    //prawo
                    if (this.litery[i - 1][j - 1] == 'S' && this.litery[i - 1][j + 1] == 'M' && this.litery[i + 1][j - 1] == 'S' && this.litery[i + 1][j + 1] == 'M')
                    {
                        this.suma++;
                    }
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}