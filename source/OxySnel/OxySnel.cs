﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using OxyPlot;

namespace OxySnel
{
    public class Snel
    {
        private const string DefaultWindowTitle = "OxyPlot";

        private static readonly object DispatcherLock = new object();
        private static CancellationTokenSource AppCancellationToken = null;

        public static void StartOnThread(Action started, bool callbackOffthread)
        {
            lock (DispatcherLock)
            {
                if (AppCancellationToken != null)
                {
                    throw new InvalidOperationException("OxySnel is already started.");
                }

                AppCancellationToken = new CancellationTokenSource();
            }

            if (callbackOffthread)
            {
                var t = new Thread(new ThreadStart(started));
                started = t.Start;
            }
            Run(AppCancellationToken.Token, started);
        }

        public static Task<IPlotWindow> Show(PlotModel plotModel, string windowTitle = DefaultWindowTitle)
        {
            return Invoke(() =>
            {
                var w = new PlotWindow() { Title = windowTitle };
                w.Context.PlotModel = plotModel;
                w.Show();
                return (IPlotWindow)w;
            });
        }

        public static void Kill()
        {
            AppCancellationToken?.Cancel();
        }

        public static Task<TResult> Invoke<TResult>(Func<TResult> action)
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

            var ts = new System.Threading.ThreadStart(() => Run(cts.Token, () => sem.Set()));
            var t = new Thread(ts);
            t.Start();

            sem.Wait();
            return cts;
        }

        private static void Run(CancellationToken cancellationToken, Action started)
        {
            BuildAvaloniaApp().Start(main, new string[0]);

            void main(Application app, string[] args)
            {
                started();
                app.Run(cancellationToken);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
        }
    }
}
