using Interval.MonitorResource;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interval.Forms.Graphic
{
    public partial class Form1 : Form
    {
        private MonitorController? monitor;

        private ChartValues<ResourceDataCartesianVO> memoryChartValues;

        private ChartValues<ResourceDataCartesianVO> processorChartValues;

        public Form1()
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
            this.Invoke(() =>
            {
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
            this.Invoke(() =>
            {
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
                processorsNameCombobox.Items.Add(new ProcessorName(theprocess.ProcessName));
            }

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if(processorsNameCombobox.SelectedItem is null)
            {
                MessageBox.Show("Deve selecionar um item", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            monitor = new MonitorController(DateTime.Now, -1, processorsNameCombobox.SelectedItem.ToString(), null);
            Task.Run(async () => await monitor.RunMonitorResourcesData());
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            monitor?.Stop();
            Thread.Sleep(2000);
            memoryChartValues.Clear();
            processorChartValues.Clear();
        }

        private void SettingChartMemory(DateTime date)
        {
            var mapper = Mappers.Xy<ResourceDataCartesianVO>()
           .X(model => model.Date.Ticks)
           .Y(model => model.Value);

            Charting.For<ResourceDataCartesianVO>(mapper);

            memoryChartValues = new ChartValues<ResourceDataCartesianVO>();

            CartesianMemory.Series = new SeriesCollection
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

            CartesianMemory.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new DateTime((long)value).ToString("hh:mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            CartesianMemory.AxisY.Add(new Axis
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

            SetAxisLimitsProcessor(date);
        }

        private void SetAxisLimitsMemory(DateTime date)
        {
            CartesianMemory.AxisX[0].MaxValue = date.Ticks + TimeSpan.FromSeconds(1).Ticks;
            CartesianMemory.AxisX[0].MinValue = date.Ticks - TimeSpan.FromSeconds(10).Ticks;
        }

        private void SetAxisLimitsProcessor(DateTime date)
        {
            processorCartesianChart.AxisX[0].MaxValue = date.Ticks + TimeSpan.FromSeconds(1).Ticks;
            processorCartesianChart.AxisX[0].MinValue = date.Ticks - TimeSpan.FromSeconds(10).Ticks;
        }
    }

    public class ProcessorName
    {
        private readonly string name;
        public ProcessorName(string name) => this.name = name;

        public override string ToString() => name;

    }

    public class ResourceDataCartesianVO
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}