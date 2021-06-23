using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;

namespace ARF_Editor.Tools
{
    class Checksum
    {
        public static ushort CRC16_CCITT(byte[] data, int start, int lenght)
        {
            byte top = 0xFF;
            byte bot = 0xFF;

            int end = start + lenght;
            for(int i = 0; i < end; i++)
            {
                var x = data[i] ^ top;
                x ^= (x >> 4);

                top = (byte)(bot ^ (x >> 3) ^ (x << 4));
                bot = (byte)(x ^ (x << 5));
            }
            return (ushort)(top << 8 | bot);
        }
    }
}
