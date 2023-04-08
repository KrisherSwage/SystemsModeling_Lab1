using System;
using System.Windows.Forms;

namespace MS_Lab1_WinF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillChart();
        }

        private void fillChart()
        {
            var myChart = new ChartForNeyman(chart1);

            int n = Convert.ToInt32(textBox1.Text);
            int m = Convert.ToInt32(textBox2.Text);
            int alpha = Convert.ToInt32(textBox3.Text);
            int beta = Convert.ToInt32(textBox4.Text);
            int a = Convert.ToInt32(textBox5.Text);
            int b = Convert.ToInt32(textBox6.Text);
            int c = Convert.ToInt32(textBox7.Text);

            myChart.Calculate(a, b, c, n, m);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
