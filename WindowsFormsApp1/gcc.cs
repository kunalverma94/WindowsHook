using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class gcc
    {
        protected delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        protected class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto,
   CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook,HookProc lpfn,IntPtr hMod,int dwThreadId);
        public gcc()
        {
            KeyHook();
        }

        private static void mouseHook()
        {
            try
            {
                SetWindowsHookEx(
              14,
              (x, y, z) =>
              {
                  // Console.WriteLine("{0}\n{1}\n{2}", x, y, z);
                  if (y == 513)
                  {
                      Snip("msPrmy");
                  }
                  return 0;
              },
              Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
              0);
            }
            finally
            {
                GC.Collect();
            }
        }

        private static void Snip(string from)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            var h = resolution.Height;
            var w = resolution.Width;
            using (Bitmap b = new Bitmap(w, h))
            {
                var gr = Graphics.FromImage(b);
                gr.CopyFromScreen(Point.Empty, Point.Empty, new Size()
                {
                    Height = h,
                    Width = w
                });
                b.Save($@"D:\Users\dmp\{from}{DateTime.Now.TimeOfDay.ToString().Replace(':', '_')}.jpg",
                    System.Drawing.Imaging.ImageFormat.Jpeg);
                gr.Dispose();
                b.Dispose();
            }
        }

        private static void KeyHook()
        {
            try
            {
                SetWindowsHookEx(
              13,
              (x, wParam, lParam) =>
              {

                  if (wParam == 256)
                  {
                      KeyboardHookStruct keyboardHookStruct =
    (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                      if (keyboardHookStruct.vkCode == 44 && keyboardHookStruct.scanCode == 55)
                      {
                          Snip("PrtScr");

                      }
                  }
                  return 0;
              },
              Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
              0);
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
