using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace System.Windows.Input
{
    public class HotKeyEventArgs : EventArgs
    {
        public HotKey Hotkey { get; private set; }

        public HotKeyEventArgs(HotKey hotkey)
        {
            Hotkey = hotkey;
        }
    }

    public class HotKey
    {
        private const int WM_HOTKEY = 786;

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, int modifiers, int key);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int UnregisterHotKey(IntPtr hwnd, int id);

        public enum Modifiers : int
        {
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            Win = 8
        }

        public Key Key { get; set; }
        public Modifiers ModifierKeys { get; set; }

        public event EventHandler<HotKeyEventArgs> HotKeyPressed;

        private int id;

        private HwndSourceHook hook;
        private HwndSource hwndSource;

        private static readonly Random rand = new Random((int)DateTime.Now.Ticks);

        public HotKey(HwndSource hwndSource)
        {
            if (hwndSource == null)
            {
                throw new ArgumentNullException("hwnd was null");
            }

            this.hook = new HwndSourceHook(WndProc);
            this.hwndSource = hwndSource;
            hwndSource.AddHook(hook);

            id = rand.Next();
        }

        public void UpdateHwndSource(HwndSource hwndSource)
        {
            this.hwndSource = hwndSource;
        }

        private bool enabled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (value == enabled) return;

                int handle = (int)hwndSource.Handle;
                if (handle == 0)
                {
                    if (value) throw new ArgumentNullException("Handle");
                    enabled = value;
                    return;
                }

                int result = value ? RegisterHotKey(hwndSource.Handle, id, (int)ModifierKeys, KeyInterop.VirtualKeyFromKey(Key))
                                   : UnregisterHotKey(hwndSource.Handle, id);

                if (result == 0)
                {
                    int error = Marshal.GetLastWin32Error();
                    if (error != 0) throw new Win32Exception(error);
                }

                enabled = value;
            }
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY && (int)wParam == id && HotKeyPressed != null)
            {
                HotKeyPressed(this, new HotKeyEventArgs(this));
            }

            return IntPtr.Zero;
        }


        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                hwndSource.RemoveHook(hook);
            }

            Enabled = false;

            disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HotKey()
        {
            Dispose();
        }
    }
}
