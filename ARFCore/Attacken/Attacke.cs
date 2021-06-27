using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using ARF_Editor.Tools;
using BitConverter = ARF_Editor.Tools.BitConverter;

namespace ARF_Editor.ARFCore.Attacken
{
    class Attacke
    {
        #region defined
        private static readonly byte ChecksumLen = 2;
        private static byte[] emptyAttack
        {
            get => headerBytes.Concat(Enumerable.Repeat((byte)0x00, 0x128).ToArray()).ToArray();
        }
        public static readonly byte[] headerBytes = new byte[] { 0x2B, 0x52, 0x4D, 0x51, 0x49, 0x3C, 0x53, 0x5D, 0x45, 0x50, 0x49, 0x1F, 0xFF };
        #endregion

        private byte[] fileContent;
        public ushort PK = 0x00;
    
        public Attacke()
        {
            this.fileContent = emptyAttack;
        }

        #region fileStream
        private FileStream fs;
        public bool fsSet
        {
            get => this.fs != null;
        }
        public void setFS(FileStream newFs) => this.fs = newFs;
        #endregion

        public Attacke(FileStream fs)
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
            this.FixChecksum();
            this.fs.Seek(0x00, SeekOrigin.Begin);
            this.fs.Write(this.fileContent, 0, this.fileContent.Length);
            this.fs.Flush();
        }

        #region Attributes
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
        private byte[] Body
        {
            get => this.fileContent[0x0D..0x135];
            set => this.fileContent = this.fileContent.Set(0x0D, value);
        }
        
        /// <summary>
        /// Die ID von der Attacke
        /// </summary>
        public ushort ID
        {
            get => BitConverter.ToUInt16(this.Body[0x00..0x02]);
            set => this.Body = this.Body.Set(0, BitConverter.GetBytes(value));
        }
        
        /// <summary>
        /// Der Attackenname
        /// </summary>
        public string Name
        {
            get => StringCode.DecodeBytes(this.Body[0x02..0x22]);
            set => this.Body = this.Body.Set(0x02, StringCode.EncodeString(value, 0x20));
        }
        
        /// <summary>
        /// Der Text der angezeigt wird wenn eine Attacke ausgeführt wird
        /// </summary>
        public string AnzeigeText
        {
            get => StringCode.DecodeBytes(this.Body[0x22..0x122]);
            set => this.Body = this.Body.Set(0x22, StringCode.EncodeString(value, 0x100));
        }
        
        /// <summary>
        /// Der Typ von der Attacke
        /// <list type="bullet">
        /// <item><c>0x00: Angriff</c></item>
        /// <item><c>0x01: Verteidigung</c></item>
        /// <item><c>0x02: Heilung</c></item>
        /// <item><c>0x03: Boost</c></item>
        /// <item><c>0x04: Effekt</c></item>
        /// </list>
        /// </summary>
        public byte Typ
        {
            get => this.Body[0x122];
            set => this.Body = this.Body.Set(0x122, value);
        }
        
        /// <summary>
        /// Der "Effekt" Block
        /// </summary>
        private byte[] EffektBlock
        {
            get => this.Body[0x123..0x125];
            set => this.Body = this.Body.Set(0x123, value);
        }
        
        /// <summary>
        /// Der Typ vom Effekt
        /// 
        /// <list type="bullet">
        /// <item><c>0x00: Keiner</c></item>
        /// <item><c>0x01: Verbrennung</c></item>
        /// <item><c>0x02: Vergiftung</c></item>
        /// <item><c>0x03: Paralyse</c></item>
        /// <item><c>0x04: Verwirrung</c></item>
        /// <item><c>0x05: Angriffswert ></c></item>
        /// <item><c>0x06: Verteidigungswert ></c></item>
        /// <item><c>0x07: Angriffswert und Verteidigungswert ></c></item>
        /// </list>
        /// </summary>
        public byte Effekt
        {
            get => this.EffektBlock[0];
            set => this.EffektBlock = this.EffektBlock.Set(0, value);
        }
        
        /// <summary>
        /// Die Chance bei der der Effekt auftritt
        /// 1:value = die Chance dass der Effekt auftritt liegt bei 1 zu dem Wert
        /// Wenn immer Status geändert wird, ist Wert 1, wenn keine Statusänderung auftritt 0
        /// </summary>
        public byte Chance
        {
            get => this.EffektBlock[1];
            set => this.EffektBlock = this.EffektBlock.Set(1, value);
        }
        
        /// <summary>
        /// Wie stark die Attacke ist
        /// </summary>
        public byte Stärke
        {
            get => (byte)BitConverter.ToUInt16(this.Body[0125..0x126]);
            set => this.Body = this.Body.Set(0x125, value);
        }

        #region Checksum
        /// <summary>
        /// Die Checksum von der Datei
        /// </summary>
        public byte[] FileChecksum
        {
            get => this.Body[0x126..0x128];
            set => this.Body = this.Body.Set(0x126, value);
        }

        /// <summary>
        /// Berrechnet die Checksum für die Datei
        /// </summary>
        /// <returns>Die berrechnete Checksum (2 bytes)</returns>
        public byte[] CalculateChecksum()
        {
            return BitConverter.GetBytes(Checksum.CRC16_CCITT(this.Body, 0, this.Body.Length - ChecksumLen));
        }

        /// <summary>
        /// Fixt die Checksum von der Karte
        /// </summary>
        private void FixChecksum()
        {
            this.FileChecksum = CalculateChecksum();
        }

        /// <summary>
        /// Ob die Checksum von der Datei gültig ist
        /// </summary>
        public bool ValidChecksum
        {
            get => this.FileChecksum.EqualTo(this.CalculateChecksum());
        }

        #endregion
        #endregion
    }
}
