open System
open OxyPlot
open OxyPlot.Axes
open OxyPlot.Series
open OxySnel

let clock () =
    let plot = PlotModel()
    plot.PlotType <- PlotType.Polar
    plot.PlotAreaBorderThickness <- new OxyThickness(0.)

    let mag = MagnitudeAxis()
    mag.Minimum <- 0.
    mag.Maximum <- 1.
    mag.MinimumPadding <- 0.
    mag.MaximumPadding <- 0.
    mag.IsAxisVisible <- false
    plot.Axes.Add(mag)

    let ang = AngleAxis()
    ang.Minimum <- 0.
    ang.Maximum <- 360.
    ang.MajorStep <- 90.
    ang.MinorStep <- 30.
    ang.StartAngle <- 90.
    ang.EndAngle <- -270.
    ang.LabelFormatter <- (fun angle -> if angle = 0. then "12" else string (angle / 30.))
    plot.Axes.Add(ang)
    
    let hour = LineSeries()
    hour.StrokeThickness <- 6.
    plot.Series.Add(hour)

    let minute = LineSeries()
    minute.StrokeThickness <- 4.
    plot.Series.Add(minute)

    let second = LineSeries()
    second.StrokeThickness <- 2.
    plot.Series.Add(second)

    let update () =
        let now = DateTime.Now
        
        plot.Title <- now.ToLongDateString()
        plot.Subtitle <- now.ToLongTimeString()

        hour.Points.Clear()
        hour.Points.Add(DataPoint(0., 30. * float (now.Hour % 12)))
        hour.Points.Add(DataPoint(0.4, 30. * float (now.Hour % 12)))

        minute.Points.Clear()
        minute.Points.Add(DataPoint(0., 6. * float now.Minute))
        minute.Points.Add(DataPoint(0.6, 6. * float now.Minute))

        second.Points.Clear()
        second.Points.Add(DataPoint(-0.2, 6. * float now.Second))
        second.Points.Add(DataPoint(0.8, 6. * float now.Second))

        plot.InvalidatePlot(true)

    Snel.Show(plot) |> ignore

    async {
        while true do
            do! Async.Sleep 100
            do! Snel.Invoke(Action update) |> Async.AwaitTask
    } |> Async.StartAsTask |> ignore

    Console.ReadKey(true) |> ignore
    Snel.Kill()
    
[<EntryPoint>]
let main argv =
    OxySnel.Snel.StartOnThread(Action clock, true)
    0
    