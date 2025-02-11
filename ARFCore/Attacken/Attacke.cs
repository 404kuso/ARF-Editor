﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ARF_Editor.Tools;
using BitConverter = ARF_Editor.Tools.BitConverter;

namespace ARF_Editor.ARFCore.Attacken
{
    class Attacke
    {
        // Vordefinierte Werte
        #region defined
        /// <summary>
        /// Wie groß die Checksum ist
        /// </summary>
        private const byte checksumLen = 2;
        /// <summary>
        /// Das standart Header für eine Attacke
        /// </summary>
        public static readonly byte[] headerBytes = new byte[] { 0x2B, 0x52, 0x4D, 0x51, 0x49, 0x3C, 0x53, 0x5D, 0x45, 0x50, 0x49, 0x1F, 0xFF };
        /// <summary>
        /// Eine leere Attacke
        /// </summary>
        private static byte[] emptyAttack
        {
            get => headerBytes.Concat(Enumerable.Repeat((byte)0x00, 0x22C).ToArray()).ToArray();
        }
        #endregion

        /// <summary>
        /// Der Datei-Inhalt für die Attacke
        /// </summary>
        private byte[] fileContent;
        /// <summary>
        /// Der Primary-Key für die Datenbank
        /// </summary>
        public ushort PK = 0x00;
    
        /// <summary>
        /// Erstellt eine komplett leere Attacke
        /// </summary>
        public Attacke()
        {
            this.fileContent = emptyAttack;
            this.Range = 1;
        }

        #region fileStream
        /// <summary>
        /// Der filestream der fürs Schreiben/Lesen genutzt wird
        /// </summary>
        private FileStream fs;
        
        /// <summary>
        /// Ob der Filestream gesetzt wurde
        /// </summary>
        public bool fsSet
        {
            get => this.fs != null;
        }
        /// <summary>
        /// Setzt den filestream
        /// </summary>
        /// <param name="newFs">Der neue filestream</param>
        public void setFS(FileStream newFs) => this.fs = newFs;
        #endregion

        /// <summary>
        /// Erstellt eine Attacke abhängig von einem Filestream
        /// </summary>
        /// <param name="fs">Der Filestream aus dem die Werte gelesen werden und alle neuen Werte reingeschrieben werden</param>
        public Attacke(FileStream fs)
        {
            this.fs = fs;

            this.fileContent = new byte[0x239];
            this.fs.Read(this.fileContent);

            this.UpdatePK();
        }
        /// <summary>
        /// Aktuallisiert die PK von der Karte aus der Datenbank
        /// </summary>
        public void UpdatePK()
        {
            if (Database.connectionHergestellt && this.ID != 0 && this.Name != "")
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
        /// Speichert die Karte
        /// </summary>
        public void Save()
        {
            this.FixChecksum();
            this.fs.Seek(0x00, SeekOrigin.Begin);
            this.fs.Write(this.fileContent, 0, this.fileContent.Length);
            this.fs.Flush();
        }

        public void SetEffekt(byte[] bits)
        {
            ushort value = this.Effekt;
            for(ushort i = 0; i < bits.Length; i++)
            {
                if (bits[i] == 1)
                    value |= Convert.ToUInt16(bits[i] << i);
                else
                    value &= Convert.ToUInt16(~(~bits[i] << i));
            }
            this.Effekt = value;
        }
        public int[] GetEffektFlags()
        {
            return new int[8] {
                this.Effekt & (0b1 << 0),
                this.Effekt & (0b1 << 1),
                this.Effekt & (0b1 << 2),
                this.Effekt & (0b1 << 3),
                this.Effekt & (0b1 << 4),
                this.Effekt & (0b1 << 5),
                this.Effekt & (0b1 << 6),
                this.Effekt & (0b1 << 7)
            };
        }

        #region Attributes
        #region Header
        /// <summary>
        /// Das Header der Attacke, normalerweise <c>2B 52 4D 51 49 3C 53 5D 45 50 49 1F FF</c>
        /// <br/> <br/>
        /// <b>Adresse</b>: [<c>0x00</c>] - [<c>0x0D</c>]
        /// </summary>
        public byte[] Header
        {
            get => fileContent[0x00..0x0D];
            set => fileContent.Set(0x00, value);
        }


        /// <summary>
        /// Fixt das Header indem es zu den Standartwerte gesetzt wird
        /// </summary>
        public void fixHeader()
        {
            this.Header = headerBytes;
        }
        #endregion
        /// <summary>
        /// Der Hauptblock der Attacke
        /// <br/> <br/>
        /// <b>Adresse</b>: [<c>0x0D</c>] - [<c>0x238</c>]
        /// </summary>
        private byte[] Body
        {
            get => this.fileContent[0x0D..0x239];
            set => this.fileContent = this.fileContent.Set(0x0D, value);
        }

        /// <summary>
        /// Die ID von der Attacke
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x00</c>] - [<c>0x02</c>]
        /// </summary>
        public ushort ID
        {
            get => BitConverter.ToUInt16(this.Body[0x00..0x02]);
            set => this.Body = this.Body.Set(0, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Der Attackenname
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x02</c>] - [<c>0x22</c>]
        /// </summary>
        public string Name
        {
            get => StringCode.DecodeBytes(this.Body[0x02..0x22]);
            set => this.Body = this.Body.Set(0x02, StringCode.EncodeString(value, 0x20));
        }

        /// <summary>
        /// Der Text der angezeigt wird wenn eine Attacke ausgeführt wird
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x22</c>] - [<c>0x122</c>]
        /// </summary>
        public string AnzeigeText
        {
            get => StringCode.DecodeBytes(this.Body[0x22..0x122]);
            set => this.Body = this.Body.Set(0x22, StringCode.EncodeString(value, 256));
        }

