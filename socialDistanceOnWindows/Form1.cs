using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using User32;

namespace socialDistancingWindows
{
    public partial class Form1 : Form
    {

        private List<Window> WindowList = new List<Window>();

        public Form1()
        {
            InitializeComponent();
        }

        private void ForceSocialDistance(List<Window> list)
        {
            foreach (var win in list)
            {
                string t = win.Text;
                if(!IsKeepSocialDistance(t)) win.Text = KeepSocialDistance(t);
            }
        }

        private string KeepSocialDistance(string denseText)
        {
            string notDenseText = "";
            foreach (var c in denseText)
            {
                notDenseText += c + " ";
            }
            return notDenseText.Trim();
        }

        private bool IsKeepSocialDistance(string text)
        {
            for (int i = 1; i <= text.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (text[i - 1] != ' ') return false;
                }
            }
            return true;
        }


        private List<Window> GetTopWindows()
        {
            var windowList = new List<Window>();

            User32.User32.EnumWindows(
                new User32.User32.EnumWindowsProc(proc),
                IntPtr.Zero
                );

            return windowList;

            bool proc(IntPtr hwnd, ref IntPtr lparam)
            {
                windowList.Add(
                    new Window(hwnd)
                    );
                return true;
            }
        }

        private List<Window> GetChildWindows(IntPtr parentHwnd)
        {
            var windowList = new List<Window>();

            User32.User32.EnumChildWindows(
                parentHwnd,
                new User32.User32.EnumWindowsProc(proc),
                IntPtr.Zero
                );

            return windowList;

            bool proc(IntPtr hwnd, ref IntPtr lparam)
            {
                windowList.Add(
                    new Window(hwnd)
                    );
                return true;
            }
        }

        private bool btnStatus = false;

        private async void MainBtn_Click(object sender, EventArgs e)
        {
            btnStatus = !btnStatus;
            if (btnStatus)
            {
                MainBtn.Text = KeepSocialDistance(MainBtn.Text);
                await Task.Run(() => mitsudesu());
            }
            else
            {
                MainBtn.Text = "Social Distance";
            }

            void mitsudesu()
            {
                while (btnStatus)
                {
                    var list = new List<Window>();
                    list.AddRange(GetTopWindows());
                    var newList = new List<Window>();
                    foreach (var win in list)
                    {
                        newList.AddRange(
                            GetChildWindows(win.hwnd)
                           );
                    }
                    list.AddRange(newList);

                    var newWindows = new List<Window>();
                    newWindows.AddRange(list.Where(v => !WindowList.Any(x => x.hwnd == v.hwnd)));
                    ForceSocialDistance(newWindows);
                    WindowList.AddRange(newWindows);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (btnStatus)
            {
                var rand = new System.Random();
                MainBtn.BackColor = Color.FromArgb(
                        rand.Next(0, 255),
                        rand.Next(0, 255),
                        rand.Next(0, 255)
                    );
            }
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://kamokamo.tk/");
        }
    }
}
