using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AeroDesk.Widgets;

public partial class ClockWidget : UserControl, IWidget
{
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };

    public string Id => "clock";
    public string WidgetName => "Clock";

    public ClockWidget()
    {
        InitializeComponent();
        UpdateClock();
        _timer.Tick += (_, _) => UpdateClock();
        _timer.Start();
    }

    private void UpdateClock()
    {
        var now = DateTime.Now;
        ClockText.Text = now.ToString("h:mm:ss tt");
        DateText.Text = now.ToString("dddd, MMMM d, yyyy");
    }
}
