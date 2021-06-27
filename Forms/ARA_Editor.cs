using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ARF_Editor;
using ARF_Editor.Tools;
using ARF_Editor.ARFCore.Attacken;
using ARF_Editor.ARFCore;
using ARF_Editor.ARFCore.Karten;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace ARF_Editor.Forms
{
    public partial class ARA_Editor : Form
    {
        /// <summary>
        /// Die ID für die aktuelle Windows Form
        /// </summary>
        public byte ID = 0;
        /// <summary>
        /// Setzt die aktuelle ID
        /// </summary>
        /// <param name="ID"></param>
        public void setID(byte ID) => this.ID = ID;

        /// <summary>
        /// Der Filestream für die aktuell geöffnete Attacke
        /// </summary>
        private FileStream attackStream;
        /// <summary>
        /// Die aktuell geöffnete Attacke
        /// </summary>
        private Attacke attack;

        /// <summary>
        /// Alle Attacken aus der Datenbank
        /// </summary>
        private (ushort, ushort, string, string)[] attacks;

        /// <summary>
        /// Die Argumente mit der das Programm gestartet wurde
        /// </summary>
        private string[] FileRunArgs;
        public ARA_Editor(string[] args)
        {
            InitializeComponent();
            this.FileRunArgs = args;
        }
        private void ARA_Editor_Load(object sender, EventArgs e)
        {
            this.checkBox_StatusAenderung_CheckedChanged(this.checkBox_StatusAenderung, null);
            this.comboBox_AttackenTyp.SelectedIndex = 0;
            this.comboBox_StatusTyp.SelectedIndex = 0;

            if(Database.connectionHergestellt)
            {
                attacks = Database.GetAttacks();
            }

            if (this.FileRunArgs.Length > 0 && File.Exists(this.FileRunArgs[0]))
            // Öffnet Datei
            OpenAttack(this.FileRunArgs[0]);


            if (attack == null)
                PrepareNewAttack();
        }


        /// <summary>
        /// Zeigt das Öffnen Zeugs an
        /// </summary>
        private void ShowOpenDialog()
        {
            openFileDialog.Filter = openFileDialog.Filter = "AnimeRoyale-Dateien|*.arc;*.ara;*.ari|AnimeRoyale-Karte|*.arc|AnimeRoyale-Attacke|*.ara|AnimeRoyale-Item|*.ari";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            string path = openFileDialog.InitialDirectory + openFileDialog.FileName;
            if (attackStream != null)
            {
                attackStream.Close();
                attackStream = null;
            }
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(11, SeekOrigin.Begin);
            byte typeFlag = (byte)fs.ReadByte();
            fs.Close();

            if (typeFlag == Typen.KARTE)
            {
                Worker._Worker.startForm("ARC", new string[] { path });
                this.Close();
            }
            else if (typeFlag == Typen.ATTACKE)
                OpenAttack(path);
            else
                MessageBox.Show("Der Datei konnte kein passender Typ zugewiesen werden");
        }

        #region Open
        private void OpenAttack(string path)
        {
            if (attackStream != null)
            {
                attackStream.Dispose();
                attackStream.Close();
                attackStream = null;
            }
            if (!File.Exists(path))
            {
                MessageBox.Show(path + " existiert nicht", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            OpenAttack(new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read));
        }

        private void OpenAttack(FileStream fs)
        {
            attackStream = fs;
            this.attack = new Attacke(attackStream);
            if (!this.attack.ValidChecksum)
            {
                DialogResult msg = MessageBox.Show("Die Checksum von der Attacke ist ungültig! Soll trotzdem fortgefahren werden? (Es können schwerwiegende Fehler auftreten)", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (msg == DialogResult.No || msg == DialogResult.Cancel)
                    return;
            }

            UpdateFormInfos();
        }
        #endregion
        #region UpdateInfos
        private void UpdateFormInfos()
        {
            this.numUpDown_attackID.Value = attack.ID;
            this.txt_AttackName.Text = attack.Name;
            this.richtTxt_AttackText.Text = attack.AnzeigeText;
            this.comboBox_AttackenTyp.SelectedIndex = attack.Typ;
            this.numericUpDown_AttackenStaerke.Value = attack.Stärke;

            SetStatusAenderungCheckState(attack.Effekt != 0x00);
            if (attack.Effekt != 0x00)
            {
                this.comboBox_StatusTyp.SelectedIndex = attack.Effekt;
                this.numericUpDown_StatusChance.Value = attack.Chance;
            }
            else
            {
                this.comboBox_StatusTyp.SelectedIndex = 0;
                this.numericUpDown_StatusChance.Value = 1;
            }

            this.numericUpDown_PK.Value = attack.PK;
        }
        /// <summary>
        /// Aktuallisiert die Infos der offenen Attacke
        /// </summary>
        public void UpdateAttackenInfos()
        {
            attack.ID = (ushort)this.numUpDown_attackID.Value;
            attack.Name = this.txt_AttackName.Text;
            attack.AnzeigeText = this.richtTxt_AttackText.Text;
            attack.Typ = (byte)this.comboBox_AttackenTyp.SelectedIndex;
            attack.Stärke = (byte)this.numericUpDown_AttackenStaerke.Value;
            attack.Effekt = this.checkBox_StatusAenderung.Checked ? (byte)this.comboBox_StatusTyp.SelectedIndex : (byte)0x00;
            attack.Chance = (byte)this.numericUpDown_StatusChance.Value;
        }
        #endregion

        /// <summary>
        /// Bereitet eine neue, leere Karte für den Editor vor
        /// </summary>
        private void PrepareNewAttack()
        {
            attack = new Attacke();
            attackStream = null;

            // Wenn eine Verbindung zur Datenbank hergesetllt wurde, wird der höchste Index von der Attacke +1 als ID genommen, ansonsten einfach 1
            attack.ID = Database.connectionHergestellt ? Convert.ToUInt16(attacks.Select(x => x.Item2).ToArray().Max() + 1) : (ushort)0x01;

            // Updatet die Form Infos von der Karte
            UpdateFormInfos();
        }

        /// <summary>
        /// Speichert die Karteninfos in der momentanen Form
        /// </summary>
        private void SaveCurrentFormAttack()
        {
            // Wenn der Filestream noch nicht gesetzt wurde
            if (!attack.fsSet)
            {
                // * Neuen 'Speichern unter' Dialog anzeigen *

                this.saveFileDialog.Filter = "AnimeRoyale-Attacke|*.ara";

                // Wenn nicht abgebrochen wurde
                if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Wenn Variable attackStream gesetzt wurde, attackStream schließen
                    if (attackStream != null)
                    {
                        attackStream.Close();
                        attackStream = null;
                    }

                    // Neuen attackStream anfordern
                    attackStream = new FileStream(saveFileDialog.InitialDirectory + saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

                    // Karteninfos updaten
                    UpdateAttackenInfos();
                    // Filestream setzten
                    attack.setFS(attackStream);
                    attack.Save();


                    MessageBox.Show("Erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Form infos updaten
                    UpdateFormInfos();
                }
            }
            // Wenn filestream gesetzt wurde
            else
            {
                // Karteninfos Updaten
                UpdateAttackenInfos();
                // Karte speichern
                attack.Save();

                MessageBox.Show("Erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Forminfos updaten
                UpdateFormInfos();
            }
        }

        #region textbox Filter
        private void txt_AttackenName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar)) && !StringCode.CanEncode(e.KeyChar))
                e.Handled = true;
        }

        private void richtTxt_AttackenBeschreibung_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar)) && !StringCode.CanEncode(e.KeyChar))
                e.Handled = true;
        }
        #endregion textbox Filter

        #region CheckBox
        void SetStatusAenderungCheckState(bool state)
        {
            checkBox_StatusAenderung.Checked = state;
            checkBox_StatusAenderung_CheckedChanged(this.checkBox_StatusAenderung, null);
        }

        private void checkBox_StatusAenderung_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox me = (sender as CheckBox);
            this.comboBox_StatusTyp.Enabled = this.label_statusTyp.Enabled =
                this.label_statusChancePrefix.Enabled = this.numericUpDown_StatusChance.Enabled = this.label_statusChance.Enabled = me.Checked;
        }
        #endregion

        #region toolStrip
        private void öffnenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ShowOpenDialog();
        }
        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentFormAttack();

        }
        private void speichernUnterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.Filter = "AnimeRoyale-Attacke|*.ara";
            this.saveFileDialog.FileName = "";

            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (attackStream != null)
                    attackStream.Close();

                attackStream = new FileStream(saveFileDialog.InitialDirectory + saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                UpdateAttackenInfos();
                attack.setFS(attackStream);
                // Speichern
                attack.Save();
                MessageBox.Show("Erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateFormInfos();
            }
        }


        private void inDatenbankEintragenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Wenn keine Verbindung hergestellt wurde
            if (!Database.connectionHergestellt)
            {
                MessageBox.Show("Es konnte keine Verbindung zur Datenbank hergestellt werden", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Es wird vorher gespeichert. Fortfahren?", "Speichern bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            // Speichert Karte
            SaveCurrentFormAttack();

            // Wenn die Karte nicht gültig ist
            if (!AttackChecker.Valid(attack))
            {
                MessageBox.Show("Die Attacke konnte nicht in die Datenbank eingetragen werden:\n" + AttackChecker.InvalidErrors(attack), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Liest Daten aus wo die PK der PK von der aktuellen Karte entspricht
            var reader = Database.Read($"SELECT * FROM attacks WHERE pk={attack.PK}");


            string attackDescriptor = @$"{attack.Name}
                ID: {attack.ID}
                
                {this.comboBox_AttackenTyp.SelectedItem}
            ".Replace("  ", "");
            
            if (attack.Typ != 0x04)
                attackDescriptor += @$"Stärke: {attack.Stärke}
                ".Replace("  ", "");
            if (this.checkBox_StatusAenderung.Checked)
                attackDescriptor += @$"{this.comboBox_StatusTyp.SelectedItem}
                    Chance: 1/{attack.Chance}
                ".Replace("  ", "");
                
            // Wenn noch kein Eintrag mit der PK vorliegt
            if (!reader.HasRows)
            {
                // * Eintrag hinzufügen *
                Database.Write($"INSERT INTO attacks (pk, ID, name, descriptor) VALUES({attacks.Length + 1}, {attack.ID}, '{attack.Name}', '{attackDescriptor}')");
                attack.PK = Convert.ToUInt16(attacks.Length + 1);
                MessageBox.Show($"Die Attacke wurde erfolgreich in die Datenbank eingetragen (PK {attack.PK})");
            }
            else
            {
                // * Variablen ändern *
                reader.Read();
                if (reader["name"].ToString() != attack.Name)
                    Database.Write($"UPDATE attacks SET name='{attack.Name}' WHERE pk={attack.PK}");
                if (Convert.ToInt16(reader["ID"]) != attack.ID)
                    Database.Write($"UPDATE attacks SET ID={attack.ID} WHERE pk={attack.PK}");
                if (Convert.ToString(reader["descriptor"]) != attackDescriptor)
                    Database.Write($"UPDATE attacks SET descriptor='{attackDescriptor}' WHERE pk={attack.PK}");
                MessageBox.Show($"Die Daten wurden erfolgreich aktualisiert");
            }
            attacks = Database.GetAttacks();
        }

        private void newARAFile(object sender, EventArgs e)
        {
            PrepareNewAttack();
        }
        private void newARCFile(object sender, EventArgs e)
        {
            Worker._Worker.startForm("ARC");
            this.Close();
        }
        #region windows
        private void changeToARC_Editor(object sender, EventArgs e)
        {
            if (Worker._Worker.howMany("ARC") > 0)
                if (MessageBox.Show("Es ist bereits ein Karten-Editor offen. Wenn ein weiteres Fenster gestartet wird, wird es sehr warscheinlich Probleme mit den Resourcen geben. Trotzdem fortfahren?", "Warnung", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;

            if (attackStream != null && MessageBox.Show("Alle ungespeicherten Änderung werden verworfen, fortfahren?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            Worker._Worker.startForm("ARC");
            this.Close();
        }
        private void changeToARI_Editor(object sender, EventArgs e)
        {
            MessageBox.Show("Der ARI Editor ist noch in Arbeit");
        }

        private void newARA_Editor(object sender, EventArgs e)
        {
            Worker._Worker.startForm("ARA");
        }
        private void newARC_Editor(object sender, EventArgs e)
        {
            Worker._Worker.startForm("ARC");
        }
        #endregion

        private void prüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateAttackenInfos();
            if (AttackChecker.Valid(attack))
                MessageBox.Show("Die Attacke ist gültig", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(AttackChecker.InvalidErrors(attack), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion
        private void comboBox_AttackenTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox_AttackenTyp.SelectedIndex == 4)
            {
                this.numericUpDown_AttackenStaerke.Enabled = false;
                this.numericUpDown_AttackenStaerke.Value = 0;
                SetStatusAenderungCheckState(true);
                this.checkBox_StatusAenderung.AutoCheck = false;
            }
            else
            {
                this.numericUpDown_AttackenStaerke.Enabled = true;
                this.numericUpDown_AttackenStaerke.Value = 1;
                SetStatusAenderungCheckState(false);
                this.checkBox_StatusAenderung.AutoCheck = true;
            }
        }

    }
}
