using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Capslock_Indicator {
    class Hooker : IDisposable
    {
        public delegate IntPtr HookDelegate(Int32 Code, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, Int32 nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);


        [DllImport("User32.dll")]
        public static extern IntPtr SetWindowsHookEx(Int32 idHook, HookDelegate lpfn, IntPtr hmod, Int32 dwThreadId);


        public event EventHandler<EventArgs> KeyBoardKeyPressed;

        private HookDelegate keyBoardDelegate;
        private IntPtr keyBoardHandle;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYUP = 0x0101;

        CapslockIndicator capslockIndicator;

        public Hooker(CapslockIndicator capslockIndicator)
        {
            this.capslockIndicator = capslockIndicator;

            keyBoardDelegate = KeyboardHookDelegate;
            keyBoardHandle = SetWindowsHookEx(
                WH_KEYBOARD_LL, keyBoardDelegate, IntPtr.Zero, 0);
        }

        private IntPtr KeyboardHookDelegate(
            Int32 Code, IntPtr wParam, IntPtr lParam)
        {
            if (Code < 0)
                return CallNextHookEx(keyBoardHandle, Code, wParam, lParam);
            else if(wParam == (IntPtr)WM_KEYUP && Marshal.ReadInt32(lParam) == 20)
                capslockIndicator.changeIcon();
            
            if (KeyBoardKeyPressed != null)
                KeyBoardKeyPressed(this, new EventArgs());

            return CallNextHookEx(
                keyBoardHandle, Code, wParam, lParam);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(keyBoardHandle);
        }
    }
}
