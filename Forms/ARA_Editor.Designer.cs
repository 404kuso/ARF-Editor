
namespace ARF_Editor.Forms
{
    partial class ARA_Editor
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.numericUpDown_PK = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.richtTxt_AttackText = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numUpDown_attackID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_AttackName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_AttackenTyp = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox_StatusAenderung = new System.Windows.Forms.CheckBox();
            this.label_statusTyp = new System.Windows.Forms.Label();
            this.comboBox_StatusTyp = new System.Windows.Forms.ComboBox();
            this.label_statusChance = new System.Windows.Forms.Label();
            this.label_statusChancePrefix = new System.Windows.Forms.Label();
            this.numericUpDown_StatusChance = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown_AttackenStaerke = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kartearcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attackearaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemariToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.öffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speichernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speichernUnterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inDatenbankEintragenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prüfenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fensterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstrip_changeToACA_Editor = new System.Windows.Forms.ToolStripMenuItem();
            this.optionenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datenbankpfadAuswählenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.numUpDown_Range = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.richTxt_beschreibung = new System.Windows.Forms.RichTextBox();
            this.label_EffektStaerke = new System.Windows.Forms.Label();
            this.numUpDown_EffektStaerke = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_attackID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_StatusChance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AttackenStaerke)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_Range)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_EffektStaerke)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown_PK
            // 
            this.numericUpDown_PK.Enabled = false;
            this.numericUpDown_PK.Location = new System.Drawing.Point(39, 262);
            this.numericUpDown_PK.Name = "numericUpDown_PK";
            this.numericUpDown_PK.Size = new System.Drawing.Size(61, 23);
            this.numericUpDown_PK.TabIndex = 27;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(12, 264);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 15);
            this.label13.TabIndex = 26;
            this.label13.Text = "PK";
            // 
            // richtTxt_AttackText
            // 
            this.richtTxt_AttackText.Location = new System.Drawing.Point(12, 93);
            this.richtTxt_AttackText.Name = "richtTxt_AttackText";
            this.richtTxt_AttackText.Size = new System.Drawing.Size(352, 72);
            this.richtTxt_AttackText.TabIndex = 24;
            this.richtTxt_AttackText.Text = "";
            this.richtTxt_AttackText.TextChanged += new System.EventHandler(this.richText_Beschreibung_Changed);
            this.richtTxt_AttackText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "Anzeigetext";
            // 
            // numUpDown_attackID
            // 
            this.numUpDown_attackID.Location = new System.Drawing.Point(36, 38);
            this.numUpDown_attackID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDown_attackID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_attackID.Name = "numUpDown_attackID";
            this.numUpDown_attackID.Size = new System.Drawing.Size(60, 23);
            this.numUpDown_attackID.TabIndex = 21;
            this.numUpDown_attackID.ThousandsSeparator = true;
            this.numUpDown_attackID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "ID";
            // 
            // txt_AttackName
            // 
            this.txt_AttackName.Location = new System.Drawing.Point(150, 38);
            this.txt_AttackName.Name = "txt_AttackName";
            this.txt_AttackName.Size = new System.Drawing.Size(214, 23);
            this.txt_AttackName.TabIndex = 23;
            this.txt_AttackName.TextChanged += new System.EventHandler(this.richText_Name_Changed);
            this.txt_AttackName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(105, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Name";
            // 
            // comboBox_AttackenTyp
            // 
            this.comboBox_AttackenTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AttackenTyp.FormattingEnabled = true;
            this.comboBox_AttackenTyp.Items.AddRange(new object[] {
            "Angriff",
            "Verteidigung",
            "Heilung",
            "Boost",
            "Statusänderung"});
            this.comboBox_AttackenTyp.Location = new System.Drawing.Point(453, 37);
            this.comboBox_AttackenTyp.Name = "comboBox_AttackenTyp";
            this.comboBox_AttackenTyp.Size = new System.Drawing.Size(245, 23);
            this.comboBox_AttackenTyp.TabIndex = 28;
            this.comboBox_AttackenTyp.SelectedIndexChanged += new System.EventHandler(this.comboBox_AttackenTyp_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(392, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 15);
            this.label4.TabIndex = 29;
            this.label4.Text = "Typ";
            // 
            // checkBox_StatusAenderung
            // 
            this.checkBox_StatusAenderung.AutoSize = true;
            this.checkBox_StatusAenderung.Location = new System.Drawing.Point(392, 106);
            this.checkBox_StatusAenderung.Name = "checkBox_StatusAenderung";
            this.checkBox_StatusAenderung.Size = new System.Drawing.Size(171, 19);
            this.checkBox_StatusAenderung.TabIndex = 30;
            this.checkBox_StatusAenderung.Text = "Statusänderung bei Attacke";
            this.checkBox_StatusAenderung.UseVisualStyleBackColor = true;
            this.checkBox_StatusAenderung.CheckedChanged += new System.EventHandler(this.checkBox_StatusAenderung_CheckedChanged);
            // 
            // label_statusTyp
            // 
            this.label_statusTyp.AutoSize = true;
            this.label_statusTyp.Location = new System.Drawing.Point(392, 136);
            this.label_statusTyp.Name = "label_statusTyp";
            this.label_statusTyp.Size = new System.Drawing.Size(39, 15);
            this.label_statusTyp.TabIndex = 32;
            this.label_statusTyp.Text = "Status";
            // 
            // comboBox_StatusTyp
            // 
            this.comboBox_StatusTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_StatusTyp.FormattingEnabled = true;
            this.comboBox_StatusTyp.Items.AddRange(new object[] {
            "Verbrennung",
            "Vergiftung",
            "Paralyse",
            "Verwirrung",
            "Angriffswert -",
            "Verteidigungswert -",
            "Angriffswert, Verteidigungswert -",
            "Schnelligkeitswert -",
            "Angriffswert, Schnelligkeitswert -",
            "Angriffswert, Verteidigungswert, Schnelligkeitswert -",
            "Verteidigungswert, Schnelligkeitswert -",
            "Angriffswert +",
            "Verteidigungswert +",
            "Angriffswert, Verteidigungswert +",
            "Schnelligkeitswert +",
            "Angriffswert und Schnelligkeitswert +",
            "Angriffswert, Verteidigungswert, Schnelligkeitswert +",
            "Verteidigungswert, Schnelligkeitswert +"});
            this.comboBox_StatusTyp.Location = new System.Drawing.Point(453, 131);
            this.comboBox_StatusTyp.Name = "comboBox_StatusTyp";
            this.comboBox_StatusTyp.Size = new System.Drawing.Size(245, 23);
            this.comboBox_StatusTyp.TabIndex = 31;
            // 
            // label_statusChance
            // 
            this.label_statusChance.AutoSize = true;
            this.label_statusChance.Location = new System.Drawing.Point(392, 166);
            this.label_statusChance.Name = "label_statusChance";
            this.label_statusChance.Size = new System.Drawing.Size(47, 15);
            this.label_statusChance.TabIndex = 33;
            this.label_statusChance.Text = "Chance";
            // 
            // label_statusChancePrefix
            // 
            this.label_statusChancePrefix.AutoSize = true;
            this.label_statusChancePrefix.Location = new System.Drawing.Point(453, 165);
            this.label_statusChancePrefix.Name = "label_statusChancePrefix";
            this.label_statusChancePrefix.Size = new System.Drawing.Size(24, 15);
            this.label_statusChancePrefix.TabIndex = 34;
            this.label_statusChancePrefix.Text = "1 / ";
            // 
            // numericUpDown_StatusChance
            // 
            this.numericUpDown_StatusChance.Location = new System.Drawing.Point(477, 163);
            this.numericUpDown_StatusChance.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_StatusChance.Name = "numericUpDown_StatusChance";
            this.numericUpDown_StatusChance.Size = new System.Drawing.Size(52, 23);
            this.numericUpDown_StatusChance.TabIndex = 35;
            this.numericUpDown_StatusChance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(393, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 36;
            this.label5.Text = "Stärke";
            // 
            // numericUpDown_AttackenStaerke
            // 
            this.numericUpDown_AttackenStaerke.Location = new System.Drawing.Point(453, 68);
            this.numericUpDown_AttackenStaerke.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_AttackenStaerke.Name = "numericUpDown_AttackenStaerke";
            this.numericUpDown_AttackenStaerke.Size = new System.Drawing.Size(99, 23);
            this.numericUpDown_AttackenStaerke.TabIndex = 37;
            this.numericUpDown_AttackenStaerke.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(710, 24);
            this.menuStrip1.TabIndex = 38;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuToolStripMenuItem,
            this.öffnenToolStripMenuItem,
            this.speichernToolStripMenuItem,
            this.speichernUnterToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            this.dateiToolStripMenuItem.ToolTipText = "Dateiverwaltung und so";
            // 
            // neuToolStripMenuItem
            // 
            this.neuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kartearcToolStripMenuItem,
            this.attackearaToolStripMenuItem,
            this.itemariToolStripMenuItem});
            this.neuToolStripMenuItem.Name = "neuToolStripMenuItem";
            this.neuToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.neuToolStripMenuItem.Text = "Neu";
            this.neuToolStripMenuItem.ToolTipText = "Neue Datei";
            // 
            // kartearcToolStripMenuItem
            // 
            this.kartearcToolStripMenuItem.Name = "kartearcToolStripMenuItem";
            this.kartearcToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.kartearcToolStripMenuItem.Text = "Karte (.arc)";
            this.kartearcToolStripMenuItem.ToolTipText = "Neue Karte";
            this.kartearcToolStripMenuItem.Click += new System.EventHandler(this.newARCFile);
            // 
            // attackearaToolStripMenuItem
            // 
            this.attackearaToolStripMenuItem.Name = "attackearaToolStripMenuItem";
            this.attackearaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.attackearaToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.attackearaToolStripMenuItem.Text = "Attacke (.ara)";
            this.attackearaToolStripMenuItem.ToolTipText = "Neue Attacke";
            this.attackearaToolStripMenuItem.Click += new System.EventHandler(this.newARAFile);
            // 
            // itemariToolStripMenuItem
            // 
            this.itemariToolStripMenuItem.Name = "itemariToolStripMenuItem";
            this.itemariToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.itemariToolStripMenuItem.Text = "Item (.ari)";
            this.itemariToolStripMenuItem.ToolTipText = "Neues Item";
            // 
            // öffnenToolStripMenuItem
            // 
            this.öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            this.öffnenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.öffnenToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.öffnenToolStripMenuItem.Text = "Öffnen";
            this.öffnenToolStripMenuItem.ToolTipText = "Öffnet eine Datei";
            this.öffnenToolStripMenuItem.Click += new System.EventHandler(this.öffnenToolStripMenuItem_Click_1);
            // 
            // speichernToolStripMenuItem
            // 
            this.speichernToolStripMenuItem.Name = "speichernToolStripMenuItem";
            this.speichernToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.speichernToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.speichernToolStripMenuItem.Text = "Speichern";
            this.speichernToolStripMenuItem.ToolTipText = "Speichert die aktuelle Attacke in der geöffneten Datei";
            this.speichernToolStripMenuItem.Click += new System.EventHandler(this.speichernToolStripMenuItem_Click);
            // 
            // speichernUnterToolStripMenuItem
            // 
            this.speichernUnterToolStripMenuItem.Name = "speichernUnterToolStripMenuItem";
            this.speichernUnterToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.speichernUnterToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.speichernUnterToolStripMenuItem.Text = "Speichern unter";
            this.speichernUnterToolStripMenuItem.ToolTipText = "Speichert die Attacke in einer anderen Datei";
            this.speichernUnterToolStripMenuItem.Click += new System.EventHandler(this.speichernUnterToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inDatenbankEintragenToolStripMenuItem,
            this.prüfenToolStripMenuItem,
            this.fensterToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.ToolTipText = "Tools für die Bearbeitung";
            // 
            // inDatenbankEintragenToolStripMenuItem
            // 
            this.inDatenbankEintragenToolStripMenuItem.Name = "inDatenbankEintragenToolStripMenuItem";
            this.inDatenbankEintragenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.inDatenbankEintragenToolStripMenuItem.ShowShortcutKeys = false;
            this.inDatenbankEintragenToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.inDatenbankEintragenToolStripMenuItem.Text = "In Datenbank eintragen";
            this.inDatenbankEintragenToolStripMenuItem.ToolTipText = "Trägt die aktuelle Attacke in die Datenbank ein";
            this.inDatenbankEintragenToolStripMenuItem.Click += new System.EventHandler(this.inDatenbankEintragenToolStripMenuItem_Click);
            // 
            // prüfenToolStripMenuItem
            // 
            this.prüfenToolStripMenuItem.Name = "prüfenToolStripMenuItem";
            this.prüfenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.P)));
            this.prüfenToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.prüfenToolStripMenuItem.Text = "Prüfen";
            this.prüfenToolStripMenuItem.ToolTipText = "Prüft die Daten der Attacke";
            this.prüfenToolStripMenuItem.Click += new System.EventHandler(this.prüfenToolStripMenuItem_Click);
            // 
            // fensterToolStripMenuItem
            // 
            this.fensterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.fensterToolStripMenuItem.Name = "fensterToolStripMenuItem";
            this.fensterToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.fensterToolStripMenuItem.Text = "Fenster";
            this.fensterToolStripMenuItem.ToolTipText = "Fensterverwaltung";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstrip_changeToACA_Editor});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.toolStripMenuItem1.Text = "Wechseln";
            this.toolStripMenuItem1.ToolTipText = "Wechselt das aktuelle Fenster";
            // 
            // toolstrip_changeToACA_Editor
            // 
            this.toolstrip_changeToACA_Editor.Name = "toolstrip_changeToACA_Editor";
            this.toolstrip_changeToACA_Editor.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.toolstrip_changeToACA_Editor.Size = new System.Drawing.Size(255, 22);
            this.toolstrip_changeToACA_Editor.Text = "ARC-Editor";
            this.toolstrip_changeToACA_Editor.Click += new System.EventHandler(this.changeToARC_Editor);
            // 
            // optionenToolStripMenuItem
            // 
            this.optionenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.datenbankpfadAuswählenToolStripMenuItem,
            this.informationenToolStripMenuItem});
            this.optionenToolStripMenuItem.Name = "optionenToolStripMenuItem";
            this.optionenToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.optionenToolStripMenuItem.Text = "Optionen";
            // 
            // datenbankpfadAuswählenToolStripMenuItem
            // 
            this.datenbankpfadAuswählenToolStripMenuItem.Name = "datenbankpfadAuswählenToolStripMenuItem";
            this.datenbankpfadAuswählenToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.datenbankpfadAuswählenToolStripMenuItem.Text = "Datenbankpfad auswählen";
            // 
            // informationenToolStripMenuItem
            // 
            this.informationenToolStripMenuItem.Name = "informationenToolStripMenuItem";
            this.informationenToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.informationenToolStripMenuItem.Text = "Informationen";
            this.informationenToolStripMenuItem.Click += new System.EventHandler(this.informationenToolStripMenuItem_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(558, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 15);
            this.label6.TabIndex = 39;
            this.label6.Text = "Fläche";
            // 
            // numUpDown_Range
            // 
            this.numUpDown_Range.Location = new System.Drawing.Point(605, 70);
            this.numUpDown_Range.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numUpDown_Range.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_Range.Name = "numUpDown_Range";
            this.numUpDown_Range.Size = new System.Drawing.Size(92, 23);
            this.numUpDown_Range.TabIndex = 40;
            this.numUpDown_Range.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 15);
            this.label7.TabIndex = 41;
            this.label7.Text = "Beschreibung";
            // 
            // richTxt_beschreibung
            // 
            this.richTxt_beschreibung.Location = new System.Drawing.Point(12, 194);
            this.richTxt_beschreibung.Name = "richTxt_beschreibung";
            this.richTxt_beschreibung.Size = new System.Drawing.Size(686, 61);
            this.richTxt_beschreibung.TabIndex = 42;
            this.richTxt_beschreibung.Text = "";
            this.richTxt_beschreibung.TextChanged += new System.EventHandler(this.richText_Beschreibung_Changed);
            this.richTxt_beschreibung.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // label_EffektStaerke
            // 
            this.label_EffektStaerke.AutoSize = true;
            this.label_EffektStaerke.Location = new System.Drawing.Point(558, 166);
            this.label_EffektStaerke.Name = "label_EffektStaerke";
            this.label_EffektStaerke.Size = new System.Drawing.Size(39, 15);
            this.label_EffektStaerke.TabIndex = 43;
            this.label_EffektStaerke.Text = "Stärke";
            // 
            // numUpDown_EffektStaerke
            // 
            this.numUpDown_EffektStaerke.Location = new System.Drawing.Point(606, 163);
            this.numUpDown_EffektStaerke.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numUpDown_EffektStaerke.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDown_EffektStaerke.Name = "numUpDown_EffektStaerke";
            this.numUpDown_EffektStaerke.Size = new System.Drawing.Size(92, 23);
            this.numUpDown_EffektStaerke.TabIndex = 44;
            this.numUpDown_EffektStaerke.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ARA_Editor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 290);
            this.Controls.Add(this.numUpDown_EffektStaerke);
            this.Controls.Add(this.label_EffektStaerke);
            this.Controls.Add(this.richTxt_beschreibung);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numUpDown_Range);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.numericUpDown_AttackenStaerke);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown_StatusChance);
            this.Controls.Add(this.label_statusChancePrefix);
            this.Controls.Add(this.label_statusChance);
            this.Controls.Add(this.label_statusTyp);
            this.Controls.Add(this.comboBox_StatusTyp);
            this.Controls.Add(this.checkBox_StatusAenderung);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_AttackenTyp);
            this.Controls.Add(this.numericUpDown_PK);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.richtTxt_AttackText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numUpDown_attackID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_AttackName);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "ARA_Editor";
            this.Text = "ARA_Editor";
            this.Load += new System.EventHandler(this.ARA_Editor_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ARA_Editor_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ARA_Editor_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_attackID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_StatusChance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AttackenStaerke)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_Range)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown_EffektStaerke)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.NumericUpDown numericUpDown_PK;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RichTextBox richtTxt_AttackText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numUpDown_attackID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_AttackName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_AttackenTyp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_StatusAenderung;
        private System.Windows.Forms.Label label_statusTyp;
        private System.Windows.Forms.ComboBox comboBox_StatusTyp;
        private System.Windows.Forms.Label label_statusChance;
        private System.Windows.Forms.Label label_statusChancePrefix;
        private System.Windows.Forms.NumericUpDown numericUpDown_StatusChance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown_AttackenStaerke;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kartearcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attackearaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemariToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem öffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speichernToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speichernUnterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inDatenbankEintragenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prüfenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fensterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolstrip_changeToACA_Editor;
        private System.Windows.Forms.ToolStripMenuItem optionenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem datenbankpfadAuswählenToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numUpDown_Range;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTxt_beschreibung;
        private System.Windows.Forms.ToolStripMenuItem informationenToolStripMenuItem;
        private System.Windows.Forms.Label label_EffektStaerke;
        private System.Windows.Forms.NumericUpDown numUpDown_EffektStaerke;
    }
}