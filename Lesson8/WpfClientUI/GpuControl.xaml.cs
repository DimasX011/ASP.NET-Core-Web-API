﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClientUI
{
    /// <summary>
    /// Логика взаимодействия для GpuControl.xaml
    /// </summary>
    public partial class GpuControl : UserControl
    {
        public SeriesCollection ColumnSeriesValues { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public GpuControl()
        {

            InitializeComponent();
            ColumnSeriesValues = new SeriesCollection
            {
                    new ColumnSeries {Values = new ChartValues<double> { 10,20,30,40,50,60,70,80,90.100 }
            }
            };
            DataContext = this;
        }


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            TimePowerChart.Update(true);
        }
    }
}
