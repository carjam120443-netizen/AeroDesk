using System.Windows.Controls;

namespace AeroDesk.Widgets;

public partial class WidgetHost : UserControl
{
    public WidgetHost()
    {
        InitializeComponent();
    }

    public void AddWidget(UserControl widget, double left, double top)
    {
        Canvas.SetLeft(widget, left);
        Canvas.SetTop(widget, top);
        WidgetCanvas.Children.Add(widget);
    }

    public void ClearWidgets() => WidgetCanvas.Children.Clear();
}
