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
            BarGraphExample();
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
                hour.Points.Add(new DataPoint(0.0, 30 * (now.Hour % 12)));
                hour.Points.Add(new DataPoint(0.4, 30 * (now.Hour % 12)));

                minute.Points.Clear();
                minute.Points.Add(new DataPoint(0.0, 6 * now.Minute));
                minute.Points.Add(new DataPoint(0.6, 6 * now.Minute));

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
    }
}