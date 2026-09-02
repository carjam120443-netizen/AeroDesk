using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using AeroDesk.Widgets;

namespace AeroDesk;

public partial class MainWindow : Window
{
    private int _widgetCount;
    private bool _desktopMode;
    private const int GwlExstyle = -20;
    private const int WsExToolwindow = 0x00000080;
    private const int WsExAppwindow = 0x00040000;
    private static readonly IntPtr HwndBottom = new(1);
    private const uint SwpNoActivate = 0x0010;
    private const uint SwpShowWindow = 0x0040;
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoMove = 0x0002;

    public MainWindow()
    {
        InitializeComponent();
        AddClock();
        Loaded += (_, _) => SetDesktopMode(true);
    }

    private void AddClock() => AddWidget(new ClockWidget(), 20 + _widgetCount * 35, 20 + _widgetCount * 35);
    private void AddSystem() => AddWidget(new SystemMonitorWidget(), 20 + _widgetCount * 35, 20 + _widgetCount * 35);
    private void AddNetwork() => AddWidget(new NetworkWidget(), 20 + _widgetCount * 35, 20 + _widgetCount * 35);
    private void AddNotes() => AddWidget(new NotesWidget(), 20 + _widgetCount * 35, 20 + _widgetCount * 35);

    private void AddWidget(UserControl widget, double left, double top)
    {
        WidgetHost.AddWidget(widget, left, top);
        _widgetCount++;
    }

    private void AddWidget_Click(object sender, RoutedEventArgs e) => WidgetMenu.Visibility = WidgetMenu.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    private void AddClock_Click(object sender, RoutedEventArgs e) { AddClock(); WidgetMenu.Visibility = Visibility.Collapsed; }
    private void AddSystem_Click(object sender, RoutedEventArgs e) { AddSystem(); WidgetMenu.Visibility = Visibility.Collapsed; }
    private void AddNetwork_Click(object sender, RoutedEventArgs e) { AddNetwork(); WidgetMenu.Visibility = Visibility.Collapsed; }
    private void AddNotes_Click(object sender, RoutedEventArgs e) { AddNotes(); WidgetMenu.Visibility = Visibility.Collapsed; }

    private void Theme_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Aero Blue is active. More Aero themes are planned.", "AeroDesk Themes");
    private void DesktopMode_Click(object sender, RoutedEventArgs e) => SetDesktopMode(!_desktopMode);
    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void SetDesktopMode(bool enabled)
    {
        _desktopMode = enabled;
        var hwnd = new WindowInteropHelper(this).Handle;
        var style = GetWindowLong(hwnd, GwlExstyle);
        style = enabled ? (style | WsExToolwindow) & ~WsExAppwindow : (style | WsExAppwindow) & ~WsExToolwindow;
        SetWindowLong(hwnd, GwlExstyle, style);
        SetWindowPos(hwnd, enabled ? HwndBottom : IntPtr.Zero, 0, 0, 0, 0, SwpNoActivate | SwpShowWindow | SwpNoSize | SwpNoMove);
        Topmost = false;
        DesktopModeButton.Content = enabled ? "Desktop ✓" : "Desktop";
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!_desktopMode && e.LeftButton == MouseButtonState.Pressed) DragMove();
    }

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
}
