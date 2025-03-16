using System.Windows;
using System.Windows.Controls;
using DevTools.Tooling.Interfaces;

namespace DevTools.Dashboard.Views;

public partial class SidebarMenu : UserControl
{
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