using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace hideForm
{
    class W32api
    {
        public delegate bool WNDENUMPROC(IntPtr HWind, int lParam);

        [DllImport("user32.dll")]
        public extern static int GetWindowTextW(IntPtr HWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public extern static bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public extern static bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public extern static bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, System.Windows.Forms.Keys vkey);

        [DllImport("user32.dll")]
        public extern static bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public extern static IntPtr GetForegroundWindow();
    }

    //保存当前设置的快捷键信息
    public struct KeySetting
    {
        public uint sfsModifiers;
        public String svkey;
        public uint hfsModifiers;
        public String hvkey;
    }

    //保存窗口句柄和窗口名称的结构体
    public struct HwndInfo
    {
        public IntPtr HWnd;
        public String HWndName;
    }
}
