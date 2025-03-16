using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DevTools.Tooling;

namespace DevTools.Dashboard.Views;

public partial class SidebarMenu : UserControl
{
    public SidebarMenu()
    {
        InitializeComponent();
    }

    public ObservableCollection<IDevTool> DevTools
    {
        get => (ObservableCollection<IDevTool>)GetValue(DevToolsProperty);
        set => SetValue(DevToolsProperty, value);
    }

    public static readonly DependencyProperty DevToolsProperty =
        DependencyProperty.Register(nameof(DevTools), typeof(ObservableCollection<IDevTool>), typeof(SidebarMenu), new PropertyMetadata(null));
}