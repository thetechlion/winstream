using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;

namespace WinStream;

public partial class MainWindow : Window
{
    private bool _webViewReady;
    private bool _isFullscreen;
    private WindowState _prevState;
    private WindowStyle _prevStyle;
    private ResizeMode _prevResize;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (_, _) =>
        {
            await InitWebViewAsync();
            _ = Updater.CheckForUpdatesAsync(); // background update check
        };
    }

    private async Task InitWebViewAsync()
    {
        try
        {
            // Persistent profile so sign-ins are remembered between launches.
            string userData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WinStream", "WebView2Profile");
            Directory.CreateDirectory(userData);

            var env = await CoreWebView2Environment.CreateAsync(null, userData);
            await Browser.EnsureCoreWebView2Async(env);

            var core = Browser.CoreWebView2;
            core.Settings.AreDefaultContextMenusEnabled = true;
            core.Settings.IsStatusBarEnabled = false;
            core.Settings.AreDevToolsEnabled = false;
            core.Settings.IsZoomControlEnabled = false;

            // Keep pop-ups / "open in new window" links inside the app.
            core.NewWindowRequested += (_, e) =>
            {
                e.Handled = true;
                core.Navigate(e.Uri);
            };

            // Let video players go fullscreen in the app window.
            core.ContainsFullScreenElementChanged += (_, _) =>
                SetFullscreen(core.ContainsFullScreenElement);

            _webViewReady = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "WinStream couldn't start the embedded Edge (WebView2) runtime.\n\n" +
                "Install the Microsoft Edge WebView2 Runtime and try again.\n\n" + ex.Message,
                "WinStream", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Tile_Click(object sender, RoutedEventArgs e)
    {
        if (!_webViewReady || sender is not Button { Tag: string url }) return;

        Launcher.Visibility = Visibility.Collapsed;
        Browser.Visibility = Visibility.Visible;
        Browser.CoreWebView2.Navigate(url);
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        if (_isFullscreen) SetFullscreen(false);

        if (_webViewReady)
        {
            // Stop any playback, then go back to the launcher.
            Browser.CoreWebView2.Navigate("about:blank");
        }

        Browser.Visibility = Visibility.Collapsed;
        Launcher.Visibility = Visibility.Visible;
    }

    private void SetFullscreen(bool on)
    {
        if (on == _isFullscreen) return;

        if (on)
        {
            _prevState = WindowState;
            _prevStyle = WindowStyle;
            _prevResize = ResizeMode;

            TopBar.Visibility = Visibility.Collapsed;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
        }
        else
        {
            WindowStyle = _prevStyle;
            ResizeMode = _prevResize;
            WindowState = _prevState;
            TopBar.Visibility = Visibility.Visible;
        }

        _isFullscreen = on;
    }
}
