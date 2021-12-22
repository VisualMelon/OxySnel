using OxyPlot;
using ReactiveUI;

namespace OxySnel
{
    public class ViewModel : ReactiveObject
    {
        public ViewModel()
        {
        }

        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get => plotModel;
            set => this.RaiseAndSetIfChanged(ref plotModel, value);
        }

        private bool showMenu = true;
        public bool ShowMenu
        {
            get => showMenu;
            set => this.RaiseAndSetIfChanged(ref showMenu, value);
        }
    }
}
