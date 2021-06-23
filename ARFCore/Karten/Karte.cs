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
        private static readonly byte ChecksumLen = 2;
        public static readonly byte[] headerBytes = new byte[] { 0x2B, 0x52, 0x4D, 0x51, 0x49, 0x3C, 0x53, 0x5D, 0x45, 0x50, 0x49, 0x0F, 0xFF };
        private static byte[] emptyDetails
        {
            get => Enumerable.Repeat((byte)0x00, 0x150).ToArray();
        }
        private static byte[] emptyStats
        {
            get => Enumerable.Repeat((byte)0x00, 8).ToArray();
        }
        private static byte[] emptyAttacks
        {
            get => Enumerable.Repeat((byte)0x00, 6).ToArray();
        }

        public static byte[] emptyCard
        {
            get => headerBytes.
                Concat(emptyDetails).ToArray().Concat(new byte[3] {0xFF, 0xFF, 0xFF}).ToArray()
                .Concat(emptyStats).ToArray().Concat(new byte[2] { 0xFF, 0xFF }).ToArray()
                .Concat(emptyAttacks).ToArray().Concat(new byte[3] { 0xFF, 0xFF, 0xFF }).ToArray();
        }
        #endregion



        #region newCard
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

            this.AttackenID_1 = 1;
            this.AttackenID_2 = 0;

        }

        #region filestream
        public void setFS(FileStream fs) => this.fs = fs;
        public bool fsSet
        {
            get => this.fs != null;
        }
        
        private FileStream fs;
        #endregion fileStream
        #endregion

        private byte[] fileContent;
        /// <summary>
        /// Der Primarykey int in der Datenbank
        /// </summary>
        public ushort PK = 0x00;
        public Karte(FileStream fs)
        {
            this.fs = fs;
            
            this.fileContent = new byte[1000];
            this.fs.Read(this.fileContent);

            this.UpdatePK();
        }
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

        public void Save()
        {
            this.FixSumDetailsBlock();
            this.FixSumStatsBlock();
            this.FixSumAttacksBlock();

            this.fs.Seek(0x00, SeekOrigin.Begin);
            this.fs.Write(this.fileContent, 0, this.fileContent.Length);
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


        public byte[] CalculateChecksum(byte[] block)
        {
            return BitConverter.GetBytes(Checksum.CRC16_CCITT(block, 0, block.Length - ChecksumLen));
        }

        #region attributes
        #region Header
        public byte[] Header
        {
            get => fileContent[0x00..0x0D];
            set => fileContent.Set(0x00, value);
        }

        public void fixHeader()
        {
            this.Header = headerBytes;
        }
        #endregion


        #region DetailsBlock
        public byte[] DetailsBlock
        {
            get => this.fileContent[0x0D..0x15D];
            set => this.fileContent.Set(0x0D, value);
        }
        public void FixSumDetailsBlock()
        {
            this.DetailsBlockChecksum = this.CalculateChecksum(this.DetailsBlock);
        }
        #endregion DetailsBlock
        #region StatsBlock
        public byte[] StatsBlock
        {
            get => this.fileContent[0x160..0x168];
            set => this.fileContent.Set(0x160, value);
        }
        public void FixSumStatsBlock()
        {
            this.StatsBlockChecksum = this.CalculateChecksum(this.StatsBlock);
        }
        #endregion StatsBlock
        #region AttacksBlock
        public byte[] AttacksBlock
        {
            get => this.fileContent[0x16A..0x170];
            set => this.fileContent.Set(0x16A, value);
        }
        public void FixSumAttacksBlock()
        {
            this.AttacksBlockChecksum = this.CalculateChecksum(this.AttacksBlock);
        }
        #endregion

        #region CardDetails
        /// <summary>
        /// Die Karten ID
        /// </summary>
        public ushort ID
        {
            get => BitConverter.ToUInt16(this.DetailsBlock[0x00..0x02]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x00, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Der Name von der Karte
        /// </summary>
        public string Name
        {
            get => StringCode.DecodeBytes(this.DetailsBlock[0x02..0x2E]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x02, StringCode.EncodeString(value, 32));
        }
        /// <summary>
        /// Die Beschreibung von der Karte
        /// </summary>
        public string Beschreibung
        {
            get => StringCode.DecodeBytes(this.DetailsBlock[0x22..0x122]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x22, StringCode.EncodeString(value, 256));
        }
        /// <summary>
        /// Aus welchem Anime die Karte stammt
        /// </summary>
        public string Herkunft
        {
            get => StringCode.DecodeBytes(this.DetailsBlock[0x122..0x142]);
            set => this.DetailsBlock = this.DetailsBlock.Set(0x122, StringCode.EncodeString(value, 32));
        }
        /// <summary>
        /// Bei welchen Karten im Team diese Karte einen Status Boost bekommt
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
        /// </summary>
        /// <value>
        /// <list type="bullet">
        /// <item><c>0x00</c> Gewöhnlich</item>
        /// <item><c>0x01</c> Ungewöhnlich</item>
        /// <item><c>0x02</c> Selten</item>
        /// <item><c>0x03</c> Episch</item>
        /// <item><c>0x04</c> Legendär</item>
        /// <item><c>0x05</c> Mystisch</item>
        /// </value>
        /// </list>
        public byte Seltenheit
        {
            get => this.DetailsBlock[0x14C];
            set => this.DetailsBlock = this.DetailsBlock.Set(0x14C, value);
        }
        /// <summary>
        /// Das Geschlecht der Karte
        /// 0x00 = Männlich
        /// 0x01 = Webilich
        /// 0x02 = 
        /// </summary>
        public byte Geschlecht
        {
            get => this.DetailsBlock[0x14D];
            set => this.DetailsBlock = this.DetailsBlock.Set(0x14D, value);
        }
        /// <summary>
        /// Die Checksum für den Detailsblock, um korrupte Karten zu erkennen
        /// </summary>
        public byte[] DetailsBlockChecksum
        {
            get => this.DetailsBlock[0x14E..0x150];
            set => this.DetailsBlock = this.DetailsBlock.Set(0x14E, value);
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
        /// <summary>
        /// Die Checksum für den Stats Block um korrupte Dateien zu erkennen
        /// </summary>
        public byte[] StatsBlockChecksum
        {
            get => this.StatsBlock[0x06..0x08];
            set => this.StatsBlock = this.StatsBlock.Set(0x06, value);
        }
        #endregion
        #region CardAttacks
        /// <summary>
        /// Die ID von der 1. Attacke
        /// </summary>
        public ushort AttackenID_1
        {
            get => BitConverter.ToUInt16(this.AttacksBlock[0x00..0x02]);
            set => this.AttacksBlock = this.AttacksBlock.Set(0x00, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Die ID von der 2. Attacke
        /// </summary>
        public ushort AttackenID_2
        {
            get => BitConverter.ToUInt16(this.AttacksBlock[0x02..0x04]);
            set => this.AttacksBlock = this.AttacksBlock.Set(0x02, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Die Checksum für den Attackenblock
        /// </summary>
        public byte[] AttacksBlockChecksum
        {
            get => this.AttacksBlock[0x04..0x06];
            set => this.AttacksBlock = this.AttacksBlock.Set(0x04, value);
        }
        #endregion
        #endregion properties
    }

}
