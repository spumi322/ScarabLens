using System.Runtime.InteropServices;

namespace ScarabLens;

internal static class NativeMethods
{
    // SetWindowLong index for extended style
    internal const int GWL_EXSTYLE = -20;

    // Extended window style flags
    internal const int WS_EX_LAYERED     = 0x00080000;
    internal const int WS_EX_TRANSPARENT = 0x00000020;

    // RegisterHotKey modifier flags
    internal const uint MOD_ALT      = 0x0001;
    internal const uint MOD_CONTROL  = 0x0002;
    internal const uint MOD_SHIFT    = 0x0004;
    internal const uint MOD_NOREPEAT = 0x4000;

    // Virtual key codes
    internal const uint VK_F2 = 0x71;

    // WM_HOTKEY window message
    internal const int WM_HOTKEY = 0x0312;

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}
