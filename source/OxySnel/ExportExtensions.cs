using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxySnel
{
    public static class ExportExtensions
    {
        public static void ExportPng(this PlotModel plotModel, string fileName)
        {
            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var exporter = new OxyPlot.Avalonia.PngExporter();

                if (plotModel.Width > 0 && plotModel.Height > 0)
                {
                    exporter.Width = (int)Math.Ceiling(plotModel.Width);
                    exporter.Height = (int)Math.Ceiling(plotModel.Height);
                }

                exporter.Export(plotModel, fs);
            }
        }

        public static void ExportSvg(this PlotModel plotModel, string fileName)
        {
            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var exporter = new OxyPlot.Avalonia.SvgExporter();

                if (plotModel.Width > 0 && plotModel.Height > 0)
                {
                    exporter.Width = (int)Math.Ceiling(plotModel.Width);
                    exporter.Height = (int)Math.Ceiling(plotModel.Height);
                }

                exporter.Export(plotModel, fs);
            }
        }

        public static void ExportPdf(this PlotModel plotModel, string fileName)
        {
            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var exporter = new OxyPlot.PdfExporter();

                if (plotModel.Width > 0 && plotModel.Height > 0)
                {
                    exporter.Width = (int)Math.Ceiling(plotModel.Width);
                    exporter.Height = (int)Math.Ceiling(plotModel.Height);
                }

                exporter.Export(plotModel, fs);
            }
        }
    }
}
