using System.Windows.Forms;

namespace Interval.Forms.Graphic
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.processorCartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CartesianMemory = new LiveCharts.WinForms.CartesianChart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.StopButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.processorsNameCombobox = new System.Windows.Forms.ComboBox();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.processorCartesianChart);
            this.groupBox1.Location = new System.Drawing.Point(274, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1151, 252);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Processador";
            // 
            // processorCartesianChart
            // 
            this.processorCartesianChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processorCartesianChart.Location = new System.Drawing.Point(3, 19);
            this.processorCartesianChart.Name = "processorCartesianChart";
            this.processorCartesianChart.Size = new System.Drawing.Size(1145, 230);
            this.processorCartesianChart.TabIndex = 0;
            this.processorCartesianChart.Text = "processorCartesianChart";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CartesianMemory);
            this.groupBox2.Location = new System.Drawing.Point(274, 270);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1151, 262);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Memória";
            // 
            // CartesianMemory
            // 
            this.CartesianMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CartesianMemory.Location = new System.Drawing.Point(3, 19);
            this.CartesianMemory.Name = "CartesianMemory";
            this.CartesianMemory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CartesianMemory.Size = new System.Drawing.Size(1145, 240);
            this.CartesianMemory.TabIndex = 0;
            this.CartesianMemory.Text = "CartessianMemory";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.StopButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.StartButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.processorsNameCombobox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SearchBox, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(256, 126);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // StopButton
            // 
            this.StopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StopButton.Location = new System.Drawing.Point(3, 96);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(250, 27);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartButton.Location = new System.Drawing.Point(3, 65);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(250, 25);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // processorsNameCombobox
            // 
            this.processorsNameCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processorsNameCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processorsNameCombobox.FormattingEnabled = true;
            this.processorsNameCombobox.Location = new System.Drawing.Point(3, 34);
            this.processorsNameCombobox.Name = "processorsNameCombobox";
            this.processorsNameCombobox.Size = new System.Drawing.Size(250, 23);
            this.processorsNameCombobox.TabIndex = 0;
            // 
            // SearchBox
            // 
            this.SearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchBox.Location = new System.Drawing.Point(3, 3);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(250, 23);
            this.SearchBox.TabIndex = 3;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1437, 544);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Gráfico de consumo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox processorsNameCombobox;
        private Button StartButton;
        private Button StopButton;
        private LiveCharts.WinForms.CartesianChart CartesianMemory;
        private LiveCharts.WinForms.CartesianChart processorCartesianChart;
        private TextBox SearchBox;
    }
}