using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public class gcc
    {
        private static readonly int h = 1080;
        private static readonly int w = 1920;
        protected delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        //[StructLayout(LayoutKind.Sequential)]
        //public class MouseLLHookStruct
        //{
        //    public Point pt;
        //    public int mouseData;
        //    public int flags;
        //    public int time;
        //    public int dwExtraInfo;
        //}

        [DllImport("user32.dll", CharSet = CharSet.Auto,
   CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(
    int idHook,
    HookProc lpfn,
    IntPtr hMod,
    int dwThreadId);
        public gcc()
        {
            try
            {
                SetWindowsHookEx(
              14,
              (x, y, z) =>
              {
                  if (y==513)
                  {
                  using (Bitmap b = new Bitmap(w,h))
                      {
                          var gr = Graphics.FromImage(b);
                          gr.CopyFromScreen(Point.Empty, Point.Empty, new Size()
                          {
                              Height=h,Width=w
                          });
                          b.Save($@"D:\Users\dmp\{DateTime.Now.TimeOfDay.ToString().Replace(':', '_')}.jpg",
                              System.Drawing.Imaging.ImageFormat.Jpeg);
                          gr.Dispose();
                          b.Dispose();
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
