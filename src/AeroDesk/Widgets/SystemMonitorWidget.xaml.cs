using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AeroDesk.Widgets;

public partial class SystemMonitorWidget : UserControl, IWidget
{
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };
    public string Id => "system-monitor";
    public string WidgetName => "System Monitor";

    public SystemMonitorWidget()
    {
        InitializeComponent();
        UpdateStats();
        _timer.Tick += (_, _) => UpdateStats();
        _timer.Start();
    }

    private void UpdateStats()
    {
        var memory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
        var processMemory = Environment.WorkingSet;
        CpuText.Text = "System monitor";
        RamText.Text = $"App RAM  {processMemory / 1024d / 1024d:0} MB / {memory / 1024d / 1024d:0} MB available";
        UptimeText.Text = $"Uptime  {TimeSpan.FromMilliseconds(Environment.TickCount64):dd\\:hh\\:mm}";
    }
}
