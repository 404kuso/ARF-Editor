using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Linq;

using ARF_Editor.Tools;
using BitConverter = ARF_Editor.Tools.BitConverter;

namespace ARF_Editor.ARFCore.Karten
{
    class Karte
    {
        
        #region defined
        /// <summary>
        /// Wie groß die Checksum ist
        /// </summary>
        private const byte checksumLen = 2;
        /// <summary>
        /// Das Standart-Header für eine Karte
        /// </summary>
        public static readonly byte[] headerBytes = new byte[] { 0x2B, 0x52, 0x4D, 0x51, 0x49, 0x3C, 0x53, 0x5D, 0x45, 0x50, 0x49, 0x0F, 0xFF };

        /// <summary>
        /// Leere Details
        /// </summary>
        private static byte[] emptyDetails
        {
            get => Enumerable.Repeat((byte)0x00, 0x153).ToArray();
        }

        /// <summary>
        /// Leere Statuswerte
        /// </summary>
        private static byte[] emptyStats
        {
            get => Enumerable.Repeat((byte)0x00, 10).ToArray();
        }

        /// <summary>
        /// Leere Attacken
        /// </summary>
        private static byte[] emptyAttacks
        {
            get => Enumerable.Repeat((byte)0x00, 10).ToArray();
        }

        /// <summary>
        /// Leere durch Levelaufstieg erlernbare Attacken
        /// </summary>
        private static byte[] emptyErlernbareAttacken
        {
            get => Enumerable.Repeat((byte)0x00, 90).ToArray();
        }

        /// <summary>
        /// Leere Attacken die erlernbar sind
        /// </summary>
        public static byte[] emptyAttackenPerItem
        {
            get => Enumerable.Repeat((byte)0x00, 300).ToArray();
        }

        /// <summary>
        /// Der Inhalt für eine komplett leere Karte
        /// </summary>
        public static byte[] emptyCard
        {
            get => headerBytes
                // Details + FF FF FF
                .Concat(emptyDetails).ToArray().Concat(new byte[2] {0xFF, 0xFF}).ToArray()
                // Stats + FF FF
                .Concat(emptyStats).ToArray().Concat(new byte[2] { 0xFF, 0xFF }).ToArray()
                // Attacken + FF FF FF
                .Concat(emptyAttacks).ToArray().Concat(new byte[1] { 0xFF }).ToArray()
                // Erlernbare Attacken
                .Concat(emptyErlernbareAttacken).ToArray()
                // Itemattacken
                .Concat(emptyAttackenPerItem).ToArray();
        }
        #endregion



        #region newCard
        /// <summary>
        /// Erstellt eine leere neue Karte
        /// </summary>
        /// <value>
        /// <b>Details</b>
        /// <list type="bullet">
        ///     <item>ID <c>1</c></item>
        ///     <item>Name <c>""</c></item>
        ///     <item>Beschreibung:<c>""</c></item>
        ///     <item>Herkunft <c>""</c></item>
        ///     <item>Geschlecht <c>männlich</c></item>
        ///     <item>Seltenheit <c>gewöhnlich</c></item>
        ///     <item></item>
        /// </list>
        /// <br/>
        /// <b>Stats</b>
        /// <list type="bullet">
        ///     <item>Level <c>1</c></item>
        ///     <item>Angriff <c>15</c></item>
        ///     <item>Verteidigung <c>15</c></item>
        ///     <item>Schnelligkeit <c>15</c></item>
        ///     <item>LP <c>40</c></item>
        /// </list>
        /// <b>Attacken</b>
        /// <list type="number">
        ///     <item>Attacke <c>1</c></item>
        ///     <item>Attacke <c>0</c></item>
        ///     <item>Attacke <c>0</c></item>
        ///     <item>Attacke <c>0</c></item>
        /// </list>
        /// <i>Die restlichen Werte sind alle 0 (leer)</i>
        /// </value>
        public Karte()
        {
            this.fileContent = emptyCard;

            this.ID = 0x01;
            this.Name = "";
            this.Beschreibung = "";
            this.Herkunft = "";
            this.Geschlecht = 0x00;
            this.Seltenheit = 0x00;

            this.Level = 1;
            this.Angriff = 15;
            this.Verteidigung = 15;
            this.Schnelligkeit = 15;
            this.LP = 40;
            this.Fähigkeit = 0;

            this.AttackenID_1 = 1;
            this.AttackenID_2 = 0;
            this.AttackenID_3 = 0;
            this.AttackenID_4 = 0;

        }

