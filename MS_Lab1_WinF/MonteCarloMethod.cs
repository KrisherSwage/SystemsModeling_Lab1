using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MS_Lab1_WinF
{
    internal class MonteCarloMethod
    {
        private Chart Chart;

        public MonteCarloMethod(Chart chart)
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
            Chart.Titles.Add("Метод Монте-Карло");

        }

        private void AddArea() //хз что
        {
            ChartArea area = new ChartArea();
            Chart.ChartAreas.Add(area);
            //area.AxisX.IsStartedFromZero = false;
            //area.AxisX.MajorTickMark.Interval = 0.10;
            //area.AxisX.Minimum = 0;
            Chart.ChartAreas[0].AxisX.Title = "Ось X"; //подпись оси
            Chart.ChartAreas[0].AxisY.Title = "Ось Y";

            //что делаю для отображения между столбцами 2
            Chart.ChartAreas[0].AxisX.IsLabelAutoFit = true; //разрешен произвольный интервал 
            Chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Number; //это для нормального отображения значений на x
            Chart.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;
            Chart.ChartAreas[0].AxisX.Interval = 1.0;

            Chart.ChartAreas[0].AxisY.IsLabelAutoFit = true;
            Chart.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number; //это для нормального отображения значений на y
            Chart.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
            Chart.ChartAreas[0].AxisY.Interval = 2.0;
        }

        private void AddSeries() //
        {
            Series series = new Series();
            series.Name = "Q(x)";
            series.ChartArea = Chart.ChartAreas[0].Name;
            series.ChartType = SeriesChartType.Spline;
            series.BorderWidth = 3; //толщина линии
            series.MarkerSize = 5;
            //series.BorderDashStyle = ChartDashStyle.Solid; //стиль границы вокруг столбцов
            //series.BorderColor = Color.Black;
            //series["PointWidth"] = "0.2"; //ширина столбца в %
            Chart.Series.Add(series);

            Series series2 = new Series();
            series2.Name = "Минимум функции";
            series2.ChartArea = Chart.ChartAreas[0].Name;
            series2.ChartType = SeriesChartType.Point;
            series2.BorderWidth = 10; //толщина линии
            series2.Color = Color.Red;

            series2.MarkerSize = 10;
            series2.LabelBorderWidth = 10;
            series2.MarkerStyle = MarkerStyle.Diamond;

            Chart.Series.Add(series2);

            Series series3 = new Series();
            series3.Name = "Значения по методу Монте-Карло";
            series3.ChartArea = Chart.ChartAreas[0].Name;
            series3.ChartType = SeriesChartType.Point;
            series3.BorderWidth = 10; //толщина линии
            series3.Color = Color.DarkGreen;
            series3.MarkerSize = 10;
            Chart.Series.Add(series3);

            Series series4 = new Series();
            series4.Name = "S(x)";
            series4.ChartArea = Chart.ChartAreas[0].Name;
            series4.ChartType = SeriesChartType.Spline;
            series4.BorderWidth = 3; //толщина линии
            series4.Color = Color.GreenYellow;
            series4.MarkerSize = 3;
            Chart.Series.Add(series4);
        }

        static Random rnd = new Random();

        public void NO_CalculateTheory(int a, int b, int c, int n, int m, int alpha, int beta)
        {
            double QfrX;

            var QfromX = new double[b - a + 1];
            var QfromY = new double[b - a + 1];

            double H = 2.0 / (2 * b - a - c); //рассчитываем H, с условием размера площади (2/49)
            double halfH = (H / 2.0); //по заданию (варианту) (1/49)

            //double incr = 0;
            int incr = 0;
            for (double i = a; i < b + 1; i += 1)
            {
                QfrX = Convert.ToDouble((alpha * (i - a) * (i - a) + beta * (i - b) * (i - b)) / (2 * (b - a)) * 1.0);
                //MessageBox.Show($"{QfrX}");
                //if (i > 11)
                //{
                //    QfrX *= H;
                //}
                //else
                //{
                //    QfrX *= halfH;
                //}
                QfrX = QfrX * halfH + QfrX * H;

                QfromY[incr] = QfrX;
                QfromX[incr] = incr + a; //1
                //MessageBox.Show($"x = {QfromX[incr]} y = {QfromY[incr]}");
                incr++;
            }

            Chart.Series[0].Points.DataBindXY(QfromX, QfromY);
            //Chart.Series[0].Points.DataBindY(QfromY);

            //double xWithStar = Convert.ToDouble((alpha * a + beta * b * 1.0) / (alpha + beta) * 1.0) /*- a*/; //2
            //double QwithStar = Convert.ToDouble((alpha * a * a + beta * b * b - (((alpha * a + beta * b) * (alpha * a + beta * b)) / (alpha + beta))) / (2.0 * (b - a)));
            ////MessageBox.Show($"x* = {xWithStar} \n Q*(x) = {QwithStar}");
            //Chart.Series[1].Points.AddXY(xWithStar, QwithStar);
            //Chart.Series[1].Label = $"x* = {xWithStar} \n Q*(x) = {QwithStar}";

        }


        public void CalculateExperiment(int a, int b, int c, int n, int m, int alpha, int beta)
        {
            var QfromXJ = new double[m]; //сколько точек по методу монте-карло
            var dispXJ = new double[m]; //дисперсия
            var stanDeviat = new double[m];
            var nowQxy = new double[n];

            double QfrJI = 0;
            double disp;
            double stan;

            double H = 2.0 / (2 * b - a - c); //рассчитываем H, с условием размера площади (2/49)
            double halfH = (H / 2.0); //по заданию (варианту) (1/49)

            double sumQxy = 0; //переменная для суммы Q(x;y)
            double sumForDisp = 0;

            double xForM = a * 1.0; //первое x - начинается с a (закончится b)
            double incrForX = (b * 1.0 - a) / m; //число для увеличения x

            for (int i = 1; i < m + 1; i++) //выступает в качестве x по формуле Q=Q1+Q2
            {
                sumQxy = 0; //переменная для суммы Q(x;y)

                for (int j = 1; j < n + 1; j++) //в качестве y
                {
                    double XRand;
                    double yRand;

                    while (true) //цикл для получения тех значений, которые попали в распределение
                    {
                        XRand = Math.Round(rnd.NextDouble() * (Convert.ToDouble(b) - Convert.ToDouble(a)) + Convert.ToDouble(a), 3);
                        yRand = Math.Round(rnd.NextDouble() * (Convert.ToDouble(H) - Convert.ToDouble(0)) + Convert.ToDouble(0), 10);
                        if (XRand > 11)
                        {
                            break;
                        }
                        else
                        {
                            if (yRand <= halfH)
                            {
                                break;
                            }
                        }
                    }

                    //определение рабочей формулы для подсчета Q
                    if (xForM > XRand)
                    {
                        QfrJI = alpha * (xForM - XRand);
                    }
                    if (xForM < XRand)
                    {
                        QfrJI = beta * (XRand - xForM);
                    }
                    if (xForM == XRand)
                    {
                        QfrJI = 0; //формула подсчета. y - случайное число
                    }

                    sumQxy += QfrJI; //сумма Q(x;y)

                    nowQxy[j - 1] = QfrJI;
                }

                QfromXJ[i - 1] = sumQxy / n; //Q среднее от x. Всего таких Q m штук
                double qfrXJ = QfromXJ[i - 1];

                //считаем дисперсию и отклонение
                sumForDisp = 0;
                for (int j = 0; j < n; j++)
                {
                    sumForDisp += (qfrXJ - nowQxy[j]) * (qfrXJ - nowQxy[j]);
                }
                disp = (1.0 / (n - 1.0)) * (sumForDisp);

                stanDeviat[i - 1] = Math.Pow(disp, 0.5);

                xForM += incrForX; //увеличиваем x
            }

            var xForQ = new double[m]; //массив для отображения точек по иксу
            xForQ[0] = a; //начало с a
            for (int i = 1; i < m; i++)
            {
                xForQ[i] = xForQ[i - 1] + Convert.ToDouble((b * 1.0 - a) / m); //заполение массива увеличивающися числом
            }

            Chart.Series[2].Points.DataBindXY(xForQ, QfromXJ); //вывод точек Монте-Карло

            Chart.Series[3].Points.DataBindXY(xForQ, stanDeviat);
        }

        public void CalculateTheory(int a, int b, int c, int n, int m, int alpha, int beta)
        {
            n = 500000;

            var QfromXJ = new double[m]; //сколько точек по методу монте-карло

            double QfrJI = 0;

            double H = 2.0 / (2 * b - a - c); //рассчитываем H, с условием размера площади (2/49)
            double halfH = (H / 2.0); //по заданию (варианту) (1/49)

            double sumQxy = 0; //переменная для суммы Q(x;y)

            double xForM = a * 1.0; //первое x - начинается с a (закончится b)
            double incrForX = (b * 1.0 - a) / m; //число для увеличения x

            for (int i = 1; i < m + 1; i++) //выступает в качестве x по формуле Q=Q1+Q2
            {
                sumQxy = 0; //переменная для суммы Q(x;y)

                for (int j = 1; j < n + 1; j++) //в качестве y
                {
                    double XRand;
                    double yRand;

                    while (true) //цикл для получения тех значений, которые попали в распределение
                    {
                        XRand = Math.Round(rnd.NextDouble() * (Convert.ToDouble(b) - Convert.ToDouble(a)) + Convert.ToDouble(a), 3);
                        yRand = Math.Round(rnd.NextDouble() * (Convert.ToDouble(H) - Convert.ToDouble(0)) + Convert.ToDouble(0), 10);
                        if (XRand > 11)
                        {
                            break;
                        }
                        else
                        {
                            if (yRand <= halfH)
                            {
                                break;
                            }
                        }
                    }

                    //определение рабочей формулы для подсчета Q
                    if (xForM > XRand)
                    {
                        QfrJI = alpha * (xForM - XRand);
                    }
                    if (xForM < XRand)
                    {
                        QfrJI = beta * (XRand - xForM);
                    }
                    if (xForM == XRand)
                    {
                        QfrJI = 0; //формула подсчета. y - случайное число
                    }

                    sumQxy += QfrJI; //сумма Q(x;y)

                }

                QfromXJ[i - 1] = sumQxy / n; //Q среднее от x. Всего таких Q m штук

                xForM += incrForX; //увеличиваем x
            }

            var xForQ = new double[m]; //массив для отображения точек по иксу
            xForQ[0] = a; //начало с a
            for (int i = 1; i < m; i++)
            {
                xForQ[i] = xForQ[i - 1] + Convert.ToDouble((b * 1.0 - a) / m);
            }

            Chart.Series[0].Points.DataBindXY(xForQ, QfromXJ);

            double min = 2000000000;
            int indexMin = 0;
            for (int i = 0; i < QfromXJ.Length; i++)
            {
                if (QfromXJ[i] < min)
                {
                    min = QfromXJ[i];
                    indexMin = i;
                }
            }

            Chart.Series[1].Points.AddXY(xForQ[indexMin], min);
            Chart.Series[1].Label = $"x* = {xForQ[indexMin]} \n Q*(x) = {min}";
        }

    }
}
