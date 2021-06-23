using ARF_Editor.Tools;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Forms;

namespace ARF_Editor.ARFCore.Attacken
{
    static class AttackChecker
    {
        public static bool Valid(Attacke attacke) => ValidHeader(attacke) && ValidID(attacke);

        public static string InvalidErrors(Attacke attacke)
        {
            List<string> errors = new List<string>();
            if (!ValidHeader(attacke))
                errors.Add("Datei Header ist nicht gültig");
            if (!ValidID(attacke))
                errors.Add("Die ID ist ungültig");

            return string.Join("\n\n", errors);
        }

        public static bool ValidHeader(Attacke attacke) => attacke.Header.EqualTo(Attacke.headerBytes);

        public static bool ValidID(Attacke attacke) => Database.connectionHergestellt ? !Database.Read($"SELECT * FROM attacks WHERE NOT pk={attacke.PK} AND ID={attacke.ID}").HasRows && attacke.ID > 0 : attacke.ID > 0;
    }
}
