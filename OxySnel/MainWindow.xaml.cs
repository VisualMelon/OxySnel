﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OxySnel
{
    public class MainWindow : Window
    {
        public ViewModel Context => ((PlotModelView)this.Content).Context;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
