using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AeroDesk;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _clockTimer = new() { Interval = TimeSpan.FromSeconds(1) };

    public MainWindow()
    {
        InitializeComponent();
        UpdateClock();
        _clockTimer.Tick += (_, _) => UpdateClock();
        _clockTimer.Start();
    }

    private void UpdateClock()
    {
        ClockText.Text = DateTime.Now.ToString("h:mm:ss tt");
        DateText.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy");
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    protected override void OnClosed(EventArgs e)
    {
        _clockTimer.Stop();
        base.OnClosed(e);
    }
}
