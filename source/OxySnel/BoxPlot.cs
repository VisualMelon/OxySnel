using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxySnel
{
    // TODO: less terrible API, closer to the Plot class (call it BoxPlot)
    public class BoxPlotting
    {
        public static PlotModel PrepareBoxPlot(IReadOnlyList<string> labels, string categoryTitle, string yTitle, string xAxisKey = null, string yAxisKey = null)
        {
            var plot = new PlotModel();
            var catAxis = new CategoryAxis() { Title = categoryTitle, Position = AxisPosition.Bottom, Key = xAxisKey ?? "x" };
            plot.Axes.Add(catAxis);
            plot.Axes.Add(new LinearAxis() { Title = yTitle, Position = AxisPosition.Left, Key = yAxisKey ?? "y" });

            foreach (var label in labels)
                catAxis.Labels.Add(label);

            return plot;
        }

        public static BoxPlotSeries BoxPlot(PlotModel plot, IReadOnlyList<IReadOnlyList<double>> samples)
        {
            var bp = new BoxPlotSeries();
            for (int i = 0; i < samples.Count; i++)
            {
                var sample = samples[i];
                var bpi = Box(i, sample);
                bp.Items.Add(bpi);
            }
            plot.Series.Add(bp);

            return bp;
        }

        public static BoxPlotItem Box(double x, IEnumerable<double> samples)
        {
            var sorted = samples.OrderBy(s => s).ToArray();

            var lq = Terp(sorted, 0.25);
            var median = Terp(sorted, 0.5);
            var uq = Terp(sorted, 0.75);
            var mean = sorted.Average();

            var lt = lq - (median - lq) * 1.5;
            var ut = uq + (uq - median) * 1.5;

            var low = Math.Min(samples.Where(s => s >= lt).Min(), lq);
            var high = Math.Max(samples.Where(s => s <= ut).Max(), uq);

            var bpi = new BoxPlotItem(x, low, lq, median, uq, high) { Mean = mean };

            foreach (var o in sorted.Where(s => s < low || s > high))
                bpi.Outliers.Add(o);

            return bpi;
        }

        public static double Terp(IReadOnlyList<double> sorted, double x)
        {
            var c = sorted.Count - 1;
            var cx = c * x;
            var l = (int)Math.Floor(cx);
            var h = (int)Math.Ceiling(cx);

            var subx = cx - l;
            return (subx * sorted[h]) + (1 - subx) * sorted[l];
        }
    }
}
