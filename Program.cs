using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace refreshRateSwitcher;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TrayApplicationContext());
    }
}

public class TrayApplicationContext : ApplicationContext
{
    private NotifyIcon _notifyIcon;

    // This is the constructor method you were missing
    public TrayApplicationContext()
    {
        var assembly = typeof(Program).Assembly;
        
        // Ensure this string matches your project namespace + filename
        using (Stream? stream = assembly.GetManifestResourceStream("refreshRateSwitcher.icon.ico"))
        {
            _notifyIcon = new NotifyIcon()
            {
                Icon = stream != null ? new Icon(stream) : SystemIcons.Application,
                ContextMenuStrip = CreateContextMenu(),
                Visible = true,
                Text = "Refresh Rate Switcher"
            };
        }
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();
        
        // We will fill this with refresh rates
        var dm = new NativeMethods.DEVMODE();
        dm.dmSize = (short)Marshal.SizeOf(dm);

        // Get current resolution to filter rates
        NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm);
        int curW = dm.dmPelsWidth;
        int curH = dm.dmPelsHeight;

        int modeIndex = 0;
        var addedRates = new HashSet<int>();

        while (NativeMethods.EnumDisplaySettings(null, modeIndex, ref dm))
        {
            if (dm.dmPelsWidth == curW && dm.dmPelsHeight == curH)
            {
                int rate = dm.dmDisplayFrequency;
                if (addedRates.Add(rate))
                {
                    menu.Items.Add($"{rate}Hz", null, (s, e) => SetRefresh(rate));
                }
            }
            modeIndex++;
        }

        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Exit", null, (s, e) => { _notifyIcon.Visible = false; Application.Exit(); });
        return menu;
    }

    private void SetRefresh(int rate)
    {
        var dm = new NativeMethods.DEVMODE();
        dm.dmSize = (short)Marshal.SizeOf(dm);
        NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm);
        
        dm.dmDisplayFrequency = rate;
        dm.dmFields = NativeMethods.DM_DISPLAYFREQUENCY;

        int result = NativeMethods.ChangeDisplaySettingsEx(null, ref dm, IntPtr.Zero, 0, IntPtr.Zero);
        if (result != 0) MessageBox.Show($"Error changing rate: {result}");
    }
}

internal static class NativeMethods
{
    [DllImport("user32.dll")]
    public static extern bool EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);

    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettingsEx(string? lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

    public const int ENUM_CURRENT_SETTINGS = -1;
    public const int DM_DISPLAYFREQUENCY = 0x400000;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
    }
}
