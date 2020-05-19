using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;

namespace OxySnel.Fluent
{
    public class Plot
    {
        public Plot(string title)
        {
            PlotModel = new PlotModel()
            {
                Title = title,
            };
        }

        public Plot(PlotModel plotModel)
        {
            PlotModel = plotModel ?? throw new ArgumentNullException(nameof(plotModel));
        }

        private int NextKey = 1;
        public PlotModel PlotModel { get; }

        public XAxis LinearX(string title, string key = null) => new XAxis(this, new LinearAxis() { Title = title, Key = key ?? ("X" + NextKey++), Position = AxisPosition.Bottom });
    }

    public class XAxis
    {
        public XAxis(Plot plot, Axis axis)
        {
            Plot = plot ?? throw new ArgumentNullException(nameof(plot));
            Axis = axis ?? throw new ArgumentNullException(nameof(axis));
        }

        private int NextKey = 1;
        public Plot Plot { get; }
        public Axis Axis { get; }

        public YAxis LinearY(string title, string key = null) => new YAxis(this, new LinearAxis() { Title = title, Key = key ?? (Axis.Key + "Y" + NextKey++), Position = AxisPosition.Left });
    }

    public class YAxis
    {
        public YAxis(XAxis xAxis, Axis axis)
        {
            XAxis = xAxis ?? throw new ArgumentNullException(nameof(XAxis));
            Axis = axis ?? throw new ArgumentNullException(nameof(axis));
        }

        public XAxis XAxis { get; }
        public Axis Axis { get; }

        public LineSeries Line(string title, double[] x, double[] y)
        {
            var ls = new LineSeries()
            {
                Title = title,
                RenderInLegend = true,
            };

            for (int i = 0; i < x.Length; i++)
            {
                ls.Points.Add(new DataPoint(x[i], y[i]));
            }

            XAxis.Plot.PlotModel.Series.Add(ls);

            return ls;
        }

        public LineSeries Function(string title, Func<double, double> f, double x0, double x1, double dx)
        {
            var fs = new FunctionSeries(f, x0, x1, dx)
            {
                Title = title,
                RenderInLegend = true
            };

            XAxis.Plot.PlotModel.Series.Add(fs);

            return fs;
        }
    }
}
