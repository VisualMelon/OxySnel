Imports System
Imports System.Linq
Imports System.Threading.Tasks
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series
Imports OxySnel

Module Program
    Sub Main(args As String())
        OxySnel.Snel.StartOnThread(AddressOf ClockExample, True)
    End Sub

    Sub ClockExample()
        Dim plot = New PlotModel()
        plot.PlotType = PlotType.Polar
        plot.PlotAreaBorderThickness = New OxyThickness(0.0)

        Dim mag = New MagnitudeAxis()
        mag.Minimum = 0.0
        mag.Maximum = 1.0
        mag.MinimumPadding = 0.0
        mag.MaximumPadding = 0.0
        mag.IsAxisVisible = False
        plot.Axes.Add(mag)

        Dim ang = New AngleAxis()
        ang.Minimum = 0.0
        ang.Maximum = 360.0
        ang.MajorStep = 90.0
        ang.MinorStep = 30.0
        ang.StartAngle = 90.0
        ang.EndAngle = -270.0
        ang.LabelFormatter =
            Function(angle)
                Return If(angle = 0, "12", (angle / 30).ToString())
            End Function
        plot.Axes.Add(ang)

        Dim hour = New LineSeries()
        hour.StrokeThickness = 6.0
        plot.Series.Add(hour)

        Dim minute = New LineSeries()
        minute.StrokeThickness = 4.0
        plot.Series.Add(minute)

        Dim second = New LineSeries()
        second.StrokeThickness = 2.0
        plot.Series.Add(second)

        Dim update =
            Sub()
                Dim now = DateTime.Now

                plot.Title = now.ToLongDateString()
                plot.Subtitle = now.ToLongTimeString()

                hour.Points.Clear()
                hour.Points.Add(New DataPoint(0.0, 30 * (now.TimeOfDay.TotalHours Mod 12.0)))
                hour.Points.Add(New DataPoint(0.4, 30 * (now.TimeOfDay.TotalHours Mod 12.0)))

                minute.Points.Clear()
                minute.Points.Add(New DataPoint(0.0, 6 * (now.TimeOfDay.TotalMinutes Mod 60)))
                minute.Points.Add(New DataPoint(0.6, 6 * (now.TimeOfDay.TotalMinutes Mod 60)))

                second.Points.Clear()
                second.Points.Add(New DataPoint(-0.2, 6 * now.Second))
                second.Points.Add(New DataPoint(0.8, 6 * now.Second))

                plot.InvalidatePlot(True)
            End Sub

        OxySnel.Snel.Show(plot)

        Task.Run(
            Async Sub()
                While True
                    Await Task.Delay(100)
                    Await Snel.Invoke(update)
                End While
            End Sub)

        Console.ReadKey(True)
        Snel.Kill()
    End Sub
End Module
