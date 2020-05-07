using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace socialDistancingWindows
{
    class Window
    {
        public Window(IntPtr hwnd)
        {
            this.hwnd = hwnd;
        }

        public IntPtr hwnd;

        public string Text
        {
            get
            {
                int textLen = User32.User32.GetWindowTextLength(hwnd);
                if (textLen > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder(textLen + 1); //ヌル文字分確保
                    User32.User32.GetWindowText(hwnd, stringBuilder, textLen + 1);
                    return stringBuilder.ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _ = User32.User32.SetWindowText(hwnd, value);
            }
        }
    }
}
