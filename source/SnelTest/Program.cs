using System;
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

            p1.LinearX("X").LinearY("Y").Function("sin", Math.Sin, -10, 10, 0.1);
            p2.LinearX("X").LinearY("Y").Function("cos", Math.Cos, -10, 10, 0.1);

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

            Console.ReadKey(true);

            Snel.Kill();
        }
    }
}
