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
        public static ushort ToUInt16(byte[] val, string byteorder = "big")
        {
            if ((byteorder == "big" && System.BitConverter.IsLittleEndian) || (byteorder == "little" && !System.BitConverter.IsLittleEndian))
                return System.BitConverter.ToUInt16(val.Reverse().ToArray());
            else if ((byteorder == "little" && System.BitConverter.IsLittleEndian) || (byteorder == "big" && !System.BitConverter.IsLittleEndian))
                return System.BitConverter.ToUInt16(val);
            else
                throw new Exception("Bytorder muss 'big' oder 'little' sein");
        }
    }
}
