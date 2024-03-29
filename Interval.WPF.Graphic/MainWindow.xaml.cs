﻿using Interval.MonitorResource;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Globalization;
using LiveCharts.Configurations;

namespace Interval.WPF.Graphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MonitorController? monitor;

        private CancellationTokenSource? cancellationTokenSource;

        private double maxMemory;

        private double maxProcess;

        private ChartValues<ResourceDataCartesianVO> memoryChartValues;

        private ChartValues<ResourceDataCartesianVO> processorChartValues;

        public MainWindow()
        {
            InitializeComponent();
            StartComboBoxProcessorName();

            ResourceDataEventHub.Instance.ProcessorDataHub.Subscribe(OnProcessorData);
            ResourceDataEventHub.Instance.MemoryDataHub.Subscribe(OnMemoryData);

            var date = DateTime.Now;

            SettingChartMemory(date);
            SettingProcessor(date);
        }

        private void OnProcessorData(object? sender, ResourceDataEventArgs e)
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            this.Dispatcher.Invoke(() =>
            {
                if (maxProcess <= e.Data.Value)
                {
                    maxProcess = e.Data.Value;
                    MaxProcessorValue.Text = (maxProcess * 2.0).ToString("P", CultureInfo.InvariantCulture);
                }

                processorChartValues.Add(new ResourceDataCartesianVO()
                {
                    Value = e.Data.Value,
                    Date = e.Data.Date
                });

                SetAxisLimitsProcessor(e.Data.Date);

                if (processorChartValues.Count > 20)
                    processorChartValues.RemoveAt(0);
            });

        }

        private void OnMemoryData(object? sender, ResourceDataEventArgs e)
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            this.Dispatcher.Invoke(() =>
            {
                if (maxMemory <= e.Data.Value)
                {
                    maxMemory = e.Data.Value;
                    MaxMemoryValue.Text = $@"{maxMemory.ToString("0.##")} MB";
                }

                memoryChartValues.Add(new ResourceDataCartesianVO()
                {
                    Value = e.Data.Value,
                    Date = e.Data.Date
                });

                SetAxisLimitsMemory(e.Data.Date);

                if (memoryChartValues.Count > 20)
                    memoryChartValues.RemoveAt(0);
            });

        }


        private void StartComboBoxProcessorName()
        {
            var processlist = Process.GetProcesses();
            processorsNameCombobox.Items.Clear();

            foreach (Process theprocess in processlist.OrderBy(x => x.ProcessName))
            {
                processorsNameCombobox.Items.Add(new ProcessorName(theprocess.ProcessName, theprocess.Id.ToString()));
            }

            processorsNameCombobox.SelectedIndex = 0;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (processorsNameCombobox.SelectedItem is null)
                return;

            cancellationTokenSource = new CancellationTokenSource();

            var processor = (ProcessorName)processorsNameCombobox.SelectedItem;

            monitor = new MonitorController(DateTime.Now, -1, processor.Name, null);
            Task.Run(async () => await monitor.RunMonitorResourcesData());
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
            monitor?.Stop();
            Thread.Sleep(2000);
            memoryChartValues.Clear();
            processorChartValues.Clear();
            maxMemory = 0;
            maxProcess = 0;
            MaxProcessorValue.Text = String.Empty;
            MaxMemoryValue.Text = String.Empty;
        }

        private void SettingChartMemory(DateTime date)
        {
            var mapper = Mappers.Xy<ResourceDataCartesianVO>()
           .X(model => model.Date.Ticks)
           .Y(model => model.Value);

            Charting.For<ResourceDataCartesianVO>(mapper);

            memoryChartValues = new ChartValues<ResourceDataCartesianVO>();

            memoryCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = memoryChartValues,
                    PointGeometrySize = 8,
                    StrokeThickness = 2,
                    Fill = System.Windows.Media.Brushes.LightGreen,
                    Stroke = System.Windows.Media.Brushes.Green
                }
            };

            memoryCartesianChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new DateTime((long)value).ToString("hh:mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            memoryCartesianChart.AxisY.Add(new Axis
            {
                DisableAnimations = false,
                LabelFormatter = value => $@"{value.ToString("0.##")} MB",

            });

            SetAxisLimitsMemory(date);
        }

        private void SettingProcessor(DateTime date)
        {
            var mapper = Mappers.Xy<ResourceDataCartesianVO>()
           .X(model => model.Date.Ticks)
           .Y(model => model.Value);

            Charting.For<ResourceDataCartesianVO>(mapper);

            processorChartValues = new ChartValues<ResourceDataCartesianVO>();

            processorCartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = processorChartValues ,
                    PointGeometrySize = 8,
                    StrokeThickness = 2,
                    Foreground = System.Windows.Media.Brushes.Yellow,
                    Fill = System.Windows.Media.Brushes.LightSalmon,
                    Stroke = System.Windows.Media.Brushes.Red
                }
            };

            processorCartesianChart.AxisX.Add(new Axis
            {
                DisableAnimations = false,
                LabelFormatter = value => new DateTime((long)value).ToString("hh:mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            processorCartesianChart.AxisY.Add(new Axis
            {
                DisableAnimations = false,
                LabelFormatter = value => (value * 2.0).ToString("P", CultureInfo.InvariantCulture)

            });

            processorCartesianChart.AxisY[0].MaxValue = 0.5;
            processorCartesianChart.AxisY[0].MinValue = 0.0;


            SetAxisLimitsProcessor(date);
        }

        private void SetAxisLimitsMemory(DateTime date)
        {
            memoryCartesianChart.AxisX[0].MaxValue = date.Ticks + TimeSpan.FromSeconds(1).Ticks;
            memoryCartesianChart.AxisX[0].MinValue = date.Ticks - TimeSpan.FromSeconds(10).Ticks;
        }

        private void SetAxisLimitsProcessor(DateTime date)
        {
            processorCartesianChart.AxisX[0].MaxValue = date.Ticks + TimeSpan.FromSeconds(1).Ticks;
            processorCartesianChart.AxisX[0].MinValue = date.Ticks - TimeSpan.FromSeconds(10).Ticks;
        }

        private void SerachBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text = SearchBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                StartComboBoxProcessorName();
                return;
            }

            if (text.Length < 3)
                return;

            var list = processorsNameCombobox.Items.Cast<ProcessorName>()
                                .Where(x => x.ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1).ToList();


            processorsNameCombobox.Items.Clear();


            foreach (var elemet in list)
            {
                processorsNameCombobox.Items.Add(elemet);
            }

            if (processorsNameCombobox.Items.Count != 0)
                processorsNameCombobox.SelectedIndex = 0;
        }
    }

    public class ProcessorName
    {
        private readonly string name;

        private readonly string pid;

        public string Name { get => name; }
        public ProcessorName(string name, string pid)
        {
            this.name = name;
            this.pid = pid;
        }

        public override string ToString() => $@"{ name } [{ pid }]";

    }

    public class ResourceDataCartesianVO
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}
