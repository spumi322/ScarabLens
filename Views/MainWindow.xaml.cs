using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Brushes        = System.Windows.Media.Brushes;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ScarabLens;

public partial class MainWindow : Window
{
    private const int HotkeyId      = 9001;
    private const int HotkeyIdCoord = 9002;

    private HwndSource? _hwndSource;
    private bool _inCoordMode;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var labels = new List<TextBlock>(SlotPositions.All.Count);

        foreach (var (_, x, y) in SlotPositions.All)
        {
            var tb = new TextBlock
            {
                Text       = "?",
                Foreground = Brushes.Yellow,
                FontSize   = 11,
                FontWeight = FontWeights.Bold,
                Effect     = new DropShadowEffect
                {
                    Color       = Colors.Black,
                    BlurRadius  = 2,
                    ShadowDepth = 1,
                    Opacity     = 1,
                },
            };

            Canvas.SetLeft(tb, x + 5);
            Canvas.SetTop(tb, y + 10);

            // Insert before HitSurface (last child) so it stays on top for coord mode
            OverlayCanvas.Children.Insert(OverlayCanvas.Children.Count - 1, tb);
            labels.Add(tb);
        }

        if (DataContext is OverlayViewModel vm)
            vm.PricesRefreshed += prices =>
                Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < labels.Count && i < prices.Count; i++)
                        labels[i].Text = prices[i];
                });
    }

    // SourceInitialized fires after the HWND exists but before the window is shown —
    // the earliest safe point for P/Invoke calls that need a valid handle.
    private void Window_SourceInitialized(object sender, EventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;

        // Make the window click-through: layered enables alpha/transparency composition,
        // transparent routes all hit-testing to whatever is below this window.
        int exStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE,
            exStyle | NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TRANSPARENT);

        // Wire up WM_HOTKEY via HwndSource hook
        _hwndSource = HwndSource.FromHwnd(hwnd);
        _hwndSource.AddHook(WndProc);

        // Ctrl+Alt+S — toggle overlay visibility
        if (!NativeMethods.RegisterHotKey(hwnd, HotkeyId,
                NativeMethods.MOD_CONTROL | NativeMethods.MOD_ALT | NativeMethods.MOD_NOREPEAT,
                (uint)'S'))
            Debug.WriteLine("[MainWindow] Failed to register Ctrl+Alt+S hotkey");

        // F2 — toggle coordinate helper mode
        if (!NativeMethods.RegisterHotKey(hwnd, HotkeyIdCoord,
                NativeMethods.MOD_NOREPEAT,
                NativeMethods.VK_F2))
            Debug.WriteLine("[MainWindow] Failed to register F2 hotkey");
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == NativeMethods.WM_HOTKEY)
        {
            int id = wParam.ToInt32();
            if (id == HotkeyId)
            {
                ToggleOverlay();
                handled = true;
            }
            else if (id == HotkeyIdCoord)
            {
                ToggleCoordMode();
                handled = true;
            }
        }
        return IntPtr.Zero;
    }

    private void ToggleOverlay()
        => Visibility = Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

    private void ToggleCoordMode()
    {
        _inCoordMode = !_inCoordMode;
        var hwnd = new WindowInteropHelper(this).Handle;

        int exStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);

        if (_inCoordMode)
        {
            // Enter coord mode: remove WS_EX_TRANSPARENT so mouse events reach this window
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE,
                exStyle & ~NativeMethods.WS_EX_TRANSPARENT);
            // Size the hit surface to fill the whole window before making it visible
            HitSurface.Width  = ActualWidth;
            HitSurface.Height = ActualHeight;
            HitSurface.Visibility = Visibility.Visible;
            CoordLabel.Visibility = Visibility.Visible;
        }
        else
        {
            // Return to overlay mode: restore full click-through
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE,
                exStyle | NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TRANSPARENT);
            HitSurface.Visibility = Visibility.Collapsed;
            CoordLabel.Visibility = Visibility.Collapsed;
        }
    }

    private void HitSurface_MouseMove(object sender, MouseEventArgs e)
    {
        var pos = e.GetPosition(this);
        CoordLabel.Text = $"X: {(int)pos.X}  Y: {(int)pos.Y}";
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        NativeMethods.UnregisterHotKey(hwnd, HotkeyId);
        NativeMethods.UnregisterHotKey(hwnd, HotkeyIdCoord);
        _hwndSource?.RemoveHook(WndProc);
    }
}
