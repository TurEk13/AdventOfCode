using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D01Z01 : IZadanie
{
    private string Mapa;
    private int Pietro;
    public D01Z01()
    {
        this.Pietro = 0;
        FileStream fs = new(".\\Dane\\2015\\01\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);

        this.Mapa = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        for(int i = 0; i < this.Mapa.Length; i++)
        {
            this.Pietro = this.Mapa[i] == '(' ? this.Pietro + 1 : this.Pietro - 1;
        }
    }

    public string PokazRozwiazanie()
    {
        return this.Pietro.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}