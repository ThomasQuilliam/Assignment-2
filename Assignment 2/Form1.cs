using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        class row // this subroutine shows us about are main fields, time, altitude, velocity and acceleration 
        {
            public double Time;
            public double Altitude;
            public double Velocity;
            public double Acceleration;

        }

        List<row> table = new List<row>();

        private void CalculateVelocity() // this subroutine shows us how to calculate the velocity, the code uses doubles and strings to 
        {
            for (int i = 1; i < table.Count; i++)
            {
                double dA = table[i].Altitude - table[i - 1].Altitude;
                double dT = table[i].Time - table[i - 1].Time;
                table[i].Velocity = dA / dT;
            }
        }

        private void CalculateAcceleration()
        {
            for (int i = 2; i < table.Count; i++)
            {
                double dV = table[i].Velocity - table[i - 1].Velocity;
                double dT = table[i].Time - table[i - 1].Time;
                table[i].Acceleration = dV / dT;
            }

        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "csv files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] r = sr.ReadLine().Split(',');
                            table.Last().Time = double.Parse(r[0]);
                            table.Last().Altitude = double.Parse(r[1]);

                        }
                    }
                    CalculateVelocity();
                    CalculateAcceleration();
                }

                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "failed to open.");
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "is not in the required format.");
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format.");
                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " has rows that are the same time.");
                }
            }

        }

        private void altitudeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Altitude",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.Time, r.Altitude);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "Altitude /m";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void saveCSVToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "csv Files|*.csv";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.WriteLine("Time /s, Altitude /C, Velocity /A, Acceleration / A/s");
                        foreach (row r in table)
                        {
                            sw.WriteLine(r.Time + "," + r.Altitude + "," + r.Velocity + "," + r.Acceleration);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + " failed to save.");
                }
            }
        }

        private void savePNGToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "png Files|*.png";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + " failed to save.");
                }
            }
        }
    }

}

    