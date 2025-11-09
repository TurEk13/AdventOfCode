using System.Globalization;
using System.IO;

namespace Zadania._2015;

public class D01Z02 : IZadanie
{
    private string Mapa;
    private int Pietro;
    private int KtoryZnak;
    public D01Z02()
    {
        this.Pietro = 0;
        FileStream fs = new(".\\Dane\\2015\\01\\dane.txt", FileMode.Open, FileAccess.Read);

        StreamReader sr = new(fs);

        this.Mapa = sr.ReadToEnd();

        sr.Close(); fs.Close();
    }

    public void RozwiazanieZadania()
    {
        int i = 0;
        while(this.Pietro != -1 && i < this.Mapa.Length)
        {
            this.Pietro = this.Mapa[i] == '(' ? this.Pietro + 1 : this.Pietro - 1;
            i++;
        }

        this.KtoryZnak = i;
    }

    public string PokazRozwiazanie()
    {
        return this.KtoryZnak.ToString("N0", CultureInfo.CreateSpecificCulture("pl-PL"));
    }
}