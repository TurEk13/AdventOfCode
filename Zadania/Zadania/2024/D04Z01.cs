using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Zadania._2024;

public class D04Z01 : IZadanie
{
    private int suma;
    private int wysokosc;
    private int szerokosc;
    private string XMASpoziom;
    private List<char[]> litery;

    public D04Z01(bool daneTestowe = false)
    {
        this.suma = 0;
        this.litery = new();
        string linia;

        this.XMASpoziom = File.ReadAllText(daneTestowe ? ".\\Dane\\2024\\04\\proba.txt" : ".\\Dane\\2024\\04\\dane.txt");

        FileStream fs = new(daneTestowe ? ".\\Dane\\2024\\04\\proba.txt" : ".\\Dane\\2024\\04\\dane.txt", FileMode.Open, FileAccess.Read);

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
        //poziom
        Regex xmasR = new Regex("xmas", RegexOptions.IgnoreCase);
        Regex xmasRR = new Regex("samx", RegexOptions.IgnoreCase);
        this.suma += xmasR.Matches(this.XMASpoziom).Count + xmasRR.Matches(this.XMASpoziom).Count;

        //pion
        for (int i = 0; i < this.wysokosc - 3; i++)
        {
            for (int j = 0; j < this.szerokosc; j++)
            {
                //góra dół
                if (this.litery[i][j] == 'X' && this.litery[i + 1][j] == 'M' && this.litery[i + 2][j] == 'A' && this.litery[i + 3][j] == 'S')
                {
                    this.suma++;
                }

                //dół góra
                if (this.litery[this.wysokosc - 1 - i][j] == 'X' && this.litery[this.wysokosc - 2 - i][j] == 'M' && this.litery[this.wysokosc - 3 - i][j] == 'A' && this.litery[this.wysokosc - 4 - i][j] == 'S')
                {
                    this.suma++;
                }
            }
        }

        //skos
        for (int i = 0; i < this.wysokosc - 3; i++)
        {
            for (int j = 0; j < this.szerokosc - 3; j++)
            {
                //prawy dół
                if (this.litery[i][j] == 'X' && this.litery[i + 1][j + 1] == 'M' && this.litery[i + 2][j + 2] == 'A' && this.litery[i + 3][j + 3] == 'S')
                {
                    this.suma++;
                }

                //lewy dół
                if (this.litery[i][this.szerokosc - 1 - j] == 'X' && this.litery[i + 1][this.szerokosc - 2 - j] == 'M' && this.litery[i + 2][this.szerokosc - 3 - j] == 'A' && this.litery[i + 3][this.szerokosc - 4 - j] == 'S')
                {
                    this.suma++;
                }

                //prawa góra
                if (this.litery[this.wysokosc - 1 - i][j] == 'X' && this.litery[this.wysokosc - 2 - i][j + 1] == 'M' && this.litery[this.wysokosc - 3 - i][j + 2] == 'A' && this.litery[this.wysokosc - 4 - i][j + 3] == 'S')
                {
                    this.suma++;
                }

                //lewa góra
                if (this.litery[this.wysokosc - 1 - i][this.szerokosc - 1 - j] == 'X' && this.litery[this.wysokosc - 2 - i][this.szerokosc - 2 - j] == 'M' && this.litery[this.wysokosc - 3 - i][this.szerokosc - 3 - j] == 'A' && this.litery[this.wysokosc - 4 - i][this.szerokosc - 4 - j] == 'S')
                {
                    this.suma++;
                }
            }
        }
    }

    public string PokazRozwiazanie()
    {
        return this.suma.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}