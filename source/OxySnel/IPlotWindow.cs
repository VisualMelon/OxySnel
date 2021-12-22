using System;
using System.Collections.Generic;
using System.Text;

namespace OxySnel
{
    public interface IPlotWindow
    {
        PlotViewModel Model { get; set; }
        void Close();
    }
}
