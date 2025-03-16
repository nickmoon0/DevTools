using System.Windows;
using System.Windows.Controls;
using DevTools.Tooling.Interfaces;

namespace DevTools.Dashboard.Views;

public partial class SidebarMenu : UserControl
{
    public static readonly DependencyProperty SelectedDevToolProperty = 
        DependencyProperty.Register(nameof(SelectedDevTool), typeof(IDevTool), typeof(SidebarMenu), new PropertyMetadata(null));

    public IDevTool SelectedDevTool
    {
        get => (IDevTool)GetValue(SelectedDevToolProperty);
        set => SetValue(SelectedDevToolProperty, value);
    }
    
    public SidebarMenu()
    {
        InitializeComponent();
    }
    
    private void SelectEnvironment_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement environment selection logic here.
        MessageBox.Show("Select Environment clicked. Functionality to be implemented.");
    }
}