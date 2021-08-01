using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ARF_Editor.Forms
{
    public partial class Worker : Form
    {
        public static class _Worker
        {
            static string current_form = "";
            static void should_close(string formType)
            {
                if (current_form == formType)
                    Application.Exit();
            }

            public static void startForm(string formType, string[] args = null)
            {
                if (formType.ToUpper() == "ARC")
                {
                    var editor = new ARC_Editor(args != null ? args : new string[] { });

                    editor.FormClosing += (object sender, FormClosingEventArgs e) => should_close(formType);
                    editor.Show();
                }
                else if (formType.ToUpper() == "ARA")
                {
                    var editor = new ARA_Editor(args != null ? args : new string[] { });

                    editor.FormClosing += (object sender, FormClosingEventArgs e) => should_close(formType);
                    editor.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Worker request!");
                }
                current_form = formType;
            }
            #region onClose
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
