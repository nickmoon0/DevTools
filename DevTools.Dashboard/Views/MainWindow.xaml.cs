using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DevTools.Dashboard.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    // Import the DWM API function.
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        var hwnd = new WindowInteropHelper(this).Handle;
        var useDarkMode = 1; // 1 to enable dark mode

        // Attribute value 20 corresponds to DWMWA_USE_IMMERSIVE_DARK_MODE
        var dwmSetWindowAttribute = DwmSetWindowAttribute(hwnd, 20, ref useDarkMode, sizeof(int));
    }
}