using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace AvaloniaVisualBasic.VisualDesigner.Views;

public partial class PropertiesToolView : UserControl
{
    public PropertiesToolView()
    {
        InitializeComponent();
    }

    private void PropertyGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (e.Source is Control c)
        {
            if (c.FindAncestorOfType<ListBoxItem>() is { } listBoxItem &&
                c.FindAncestorOfType<ListBox>() is { } listBox)
            {
                var dataContext = listBoxItem.DataContext;
                listBox.SelectedItem = dataContext;
            }
        }
    }
}