using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeroDesk.Widgets;

public partial class WidgetHost : UserControl
{
    private readonly Dictionary<UserControl, string> _ids = new();
    private UserControl? _dragging;
    private System.Windows.Point _dragStart;
    private double _startLeft;
    private double _startTop;
    private readonly string _layoutPath = Path.Combine(AppContext.BaseDirectory, "AeroDeskWidgets.json");

    public WidgetHost() => InitializeComponent();

    public void AddWidget(UserControl widget, double left, double top)
    {
        var id = widget is IWidget named ? named.Id : widget.GetType().Name;
        if (TryGetSavedPosition(id, out var saved)) { left = saved.Left; top = saved.Top; }
        Canvas.SetLeft(widget, left); Canvas.SetTop(widget, top);
        widget.MouseLeftButtonDown += Widget_MouseLeftButtonDown;
        widget.MouseMove += Widget_MouseMove;
        widget.MouseLeftButtonUp += Widget_MouseLeftButtonUp;
        widget.Cursor = Cursors.SizeAll;
        _ids[widget] = id;
        WidgetCanvas.Children.Add(widget);
    }

    private void Widget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not UserControl widget) return;
        _dragging = widget; _dragStart = e.GetPosition(WidgetCanvas);
        _startLeft = Canvas.GetLeft(widget); _startTop = Canvas.GetTop(widget);
        widget.CaptureMouse(); e.Handled = true;
    }

    private void Widget_MouseMove(object sender, MouseEventArgs e)
    {
        if (_dragging is null || e.LeftButton != MouseButtonState.Pressed) return;
        var current = e.GetPosition(WidgetCanvas);
        Canvas.SetLeft(_dragging, Math.Max(0, _startLeft + current.X - _dragStart.X));
        Canvas.SetTop(_dragging, Math.Max(0, _startTop + current.Y - _dragStart.Y));
    }

    private void Widget_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_dragging is null) return;
        _dragging.ReleaseMouseCapture(); _dragging = null; SavePositions();
    }

    private bool TryGetSavedPosition(string id, out WidgetPosition position)
    {
        position = new WidgetPosition(0, 0);
        if (!File.Exists(_layoutPath)) return false;
        try
        {
            var all = JsonSerializer.Deserialize<Dictionary<string, WidgetPosition>>(File.ReadAllText(_layoutPath));
            return all is not null && all.TryGetValue(id, out position!);
        }
        catch { return false; }
    }

    private void SavePositions()
    {
        var positions = new Dictionary<string, WidgetPosition>();
        foreach (var pair in _ids)
            positions[pair.Value] = new WidgetPosition(Canvas.GetLeft(pair.Key), Canvas.GetTop(pair.Key));
        try { File.WriteAllText(_layoutPath, JsonSerializer.Serialize(positions, new JsonSerializerOptions { WriteIndented = true })); }
        catch { }
    }

    public void ClearWidgets()
    {
        foreach (var widget in _ids.Keys) widget.ReleaseMouseCapture();
        _ids.Clear(); WidgetCanvas.Children.Clear();
    }

    private sealed record WidgetPosition(double Left, double Top);
}
