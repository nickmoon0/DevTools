using System.Windows;
using System.Windows.Input;
using DevTools.Dashboard.ViewModels;

namespace DevTools.Dashboard.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    // Allow dragging of the window when clicking on the title bar.
    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Initiate dragging of the window.
        if (e.ClickCount == 2)
        {
            ToggleMaximize();
        }
        else
        {
            DragMove();
        }
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        ToggleMaximize();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ToggleMaximize()
    {
        WindowState = WindowState == WindowState.Normal 
            ? WindowState.Maximized 
            : WindowState.Normal;
    }
}