        #region filestream
        /// <summary>
        /// Setzt den filestream der Karte
        /// </summary>
        /// <param name="fs">Der neue Filestream</param>
        public void setFS(FileStream fs) => this.fs = fs;
        
        /// <summary>
        /// Ob der filestream gesetzt wurde
        /// </summary>
        public bool fsSet
        {
            get => this.fs != null;
        }
        
        /// <summary>
        /// Der filestream fürs Lesen/Schreiben
        /// </summary>
        private FileStream fs;
        #endregion fileStream
        #endregion
        
        /// <summary>
        /// Der Datei-Inhalt für die Karte
        /// </summary>
        private byte[] fileContent;

        /// <summary>
        /// Der Primary-Key für die Datenbank
        /// </summary>
        public ushort PK = 0x00;

        /// <summary>
        /// Erstellt eine neue Karte abhängig von einem Filestream
        /// </summary>
        /// <param name="fs">Der Filestream, aus dem die Karteninfos rausgelesen werden sollen und in den auch geschrieben wird</param>
        public Karte(FileStream fs)
        {
            this.fs = fs;
            
            this.fileContent = new byte[0x2FD];
            this.fs.Read(this.fileContent);

            this.UpdatePK();
        }

        /// <summary>
        /// Aktuallisiert den PK Wert, der aus der Datenbank ausgelesen wird
        /// </summary>
        public void UpdatePK()
        {
            if (Database.connectionHergestellt)
            {
                var reader = Database.Read($"SELECT * FROM attacks WHERE ID={this.ID} AND name='{this.Name}'");
                if (reader.HasRows)
                {
                    reader.Read();
                    this.PK = Convert.ToUInt16(reader["pk"]);
                }
            }
        }

        /// <summary>
        /// Speichert die Karte in dem aktuellen Filestream
        /// </summary>
        public void Save()
        {
            // fixt checksums von den Blöcken
            this.FixSumDetailsBlock();
            this.FixSumStatsBlock();
            this.FixSumAttacksBlock();

            // Setzt die filestream position zum Anfang (0x00)
            this.fs.Seek(0x00, SeekOrigin.Begin);
            // Schreibt den Karten content in die Datei
            this.fs.Write(this.fileContent, 0, this.fileContent.Length);
            // Speichert
            this.fs.Flush();

            #region seeks
            //bw.Seek(0x00, SeekOrigin.Begin);
            //bw.Write(headerBytes);

            //bw.Seek(0x0D, SeekOrigin.Begin);
            //bw.Write(this.DetailsBlock);

            // Platzhalter (0xFF, 0xFF)
            //bw.Seek(0x15D, SeekOrigin.Begin);
            //bw.Write(new byte[] { 0xFF, 0xFF, 0xFF });

            //bw.Seek(0x16A, SeekOrigin.Begin);
            //bw.Write(this.StatsBlock);

            // Platzhalter (0xFF, 0xFF)
            //bw.Seek(0x168, SeekOrigin.Begin);
            //bw.Write(new byte[] { 0xFF, 0xFF });
            #endregion
        }

        /// <summary>
        /// Berechnet die Checksum für einen Block
        /// </summary>
        /// <param name="block">Der Block für den berechnet werden soll</param>
        /// <returns>Die berechnete Checksum</returns>
        public byte[] CalculateChecksum(byte[] block)
        {
            return BitConverter.GetBytes(Checksum.CRC16_CCITT(block, 0, block.Length - checksumLen));
        }

        #region Attributes
        #region Header
        /// <summary>
        /// Das Header der Datei, sollte normalerweise <c>2B 52 4D 51 49 3C 53 5D 45 50 49 0F FF</c> sein
        /// <br/>
        /// <b>Adresse</b>: [<c>0x00</c>] - [<c>0x0D</c>]
        /// </summary>
        public byte[] Header
        {
            get => fileContent[0x00..0x0D];
            set => fileContent.Set(0x00, value);
        }

        /// <summary>
        /// Fixt das Header in dem das Header zu den Standartwerden gesetzt wird
        /// </summary>
        public void fixHeader()
        {
            this.Header = headerBytes;
        }
        #endregion


        #region DetailsBlock
        /// <summary>
        /// Der Teil in der Datei in dem die Karteninfos stehen
        /// <br/>
        /// <b>Adresse</b>: [<c>0x0D</c>] - [<c>0x160</c>]
        /// </summary>
        public byte[] DetailsBlock
        {
            get => this.fileContent[0x0D..0x160];
            set => this.fileContent.Set(0x0D, value);
        }

