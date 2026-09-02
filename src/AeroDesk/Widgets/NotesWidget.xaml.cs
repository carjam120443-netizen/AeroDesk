using System.IO;
using System.Windows.Controls;

namespace AeroDesk.Widgets;

public partial class NotesWidget : UserControl, IWidget
{
    private readonly string _path = Path.Combine(System.AppContext.BaseDirectory, "AeroDeskNote.txt");
    public string Id => "notes";
    public string WidgetName => "Notes";

    public NotesWidget()
    {
        InitializeComponent();
        if (File.Exists(_path))
            NoteText.Text = File.ReadAllText(_path);
    }

    private void NoteText_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsInitialized)
            File.WriteAllText(_path, NoteText.Text);
    }
}
