using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ARF_Editor;
using ARF_Editor.Tools;
using ARF_Editor.ARFCore;
using ARF_Editor.ARFCore.Karten;
using System.Threading;

namespace ARF_Editor.Forms
{
    public partial class ARC_Editor : Form
    {
        /// <summary>
        /// Die momentane Forminstanzen ID
        /// </summary>
        public byte ID = 0;
        /// <summary>
        /// Seztt die ID zu einem Wert
        /// </summary>
        /// <param name="ID">Die momentane ID</param>
        public void setID(byte ID) => this.ID = ID;

        /// <summary>
        /// Die momentan geöffnete Karte
        /// </summary>
        private Karte card;
        /// <summary>
        /// Der FileStream für die Karte
        /// </summary>
        private FileStream cardStream;
        /// <summary>
        /// Die Einstellungen
        /// </summary>
        private INISettings settings;
        /// <summary>
        /// Die Parameter mit der das Programm gestartet wurde
        /// </summary>
        private string[] FileRunArgs;


        /// <summary>
        /// Alle Karten aus der Datenbank
        /// </summary>
        private (ushort, ushort, string, string)[] cards;
        /// <summary>
        /// Alle Attacken aus der Datenbank
        /// </summary>
        private (ushort, ushort, string, string)[] attacks;

        public ARC_Editor(string[] args)
        {
            InitializeComponent();
            // Setzt die Startargumente, damit auf sie von außerhalb dieser Funktion zugegriffen werden kann
            this.FileRunArgs = args;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            #region settings
            // Directory für App Settings, etc.
            if (!Directory.Exists(Environment.GetEnvironmentVariable("appdata") + @"\ARF-Editor"))
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("appdata") + @"\ARF-Editor");
            // Setzt die Einstellungen
            settings = new INISettings();

            if (File.Exists(settings.objects["Database"]["path"]))
                Database.Settings.dbPath = settings.objects["Database"]["path"];
            #endregion
            #region DatabaseSetup
            try
            {   
                // Überprüft ob der Pfad in den Einstellungen gültig ist
                if (Database.Settings.dbPath == "" || !File.Exists(Database.Settings.dbPath))
                    throw new System.Data.SQLite.SQLiteException();
                // Probiert sich mit der Datenbank zu verbinden
                Database.Setup();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                // Wenn der angegebene Pfad für die Datenbank nicht existiert
                if (!File.Exists(Database.Settings.dbPath))
                {
                    // Fragt den User ob er die Datenbank manuell lokalisierten will
                    if (MessageBox.Show("Die Datenbank konnte nicht gefunden werden. Möchtest du sie lokalisieren?", "Datenbank Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        // Wenn ja, wird der Pfad zum Ergebnis von dem FileDialog gesetzt
                        string pfad = OpenDatabase();

                        // Wenn abgebrochen wurde (der Pfad ist null)
                        if (pfad == null)
                            return;
                        settings.Set("Database", "path", pfad);
                        MessageBox.Show("Der Pfad wurde erfolgreich aktuallisiert!");
                    }
                }
            }
            #endregion

            // Setzt die Farbe für den Statsumchecker
            this.txt_StatsSum.BackColor = Color.FromArgb(0, 255, 145);

            // Wenn eine Verbindung mit der Datenbank hergestellt wurde
            if (Database.connectionHergestellt)
            {
                // Liest alle Karten aus der Datenbank
                cards = Database.GetCards();
                // Liest alle Attacken aus der Datenbank
                attacks = Database.GetAttacks();

                // leert alle Dropdownitems, damit falls sie schon gesetzt sind, nicht nochmal die Daten hinten angehangen werden 
                ResetDropItems();
                // setzt alle Dropdownitems
                SetDropItems();
            }
            // Wenn Datei mit Programm geöffnet wurde
            if (this.FileRunArgs.Length > 0 && File.Exists(this.FileRunArgs[0]))
                // Öffnet Datei
                OpenFile(this.FileRunArgs[0]);

            // Wenn die Karte null ist
            if (card == null)
                // Wird eine leere Karte vorbereitet
                PrepareNewCard();
        }