        /// <summary>
        /// Fixt die Checksum vom Details Block
        /// </summary>
        public void FixSumDetailsBlock()
        {
            this.DetailsBlockChecksum = this.CalculateChecksum(this.DetailsBlock);
        }
        #endregion DetailsBlock
        #region StatsBlock
        public byte[] StatsBlock
        {
            get => this.fileContent[0x162..0x16C];
            set => this.fileContent.Set(0x162, value);
        }
        public void FixSumStatsBlock()
        {
            this.StatsBlockChecksum = this.CalculateChecksum(this.StatsBlock);
        }
        #endregion StatsBlock
        #region AttacksBlock
        /// <summary>
        /// Der Block in denen die Attacken, die die Karte hat, enthalten sind
        /// <br/>
        /// <b>Adresse</b>: [<c>0x16C</c>] - [<c>0x176</c>]
        /// </summary>
        public byte[] AttacksBlock
        {
            get => this.fileContent[0x16C..0x176];
            set => this.fileContent.Set(0x16C, value);
        }

        /// <summary>
        /// Fixt die Checksum vom Attacken Block
        /// </summary>
        public void FixSumAttacksBlock()
        {
            this.AttacksBlockChecksum = this.CalculateChecksum(this.AttacksBlock);
        }
        #endregion

        #region CardDetails
        /// <summary>
        /// Die Karten ID
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x00</c>] - [<c>0x02</c>]
        /// </summary>
        public ushort ID
        {
            get => BitConverter.ToUInt16(this.DetailsBlock[0x00..0x02]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x00, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Der Name von der Karte
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x02</c>] - [<c>0x22</c>]
        /// </summary>
        public string Name
        {
            get => StringCode.DecodeBytes(this.DetailsBlock[0x02..0x22]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x02, StringCode.EncodeString(value, 32));
        }

        /// <summary>
        /// Die Beschreibung von der Karte
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x22</c>] - [<c>0x122</c>]
        /// </summary>
        public string Beschreibung
        {
            get => StringCode.DecodeBytes(this.DetailsBlock[0x22..0x122]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x22, StringCode.EncodeString(value, 256));
        }

