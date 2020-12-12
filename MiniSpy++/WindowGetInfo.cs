using System;
using System.Drawing;
using System.Text;

namespace MiniSpy__
{
    public static class WindowGetInfo{
        public static string GetClass(IntPtr handle)
        {
            StringBuilder sbClass = new StringBuilder(256);
            Win32Functions.GetClassName(handle.ToInt32(), sbClass, sbClass.Capacity);
            return sbClass.ToString();
        }
        public static string GetText(IntPtr handle)
        {
            int txtLength = Win32Functions.SendMessage(handle.ToInt32(), Win32Functions.WM_GETTEXTLENGTH, 0, 0);
            StringBuilder sbText = new StringBuilder(txtLength + 1);
            Win32Functions.SendMessage(handle.ToInt32(), Win32Functions.WM_GETTEXT, sbText.Capacity, sbText);
            return sbText.ToString();
        }

       public static string GetRect(IntPtr handle)
        {
            Rectangle rect = new Rectangle();
           Win32Functions.GetWindowRect(handle.ToInt32(),ref rect);
            rect.Width = rect.Width - rect.X;
            rect.Height = rect.Height - rect.Y;
            return rect.ToString();
        }

        public static string GetStyle(IntPtr handle)
        {
            IntPtr value = IntPtr.Zero;
            //if (IntPtr.Size == 8)
                 value = Win32Functions.GetWindowLong(handle, (int)GWL.GWL_STYLE);
            //else
            //    value = Win32Functions.GetWindowLongPtr(handle, (int)GWL.GWL_STYLE);
            return value.ToString();
        }
    }
}
