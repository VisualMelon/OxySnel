using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.Threading;
using OxyPlot;

namespace OxySnel
{
    public class Snel
    {
        private const string DefaultWindowTitle = "OxyPlot";

        private static readonly object DispatcherLock = new object();
        private static CancellationTokenSource AppCancellationToken = null;

        public static Task Show(PlotModel plotModel, string windowTitle = DefaultWindowTitle)
        {
            return Invoke(() =>
            {
                var w = new MainWindow() { Title = windowTitle };
                w.Context.PlotModel = plotModel;
                w.Show();
            });
        }

        public static void Kill()
        {
            AppCancellationToken?.Cancel();
        }

        public static Task Invoke(Action action)
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

            return Dispatcher.UIThread.InvokeAsync(action);
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
