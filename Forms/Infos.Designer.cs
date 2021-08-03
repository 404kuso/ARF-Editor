
namespace ARF_Editor.Forms
{
    partial class Infos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_name = new System.Windows.Forms.Label();
            this.label_description = new System.Windows.Forms.Label();
            this.label_company = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_name.Location = new System.Drawing.Point(179, 9);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(85, 21);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "ARF-Editor";
            // 
            // label_description
            // 
            this.label_description.AutoSize = true;
            this.label_description.Location = new System.Drawing.Point(13, 39);
            this.label_description.Name = "label_description";
            this.label_description.Size = new System.Drawing.Size(418, 15);
            this.label_description.TabIndex = 1;
            this.label_description.Text = "yeah yeah Beschreibung hier bitte einfügen Vielen Dank  und sowas yeah yeah";
            // 
            // label_company
            // 
            this.label_company.AutoSize = true;
            this.label_company.Location = new System.Drawing.Point(13, 91);
            this.label_company.Name = "label_company";
            this.label_company.Size = new System.Drawing.Size(103, 15);
            this.label_company.TabIndex = 2;
            this.label_company.Text = "HanimeJunkies.TV";
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(400, 91);
            this.label_version.Name = "label_version";
            this.label_version.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_version.Size = new System.Drawing.Size(31, 15);
            this.label_version.TabIndex = 3;
            this.label_version.Text = "0.0.0";
            // 
            // Infos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 115);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.label_company);
            this.Controls.Add(this.label_description);
            this.Controls.Add(this.label_name);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Infos";
            this.Text = "404kuso war hier";
            this.Load += new System.EventHandler(this.Infos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_description;
        private System.Windows.Forms.Label label_company;
        private System.Windows.Forms.Label label_version;
    }
}