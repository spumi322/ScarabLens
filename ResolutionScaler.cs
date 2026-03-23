namespace ScarabLens;

internal sealed class ResolutionScaler
{
    public bool Found  { get; private set; }
    public int  Left   { get; private set; }
    public int  Top    { get; private set; }
    public int  Width  { get; private set; }
    public int  Height { get; private set; }

    private double _scaleX = 1.0;
    private double _scaleY = 1.0;

    public void Refresh()
    {
        var hwnd = NativeMethods.FindWindow(null, "Path of Exile");
        if (hwnd == IntPtr.Zero)
        {
            Found = false;
            return;
        }

        if (!NativeMethods.GetWindowRect(hwnd, out var rect))
        {
            Found = false;
            return;
        }

        uint dpi      = NativeMethods.GetDpiForWindow(hwnd);
        double scale  = dpi > 0 ? dpi / 96.0 : 1.0;

        Width  = (int)Math.Round((rect.Right  - rect.Left) / scale);
        Height = (int)Math.Round((rect.Bottom - rect.Top)  / scale);
        Left   = (int)Math.Round(rect.Left / scale);
        Top    = (int)Math.Round(rect.Top  / scale);

        _scaleX = Width  / 1920.0;
        _scaleY = Height / 1080.0;
        Found   = true;
    }

    public double X(double baseX)       => baseX * _scaleX;
    public double Y(double baseY)       => baseY * _scaleY;
    public double Font(double baseSize) => baseSize * _scaleY;
}
