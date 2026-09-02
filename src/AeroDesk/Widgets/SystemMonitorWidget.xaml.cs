using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AeroDesk.Widgets;

public partial class SystemMonitorWidget : UserControl, IWidget
{
    private readonly PerformanceCounter _cpu = new("Processor", "% Processor Time", "_Total");
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };

    public string Id => "system-monitor";
    public string WidgetName => "System Monitor";

    public SystemMonitorWidget()
    {
        InitializeComponent();
        _cpu.NextValue();
        UpdateStats();
        _timer.Tick += (_, _) => UpdateStats();
        _timer.Start();
    }

    private void UpdateStats()
    {
        CpuText.Text = $"CPU  {_cpu.NextValue():0}%";
        var memory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
        var processMemory = Environment.WorkingSet;
        RamText.Text = $"RAM  {processMemory / 1024d / 1024d:0} MB app / {memory / 1024d / 1024d:0} MB available";
        UptimeText.Text = $"Uptime  {TimeSpan.FromMilliseconds(Environment.TickCount64):dd\\:hh\\:mm}";
    }
}
