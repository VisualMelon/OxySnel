using OxyPlot;
using ReactiveUI;

namespace OxySnel
{
    public class PlotViewModel : ReactiveObject
    {
        public PlotViewModel()
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
