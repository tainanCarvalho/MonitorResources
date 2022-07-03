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

        private double maxMemory;

        private double maxProcess;

        private SemaphoreSlim semaphoreSlimProcessor;

        private CancellationTokenSource cancellationTokenSource;

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
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            Task.Run(async () => 
            {
                try
                {
                    if (!await semaphoreSlimProcessor.WaitAsync(60000))
                        return;

                    this.Invoke(() =>
                    {
                        var data = e.Data;
                        var value = data.Value;

                        if (maxProcess <= value)
                        {
                            maxProcess = value;
                            MaxProcessLabelValue.Text = (maxProcess * 2.0).ToString("P", CultureInfo.InvariantCulture);
                        }

                        processorChartValues.Add(new ResourceDataCartesianVO()
                        {
                            Value = value,
                            Date = data.Date
                        });

                        SetAxisLimitsProcessor(data.Date);

                        if (processorChartValues.Count > 20)
                            processorChartValues.RemoveAt(0);

                    });
                }
                finally
                {
                    semaphoreSlimProcessor.Release();
                }
            });
        }

        private void OnMemoryData(object? sender, ResourceDataEventArgs e)
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            this.Invoke(() =>
            {
                var data = e.Data;

                if (maxMemory <= data.Value)
                {
                    maxMemory = data.Value;
                    MaxMemoryLabelValue.Text = $@"{maxMemory.ToString("0.##")} MB";
                }

                memoryChartValues.Add(new ResourceDataCartesianVO()
                {
                    Value = data.Value,
                    Date = data.Date
                });

                SetAxisLimitsMemory(data.Date);

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

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (processorsNameCombobox.SelectedItem is null)
            {
                MessageBox.Show("Deve selecionar um item", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var process = (ProcessorName)processorsNameCombobox.SelectedItem;

            cancellationTokenSource = new CancellationTokenSource();
            semaphoreSlimProcessor = new(1, 1);
            monitor = new MonitorController(DateTime.Now, -1, process.Name, null);

            Task.Run(async () => await monitor.RunMonitorResourcesData());
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            monitor?.Stop();
            memoryChartValues.Clear();
            processorChartValues.Clear();
            maxMemory = 0;
            maxProcess = 0;
            MaxProcessLabelValue.Text = string.Empty;
            MaxMemoryLabelValue.Text = string.Empty;
            semaphoreSlimProcessor.Dispose();
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

        private void SearchBox_TextChanged(object sender, EventArgs e)
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