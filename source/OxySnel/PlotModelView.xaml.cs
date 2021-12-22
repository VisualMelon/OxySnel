using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OxySnel
{
    public class PlotModelView : UserControl
    {
        public PlotViewModel Context { get; } = new PlotViewModel();

        /// <summary>
        /// Attempts to find the parent Window. This will fail on non-desktop I believe.
        /// </summary>
        /// <returns></returns>
        private Window FindWindow()
        {
            IControl p = this;

            while (p != null)
            {
                if (p is Window w)
                    return w;
                else
                    p = p.Parent;
            }

            return null;
        }

        public PlotModelView()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ZoomIn()
        {
            Context.PlotModel.ZoomAllAxes(1.25);
            Context.PlotModel.InvalidatePlot(false);
        }

        public void ZoomOut()
        {
            Context.PlotModel.ZoomAllAxes(0.8);
            Context.PlotModel.InvalidatePlot(false);
        }

        public void ResetView()
        {
            Context.PlotModel.ResetAllAxes();
            Context.PlotModel.InvalidatePlot(false);
        }

        public async void SavePng()
        {
            var window = FindWindow();

            var sfd = new SaveFileDialog()
            {
                DefaultExtension = "png",
            };

            var res = await sfd.ShowAsync(window);
            if (res is string filename)
            {
                Context.PlotModel.ExportPng(filename);
            }
        }

        public async void SavePdf()
        {
            var window = FindWindow();

            var sfd = new SaveFileDialog()
            {
                DefaultExtension = "pdf",
            };

            var res = await sfd.ShowAsync(window);
            if (res is string filename)
            {
                Context.PlotModel.ExportPdf(filename);
            }
        }

        public async void SaveSvg()
        {
            var window = FindWindow();

            var sfd = new SaveFileDialog()
            {
                DefaultExtension = "svg",
            };

            var res = await sfd.ShowAsync(window);
            if (res is string filename)
            {
                Context.PlotModel.ExportSvg(filename);
            }
        }
    }
}
