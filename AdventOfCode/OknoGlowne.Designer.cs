using System.Drawing;
using System.Windows.Forms;

namespace AdventOfCode;

partial class OknoGlowne
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        RokComboBox = new ComboBox();
        DzienComboBox = new ComboBox();
        ZadanieComboBox = new ComboBox();
        RokLabel = new Label();
        DzienLabel = new Label();
        ZadanieLabel = new Label();
        TestButton = new Button();
        ZadanieButton = new Button();
        WynikTextBox = new TextBox();
        SuspendLayout();
        // 
        // RokComboBox
        // 
        RokComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        RokComboBox.FormattingEnabled = true;
        RokComboBox.Items.AddRange(new object[] { "2024" });
        RokComboBox.Location = new Point(68, 12);
        RokComboBox.Name = "RokComboBox";
        RokComboBox.Size = new Size(100, 23);
        RokComboBox.TabIndex = 1;
        // 
        // DzienComboBox
        // 
        DzienComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        DzienComboBox.FormattingEnabled = true;
        DzienComboBox.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "9", "11", "14", "15" });
        DzienComboBox.Location = new Point(68, 41);
        DzienComboBox.Name = "DzienComboBox";
        DzienComboBox.Size = new Size(100, 23);
        DzienComboBox.TabIndex = 2;
        // 
        // ZadanieComboBox
        // 
        ZadanieComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        ZadanieComboBox.FormattingEnabled = true;
        ZadanieComboBox.Items.AddRange(new object[] { "1", "2" });
        ZadanieComboBox.Location = new Point(68, 70);
        ZadanieComboBox.Name = "ZadanieComboBox";
        ZadanieComboBox.Size = new Size(100, 23);
        ZadanieComboBox.TabIndex = 3;
        // 
        // RokLabel
        // 
        RokLabel.AutoSize = true;
        RokLabel.Location = new Point(10, 15);
        RokLabel.Name = "RokLabel";
        RokLabel.Size = new Size(30, 15);
        RokLabel.TabIndex = 4;
        RokLabel.Text = "Rok:";
        // 
        // DzienLabel
        // 
        DzienLabel.AutoSize = true;
        DzienLabel.Location = new Point(10, 44);
        DzienLabel.Name = "DzienLabel";
        DzienLabel.Size = new Size(39, 15);
        DzienLabel.TabIndex = 5;
        DzienLabel.Text = "Dzień:";
        // 
        // ZadanieLabel
        // 
        ZadanieLabel.AutoSize = true;
        ZadanieLabel.Location = new Point(10, 73);
        ZadanieLabel.Name = "ZadanieLabel";
        ZadanieLabel.Size = new Size(52, 15);
        ZadanieLabel.TabIndex = 6;
        ZadanieLabel.Text = "Zadanie:";
        // 
        // TestButton
        // 
        TestButton.Location = new Point(68, 99);
        TestButton.Name = "TestButton";
        TestButton.Size = new Size(100, 23);
        TestButton.TabIndex = 8;
        TestButton.Text = "Test";
        TestButton.UseVisualStyleBackColor = true;
        TestButton.Click += TestButton_Click;
        // 
        // ZadanieButton
        // 
        ZadanieButton.Location = new Point(68, 128);
        ZadanieButton.Name = "ZadanieButton";
        ZadanieButton.Size = new Size(100, 23);
        ZadanieButton.TabIndex = 9;
        ZadanieButton.Text = "Zadanie";
        ZadanieButton.UseVisualStyleBackColor = true;
        ZadanieButton.Click += ZadanieButton_Click;
        // 
        // WynikTextBox
        // 
        WynikTextBox.Location = new Point(188, 7);
        WynikTextBox.Multiline = true;
        WynikTextBox.Name = "WynikTextBox";
        WynikTextBox.ReadOnly = true;
        WynikTextBox.Size = new Size(584, 542);
        WynikTextBox.TabIndex = 10;
        // 
        // OknoGlowne
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 561);
        Controls.Add(WynikTextBox);
        Controls.Add(ZadanieButton);
        Controls.Add(TestButton);
        Controls.Add(ZadanieLabel);
        Controls.Add(DzienLabel);
        Controls.Add(RokLabel);
        Controls.Add(ZadanieComboBox);
        Controls.Add(DzienComboBox);
        Controls.Add(RokComboBox);
        IsMdiContainer = true;
        Name = "OknoGlowne";
        Text = "Advent of Code";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ComboBox RokComboBox;
    private ComboBox DzienComboBox;
    private ComboBox ZadanieComboBox;
    private Label RokLabel;
    private Label DzienLabel;
    private Label ZadanieLabel;
    private Button TestButton;
    private Button ZadanieButton;
    private TextBox WynikTextBox;
}
