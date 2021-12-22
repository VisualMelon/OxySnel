using System;
using System.Linq;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxySnel;

namespace SnelTestCS
{
    class Program
    {
        static void Main(string[] args)
        {
            OxySnel.Snel.StartOnThread(HistogramExample, true);
        }

        static void ClockExample()
        {
            var plot = new PlotModel();
            plot.PlotType = PlotType.Polar;
            plot.PlotAreaBorderThickness = new OxyThickness(0.0);

            var mag = new MagnitudeAxis();
            mag.Minimum = 0.0;
            mag.Maximum = 1.0;
            mag.MinimumPadding = 0.0;
            mag.MaximumPadding = 0.0;
            mag.IsAxisVisible = false;
            plot.Axes.Add(mag);

            var ang = new AngleAxis();
            ang.Minimum = 0.0;
            ang.Maximum = 360.0;
            ang.MajorStep = 90.0;
            ang.MinorStep = 30.0;
            ang.StartAngle = 90.0;
            ang.EndAngle = -270.0;
            ang.LabelFormatter = angle => angle == 0 ? "12" : (angle / 30).ToString();
            plot.Axes.Add(ang);

            var hour = new LineSeries();
            hour.StrokeThickness = 6.0;
            plot.Series.Add(hour);

            var minute = new LineSeries();
            minute.StrokeThickness = 4.0;
            plot.Series.Add(minute);

            var second = new LineSeries();
            second.StrokeThickness = 2.0;
            plot.Series.Add(second);

            void update()
            {
                var now = DateTime.Now;

                plot.Title = now.ToLongDateString();
                plot.Subtitle = now.ToLongTimeString();

                hour.Points.Clear();
                hour.Points.Add(new DataPoint(0.0, 30 * (now.TimeOfDay.TotalHours % 12.0)));
                hour.Points.Add(new DataPoint(0.4, 30 * (now.TimeOfDay.TotalHours % 12.0)));

                minute.Points.Clear();
                minute.Points.Add(new DataPoint(0.0, 6 * (now.TimeOfDay.TotalMinutes % 60)));
                minute.Points.Add(new DataPoint(0.6, 6 * (now.TimeOfDay.TotalMinutes % 60)));

                second.Points.Clear();
                second.Points.Add(new DataPoint(-0.2, 6 * now.Second));
                second.Points.Add(new DataPoint(0.8, 6 * now.Second));

                plot.InvalidatePlot(true);
            }

            OxySnel.Snel.Show(plot);

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(100);
                    await Snel.Invoke(update);
                }
            });

            Console.ReadKey(true);
            Snel.Kill();
        }

        static void BarGraphExample()
        {
            // make a plot...
            var plot = new PlotModel() { Title = "Time-to-plot" };

            // add some series...
            var bs = new BarSeries() { BaseValue = 0 };
            bs.Items.Add(new BarItem(10));
            bs.Items.Add(new BarItem(2));
            plot.Series.Add(bs);

            // add some axes
            var categoryAxis = new CategoryAxis() { Position = AxisPosition.Left, Angle = -90 };
            categoryAxis.Labels.Add("Without OxySnel");
            categoryAxis.Labels.Add("With OxySnel");
            plot.Axes.Add(categoryAxis);

            var timeAxis = new LinearAxis() { Position = AxisPosition.Bottom, Title = "Time", Unit = "Money" };
            plot.Axes.Add(timeAxis);

            // show the plot
            OxySnel.Snel.Show(plot);

            // pause
            Console.ReadKey(true);

            // kill it
            OxySnel.Snel.Kill();
        }

        static void QuickAndEasyExamples()
        {
            var rnd = new Random();

            // generate random data
            var xs = Enumerable.Range(0, 101).Select(i => (double)i).ToList();
            var ys1 = xs.Select(x => x + x * rnd.NextDouble()).ToList();
            var ys2 = xs.Select(x => x * 1.4 + x * rnd.NextDouble() * 0.2).ToList();

            // time plot
            var timePlot = Plot.Linear("Time", "Swans");
            timePlot.Scatter(xs, ys1);
            timePlot.Line(xs, ys2);
            Snel.Show(timePlot, "Swans over time");

            // sample plot
            var samplePlot = Plot.Linear("Value", "Frequency");
            samplePlot.Histogram(ys1);
            Snel.Show(samplePlot, "Swans");

            // matrix plot
            var matrixPlot = Plot.Linear("Col", "Row");
            matrixPlot.PlotModel.PlotType = PlotType.Cartesian;
            foreach (var a in matrixPlot.PlotModel.Axes)
                if (a is LinearAxis la)
                    la.MinimumMinorStep = la.MinimumMajorStep = 1;
            matrixPlot.LinearColorAxis(null, OxyPalettes.Hot64);
            matrixPlot.Matrix(Array2d(100, 100, (x, y) => Math.Sin(Math.Sqrt(x * x + y * y) / 10)), null);
            Snel.Show(matrixPlot);
        }

        static void HistogramExample()
        {
            var rnd = new Random();

            var plot = Plot.Linear("Frequency", "Joyness");
            plot.Histogram(Uniform(rnd, 15, 5, 250, 3));

            Snel.Show(plot, "A box plot");
        }

        static void BoxPlotExample()
        {
            var rnd = new Random();

            var plot = BoxPlotting.PrepareBoxPlot(new[] { "OxySnel", "SloomPlot" }, "Package", "Joyness");
            BoxPlotting.BoxPlot(plot, new[] { Uniform(rnd, 15, 5, 80, 3), Uniform(rnd, 10, 8, 50, 3) });

            Snel.Show(plot, "A box plot").ContinueWith(t => t.Result.ShowMenu = false);
        }

        private static double[] Uniform(Random rnd, double mean, double width, int count, int k = 1)
        {
            return Enumerable.Range(0, count)
                .Select(_ => Enumerable.Range(0, k).Select(_ => rnd.NextDouble()).Average())
                .Select(r => mean + width * (r * 2 - 1))
                .ToArray();
        }

        private static T[,] Array2d<T>(int d1, int d2, Func<int, int, T> func)
        {
            var arr = new T[d1, d2];
            
            for (int i = 0; i < d1; i++)
            {
                for (int j = 0; j < d2; j++)
                {
                    arr[i, j] = func(i, j);
                }
            }

            return arr;
        }
    }
}