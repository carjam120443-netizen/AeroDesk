using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AeroDesk.Widgets;

namespace AeroDesk;

public partial class MainWindow : Window
{
    private readonly List<WidgetWindow> _widgetWindows = new();
    private int _widgetCount;

    public MainWindow()
    {
        InitializeComponent();
        AddClock();
    }

    private void AddClock() => AddWidget(new ClockWidget());
    private void AddSystem() => AddWidget(new SystemMonitorWidget());
    private void AddNetwork() => AddWidget(new NetworkWidget());
    private void AddNotes() => AddWidget(new NotesWidget());

    private void AddWidget(UserControl widget)
    {
        var offset = _widgetCount * 35;
        var window = new WidgetWindow(widget, 30 + offset, 80 + offset);
        _widgetWindows.Add(window);
        window.Closed += (_, _) => _widgetWindows.Remove(window);
        window.Show();
        _widgetCount++;
    }

    private void AddWidget_Click(object sender, RoutedEventArgs e) =>
        WidgetMenu.Visibility = WidgetMenu.Visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;

    private void AddClock_Click(object sender, RoutedEventArgs e)
    {
        AddClock();
        WidgetMenu.Visibility = Visibility.Collapsed;
    }

    private void AddSystem_Click(object sender, RoutedEventArgs e)
    {
        AddSystem();
        WidgetMenu.Visibility = Visibility.Collapsed;
    }

    private void AddNetwork_Click(object sender, RoutedEventArgs e)
    {
        AddNetwork();
        WidgetMenu.Visibility = Visibility.Collapsed;
    }

    private void AddNotes_Click(object sender, RoutedEventArgs e)
    {
        AddNotes();
        WidgetMenu.Visibility = Visibility.Collapsed;
    }

    private void Theme_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Aero Blue is active. More Aero themes are planned.", "AeroDesk Themes");

    private void WidgetsDesktop_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Widgets are running as independent desktop windows. Move this manager anywhere you want.", "AeroDesk");

    private void Minimize_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState.Minimized;

    private void Close_Click(object sender, RoutedEventArgs e) =>
        Close();

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}
