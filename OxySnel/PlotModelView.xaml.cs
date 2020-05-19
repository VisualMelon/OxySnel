using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OxySnel
{
    public class PlotModelView : UserControl
    {
        public ViewModel Context { get; } = new ViewModel();

        public PlotModelView()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
