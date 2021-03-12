using OxyPlot;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

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
