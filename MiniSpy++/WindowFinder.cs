using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSpy__
{
    public partial class WindowFinder : Form
    {
        IntPtr mouseHook { get; set; } = IntPtr.Zero;
        IntPtr currentWindow { get; set; } = IntPtr.Zero;
        private static LowLevelMouseProc LowLevelMouseProc = new LowLevelMouseProc(MouseProcess);
        private static event EventHandler MouseEvent;

        public WindowFinder()
        {
            InitializeComponent();
            MouseEvent += HandleReceived;
        }
        #region EventHandlers
        //FillAll fields at textbox
        private void HandleReceived(object sender, EventArgs e)
        {
            var handle = (IntPtr)sender;
            if (handle != IntPtr.Zero&&currentWindow != handle)
            {
                currentWindow = handle;
               
                    Handletxtbox.Text = handle.ToString();
                    Classtxtbox.Text = WindowGetInfo.GetClass(handle);
                    Captiontxtbox.Text = WindowGetInfo.GetText(handle);
                    Recttxtbox.Text = WindowGetInfo.GetRect(handle);
                    Styletxtbox.Text = WindowGetInfo.GetStyle(handle);
                
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Cursor = Cursors.Cross;
                InstallHook();
            }
            else
            {
                Cursor = Cursors.Default;
                UnInstallHook();

            }
        }
        private void WindowFinder_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnInstallHook();
        }
        #endregion
        #region MouseProcess
        private static IntPtr MouseProcess(int nCode, UIntPtr wParam, IntPtr lParam)
        {

            IntPtr hWnd = IntPtr.Zero;
            if (nCode >= 0)
            {
                var p = Marshal.PtrToStructure<MOUSEHOOKSTRUCT>(lParam);
                hWnd = Win32Functions.WindowFromPoint(p.pt);
                MouseEvent?.Invoke(hWnd, EventArgs.Empty);

            }
            return Win32Functions.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
        #endregion
        
        #region HookInstalers
        private void InstallHook()
        {
            if (mouseHook == IntPtr.Zero)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    IntPtr ptr = Win32Functions.GetModuleHandle(curModule.ModuleName);
                    Win32Functions.SetWindowsHookEx(HookType.LowLevelMouse, LowLevelMouseProc,
                         Win32Functions.GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }
        private void UnInstallHook()
        {
            Win32Functions.UnhookWindowsHookEx(mouseHook);
            mouseHook = IntPtr.Zero;
        }
        #endregion
      

       

    

        
    }
   // public delegate void FillFieldsD(IntPtr handle);
}
