using System.Windows;
using System.Windows.Input;
using AeroDesk.Widgets;

namespace AeroDesk;

public partial class MainWindow : Window
{
    private int _widgetCount;

    public MainWindow()
    {
        InitializeComponent();
        AddClock();
    }

    private void AddClock()
    {
        var offset = _widgetCount * 35;
        WidgetHost.AddWidget(new ClockWidget(), 20 + offset, 20 + offset);
        _widgetCount++;
    }

    private void AddWidget_Click(object sender, RoutedEventArgs e)
        => WidgetMenu.Visibility = WidgetMenu.Visibility == Visibility.Visible
            ? Visibility.Collapsed : Visibility.Visible;

    private void AddClock_Click(object sender, RoutedEventArgs e)
    {
        AddClock();
        WidgetMenu.Visibility = Visibility.Collapsed;
    }

    private void Theme_Click(object sender, RoutedEventArgs e)
        => MessageBox.Show("More Aero themes are coming soon.", "AeroDesk Themes");

    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}