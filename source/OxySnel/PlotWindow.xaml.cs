using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OxySnel
{
    public class PlotWindow : Window, IPlotWindow
    {
        public PlotViewModel Context => ((PlotModelView)this.Content).Context;

        public PlotViewModel Model
        {
            get => Context;
            set
            {
                this.Content = value;
            }
        }

        void IPlotWindow.Close()
        {
            this.Close();
        }

        public PlotWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
