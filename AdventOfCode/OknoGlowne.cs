using System;
using System.Windows.Forms;
using Zadania;

namespace AdventOfCode;

public partial class OknoGlowne : Form
{
    private int Rok;
    private int Dzien;
    private int Zadanie;

    private IZadanie wykonajZadanie;

    public OknoGlowne()
    {
        InitializeComponent();

        RokComboBox.Items.AddRange(["2015", "2016", "2024", "2025"]);
        DzienComboBox.Items.AddRange(["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25"]);

        this.RokComboBox.SelectedIndex = this.RokComboBox.Items.Count - 3;
        this.DzienComboBox.SelectedIndex = 2;
        this.ZadanieComboBox.SelectedIndex = 0;
    }

    private void TestButton_Click(object sender, EventArgs e)
    {
        this.Rok = Convert.ToInt32(this.RokComboBox.SelectedItem);
        this.Dzien = Convert.ToInt32(this.DzienComboBox.SelectedItem);
        this.Zadanie = this.ZadanieComboBox.SelectedIndex;

        this.WynikTextBox.Text = $"{this.Rok} {this.Dzien} {this.Zadanie}";

        switch (this.Rok)
        {
            case 2015:
                switch (this.Dzien)
                {
                    case 4:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D04Z01(true) : throw new NotImplementedException();
                        break;
                    case 7:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D07Z01(true) : throw new NotImplementedException();
                        break;
                    case 8:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D08Z01(true) : new Zadania._2015.D08Z02(true);
                        break;
                    case 9:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D09Z01(true) : throw new NotImplementedException();
                        break;
                    case 10:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D10Z01(true) : throw new NotImplementedException();
                        break;
                    case 11:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D11Z01(true) : throw new NotImplementedException();
                        break;
                    case 12:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D12Z01(true) : new Zadania._2015.D12Z02(true);
                        break;
                    case 13:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D13Z01(true) : throw new NotImplementedException();
                        break;
                    case 14:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D14Z01(true) : new Zadania._2015.D14Z02(true);
                        break;
                    case 15:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D15Z01(true) : new Zadania._2015.D15Z02(true);
                        break;
                    case 17:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D17Z01(true) : new Zadania._2015.D17Z02(true);
                        break;
                    case 18:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D18Z01(true) : new Zadania._2015.D18Z02(true);
                        break;
                    case 19:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D19Z01(true) : new Zadania._2015.D19Z02(true);
                        break;
                    case 23:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D23Z01(true) : new Zadania._2015.D23Z01(true);
                        break;
                }
                break;
            case 2016:
                switch(this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2016.D01Z01(true) : new Zadania._2016.D01Z02(true);
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2016.D02Z01(true) : new Zadania._2016.D02Z02(true);
                        break;
                }
                break;
            case 2024:
                switch (this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D01Z01(true) : new Zadania._2024.D01Z02(true);
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D02Z01(true) : new Zadania._2024.D02Z02(true);
                        break;
                    case 3:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D03Z01(true) : new Zadania._2024.D03Z02(true);
                        break;
                    case 4:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D04Z01(true) : new Zadania._2024.D04Z02(true);
                        break;
                    case 5:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D05Z01(true) : new Zadania._2024.D05Z02(true);
                        break;
                    case 6:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D06Z01(true) : throw new NotImplementedException();
                        break;
                    case 7:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D07Z01(true) : new Zadania._2024.D07Z02(true);
                        break;
                    case 9:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D09Z01(true) : new Zadania._2024.D09Z02(true);
                        break;
                    case 11:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D11Z01(true) : new Zadania._2024.D11Z02(true);
                        break;
                    case 14:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D14Z01(true) : new Zadania._2024.D14Z02(true);
                        break;
                    case 15:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D15Z01(true) : new Zadania._2024.D15Z02(true);
                        break;
                    case 23:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D23Z01(true) : new Zadania._2024.D23Z02(true);
                        break;
                    case 24:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D24Z01(true) : throw new NotImplementedException();
                        break;
                    case 25:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D25Z01(true) : throw new NotImplementedException();
                        break;
                }
                break;
            case 2025:
                switch(this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2025.D01Z01(true) : new Zadania._2025.D01Z02(true);
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2025.D02Z01(true) : new Zadania._2025.D02Z02(true);
                        break;
                    case 3:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2025.D03Z01(true) : new Zadania._2025.D03Z02(true);
                        break;
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                        throw new NotImplementedException();
                }
                break;
        }

        try
        {
            this.wykonajZadanie.RozwiazanieZadania();
        }
        catch (NullReferenceException)
        {
            MessageBox.Show("Podany dzień lub zadanie zostały nie utworzone");
            return;
        }
        catch (Exception ex)
        {
            //
        }

        this.WynikTextBox.Text = $"Rozwiązanie: {this.wykonajZadanie.PokazRozwiazanie()}";
    }

