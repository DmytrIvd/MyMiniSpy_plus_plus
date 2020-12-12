using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSpy__
{
    public partial class MiniSpy : Form
    {

        public MiniSpy()
        {
            InitializeComponent();
            InitializeWindows();
        }
        #region Process
        private async void InitializeWindows()
        {
            var arr = await GetExecWindows();
            treeView1.Nodes.AddRange(arr);
        }

        private async void toolStripButton1_ClickAsync(object sender, EventArgs e)
        {

            treeView1.BeginUpdate();
            Cursor = Cursors.WaitCursor;
            treeView1.Nodes.Clear();
            var s = await GetExecWindows();
            treeView1.Nodes.AddRange(s);
            treeView1.EndUpdate();
            Cursor = Cursors.Default;
        }

        async Task<TreeNode[]> GetExecWindows()
        {
            var nodes = new List<TreeNode>();
            var process = await Task.Factory.StartNew(() => Process.GetProcesses());
            var WE = new WindowsEnumeration();
            foreach (var p in process)
                foreach (ProcessThread t in p.Threads)
                {

                    var mainWindows = WE.GetMainWindowsHandlesForThread(t.Id);
                    if (mainWindows != null && mainWindows.Length > 0)
                    {
                        foreach (IntPtr hWnd in mainWindows)
                        {
                            try
                            {
                                //FirstLevel
                                var node = GetTreeNode(hWnd);

                                if (node != null)
                                {
                                    nodes.Add(node);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                        TurnOnDeathMashine(nodes, t.Id, WE);
                    }

                }


            return nodes.ToArray();
        }

        private void TurnOnDeathMashine(List<TreeNode> nodes, int ThreadId, WindowsEnumeration win)
        {



            foreach (var n in nodes)
            {

                var LevelTwo = win.GetChildWindowHandles((IntPtr)n.Tag, ThreadId);
                if (LevelTwo != null && LevelTwo.Length > 0)
                {
                    var Coll = LevelTwo.Select(h => GetTreeNode(h)).ToList();
                    n.Nodes.AddRange(Coll.ToArray());
                    TurnOnDeathMashine(Coll, ThreadId, win);
                }
            }
        }

        TreeNode GetTreeNode(IntPtr handle)
        {
            TreeNode treeNode = new TreeNode();
            treeNode.Tag = handle;

            treeNode.Name = handle.ToString();
            // Get the class.

            var Sbclass = WindowGetInfo.GetClass(handle);
            // Get the text.
            var Sbtxt = WindowGetInfo.GetText(handle);

            //Form to tree node
            treeNode.Text = $"Window {handle} \"{Sbtxt}\" {Sbclass}";
            return treeNode;
        }
        #endregion

        private void MiniSpy_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            WindowFinder windowFinder = new WindowFinder();
            windowFinder.Show();

        }
    }
}
