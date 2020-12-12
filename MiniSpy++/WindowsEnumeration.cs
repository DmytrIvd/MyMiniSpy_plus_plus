using System;
using System.Collections.Generic;

namespace MiniSpy__
{
    public class WindowsEnumeration
    {
        List<IntPtr> _windows=new List<IntPtr>();
        public IntPtr[] GetMainWindowsHandlesForThread(int id)
        {
            _windows.Clear();
            Win32Functions.EnumWindows(EnumWindows, id);
            return _windows.ToArray();
        }
        //Gets the first level of windows(associated with threads)
        private int EnumWindows(IntPtr hWnd, int lParam)
        {
            int processID = 0;
            int threadID = Win32Functions.GetWindowThreadProcessId(hWnd, out processID);
            if (threadID == lParam)
            {
                _windows.Add(hWnd);
                //Enumerate child also
               // EnumChildWindows(hWnd, WindowEnum, threadID);
            }
            return 1;
        }
        public IntPtr[] GetChildWindowHandles(IntPtr hWnd,int thread){
            _windows.Clear();
            Win32Functions.EnumChildWindows(hWnd, EnumWindows, thread);
            return _windows.ToArray();
        }
        //private int EnumChildWindows(IntPtr hWnd, int lParam)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
