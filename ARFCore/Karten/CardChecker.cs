using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using ARF_Editor.Tools;

namespace ARF_Editor.ARFCore.Karten
{
    static class CardChecker
    {

        #region Content
        /// <summary>
        /// Überprüft ob eine Karte gültige Werte enthält
        /// </summary>
        /// <param name="card">Die Karte zur überprüfung</param>
        /// <returns>Ob die Karte gültig ist</returns>
        public static bool Valid(Karte card) => ValidHeader(card) && ValidDetails(card) && ValidStats(card) && ValidBoosts(card) && ValidAttacks(card);
        
        /// <summary>
        /// Gibt alle Errors für die ungültige Karte zurück
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string InvalidErrors(Karte card)
        {
            List<string> errors = new List<string>();
            if (!ValidHeader(card))
                errors.Add("Ungültiges Header in der Datei");
            if (!ValidDetails(card))
                errors.Add(InvalidDetailsErrors(card));
            if (!ValidStats(card))
                errors.Add(InvalidStatsErrors(card));
            if (!ValidBoosts(card))
                errors.Add(InvalidBoostsErrors(card));
            if (!ValidAttacks(card))
                errors.Add(InvalidAttacksErrors(card));

            return string.Join("\n\n", errors);
        }

        /// <summary>
        /// Überpüft ob die Karte ein gültiges Header erhält
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool ValidHeader(Karte card) => card.Header.EqualTo(Karte.headerBytes);
        #endregion

        #region Details
        /// <summary>
        /// Überprüft ob eine Karte gültige Details hat
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool ValidDetails(Karte card) => ValidID(card) && ValidName(card);
        /// <summary>
        /// Gibt alle Fehler zurück für die ungültigen Details
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string InvalidDetailsErrors(Karte card)
        {
            string errors = "";
            if (!ValidID(card))
                errors += "Ungültige ID\n";
            if (!ValidName(card))
                errors += "Ungültiger Name\n";
            return errors;
        }

        /// <summary>
        /// Prüft auf ein gültiges Checksum
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool ValidDetailsChecksum(Karte card) => card.DetailsBlockChecksum.EqualTo(card.CalculateChecksum(card.DetailsBlock));

        /// <summary>
        /// Prüft auf eine gültige ID
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool ValidID(Karte card) => Database.connectionHergestellt ? !Database.Read($"SELECT * FROM cards WHERE NOT pk={card.PK} AND ID={card.ID}").HasRows && card.ID > 0 : card.ID > 0; 

        /// <summary>
        /// Prüft auf einen gültigen Namen
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool ValidName(Karte card) => Database.connectionHergestellt ? (!Database.Read($"SELECT * FROM cards WHERE NOT pk={card.PK} AND LOWER(name)='{card.Name.ToLower()}'").HasRows && card.Name != "") : card.Name != "";
        
        #endregion

        #region Stats
        /// <summary>
        /// Prüft ob die Stats gültig sind
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static bool ValidStats(Karte card) => !(card.StatsSum > card.MaxStatsSum);
        /// <summary>
        /// Gibt alle Fehler für die ungültigen Stats zurück
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string InvalidStatsErrors(Karte card)
        {
            if (!ValidStats(card) && card.StatsSum > card.MaxStatsSum)
                return $"Kartenstats dürfen zusammen nicht höher als das Maximum ({card.MaxStatsSum}) sein\n";
            else
                return "";
        }

        #endregion
        
        #region Boosts
        /// <summary>
        /// Überprüft ob die Boost gültig sind
        /// </summary>
        /// <param name="card"></param>
        /// <returns>Gültig wenn eine Verbindung zur Datenbank hergestellt wurde und alle BoostIDs einer Karte in der Datenbank zugewiesen werden können und wenn alle BoostIDs unterschiedlich sind</returns>
        public static bool ValidBoosts(Karte card)
        {
            // Wenn eine BoostID mehrmals vorkommt
            if (card.ZusammenSpiel.Where(x => x != 0x00).Count() != card.ZusammenSpiel.Where(x => x != 0x00).Distinct().Count())
                return false;

            // Für jede boost ID
            foreach (ushort boost in card.ZusammenSpiel)
                // Wenn *nicht* eine Karte in der Datenbank mit dieser ID vorhanden ist
                if (Database.connectionHergestellt && boost != 0x00 && !Database.Read($"SELECT * FROM cards WHERE ID={boost}").HasRows)
                    return false;
            // Ansonsten true
            return true;
        }
        /// <summary>
        /// Gibt alle Fehler von den ungültigen Boosts zurück
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string InvalidBoostsErrors(Karte card)
        {
            string errors = "";

            if (card.ZusammenSpiel.Where(x => x != 0x00).Count() != card.ZusammenSpiel.Where(x => x != 0x00).Distinct().Count())
                errors += "Die Karte darf nicht zweimal den selben Boost haben\n";
            for (ushort i = 0; i < card.ZusammenSpiel.Length; i++)
                if (Database.connectionHergestellt && card.ZusammenSpiel[i] != 0x00 && !Database.Read($"SELECT * FROM cards WHERE ID={card.ZusammenSpiel[i]}").HasRows)
                    errors += $"Der {i + 1}. Boost ist nicht als Karte in der Datenbank vorhanden\n";

            return errors;
        }
        #endregion
        
        #region Attacks
        /// <summary>
        /// Überprüft ob die Karte gültige Attacken hat
        /// </summary>
        /// <param name="card"></param>
        /// <returns>Gültig wenn eine Atacke da ist und beide Attacken nicht die selbe ist</returns>
        public static bool ValidAttacks(Karte card) => !(card.AttackenID_1 == 0x00 && card.AttackenID_2 == 0x00 && card.AttackenID_1 == card.AttackenID_2);
        /// <summary>
        /// Gibt alle Fehler von den ungültigen Attacken zurück
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string InvalidAttacksErrors(Karte card)
        {
            string errors = "";
            if (card.AttackenID_1 == 0x00 && card.AttackenID_2 == 0x00)
                errors += "Karte muss mindestens eine Attacke haben\n";
            if ( (card.AttackenID_1 == card.AttackenID_2) && (card.AttackenID_1 != 0x00 && card.AttackenID_2 != 0x00) )
                errors += "Karte darf nicht zweimal die selbe Attacke haben\n";

            return errors;
        }
        #endregion
    }
}
