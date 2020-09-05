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
                new LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = xtitle },
                new LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Left, Title = ytitle }
            );
        }

        public static Plot Log(string xtitle, string ytitle)
        {
            return new Plot(
                new LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = xtitle },
                new LogarithmicAxis() { Position = OxyPlot.Axes.AxisPosition.Left, Title = ytitle }
            );
        }

        public static Plot LogX(string xtitle, string ytitle)
        {
            return new Plot(
                new LogarithmicAxis() { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = xtitle },
                new LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Left, Title = ytitle }
            );
        }

        public static Plot LogLog(string xtitle, string ytitle)
        {
            return new Plot(
                new LogarithmicAxis() { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = xtitle },
                new LogarithmicAxis() { Position = OxyPlot.Axes.AxisPosition.Left, Title = ytitle }
            );
        }

        public LinearColorAxis LinearColorAxis(string title, OxyPalette pallete = null)
        {
            pallete = pallete ?? OxyPalettes.Gray(100);
            var lca = new LinearColorAxis() { Title = title, Position = AxisPosition.Right };

            PlotModel.Axes.Add(lca);

            return lca;
        }

        public ScatterSeries Scatter(IEnumerable<double> x, IEnumerable<double> y, string title = null, MarkerType markerType = (MarkerType)(-1))
        {
            if (markerType < 0)
                markerType = NextMarkerType();

            var ss = new ScatterSeries() { Title = title, MarkerType = markerType };

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

            var ls = new LineSeries() { Title = title, MarkerType = markerType };

            foreach (var dp in ZipXY(x, y))
            {
                ls.Points.Add(dp);
            }

            PlotModel.Series.Add(ls);

            return ls;
        }

        public HeatMapSeries Matrix(double[,] data, string title = null, MarkerType markerType = (MarkerType)(-1))
        {
            var hms = new HeatMapSeries() { Title = title, Data = data, X0 = 1, X1 = data.GetLength(0), Y0 = 1, Y1 = data.GetLength(1), Interpolate = false };

            PlotModel.Series.Add(hms);

            return hms;
        }

        private static IEnumerable<DataPoint> ZipXY(IEnumerable<double> x, IEnumerable<double> y)
        {
            return x.Zip(y, (px, py) => new DataPoint(px, py));
        }

        public HistogramSeries Histogram(IList<double> samples, string title = null)
        {
            var hs = new HistogramSeries() { Title = title };

            var bins = HistogramHelpers.CreateUniformBins(samples.Min(), samples.Max(), 10);
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

        public static implicit operator PlotModel(Plot p)
        {
            return p.PlotModel;
        }
    }
}