        /// <summary>
        /// Der Typ von der Attacke
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x122</c>] - [<c>0x123</c>]
        /// </summary>
        /// <value>
        /// <list type="bullet">
        ///     <item><c>0x00</c> Angriff</item>
        ///     <item><c>0x01</c> Verteidigung</item>
        ///     <item><c>0x02</c> Heilung</item>
        ///     <item><c>0x03</c> Boost</item>
        ///     <item><c>0x04</c> Effekt</item>
        /// </list>
        /// </value>
        public byte Typ
        {
            get => BitConverter.ToByte(this.Body[0x122..0x123]);
            set => this.Body = this.Body.Set(0x122, value);
        }

        /// <summary>
        /// Wieviele Gegner die Attacke trifft
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x123</c>] - [<c>0x124</c>]
        /// </summary>
        public byte Range
        {
            get => BitConverter.ToByte(this.Body[0x123..0x124]);
            set => this.Body = this.Body.Set(0x123, value);
        }
        /// <summary>
        /// Bitflag was beeinträchtigt wird vom Effekt
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x124</c>] - [<c>0x0126</c>]
        /// </summary>
        /// <value>
        /// <list type="bullet">
        ///     <item><c>1 _lshift 0</c>: Angriffswert</item>
        ///     <item><c>1 _lshift 1</c>: Verteidigungswert</item>
        ///     <item><c>1 _lshift 2</c>: Schnelligkeitswert</item>
        ///     <item><c>1 _lshift 3</c>: LP</item>
        ///     <item><c>1 _lshift 4</c>: Verbrennung</item>
        ///     <item><c>1 _lshift 5</c>: Vergiftung</item>
        ///     <item><c>1 _lshift 6</c>: Paralyse</item>
        ///     <item><c>1 _lshift 7</c>: Verwirrung</item>
        /// </list>
        /// </value>
        public ushort Effekt
        {
            get => BitConverter.ToUInt16(this.Body[0x124..0x126]);
            set => this.Body = this.Body.Set(0x124, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Wie stark der Effekt ist
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x126</c>] - [<c>0x127</c>]
        /// </summary>
        public byte EffektStärke
        {
            get => BitConverter.ToByte(this.Body[0x126..0x127]);
            set => this.Body = this.Body.Set(0x126, value);
        }

        /// <summary>
        /// Die Chance bei der der Effekt auftritt <br/><br/>
        /// 1 : Wert = die Chance dass der Effekt auftritt liegt bei 1 zu dem Wert <br/>
        /// Wenn immer Status geändert wird, ist Wert 1, wenn keine Statusänderung auftritt 0
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x127</c>] - [<c>0x128</c>]
        /// </summary>
        public byte Chance
        {
            get => BitConverter.ToByte(this.Body[0x127..0x128]);
            set => this.Body = this.Body.Set(0x127, value);
        }

        /// <summary>
        /// Wie stark die Attacke ist
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x128</c>] - [<c>0x129</c>]
        /// </summary>
        public byte Stärke
        {
            get => BitConverter.ToByte(this.Body[0x128..0x129]);
            set => this.Body = this.Body.Set(0x128, value);
        }

        /// <summary>
        /// Die Beschreibung von der Attacke
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x129</c>] - [<c>0x229</c>]
        /// </summary>
        public string Beschreibung
        {
            get => StringCode.DecodeBytes(this.Body[0x129..0x229]);
            set => this.Body = this.Body.Set(0x129, StringCode.EncodeString(value, 256));
        }
        /// <summary>
        /// Das Element der Attacke
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x229</c>] - [<c>0x22A</c>]
        /// </summary>
        /// <value>
        /// <list type="bullet">
        ///     <item><c>0x00</c> Feuer</item>
        ///     <item><c>0x01</c> Wasser</item>
        ///     <item><c>0x02</c> Blitz</item>
        ///     <item><c>0x03</c> Wind</item>
        ///     <item><c>0x04</c> Eis</item>
        ///     <item><c>0x05</c> Pflanze</item>
        ///     <item><c>0x06</c> Erde</item>
        ///     <item><c>0x07</c> Medizin</item>
        /// </list>
        /// </value>
        public byte Element
        {
            get => BitConverter.ToByte(this.Body[0x229..0x22A]);
            set => this.Body = this.Body.Set(0x229, value);

        }
        #region Checksum
        /// <summary>
        /// Die Checksum von der Datei
        /// <br/> <br/>
        /// <b>Adresse</b> (im Block): [<c>0x229</c>] - [<c>0x22B</c>]
        /// </summary>
        public byte[] FileChecksum
        {
            get => this.Body[0x22A..0x22C];
            set => this.Body = this.Body.Set(0x22A, value);
        }

        /// <summary>
        /// Berrechnet die Checksum für die Datei
        /// </summary>
        /// <returns>Die berrechnete Checksum (2 bytes)</returns>
        public byte[] CalculateChecksum()
        {
            return BitConverter.GetBytes(Checksum.CRC16_CCITT(this.Body, 0, this.Body.Length - checksumLen));
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