        #region DropItems
        /// <summary>
        /// Leert alle dropbox Items
        /// </summary>
        private void ResetDropItems()
        {
            this.comboBox_boostCards1.Items.Clear();
            this.comboBox_boostCards2.Items.Clear();
            this.comboBox_boostCards3.Items.Clear();
            this.comboBox_boostCards4.Items.Clear();
            this.comboBox_boostCards5.Items.Clear();

            this.comboBox_attack1.Items.Clear();
            this.comboBox_attack2.Items.Clear();
        }
        /// <summary>
        /// Setzt alle dropbox items
        /// </summary>
        private void SetDropItems()
        {
            if (!Database.connectionHergestellt)
                return;
            List<ComboBoxItem> _items = new List<ComboBoxItem>();
            _items.Add(new ComboBoxItem("(keine)", 0x00));
            _items.AddRange(cards.Select(x => new ComboBoxItem(x.Item3, x.Item2)));

            #region unsafe
            // * Unsicher weil das würde die BoostIDs um 1 verschieben *
            // Wenn eine aktuelle Karte da ist, dann wird von Boost Karten gefiltert, ansonsten einfach alle
            //_items.AddRange( card != null ? 
            //    cards.Where(x => x.Item1 != card.PK).Select(x => new ComboBoxItem(x.Item3, x.Item2)) :
            //    cards.Select(x => new ComboBoxItem(x.Item3, x.Item2)) );
            #endregion
            object[] items = _items.ToArray();


            this.comboBox_boostCards1.Items.AddRange(items);
            this.comboBox_boostCards2.Items.AddRange(items);
            this.comboBox_boostCards3.Items.AddRange(items);
            this.comboBox_boostCards4.Items.AddRange(items);
            this.comboBox_boostCards5.Items.AddRange(items);

            this.comboBox_boostCards1.SelectedIndex = this.comboBox_boostCards2.SelectedIndex = this.comboBox_boostCards3.SelectedIndex = this.comboBox_boostCards4.SelectedIndex = this.comboBox_boostCards5.SelectedIndex = 0;


            this.comboBox_attack1.Items.Add(new ComboBoxItem("(keine)", 0x00));
            this.comboBox_attack1.Items.AddRange(attacks.Select(x => new ComboBoxItem(x.Item3, x.Item2)).ToArray());
            this.comboBox_attack2.Items.Add(new ComboBoxItem("(keine)", 0x00));
            this.comboBox_attack2.Items.AddRange(attacks.Select(x => new ComboBoxItem(x.Item3, x.Item2)).ToArray());

            if(attacks.Length > 0)
                this.comboBox_attack1.SelectedIndex = 1;
            this.comboBox_attack2.SelectedIndex = 0;

        }
        #endregion


