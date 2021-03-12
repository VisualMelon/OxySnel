using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace OxySnel
{
    public class Plot
    {
        public string Title
        {
            get => PlotModel.Title;
            set => PlotModel.Title = value;
        }

        public Axis XAxis { get; }
        public Axis YAxis { get; }
        public PlotModel PlotModel { get; }

        int nextMarkerType = 1;
        private MarkerType NextMarkerType()
        {
            var n = nextMarkerType;
            nextMarkerType = (nextMarkerType + 1) % 8;
            if (nextMarkerType == 0)
                nextMarkerType = 1;
            return (MarkerType)n;
        }

        public static Plot Linear(string xtitle, string ytitle)
        {
            return new Plot(
                new LinearAxis() { Position = AxisPosition.Bottom, Title = xtitle, Key = "x" },
                new LinearAxis() { Position = AxisPosition.Left, Title = ytitle, Key = "y" }
            );
        }

        public static Plot LogY(string xtitle, string ytitle)
        {
            return new Plot(
                new LinearAxis() { Position = AxisPosition.Bottom, Title = xtitle, Key = "x" },
                new LogarithmicAxis() { Position = AxisPosition.Left, Title = ytitle, Key = "y" }
            );
        }

        public static Plot LogX(string xtitle, string ytitle)
        {
            return new Plot(
                new LogarithmicAxis() { Position = AxisPosition.Bottom, Title = xtitle, Key = "x" },
                new LinearAxis() { Position = AxisPosition.Left, Title = ytitle, Key = "y" }
            );
        }

        public static Plot LogLog(string xtitle, string ytitle)
        {
            return new Plot(
                new LogarithmicAxis() { Position = AxisPosition.Bottom, Title = xtitle, Key = "x" },
                new LogarithmicAxis() { Position = AxisPosition.Left, Title = ytitle, Key = "y" }
            );
        }

        public LinearColorAxis LinearColorAxis(string title, OxyPalette pallete = null)
        {
            pallete = pallete ?? OxyPalettes.Gray(100);
            var lca = new LinearColorAxis() { Title = title, Position = AxisPosition.Right, Key = "c" };

            PlotModel.Axes.Add(lca);

            return lca;
        }

        public ScatterSeries Scatter(IEnumerable<double> x, IEnumerable<double> y, string title = null, MarkerType markerType = (MarkerType)(-1))
        {
            if (markerType < 0)
                markerType = NextMarkerType();

            var ss = new ScatterSeries() { Title = title, MarkerType = markerType, XAxisKey = XAxis.Key, YAxisKey = YAxis.Key };

            foreach (var dp in ZipXY(x, y))
            {
                ss.Points.Add(new ScatterPoint(dp.X, dp.Y));
            }

            PlotModel.Series.Add(ss);

            return ss;
        }

        public LineSeries Line(IEnumerable<double> x, IEnumerable<double> y, string title = null, MarkerType markerType = (MarkerType)(-1))
        {
            if (markerType < 0)
                markerType = NextMarkerType();

            var ls = new LineSeries() { Title = title, MarkerType = markerType, XAxisKey = XAxis.Key, YAxisKey = YAxis.Key };

            foreach (var dp in ZipXY(x, y))
            {
                ls.Points.Add(dp);
            }

            PlotModel.Series.Add(ls);

            return ls;
        }

        public HeatMapSeries Matrix(double[,] data, string title = null)
        {
            var hms = new HeatMapSeries() { Title = title, Data = data, X0 = 1, X1 = data.GetLength(0), Y0 = 1, Y1 = data.GetLength(1), Interpolate = false, XAxisKey = XAxis.Key, YAxisKey = YAxis.Key };

            PlotModel.Series.Add(hms);

            return hms;
        }

        private static IEnumerable<DataPoint> ZipXY(IEnumerable<double> x, IEnumerable<double> y)
        {
            return x.Zip(y, (px, py) => new DataPoint(px, py));
        }

        public HistogramSeries Histogram(IReadOnlyList<double> samples, string title = null, int binCount = 10, double? min = null, double? max = null)
        {
            var hs = new HistogramSeries() { Title = title, XAxisKey = XAxis.Key, YAxisKey = YAxis.Key };

            var bins = HistogramHelpers.CreateUniformBins(min ?? samples.Min(), max ?? samples.Max(), binCount);
            var items = HistogramHelpers.Collect(samples, bins, new BinningOptions(BinningOutlierMode.RejectOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.IncludeExtremeValues));
            hs.Items.AddRange(items);

            PlotModel.Series.Add(hs);

            return hs;
        }

        public Plot(Axis xAxis, Axis yAxis)
        {
            XAxis = xAxis ?? throw new ArgumentNullException(nameof(xAxis));
            YAxis = yAxis ?? throw new ArgumentNullException(nameof(yAxis));

            PlotModel = new PlotModel();
            PlotModel.Axes.Add(XAxis);
            PlotModel.Axes.Add(YAxis);
        }

        // from existing plot (for e.g. Y2)
        private Plot(PlotModel plotModel, Axis xAxis, Axis yAxis)
        {
            PlotModel = plotModel ?? throw new ArgumentNullException(nameof(plotModel));
            XAxis = xAxis ?? throw new ArgumentNullException(nameof(xAxis));
            YAxis = yAxis ?? throw new ArgumentNullException(nameof(yAxis));
        }

        public static implicit operator PlotModel(Plot p)
        {
            return p.PlotModel;
        }

        public Plot Y2(string y2title)
        {
            return new Plot(
                PlotModel,
                XAxis,
                new LinearAxis() { Position = AxisPosition.Right, Title = y2title, Key = "y2" }
            );
        }

        public Plot LogY2(string y2title)
        {
            return new Plot(
                PlotModel,
                XAxis,
                new LogarithmicAxis() { Position = AxisPosition.Right, Title = y2title, Key = "y2" }
            );
        }
    }
}
