using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ARF_Editor.Forms
{
    public partial class Infos : Form
    {
        public Infos()
        {
            InitializeComponent();
        }

        private void Infos_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.label_name.Left = (this.Width - this.label_name.Size.Width) / 2;
            this.label_description.AutoSize = true;
            this.label_description.MaximumSize = Size;


            Assembly assembly = Assembly.GetExecutingAssembly();
            string description= assembly
            .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
            .OfType<AssemblyDescriptionAttribute>()
            .FirstOrDefault().Description;
            
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            this.label_name.Text = fileInfo.ProductName;
            this.label_description.Text = description;
            this.label_company.Text = fileInfo.CompanyName;
            this.label_version.Text = fileInfo.ProductVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("cmd", "/c start https://github.com/AnimeJunkies-TV/ARF-Editor/wiki/UI");
        }
    }
}
