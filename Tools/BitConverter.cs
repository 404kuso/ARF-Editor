using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARF_Editor.Tools
{
    static class BitConverter
    {
        public static byte[] GetBytes(int val, string byteorder = "big")
        {
            if ( (byteorder == "big" && System.BitConverter.IsLittleEndian) || (byteorder == "little" && !System.BitConverter.IsLittleEndian) )
                return System.BitConverter.GetBytes(val).Reverse().ToArray()[2..4];
            else if ( (byteorder == "little" && System.BitConverter.IsLittleEndian) || (byteorder == "big" && !System.BitConverter.IsLittleEndian) )
                return System.BitConverter.GetBytes(val);
            else
                throw new Exception("Bytorder muss 'big' oder 'little' sein");
        }
        /// <summary>
        /// Konvertiert ein bytearray zu einem unsigned short
        /// </summary>
        /// <param name="val">Das Array was konvertiert wird</param>
        /// <param name="byteorder">Die Byteorder von der Datei (big: 00 01 = 1, little: 00 01 = 256</param>
        /// <returns>Der konvertierte Wert</returns>
        public static ushort ToUInt16(byte[] val, string byteorder = "big")
        {
            if ((byteorder == "big" && System.BitConverter.IsLittleEndian) || (byteorder == "little" && !System.BitConverter.IsLittleEndian))
                return val.Length > 1 ? System.BitConverter.ToUInt16(val.Reverse().ToArray()) : val.First();
            else if ((byteorder == "little" && System.BitConverter.IsLittleEndian) || (byteorder == "big" && !System.BitConverter.IsLittleEndian))
                return val.Length > 1 ? System.BitConverter.ToUInt16(val) : val.First();
            else
                throw new Exception("Bytorder muss 'big' oder 'little' sein");
        }
        /// <summary>
        /// Konvertiert ein bytearray zu einem byte
        /// </summary>
        /// <param name="val">Das Array was konvertiert wird</param>
        /// <param name="byteorder">Die Byteorder von der Datei (big: 00 01 = 1, little: 00 01 = 256</param>
        /// <returns>Der konvertierte Wert</returns>
        public static byte ToByte(byte[] val, string byteorder = "big")
        {
            return Convert.ToByte(ToUInt16(val, byteorder));
        }
    }
}
