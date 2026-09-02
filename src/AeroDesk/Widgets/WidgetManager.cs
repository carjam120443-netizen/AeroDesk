using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AeroDesk.Widgets;

public sealed class WidgetManager
{
    private readonly List<IWidget> _widgets = new();

    public IReadOnlyList<IWidget> Widgets => _widgets;

    public void Register(IWidget widget)
    {
        if (_widgets.All(existing => existing.Id != widget.Id))
            _widgets.Add(widget);
    }

    public bool Remove(string id)
    {
        var widget = _widgets.FirstOrDefault(item => item.Id == id);
        return widget is not null && _widgets.Remove(widget);
    }
}
