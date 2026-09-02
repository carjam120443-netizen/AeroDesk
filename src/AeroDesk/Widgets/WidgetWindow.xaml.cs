using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace AeroDesk.Widgets;

public partial class WidgetWindow : Window
{
    private const int GwlExstyle = -20;
    private const int WsExToolwindow = 0x00000080;
    private const int WsExAppwindow = 0x00040000;
    private static readonly IntPtr HwndBottom = new(1);
    private const uint SwpNoActivate = 0x0010;
    private const uint SwpShowWindow = 0x0040;
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoMove = 0x0002;

    private static readonly string LayoutDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AeroDesk");
    private static readonly string LayoutPath = Path.Combine(LayoutDirectory, "AeroDeskWidgets.json");

    private readonly string _id;
    private bool _dragging;
    private Point _dragStart;
    private double _startLeft;
    private double _startTop;

    public WidgetWindow(UserControl widget, double defaultLeft, double defaultTop)
    {
        InitializeComponent();
        _id = widget is IWidget named ? named.Id : widget.GetType().Name;
        WidgetContent.Content = widget;

        Loaded += (_, _) =>
        {
            if (TryGetSavedPosition(_id, out var position))
            {
                Left = position.Left;
                Top = position.Top;
            }
            else
            {
                Left = defaultLeft;
                Top = defaultTop;
            }

            PutOnDesktop();
        };

        MouseLeftButtonDown += WidgetWindow_MouseLeftButtonDown;
        MouseMove += WidgetWindow_MouseMove;
        MouseLeftButtonUp += WidgetWindow_MouseLeftButtonUp;
        Activated += (_, _) => PutOnDesktop();
    }

    private void WidgetWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is TextBoxBase)
            return;

        _dragging = true;
        _dragStart = e.GetPosition(null);
        _startLeft = Left;
        _startTop = Top;
        CaptureMouse();
        e.Handled = true;
    }

    private void WidgetWindow_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_dragging || e.LeftButton != MouseButtonState.Pressed)
            return;

        var current = e.GetPosition(null);
        Left = Math.Max(0, _startLeft + current.X - _dragStart.X);
        Top = Math.Max(0, _startTop + current.Y - _dragStart.Y);
    }

    private void WidgetWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_dragging)
            return;

        _dragging = false;
        ReleaseMouseCapture();
        SavePosition();
        PutOnDesktop();
    }

    private void PutOnDesktop()
    {
        if (!IsLoaded)
            return;

        var hwnd = new WindowInteropHelper(this).Handle;
        var style = GetWindowLong(hwnd, GwlExstyle);
        style = (style | WsExToolwindow) & ~WsExAppwindow;
        SetWindowLong(hwnd, GwlExstyle, style);
        SetWindowPos(hwnd, HwndBottom, 0, 0, 0, 0,
            SwpNoActivate | SwpShowWindow | SwpNoSize | SwpNoMove);
        Topmost = false;
    }

    private bool TryGetSavedPosition(string id, out WidgetPosition position)
    {
        position = new WidgetPosition(0, 0);
        if (!File.Exists(LayoutPath))
            return false;

        try
        {
            var all = JsonSerializer.Deserialize<Dictionary<string, WidgetPosition>>(
                File.ReadAllText(LayoutPath));
            return all is not null && all.TryGetValue(id, out position!);
        }
        catch
        {
            return false;
        }
    }

    private void SavePosition()
    {
        try
        {
            Directory.CreateDirectory(LayoutDirectory);
            Dictionary<string, WidgetPosition> positions;

            if (File.Exists(LayoutPath))
            {
                positions = JsonSerializer.Deserialize<Dictionary<string, WidgetPosition>>(
                    File.ReadAllText(LayoutPath)) ?? new();
            }
            else
            {
                positions = new();
            }

            positions[_id] = new WidgetPosition(Left, Top);
            File.WriteAllText(LayoutPath, JsonSerializer.Serialize(positions,
                new JsonSerializerOptions { WriteIndented = true }));
        }
        catch
        {
            // Widget positioning should never crash AeroDesk.
        }
    }

    private sealed record WidgetPosition(double Left, double Top);

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);
}
