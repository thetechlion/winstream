using System.Windows;
using Velopack;

namespace WinStream;

public partial class App : Application
{
    [STAThread]
    private static void Main(string[] args)
    {
        // Must run first: handles install / update / uninstall hooks.
        VelopackApp.Build().Run();

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}
