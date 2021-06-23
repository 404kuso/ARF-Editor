using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ARF_Editor.Forms
{
    public partial class Worker : Form
    {
        public static class _Worker
        {
            static List<object> windows = new List<object>();
            public static void startForm(string formType, string[] args = null)
            {
                if (formType.ToUpper() == "ARC")
                {
                    var editor = new ARC_Editor(args != null ? args : new string[] { });
                    editor.setID((byte)windows.Count);

                    editor.FormClosing += ARC_FormClosing;
                    windows.Add(editor);
                    editor.Show();
                }
                else if (formType.ToUpper() == "ARA")
                {
                    var editor = new ARA_Editor(args != null ? args : new string[] { });
                    editor.setID((byte)windows.Count);

                    editor.FormClosing += ARA_FormClosing;
                    windows.Add(editor);
                    editor.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Worker request!");
                }
            }
            public static byte howMany(string formType)
            {
                if (formType.ToUpper() == "ARC")
                    return (byte)windows.Where(x => x.GetType().ToString() == "ARF_Editor.Forms.ARC_Editor").Count();
                else if (formType.ToUpper() == "ARA")
                    return (byte)windows.Where(x => x.GetType().ToString() == "ARF_Editor.Forms.ARA_Editor").Count();
                else
                    return 0;
            }
            private static void CheckWindows()
            {
                if (windows.Count == 0)
                    Application.Exit();
            }
            #region onClose
            private static void ARC_FormClosing(object sender, FormClosingEventArgs e)
            {
                if (windows.IndexOf((sender as ARC_Editor)) == -1)
                    return;

                ARC_Editor me = (windows.Where(x => x.GetType().ToString() == "ARF_Editor.Forms.ARC_Editor" && (x as ARC_Editor).ID == (sender as ARC_Editor).ID).ToArray()[0] as ARC_Editor);
                windows.Remove(me);

                CheckWindows();
                me.Close();
            }
            private static void ARA_FormClosing(object sender, FormClosingEventArgs e)
            {
                if (windows.IndexOf((sender as ARA_Editor)) == -1)
                    return;

                ARA_Editor me = (windows.Where(x => x.GetType().ToString() == "ARF_Editor.Forms.ARA_Editor" && (x as ARA_Editor).ID == (sender as ARA_Editor).ID).ToArray()[0] as ARA_Editor);
                windows.Remove(me);

                CheckWindows();
                me.Close();
            }
            #endregion
        }


        public Worker(string[] args)
        {
            InitializeComponent();

            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            _Worker.startForm("ARC", args);
        }

    }
}
