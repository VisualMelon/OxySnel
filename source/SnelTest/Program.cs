using System;
using System.Linq;
using System.Threading.Tasks;
using OxyPlot;
using OxySnel;
using OxySnel.Fluent;

namespace SnelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = new Plot("Plot 1");
            var p2 = new Plot("Plot 2");

            var xs = Enumerable.Range(0, 1000).Select(x => x / 100.0).ToArray();
            var s = p1.LinearX("X").LinearY("Y").Line("sin", xs, xs.Select(Math.Sin).ToArray());
            var p2y = p2.LinearX("X").LinearY("Y");
            var c = p2y.Line("delta", new double[0], new double[0]);
            p2y.Axis.Minimum = 0.0; p2y.Axis.Maximum = 0.1;

            Snel.Invoke(() =>
            {
                var w = new MainWindow();
                w.Context.PlotModel = p1.PlotModel;
                w.Show();
            });

            Snel.Invoke(() =>
            {
                var w = new MainWindow();
                w.Context.PlotModel = p2.PlotModel;
                w.Show();
            });

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var last = sw.Elapsed.TotalSeconds;
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(30);

                    var next = sw.Elapsed.TotalSeconds;

                    lock (p1.PlotModel.SyncRoot)
                    {
                        s.Points.Clear();
                        for (var t = 0.0; t < 0 + 100; t += 0.01)
                        {
                            s.Points.Add(new OxyPlot.DataPoint(t, Math.Sin(t + next)));
                        }
                    }

                    lock (p2.PlotModel.SyncRoot)
                    {
                        if (c.Points.Count > 1000)
                            c.Points.RemoveAt(0);
                        c.Points.Add(new DataPoint(next, next - last));
                    }

                    last = next;

                    await Snel.Invoke(() =>
                    {
                        p1.PlotModel.InvalidatePlot(true);
                        p2.PlotModel.InvalidatePlot(true);
                    });
                }
            });

            Console.ReadKey(true);

            Snel.Kill();
        }
    }
}