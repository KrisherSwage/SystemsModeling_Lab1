using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MS_Lab1_WinF
{
    internal class ChartForNeyman
    {
        private Chart Chart;

        public ChartForNeyman(Chart chart)
        {
            Chart = chart;
            SetUpChart();
        }

        private void Clear() //очищаем все, чтобы не дублировалось
        {
            Chart.Series.Clear();
            Chart.ChartAreas.Clear();
            Chart.Titles.Clear();
        }

        private void SetUpChart() //начальные настройки
        {
            Clear();
            AddArea();
            AddSeries();
            Chart.Titles.Add("Метод фон Неймана");

        }

        private void AddArea() //хз что
        {
            ChartArea area = new ChartArea();
            Chart.ChartAreas.Add(area);
            //area.AxisX.IsStartedFromZero = false;
            //area.AxisX.MajorTickMark.Interval = 0.10;
            //area.AxisX.Minimum = 0;
            Chart.ChartAreas[0].AxisX.Title = "Ось X"; //подпись оси
            Chart.ChartAreas[0].AxisY.Title = "Высота столбцов";

            //что делаю для отображения между столбцами 2
            Chart.ChartAreas[0].AxisX.IsLabelAutoFit = true; //разрешен произвольный интервал 
            Chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Number; //это для нормального отображения значений на x
            Chart.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;

            Chart.ChartAreas[0].AxisY.IsLabelAutoFit = true;
            Chart.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number; //это для нормального отображения значений на y
            Chart.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
        }

        private void AddSeries() //
        {
            Series series = new Series();
            series.Name = "Метод_фон_Неймана";
            series.ChartArea = Chart.ChartAreas[0].Name;
            series.ChartType = SeriesChartType.Column;
            series.BorderWidth = 3; //граница вокруг столбцов
            series.BorderDashStyle = ChartDashStyle.Solid; //стиль границы вокруг столбцов
            series.BorderColor = Color.Black;
            series["PointWidth"] = "0.92"; //ширина столбца в %
            //Chart.AlignDataPointsByAxisLabel();
            Chart.Series.Add(series);

            Chart.Series.Add("Line"); //линия для функции
            Chart.Series["Line"].Color = Color.Red; //цвет будущей линии
            Chart.Series["Line"].BorderWidth = 3; //ширина будущей линии
        }

        static Random rnd = new Random();

        public void Calculate(int a, int b, int c, int n)
        {
            double square = 1.0; //площадь под функцией равна единице
            //double H = (2.0 * square) / ((c - a) + 2.0 * (b - c)); //рассчитываем H, с условием размера площади
            double H = 2.0 / (2 * b - a - c); //рассчитываем H, с условием размера площади (2/49)
            double halfH = (H / 2.0); //по заданию (варианту) (1/49)

            double[,] coordinates = new double[n, 2]; //массив для генерации чисел со случайными координатами

            //Celling - округление вверх
            //Floor - округление вниз
            //Round - округление к целому
            //Truncate - отсекает дробную часть
            for (int i = 0; i < n; i++)
            {
                double x = Math.Ceiling(rnd.NextDouble() * (Convert.ToDouble(b) - Convert.ToDouble(a)) + Convert.ToDouble(a)); //х от a до b
                double y = Math.Round(rnd.NextDouble() * (Convert.ToDouble(H) - Convert.ToDouble(0)) + Convert.ToDouble(0), 10); //y от 0 до H

                coordinates[i, 0] = x;
                coordinates[i, 1] = y;

            }

            var quantity = new double[b - a]; //массив для записи количества попавших чисел
            for (int i = 0; i < n; i++)
            {
                int coorX = Convert.ToInt32(coordinates[i, 0]);

                if (coorX > c) //если x > c
                {
                    quantity[coorX - 1] += 1;
                }
                else
                {
                    if (coordinates[i, 1] <= halfH) //если y <= H/2
                    {
                        quantity[coorX - 1] += 1;
                    }
                }

            }

            int averageAmount = n / (b - a);
            for (int i = a; i < b; i++) //изменение высоты столбцов (чтобы соответствовало площади S)
            {
                quantity[i] /= averageAmount / H;
            }

            //заполнение значений х
            var xForChart = new double[b - a];
            xForChart[0] = 0.5; //что делаю для отображения между столбцами шаг 1 //сдвигаю изначальное значение
            for (int i = a + 1; i < b; i++)
            {
                xForChart[i] = xForChart[i - 1] + 1;
            }

            Chart.ChartAreas[0].AxisX.Interval = 1.0; //что делаю для отображения между столбцами шаг 3
            Chart.ChartAreas[0].AxisX.IntervalOffset = 0; //сдвигаю начальный отступ

            Chart.Series[0].Points.DataBindXY(xForChart, quantity);

            Chart.Series["Line"].Points.Add(new DataPoint(a, halfH)); //точка 1
            Chart.Series["Line"].Points.Add(new DataPoint(c, halfH)); //точка 2
            Chart.Series["Line"].Points.Add(new DataPoint(c, H)); //точка 3
            Chart.Series["Line"].Points.Add(new DataPoint(b, H)); //точка 4
            Chart.Series["Line"].ChartType = SeriesChartType.Line; //черчение линии по точкам

        }


    }
}
