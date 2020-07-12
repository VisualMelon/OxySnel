# OxySnel

Throw an interactive plot at the screen quickly from a .NET Core 2.0 console application. Might be cross-platform as it's based on [OxyPlot-Avalonia](https://github.com/oxyplot/oxyplot-avalonia); atleast that was the idea, but I'm too lazy to test it.

This repo serves the purpose of being a minimal but not completely pointless example of OxyPlot-Avalonia where I can find it, and to provide a simple example of OxyPlot under F#. I might add some more languages if I can be bothered and they actually work (VB.NET, PowerShell). It's also kind of useful if you need an interactive plot in a hurry.

## Minimal API

Only four methods that matter:

 - `public static void OxySnel.Snel.StartOnThread(Action starter, bool callbackOffthread)`

    Starts OxySnel on the current thread: this may be necessary for some systems (not sure yet....)

 - `public static Task OxySnel.Snel.Show(PlotModel plotModel, string windowTitle = DefaultWindowTitle)`

    Opens a new Avalonia window showing the given PlotModel with its own UI Thread.

 - `public static Task OxySnel.Snel.Invoke(Action action)`

    Invokes an `Action` on the UI Thread

 - `public static void OxySnel.Snel.Kill()`

    Kills the OxySnel background stuff permanently. Once you call `Show`, the application won't end until you call `Kill`. _This aspect of the API is liable to change in the future when I've come up with something less terrible._

## Example

    // make a plot...
    var plot = new PlotModel() { Title = "Time-to-plot" };

    // add some series...
    var bs = new BarSeries() { BaseValue = 0 };
    bs.Items.Add(new BarItem(10));
    bs.Items.Add(new BarItem(2));
    plot.Series.Add(bs);

    // add some axes
    var categoryAxis = new CategoryAxis() { Position = AxisPosition.Left, Angle = -90 };
    categoryAxis.Labels.Add("Without OxySnel");
    categoryAxis.Labels.Add("With OxySnel");
    plot.Axes.Add(categoryAxis);

    var timeAxis = new LinearAxis() { Position = AxisPosition.Bottom, Title = "Time", Unit = "Money" };
    plot.Axes.Add(timeAxis);

    // show the plot
    OxySnel.Snel.Show(plot);

    // pause
    Console.ReadKey(true);

    // kill it
    OxySnel.Snel.Kill();

A more fun example is provided for C# and F# in the `SnelTestCS` and `SnelTestFS` directories.

## Getting the thing

Why would you want to do that?

Package feed from MyGet: https://www.myget.org/F/oxysnel/api/v3/index.json

This is too shoddy a product to be on NuGet... and I can't remember my credentials.

Otherwise:

 1. Clone the repo

    `git clone https://github.com/VisualMelon/OxySnel.git`

 2. Refresh the git submodule for oxyplot avalonia (this will be replaced with a NuGet reference at some point)

    `git submodule init`
    `git submodule update`

 3. Build it
 
    `dotnet build Source/OxySnel.sln`

 4. Run an exaple?

    `dotnet run --project Source/SnelTestCS/SnelTestCS.csproj`

    `dotnet run --project Source/SnelTestFS/SnelTestFS.fsproj`