        /// <summary>
        /// Aus welchem Anime die Karte stammt
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x122</c>] - [<c>0x142</c>]
        /// </summary>
        public string Herkunft
        {
            get => StringCode.DecodeBytes(this.DetailsBlock[0x122..0x142]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x122, StringCode.EncodeString(value, 32));
        }

        /// <summary>
        /// Die IDs von den Karten bei denen diese Karte im Team einen Status Boost bekommt
        /// <br/>
        /// <br/>
        /// Adressen (im Block): <br/>
        ///     [<c>0x142</c>] - [<c>0x144</c>] <br/>
        ///     [<c>0x144</c>] - [<c>0x146</c>] <br/>
        ///     [<c>0x146</c>] - [<c>0x148</c>] <br/>
        ///     [<c>0x148</c>] - [<c>0x14A</c>] <br/>
        ///     [<c>0x14A</c>] - [<c>0x14C</c>] <br/>
        /// </summary>
        public ushort[] ZusammenSpiel
        {
            get => new ushort[5] {
                    BitConverter.ToUInt16(this.DetailsBlock[0x142..0x144]),
                    BitConverter.ToUInt16(this.DetailsBlock[0x144..0x146]),
                    BitConverter.ToUInt16(this.DetailsBlock[0x146..0x148]),
                    BitConverter.ToUInt16(this.DetailsBlock[0x148..0x14A]),
                    BitConverter.ToUInt16(this.DetailsBlock[0x14A..0x14C])
            };
            set => this.DetailsBlock = this.DetailsBlock.Set(0x142, value.Select(b => BitConverter.GetBytes(b)).ToArray().TwoDimensionsToOne());
        }

        /// <summary>
        /// Die Seltenheit der Karte
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x14C</c>] - [<c>0x14D</c>]
        /// </summary>
        /// <value>
        /// <list type="table">
        /// <item><c>0x00</c> Gewöhnlich</item>
        /// <item><c>0x01</c> Ungewöhnlich</item>
        /// <item><c>0x02</c> Selten</item>
        /// <item><c>0x03</c> Episch</item>
        /// <item><c>0x04</c> Legendär</item>
        /// <item><c>0x05</c> Mystisch</item>
        /// </list>
        /// </value>
        public byte Seltenheit
        {
            get => BitConverter.ToByte(this.DetailsBlock[0x14C..0x14D]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x14C, value);
        }

        /// <summary>
        /// Das Geschlecht der Karte
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x14D</c>] - [<c>0x14E</c>]
        /// </summary>
        /// <value>
        /// <list type="table">
        /// <item><c>0x00</c> Männlich</item>
        /// <item><c>0x01</c> Webilich</item>
        /// <item><c>0x02</c> Divers</item>
        /// <item><c>0x03</c> Unbekannt</item>
        /// </list>
        /// </value>
        public byte Geschlecht
        {
            get => BitConverter.ToByte(this.DetailsBlock[0x14D..0x14E]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x14D, value);
        }

        public byte[] Elemente
        {
            get => this.DetailsBlock[0x14E..0x150];
            set => this.DetailsBlock = this.DetailsBlock.Set(0x14E, value);
        }

        public byte EventFlag
        {
            get => this.DetailsBlock[0x150];
            set => this.DetailsBlock = this.DetailsBlock.Set(0x150, value);
        }

        /// <summary>
        /// 2 Byte Checksum für den Detailsblock, um korrupte Karten zu erkennen<br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x151</c>] - [<c>0x153</c>]
        /// </summary>
        public byte[] DetailsBlockChecksum
        {
            get => this.DetailsBlock[0x151..0x153];
            set => this.DetailsBlock = this.DetailsBlock.Set(0x151, value);
        }
        #endregion CardDetails
        #region CardStats
        /// <summary>
        /// Ab welchem Mindestlevel die Karte erhältlich ist
        /// Die Statuswerte gelten für dieses Level
        /// </summary>
        public byte Level
        {
            get => this.StatsBlock[0x00];
            set => this.StatsBlock = this.StatsBlock.Set(0x00, value);
        }
        /// <summary>
        /// Wie stark die Angriffe sind
        /// </summary>
        public byte Angriff
        {
            get => this.StatsBlock[0x01];
            set => this.StatsBlock = this.StatsBlock.Set(0x01, value);
        }
        /// <summary>
        /// Der Verteidigungswert, der den Schaden einer gegnerischen Attacke verringert
        /// </summary>
        public byte Verteidigung
        {
            get => this.StatsBlock[0x02];
            set => this.StatsBlock = this.StatsBlock.Set(0x02, value);
        }
        /// <summary>
        /// Wie schnell die Karte ist
        /// Dieser Wert entscheidet wer in einem Kampf anfängt
        /// </summary>
        public byte Schnelligkeit
        {
            get => this.StatsBlock[0x03];
            set => this.StatsBlock = this.StatsBlock.Set(0x03, value);
        }
        /// <summary>
        /// Die Anzahl von Lebenspunkten der Karte
        /// </summary>
        public ushort LP
        {
            get => BitConverter.ToUInt16(this.StatsBlock[0x04..0x06]);
            set => this.StatsBlock = this.StatsBlock.Set(0x04, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Die Stats zusammegerechnet
        /// </summary>
        public uint StatsSum
        {
            get => (ushort)(this.Angriff + this.Verteidigung + this.Schnelligkeit + this.LP);
        }
        /// <summary>
        /// Der maxmiale Wert für alle Stats zusammen
        /// </summary>
        public uint MaxStatsSum
        {
            get => (uint)((this.Level * 0.5f) * 50 + 100);
        }
        public ushort Fähigkeit
        {
            get => BitConverter.ToUInt16(this.StatsBlock[0x06..0x08]);
            set => this.StatsBlock = this.StatsBlock.Set(0x06, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Die Checksum für den Stats Block um korrupte Dateien zu erkennen
        /// </summary>
        public byte[] StatsBlockChecksum
        {
            get => this.StatsBlock[0x08..0x0A];
            set => this.StatsBlock = this.StatsBlock.Set(0x08, value);
        }
        #endregion
        #region CardAttacks
        /// <summary>
        /// Die ID von der 1. Attacke
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x00</c>] - [<c>0x02</c>]
        /// </summary>
        public ushort AttackenID_1
        {
            get => BitConverter.ToUInt16(this.AttacksBlock[0x00..0x02]);
            set => this.AttacksBlock = this.AttacksBlock.Set(0x00, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Die ID von der 2. Attacke
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x02</c>] - [<c>0x04</c>]
        /// </summary>
        public ushort AttackenID_2
        {
            get => BitConverter.ToUInt16(this.AttacksBlock[0x02..0x04]);
            set => this.AttacksBlock = this.AttacksBlock.Set(0x02, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Die ID von der 3. Attacke
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x04</c>] - [<c>0x06</c>]
        /// </summary>
        public ushort AttackenID_3
        {
            get => BitConverter.ToUInt16(this.AttacksBlock[0x04..0x06]);
            set => this.AttacksBlock = this.AttacksBlock.Set(0x04, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Die ID von der 4. Attacke
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x06</c>] - [<c>0x08</c>]
        /// </summary>
        public ushort AttackenID_4
        {
            get => BitConverter.ToUInt16(this.AttacksBlock[0x06..0x08]);
            set => this.AttacksBlock = this.AttacksBlock.Set(0x06, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Checksum für den Attackenblock
        /// <br/>
        /// <br/>
        /// <b>Adresse</b> (im Block): [<c>0x08</c>] - [<c>0x0A</c>]
        /// </summary>
        public byte[] AttacksBlockChecksum
        {
            get => this.AttacksBlock[0x08..0x0A];
            set => this.AttacksBlock = this.AttacksBlock.Set(0x08, value);
        }
        #endregion
        #region Erlernbare Atacken
        /// <summary>
        /// Block in dem die Attacken, die durch Levelaufstieg erlernbar sind enthalten sind
        /// <br/>
        /// <br/>
        /// <b>Adresse</b>: [<c>0x177</c>] - [<c>0x1D1</c>]
        /// </summary>
        public byte[] ErlernbareAttackenBlock
        {
            get => this.fileContent[0x177..0x1D1];
            set => this.fileContent = this.fileContent.Set(0x177, value);
        }

        /// <summary>
        /// 30 Attacken die durch Levelaufstieg erlernbar sind
        /// <br/>
        /// <br/>
        /// 
        /// <b>Variablen-Format</b> <br/>
        ///     <c>(level, attackenID)</c> <br/> <br/>
        ///     
        /// <b>Datei-Format</b><br/>
        ///     [<c>0x00</c>]                           Level
        ///     <br/>
        ///     [<c>0x01</c>] - [<c>0x02</c>]           Attacken ID
        /// </summary>
        public (byte, ushort)[] ErlernbareAtacken
        {
            get
            {
                (byte, ushort)[] attacken = new (byte, ushort)[30];

                int attack_index = 0;
                for (int i = 0; i < this.ErlernbareAttackenBlock.Length; i += 3)
                    attacken[attack_index++] = ( this.ErlernbareAttackenBlock[i], BitConverter.ToUInt16(this.ErlernbareAttackenBlock[(i + 1)..(i + 3)]) );
                return attacken.ToArray();
            }
            set
            {
                List<byte> attacken = new List<byte>();
                foreach ((byte, ushort) x in value)
                    attacken.AddRange( new byte[] { x.Item1 }.Concat( BitConverter.GetBytes(x.Item2)) );
                this.ErlernbareAttackenBlock = this.ErlernbareAttackenBlock.Set(0, attacken.ToArray());
            }
        }
        #endregion
        #region ItemAttacken
        /// <summary>
        /// Block in dem Attacken enthalten sind, die für die Karte erlernbar sind
        /// <br/>
        /// <br/>
        /// <b>Adresse</b>: [<c>0x1D1</c>] - [<c>0x2FD</c>]
        /// </summary>
        private byte[] AttackenPerItemBlock
        {
            get => this.fileContent[0x1D1..0x2FD];
            set => this.fileContent = this.fileContent.Set(0x1D1, value);
        }

        /// <summary>
        /// 100 Attacken die die Karte erlernen kann
        /// <br/>
        /// <br/>
        /// <b>Variablen-Format</b> <br/>
        ///     <c>attackenID</c> <br/> <br/>
        ///     
        /// <b>Datei-Format</b><br/>
        ///     [<c>0x00</c>]                     Level (immer 0)
        ///     <br/>
        ///     [<c>0x01</c>] - [<c>0x02</c>]     Attacken ID
        /// </summary>
        public ushort[] AttackenPerItem
        {
            get
            {
                ushort[] attacken = new ushort[100];

                int attIndex = 0;
                for (int i = 0; i < AttackenPerItemBlock.Length; i += 3)
                    attacken[attIndex++] = BitConverter.ToUInt16(this.AttackenPerItemBlock[(i + 1)..(i + 3)]);
                return attacken;
            }
            set
            {
                List<byte> attacken = new List<byte>();
                foreach (ushort u in value)
                    attacken.AddRange( (new byte[1] { 0x00 }).Concat(BitConverter.GetBytes(u)).ToArray());

                if (300 - attacken.Count > 0)
                    attacken.AddRange(Enumerable.Repeat((byte)0x00, 300 - attacken.Count));
                this.AttackenPerItemBlock = this.AttackenPerItemBlock.Set(0, attacken.ToArray());

            }
        }
        #endregion
        #endregion properties
    }

}
