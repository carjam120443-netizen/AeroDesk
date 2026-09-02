using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Controls;
using System.Windows.Threading;
using System;

namespace AeroDesk.Widgets;

public partial class NetworkWidget : UserControl, IWidget
{
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(2) };
    public string Id => "network";
    public string WidgetName => "Network";

    public NetworkWidget()
    {
        InitializeComponent();
        UpdateNetwork();
        _timer.Tick += (_, _) => UpdateNetwork();
        _timer.Start();
    }

    private void UpdateNetwork()
    {
        var active = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback);
        StatusText.Text = active is null ? "Offline" : "Online";
        HostText.Text = active is null ? "No active adapter" : active.Name;
    }
}
