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

        this.RokComboBox.SelectedIndex = this.RokComboBox.Items.Count - 1;
        this.DzienComboBox.SelectedIndex = 11;
        this.ZadanieComboBox.SelectedIndex = 0;

        //this.TestButton_Click(null, null);
    }

    private void TestButton_Click(object sender, EventArgs e)
    {
        this.Rok = Convert.ToInt32(this.RokComboBox.SelectedItem);
        this.Dzien = Convert.ToInt32(this.DzienComboBox.SelectedItem);
        this.Zadanie = this.ZadanieComboBox.SelectedIndex;

        this.WynikTextBox.Text = $"{this.Rok} {this.Dzien} {this.Zadanie}";

        switch(this.Rok)
        {
            case 2024:
                switch(this.Dzien)
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
                    case 25:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D25Z01(true) : throw new NotImplementedException();
                        break;
                }
                break;
        }

        try
        {
            this.wykonajZadanie.RozwiazanieZadania();
        }
        catch(NullReferenceException)
        {
            MessageBox.Show("Podany dzień lub zadanie zostały nie utworzone");
            return;
        }
        catch(Exception ex)
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
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D15Z02() : new Zadania._2024.D15Z02();
                        break;
                    case 25:
                        this.wykonajZadanie = this.Zadanie == 0 ? new Zadania._2024.D25Z01() : throw new NotImplementedException();
                        break;
                }
                break;
        }

        try
        {
            this.wykonajZadanie.RozwiazanieZadania();
        }
        catch(NullReferenceException)
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