        #region Tools
        #region Update
        /// <summary>
        /// Aktuallisiert die Karteninfos in der Form
        /// </summary>
        private void UpdateFormInfos()
        {
            #region cardDetails
            this.numUpDown_cardID.Value = card.ID;
            this.txt_cardName.Text = card.Name;
            this.richtTxt_CardBeschreibung.Text = card.Beschreibung;
            this.txt_cardHerkunft.Text = card.Herkunft;
            this.comboBox_cardSeltenheit.SelectedItem = this.comboBox_cardSeltenheit.Items[card.Seltenheit];
            this.comboBox_cardGeschlecht.SelectedItem = this.comboBox_cardGeschlecht.Items[card.Geschlecht];
            this.numericUpDown_PK.Value = card.PK;

            #region boostCards
            if(Database.connectionHergestellt)
                for (int i = 0; i < cards.Length; i++)
                {
                    if (Convert.ToUInt16((this.comboBox_boostCards1.Items[i] as ComboBoxItem).Value) == card.ZusammenSpiel[0])
                        this.comboBox_boostCards1.SelectedItem = this.comboBox_boostCards1.Items[i];

                    if (Convert.ToUInt16((this.comboBox_boostCards2.Items[i] as ComboBoxItem).Value) == card.ZusammenSpiel[1])
                        this.comboBox_boostCards2.SelectedItem = this.comboBox_boostCards2.Items[i];

                    if (Convert.ToUInt16((this.comboBox_boostCards3.Items[i] as ComboBoxItem).Value) == card.ZusammenSpiel[2])
                        this.comboBox_boostCards3.SelectedItem = this.comboBox_boostCards3.Items[i];

                    if (Convert.ToUInt16((this.comboBox_boostCards4.Items[i] as ComboBoxItem).Value) == card.ZusammenSpiel[3])
                        this.comboBox_boostCards4.SelectedItem = this.comboBox_boostCards4.Items[i];

                    if (Convert.ToUInt16((this.comboBox_boostCards5.Items[i] as ComboBoxItem).Value) == card.ZusammenSpiel[4])
                        this.comboBox_boostCards5.SelectedItem = this.comboBox_boostCards5.Items[i];
                }
            else
            {
                this.comboBox_boostCards1.SelectedValue = card.ZusammenSpiel[0];
                this.comboBox_boostCards2.SelectedValue = card.ZusammenSpiel[1];
                this.comboBox_boostCards3.SelectedValue = card.ZusammenSpiel[2];
                this.comboBox_boostCards4.SelectedValue = card.ZusammenSpiel[3];
                this.comboBox_boostCards5.SelectedValue = card.ZusammenSpiel[4];

            }
            #endregion
            #endregion
            #region stats
            this.numericUpDown_Level.Value = card.Level;
            this.numericUpDown_Att.Value = card.Angriff;
            this.numericUpDown_Def.Value = card.Verteidigung;
            this.numericUpDown_Spe.Value = card.Schnelligkeit;
            this.numericUpDown_LP.Value = card.LP;
            // Für den Statssum checker
            this.txt_StatsSum.Text = $"{card.StatsSum}/{card.MaxStatsSum}";
            #endregion
            #region Attacks
            // Wenn Verbindung mit Datenbank hergestellt wurde
            if (Database.connectionHergestellt)
                // Für [i] in attacken
                for (int i = 0; i < attacks.Length; i++)
                {
                    // * Checkt ob das Comboboxitem (die ID von der Attacke) der AttackenID von der Karte entspricht, wenn ja wird der index ausgewählt *

                    if (Convert.ToUInt16((this.comboBox_attack1.Items[i] as ComboBoxItem).Value) == card.AttackenID_1)
                        this.comboBox_attack1.SelectedItem = this.comboBox_attack1.Items[i];

                    if (Convert.ToUInt16((this.comboBox_attack2.Items[i] as ComboBoxItem).Value) == card.AttackenID_2)
                        this.comboBox_attack2.SelectedItem = this.comboBox_attack2.Items[i];
                }
            else
            {
                // * Ansonsten wird einfach als ausgwähltes Item einfach die ID genommen *

                this.comboBox_attack1.SelectedValue = card.AttackenID_1.ToString();
                this.comboBox_attack2.SelectedValue = card.AttackenID_2.ToString();
            }
            #endregion
        }
        /// <summary>
        /// AKtuallisiert die Karteninfos und weist die Infos von den Form Daten der Karte zu
        /// </summary>
        private void UpdateKartenInfos()
        {
            #region Details
            card.ID = (ushort)this.numUpDown_cardID.Value;
            card.Name = this.txt_cardName.Text;
            card.Beschreibung = this.richtTxt_CardBeschreibung.Text;
            card.Herkunft = this.txt_cardHerkunft.Text;
            card.Seltenheit = (byte)this.comboBox_cardSeltenheit.SelectedIndex;
            card.Geschlecht = (byte)this.comboBox_cardGeschlecht.SelectedIndex;

            card.ZusammenSpiel = card.ZusammenSpiel.Set(0, Convert.ToUInt16((this.comboBox_boostCards1.SelectedItem as ComboBoxItem).Value));
            card.ZusammenSpiel = card.ZusammenSpiel.Set(1, Convert.ToUInt16((this.comboBox_boostCards2.SelectedItem as ComboBoxItem).Value));
            card.ZusammenSpiel = card.ZusammenSpiel.Set(2, Convert.ToUInt16((this.comboBox_boostCards3.SelectedItem as ComboBoxItem).Value));
            card.ZusammenSpiel = card.ZusammenSpiel.Set(3, Convert.ToUInt16((this.comboBox_boostCards4.SelectedItem as ComboBoxItem).Value));
            card.ZusammenSpiel = card.ZusammenSpiel.Set(4, Convert.ToUInt16((this.comboBox_boostCards5.SelectedItem as ComboBoxItem).Value));
            #endregion
            #region stats
            card.Level = (byte)this.numericUpDown_Level.Value;
            card.Angriff = (byte)this.numericUpDown_Att.Value;
            card.Verteidigung = (byte)this.numericUpDown_Def.Value;
            card.Schnelligkeit = (byte)this.numericUpDown_Spe.Value;
            card.LP = (ushort)this.numericUpDown_LP.Value;
            #endregion
            #region attacks
            card.AttackenID_1 = Convert.ToUInt16((this.comboBox_attack1.SelectedItem as ComboBoxItem).Value);
            card.AttackenID_2 = Convert.ToUInt16((this.comboBox_attack2.SelectedItem as ComboBoxItem).Value);
            #endregion
        }
        #endregion Update
        /// <summary>
        /// Bereitet eine neue, leere Karte für den Editor vor
        /// </summary>
        private void PrepareNewCard()
        {
            card = new Karte();
            cardStream = null;

            // Wenn eine Verbindung zur Datenbank hergesetllt wurde, wird der höchste Index von der Karte +1 als ID genommen, ansonsten einfach 1
            card.ID = Database.connectionHergestellt ? Convert.ToUInt16(cards.Select(x => x.Item2).ToArray().Max() + 1) : (ushort)0x01;

            // * Automatische Auswahl vom 0ten Objekt *
            this.comboBox_cardGeschlecht.SelectedIndex = 0;
            this.comboBox_cardSeltenheit.SelectedIndex = 0;

            // Updatet die Form Infos von der Karte
            UpdateFormInfos();
        }

