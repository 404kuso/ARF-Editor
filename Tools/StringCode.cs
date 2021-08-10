using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARF_Editor.Tools
{
    class StringCode
    {
        /// <summary>
        /// Überprüft ob ein String encoded werden kann
        /// </summary>
        /// <param name="s">Der String zur überprüfung</param>
        /// <returns></returns>
        public static bool CanEncode(string s)
        {
            foreach(char c in s)
                if (CanEncode(c) == false)
                    return false;
            return true;
        }
        public static bool CanEncode(char c)
        {
            return charMap.ContainsKey(c);
        }

        public static byte[] EncodeString(string s)
        {
            List<byte> encodedBytes = new List<byte>();

            foreach (char c in s)
                encodedBytes.Add(charMap[c]);
            
            return encodedBytes.ToArray();
        }
        public static byte[] EncodeString(string s, int len)
        {
            List<byte> encodedBytes = EncodeString(s).ToList();
            
            if (encodedBytes.Count > len)
                throw new Exception("Text ist zu lang");

            for (int i = 0; i < len - encodedBytes.Count; i++)
                encodedBytes.Add( (byte)0x00 );

            return encodedBytes.ToArray();
        }


        public static string DecodeBytes(byte[] b)
        {
            string decodedBytes = "";
            foreach (byte a in b)
            {
                if (a == 0x00)
                    break;
                decodedBytes += DecodeByte(a);
            }
            return decodedBytes;
        }


        #region single
        /// <summary>
        /// Wandelt ein verschlüsseltes Zeichen in ein normales Zeichen um
        /// </summary>
        /// <param name="c">Das verschlüsselte Zeichen</param>
        /// <returns>Das entschlüsselte Zeichen</returns>
        private static char DecodeByte(byte c)
        {
            return charMap.FirstOrDefault(x => x.Value == c).Key;
        }
        #endregion
        
        private static Dictionary<char, byte> charMap = new Dictionary<char, byte>() {
                ['0'] = 0x21,
                ['1'] = 0x22,
                ['2'] = 0x23,
                ['3'] = 0x24,
                ['4'] = 0x25,
                ['5'] = 0x26,
                ['6'] = 0x27,
                ['7'] = 0x28,
                ['8'] = 0x29,
                ['9'] = 0x2A,

                ['A'] = 0x2B,
                ['B'] = 0x2C,
                ['C'] = 0x2D,
                ['D'] = 0x2E,
                ['E'] = 0x2F,
                ['F'] = 0x30,
                ['G'] = 0x31,
                ['H'] = 0x32,
                ['I'] = 0x33,
                ['J'] = 0x34,
                ['K'] = 0x35,
                ['L'] = 0x36,
                ['M'] = 0x37,
                ['N'] = 0x38,
                ['O'] = 0x39,
                ['P'] = 0x3A,
                ['Q'] = 0x3B,
                ['R'] = 0x3C,
                ['S'] = 0x3D,
                ['T'] = 0x3E,
                ['U'] = 0x3F,
                ['V'] = 0x40,
                ['W'] = 0x41,
                ['X'] = 0x42,
                ['Y'] = 0x43,
                ['Z'] = 0x44,

                ['a'] = 0x45,
                ['b'] = 0x46,
                ['c'] = 0x47,
                ['d'] = 0x48,
                ['e'] = 0x49,
                ['f'] = 0x4A,
                ['g'] = 0x4B,
                ['h'] = 0x4C,
                ['i'] = 0x4D,
                ['j'] = 0x4E,
                ['k'] = 0x4F,
                ['l'] = 0x50,
                ['m'] = 0x51,
                ['n'] = 0x52,
                ['o'] = 0x53,
                ['p'] = 0x54,
                ['q'] = 0x55,
                ['r'] = 0x56,
                ['s'] = 0x57,
                ['t'] = 0x58,
                ['u'] = 0x59,
                ['v'] = 0x5A,
                ['w'] = 0x5B,
                ['x'] = 0x5C,
                ['y'] = 0x5D,
                ['z'] = 0x5E,

                ['ß'] = 0x5F,
                ['ä'] = 0x60,
                ['ö'] = 0x61,
                ['ü'] = 0x62,

                ['Ä'] = 0x63,
                ['Ö'] = 0x64,
                ['Ü'] = 0x65,

                ['\n'] = 0x6A,
                [' '] = 0x70,
                [','] = 0x71,
                ['.'] = 0x72,
                ['{'] = 0x80,
                ['}'] = 0x81,
                ['-'] = 0x82,
                ['('] = 0x83,
                [')'] = 0x84
        };
    }
}