    private void ZadanieButton_Click(object sender, EventArgs e)
    {
        this.Rok = Convert.ToInt32(this.RokComboBox.SelectedItem);
        this.Dzien = Convert.ToInt32(this.DzienComboBox.SelectedItem);
        this.Zadanie = this.ZadanieComboBox.SelectedIndex;

        switch (this.Rok)
        {
            case 2015:
                switch (this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D01Z01() : new Zadania._2015.D01Z02();
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D02Z01() : new Zadania._2015.D02Z02();
                        break;
                    case 3:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D03Z01() : new Zadania._2015.D03Z02();
                        break;
                    case 4:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D04Z01() : new Zadania._2015.D04Z02();
                        break;
                    case 5:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D05Z01() : new Zadania._2015.D05Z02();
                        break;
                    case 6:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D06Z01() : new Zadania._2015.D06Z02();
                        break;
                    case 7:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D07Z01() : new Zadania._2015.D07Z01();
                        break;
                    case 8:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D08Z01() : new Zadania._2015.D08Z02();
                        break;
                    case 9:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D09Z01() : new Zadania._2015.D09Z01();
                        break;
                    case 10:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D10Z01() : throw new NotImplementedException();
                        break;
                    case 11:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D11Z01() : new Zadania._2015.D11Z01();
                        break;
                    case 12:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D12Z01() : new Zadania._2015.D12Z02();
                        break;
                    case 13:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D13Z01() : new Zadania._2015.D13Z02();
                        break;
                    case 14:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D14Z01() : new Zadania._2015.D14Z02();
                        break;
                    case 15:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D15Z01() : new Zadania._2015.D15Z02();
                        break;
                    case 16:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D16Z01() : new Zadania._2015.D16Z02();
                        break;
                    case 17:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D17Z01() : new Zadania._2015.D17Z02();
                        break;
                    case 18:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D18Z01() : new Zadania._2015.D18Z02();
                        break;
                    case 19:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D19Z01() : new Zadania._2015.D19Z02();
                        break;
                    case 20:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D20Z01() : new Zadania._2015.D20Z02();
                        break;
                    case 21:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D21Z01() : new Zadania._2015.D21Z02();
                        break;
                    case 23:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2015.D23Z01() : new Zadania._2015.D23Z01();
                        break;
                }
                break;
            case 2016:
                switch(this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2016.D01Z01() : new Zadania._2016.D01Z02();
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2016.D02Z01() : new Zadania._2016.D02Z02();
                        break;
                    case 3:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2016.D03Z01() : new Zadania._2016.D03Z02();
                        break;
                }
                break;
            case 2024:
                switch (this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D01Z01() : new Zadania._2024.D01Z02();
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D02Z01() : new Zadania._2024.D02Z02();
                        break;
                    case 3:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D03Z01() : new Zadania._2024.D03Z02();
                        break;
                    case 4:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D04Z01() : new Zadania._2024.D04Z02();
                        break;
                    case 5:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D05Z01() : new Zadania._2024.D05Z02();
                        break;
                    case 6:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D06Z01() : throw new NotImplementedException();
                        break;
                    case 7:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D07Z01() : new Zadania._2024.D07Z02();
                        break;
                    case 9:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D09Z01() : new Zadania._2024.D09Z02();
                        break;
                    case 11:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D11Z01() : new Zadania._2024.D11Z02();
                        break;
                    case 14:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D14Z01() : new Zadania._2024.D14Z02();
                        break;
                    case 15:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D15Z01() : new Zadania._2024.D15Z02();
                        break;
                    case 23:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D23Z01() : new Zadania._2024.D23Z02();
                        break;
                    case 24:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D24Z01() : throw new NotImplementedException();
                        break;
                    case 25:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D25Z01() : throw new NotImplementedException();
                        break;
                }
                break;
            case 2025:
                switch(this.Dzien)
                {
                    case 1:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2025.D01Z01() : new Zadania._2025.D01Z02();
                        break;
                    case 2:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2025.D02Z01() : new Zadania._2025.D02Z02();
                        break;
                    case 3:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2025.D03Z01() : new Zadania._2025.D03Z02();
                        break;
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                        throw new NotImplementedException();
                }
                break;
        }

        try
        {
            this.wykonajZadanie.RozwiazanieZadania();
        }
        catch (NullReferenceException)
        {
            MessageBox.Show("Podany dzień lub zadanie zostały nie utworzone");
            return;
        }
        catch (Exception ex)
        {
            //
        }

        this.WynikTextBox.Text = $"Rozwiązanie: {this.wykonajZadanie.PokazRozwiazanie()}";
    }
}