        #region OpenFileDialog
        /// <summary>
        /// Zeigt den openFileDialog für die Datenbank an
        /// </summary>
        /// <returns>Der ausgewählte Pfad, wenn nicht ok gedrückt wurde <c>null</c></returns>
        private string OpenDatabase()
        {
            openFileDialog.Filter = "Datenbank|index.db";
            openFileDialog.FileName = "index.db";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Database.Settings.dbPath = openFileDialog.InitialDirectory + openFileDialog.FileName;
                return openFileDialog.InitialDirectory + openFileDialog.FileName;
            }
            return null;
        }


        private void OpenCard(FileStream fs)
        {
            cardStream = fs;
            this.card = new Karte(cardStream);

            if (!CardChecker.ValidHeader(card))
            {
                MessageBox.Show("Die Datei scheint keine AnimeRoyale Karte zu sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!CardChecker.ValidDetailsChecksum(card))
            {
                if (MessageBox.Show($"Die Kartendetails scheinen korrupt zu sein. Fortfahren?\n{string.Join(", ", card.CalculateChecksum(card.DetailsBlock))}\n{string.Join(", ", card.DetailsBlockChecksum)}", "Warnung", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;
            }
            UpdateFormInfos();
        }
        
        /// <summary>
        /// Zeigt das Öffnen Zeugs an
        /// </summary>
        private void ShowOpenDialog()
        {
            openFileDialog.Filter = "AnimeRoyale-Dateien|*.arc;*.ara;*.ari|AnimeRoyale-Karte|*.arc|AnimeRoyale-Attacke|*.ara|AnimeRoyale-Item|*.ari";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            string path = openFileDialog.InitialDirectory + openFileDialog.FileName;
            if (cardStream != null)
            {
                cardStream.Close();
                cardStream = null;
            }

            OpenFile(path);
        }
        private void OpenFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(11, SeekOrigin.Begin);
            byte typeFlag = (byte)fs.ReadByte();
            fs.Close();

            if (typeFlag == Typen.KARTE)
                OpenCard(path);
            else if (typeFlag == Typen.ATTACKE)
            {
                Worker._Worker.startForm("ARA", new string[] { path });
                this.Close();
            }
            else
                MessageBox.Show("Der Datei konnte kein passender Typ zugewiesen werden");
        }

        /// <summary>
        /// Öffnet eine Karte in den Editor
        /// </summary>
        /// <param name="path">Der Pfad von dem die Karte geöffnet werden soll</param>
        private void OpenCard(string path)
        {
            if (cardStream != null)
            {
                cardStream.Dispose();
                cardStream.Close();
                cardStream = null;
            }
            if(!File.Exists(path))
            {
                MessageBox.Show(path, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            OpenCard(new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read));
        }
        #endregion

        #region SaveFileDialog
        /// <summary>
        /// Speichert die Karteninfos in der momentanen Form
        /// </summary>
        private void SaveCurrentFormCard()
        {
            // Wenn der Filestream noch nicht gesetzt wurde
            if (!card.fsSet)
            {
                // * Neuen 'Speichern unter' Dialog anzeigen *

                this.saveFileDialog.Filter = "AnimeRoyale-Karte|*.arc";
                
                // Wenn nicht abgebrochen wurde
                if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Wenn Variable cardStream gesetzt wurde, cardstream schließen
                    if (cardStream != null)
                    {
                        cardStream.Close();
                        cardStream = null;
                    }

                    // Neuen Cardstream anfordern
                    cardStream = new FileStream(saveFileDialog.InitialDirectory + saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

                    // Karteninfos updaten
                    UpdateKartenInfos();
                    // Filestream setzten
                    card.setFS(cardStream);
                    card.Save();

                    
                    MessageBox.Show("Erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Form infos updaten
                    UpdateFormInfos();
                }
            }
            // Wenn filestream gesetzt wurde
            else
            {
                // Karteninfos Updaten
                UpdateKartenInfos();
                // Karte speichern
                card.Save();

                MessageBox.Show("Erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Forminfos updaten
                UpdateFormInfos();
            }
        }
        #endregion SaveFileDialog
        #endregion

        #region FormKartenInfos Update
        private void txt_cardName_TextChanged(object sender, EventArgs e)
        {
            TextBox me = (sender as TextBox);
            if (me.Text.Length >= 32)
            {
                me.Text = me.Text[..^( me.Text.Length - 32 + 1 )];
                me.SelectionStart = me.Text.Length;
                me.SelectionLength = 0;
                return;
            }
        }

        private void richtTxt_CardBeschreibung_TextChanged(object sender, EventArgs e)
        {
            RichTextBox me = (sender as RichTextBox);
            if (me.Text.Length >= 256)
            {
                me.Text = me.Text[..^(me.Text.Length - 256 + 1)];
                me.SelectionStart = me.Text.Length;
                me.SelectionLength = 0;
                return;
            }

        }

        private void txt_cardHerkunft_TextChanged(object sender, EventArgs e)
        {
            TextBox me = (sender as TextBox);
            if (me.Text.Length >= 32)
            {
                me.Text = me.Text[..^(me.Text.Length - 32 + 1)];
                me.SelectionStart = me.Text.Length;
                me.SelectionLength = 0;
                return;
            }
        }

        private void numericUpDown_Att_ValueChanged(object sender, EventArgs e)
        {
            card.Angriff = (byte)(sender as NumericUpDown).Value;
            this.txt_StatsSum.Text = $"{card.StatsSum}/{card.MaxStatsSum}";
        }

        private void numericUpDown_Def_ValueChanged(object sender, EventArgs e)
        {
            card.Verteidigung = (byte)(sender as NumericUpDown).Value;
            this.txt_StatsSum.Text = $"{card.StatsSum}/{card.MaxStatsSum}";
        }

        private void numericUpDown_Spe_ValueChanged(object sender, EventArgs e)
        {
            card.Schnelligkeit = (byte)(sender as NumericUpDown).Value;
            this.txt_StatsSum.Text = $"{card.StatsSum}/{card.MaxStatsSum}";
        }

        private void numericUpDown_LP_ValueChanged(object sender, EventArgs e)
        {
            card.LP = (ushort)(sender as NumericUpDown).Value;
            this.txt_StatsSum.Text = $"{card.StatsSum}/{card.MaxStatsSum}";
        }

        private void numericUpDown_Level_ValueChanged(object sender, EventArgs e)
        {
            card.Level = (byte)(sender as NumericUpDown).Value;
            this.txt_StatsSum.Text = $"{card.StatsSum}/{card.MaxStatsSum}";
        }

        private void txt_StatsSum_TextChanged(object sender, EventArgs e)
        {
            if (card.StatsSum > card.MaxStatsSum)
                (sender as TextBox).BackColor = Color.Red;
            else
                (sender as TextBox).BackColor = (sender as TextBox).BackColor = Color.FromArgb(0, 255, 145);
        }
        #endregion

        #region textbox Filter
        private void txt_cardName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar)) && !StringCode.CanEncode(e.KeyChar))
                e.Handled = true;
        }

        private void richtTxt_CardBeschreibung_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar)) && !StringCode.CanEncode(e.KeyChar))
                e.Handled = true;
        }

        private void txt_cardHerkunft_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar)) && !StringCode.CanEncode(e.KeyChar))
                e.Handled = true;
        }
        #endregion textbox Filter

        #region Click
        #region Zeugs
        /// <summary>
        /// Wenn eine neue Kartendatei gedrückt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kartearcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrepareNewCard();
        }
        /// <summary>
        /// Wenn Karte in die Datenbank eingetragen werden soll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inDatenbankEintragenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Wenn keine Verbindung hergestellt wurde
            if(!Database.connectionHergestellt)
            {
                MessageBox.Show("Es konnte keine Verbindung zur Datenbank hergestellt werden", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Es wird vorher gespeichert. Fortfahren?", "Speichern bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            // Speichert Karte
            SaveCurrentFormCard();

            // Wenn die Karte nicht gültig ist
            if (!CardChecker.Valid(card))
            {
                MessageBox.Show("Die Karte konnte nicht in die Datenbank eingetragen werden:\n" + CardChecker.InvalidErrors(card), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Liest Daten aus wo die PK der PK von der aktuellen Karte entspricht
            var reader = Database.Read($"SELECT * FROM cards WHERE pk={card.PK}");


            string cardDescriptor = @$"{card.Name}
                {this.comboBox_cardSeltenheit.SelectedItem}
                ID: {card.ID}

                Lvl: {card.Level}
                Att: {card.Angriff}
                Def: {card.Verteidigung}
                Spe: {card.Schnelligkeit}
                LP: {card.LP}
               
                [Attacken]
                {this.comboBox_attack1.SelectedItem}
                {this.comboBox_attack2.SelectedItem}
            ".Replace("  ", "");
            // Wenn noch kein Eintrag mit der PK vorliegt
            if (!reader.HasRows)
            {
                // * Eintrag hinzufügen *
                Database.Write($"INSERT INTO cards (pk, ID, name, descriptor) VALUES({cards.Length + 1}, {card.ID}, '{card.Name}', '{cardDescriptor}')");
                card.PK = Convert.ToUInt16(cards.Length + 1);
                MessageBox.Show($"Die Karte wurde erfolgreich in die Datenbank eingetragen (PK {card.PK})");
            }
            else
            {
                // * Variablen ändern *
                reader.Read();
                if (reader["name"].ToString() != card.Name)
                    Database.Write($"UPDATE cards SET name='{card.Name}' WHERE pk={card.PK}");
                if (Convert.ToInt16(reader["ID"]) != card.ID)
                    Database.Write($"UPDATE cards SET ID={card.ID} WHERE pk={card.PK}");
                if (Convert.ToString(reader["descriptor"]) != cardDescriptor)
                    Database.Write($"UPDATE cards SET descriptor='{cardDescriptor}' WHERE pk={card.PK}");
                MessageBox.Show($"Die Daten wurden erfolgreich aktualisiert");
            }
            cards = Database.GetCards();
            attacks = Database.GetAttacks();
            ResetDropItems();
            SetDropItems();
        }
        #endregion

        #region checks
        // ID check
        private void iDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (!CardChecker.ValidID(card))
                MessageBox.Show("Diese ID ist schon vorhanden", "Ungültige ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Die ID ist gültig", "ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Name check
        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (!CardChecker.ValidName(card))
                MessageBox.Show(card.Name == "" ? "Der Name darf nicht leer sein" : "Der Name ist schon vorhanden", "Ungültiger Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Die Name ist gültig", "Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Stats check
        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (!CardChecker.ValidStats(card))
                MessageBox.Show(CardChecker.InvalidStatsErrors(card), "Ungültige Stats", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Stats sind gültig", "Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Details check
        private void gültigeDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (CardChecker.ValidDetails(card))
                MessageBox.Show("Die Details sind alle gültig", "Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(CardChecker.InvalidDetailsErrors(card), "Details ungültig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Boosts check
        private void boostsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (CardChecker.ValidBoosts(card))
                MessageBox.Show("Die Boosts sind alle gültig", "Boosts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(CardChecker.InvalidBoostsErrors(card), "Details ungültig", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        // Attacken check
        private void attackenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (CardChecker.ValidAttacks(card))
                MessageBox.Show("Die Attacken sind alle gültig", "Boosts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(CardChecker.InvalidAttacksErrors(card), "Attacken ungültig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Karten check
        private void prüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateKartenInfos();
            if (CardChecker.Valid(card))
                MessageBox.Show("Karte ist gültig", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(CardChecker.InvalidErrors(card), "Ungültige Karte", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion
        #region öffnen/speichern
        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentFormCard();
        }
        
        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenDialog();   
        }
        private void datenbankpfadAuswählenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pfad = OpenDatabase();
            // Wenn abgebrochen wurde (der Pfad ist null)
            if (pfad == null)
                return;
            // Setzt den neuen Pfad
            settings.Set("Database", "path", pfad);
            MessageBox.Show("Der Datenbankpfad wurde aktualisiert");
            
            // Updatet PK für die Karet
            card.UpdatePK();
            // Updatet Forminfos
            UpdateFormInfos();
        }

        private void speichernUnterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.Filter = "AnimeRoyale-Karte|*.arc";
            this.saveFileDialog.FileName = "";

            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(cardStream != null)
                    cardStream.Close();

                cardStream = new FileStream(saveFileDialog.InitialDirectory + saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                UpdateKartenInfos();
                card.setFS(cardStream);
                // Speichern
                card.Save();
                MessageBox.Show("Erfolgreich gespeichert!", "Gespeichert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateFormInfos();
            }
        }
        #endregion
        #endregion

        // Fenster -> Neu
        private void aRANewEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Worker._Worker.howMany("ARA") > 0)
                if (MessageBox.Show("Es ist bereits ein Attacken-Editor offen. Wenn ein weiteres Fenster gestartet wird, wird es sehr warscheinlich Probleme mit den Resourcen geben. Trotzdem fortfahren?", "Warnung", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;

            Worker._Worker.startForm("ARA");
        }
        // Fenster -> Wechseln
        private void aRAWechselEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Worker._Worker.howMany("ARA") > 0)
                if (MessageBox.Show("Es ist bereits ein Attacken-Editor offen. Wenn ein weiteres Fenster gestartet wird, wird es sehr warscheinlich Probleme mit den Resourcen geben. Trotzdem fortfahren?", "Warnung", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;

            if (cardStream != null && MessageBox.Show("Alle ungespeicherten Änderung werden verworfen, fortfahren?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            Worker._Worker.startForm("ARA");
            this.Close();
        }

        // Neue Datei
        private void NewARAFile_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox_boostCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox me = sender as ComboBox;
            if (me.SelectedIndex == 0)
                this.toolTip_cardInfo.SetToolTip(me, "[leer]");
            else
            {
                string[] desc = cards.Where(x => x.Item2 == (ushort)(me.SelectedItem as ComboBoxItem).Value).Select(x => x.Item4).ToArray();
                if(desc.Length > 0)
                    this.toolTip_cardInfo.SetToolTip(me, desc[0]);
                else
                    this.toolTip_cardInfo.SetToolTip(me, "[leer]");
            }
        }

        private void comboBox_attack_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox me = sender as ComboBox;
            if (me.SelectedIndex == 0)
                this.toolTip_attackInfo.SetToolTip(me, "[leer]");
            else
            {
                string[] desc = attacks.Where(x => x.Item2 == (ushort)(me.SelectedItem as ComboBoxItem).Value).Select(x => x.Item4).ToArray();
                if (desc.Length > 0)
                    this.toolTip_attackInfo.SetToolTip(me, desc[0]);
                else
                    this.toolTip_attackInfo.SetToolTip(me, "[leer]");
            }
        }
    }
}
