using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.Threading;
using OxyPlot;
using OxySnel.Fluent;
using SharpDX.DXGI;

namespace OxySnel
{
    public class Snel
    {
        private static readonly object DispatcherLock = new object();
        private static CancellationTokenSource AppCancellationToken = null;

        public static void Show(Plot plot)
        {
            Invoke(() =>
            {
                var w = new MainWindow();
                w.Context.PlotModel = plot.PlotModel;
                w.Show();
            });
        }

        public static void Show(PlotModel plotModel)
        {
            Invoke(() =>
            {
                var w = new MainWindow();
                w.Context.PlotModel = plotModel;
                w.Show();
            });
        }

        public static void Kill()
        {
            AppCancellationToken?.Cancel();
        }

        public static void Invoke(Action action)
        {
            if (AppCancellationToken == null)
            {
                lock (DispatcherLock)
                {
                    if (AppCancellationToken == null)
                    {
                        AppCancellationToken = SpinUp();
                    }
                }
            }

            Dispatcher.UIThread.InvokeAsync(action);
        }

        private static CancellationTokenSource SpinUp()
        {
            var cts = new CancellationTokenSource();
            var sem = new ManualResetEventSlim(false);
            void start()
            {
                BuildAvaloniaApp().Start(main, new string[0]);

                void main(Application app, string[] args)
                {
                    sem.Set();
                    app.Run(cts.Token);
                }
            }

            var ts = new System.Threading.ThreadStart(start);
            var t = new Thread(ts);
            t.Start();

            sem.Wait();
            return cts;
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
        }
    }
}
