using System.Threading;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox   = System.Windows.MessageBox;

namespace ScarabLens;

public partial class App : Application
{
    private const string MutexName = "Global\\ScarabLens_SingleInstance";
    private Mutex? _mutex;
    private System.Windows.Forms.NotifyIcon? _trayIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new Mutex(initiallyOwned: true, MutexName, out bool createdNew);
        if (!createdNew)
        {
            MessageBox.Show("ScarabLens is already running.", "ScarabLens",
                MessageBoxButton.OK, MessageBoxImage.Information);
            Shutdown();
            return;
        }

        base.OnStartup(e);

        var apiService = new ApiService();
        var priceCache = new PriceCache(apiService);
        var vm         = new OverlayViewModel(priceCache);

        var window = new MainWindow { DataContext = vm };
        window.Show();
        window.Visibility = Visibility.Hidden;

        // Fetch prices in the background; update labels when done
        _ = priceCache.RefreshAsync().ContinueWith(_ => vm.RefreshDisplay());

        _trayIcon = CreateTrayIcon();
    }

    private static System.Windows.Forms.NotifyIcon CreateTrayIcon()
    {
        var streamInfo = GetResourceStream(new Uri("pack://application:,,,/Assets/scarablens_icon.ico"));
        var icon = new System.Drawing.Icon(streamInfo.Stream);

        var exitItem = new System.Windows.Forms.ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) => Current.Shutdown();

        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add(exitItem);

        return new System.Windows.Forms.NotifyIcon
        {
            Icon             = icon,
            Visible          = true,
            Text             = "ScarabLens",
            ContextMenuStrip = menu,
        };
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon?.Dispose();
        _mutex?.ReleaseMutex();
        _mutex?.Dispose();
        base.OnExit(e);
    }
}
