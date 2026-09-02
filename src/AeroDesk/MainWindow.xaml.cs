using System.Windows;
using System.Windows.Input;
using AeroDesk.Widgets;

namespace AeroDesk;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        WidgetHost.AddWidget(new ClockWidget(), 0, 0);
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}
