using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ARF_Editor.Tools
{
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
        public ComboBoxItem(string text, object value)
        {
            this.Text = text;
            this.Value = value;
        }
    }

    public static class ArrayExt
    {
        /// <summary>
        /// Überschreibt ein Array ab einer bestimmten Stelle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="index">Ab welcher stelle überschrieben werden soll</param>
        /// <param name="values">Mit welchen Werten überschrieben werden soll</param>
        /// <returns></returns>
        public static T[] Set<T>(this T[] self, int index, params T[] values)
        {
            Array.Copy(values, 0, self, index, values.Length);
            return self;
        }
        /// <summary>
        /// Konvertiert ein Array mit 2 Dimensionen zu einem mit einem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T[] TwoDimensionsToOne<T>(this T[][] self)
        {
            if (self.Length == 0)
                return Array.Empty<T>();

            List<T> oneDimensional = new List<T>();
            for (int i = 0; i < self.Length; i++)
            {
                for (int j = 0; j < self[i].Length; j++)
                    oneDimensional.Add(self[i][j]);
            }
            return oneDimensional.Count > 0 ? oneDimensional.ToArray() : Array.Empty<T>();
        }
        /// <summary>
        /// Überprüft ob ein Array mit einem anderen übereinstimmt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool EqualTo<T>(this T[] self, T[] check)
        {
            if (self.Length != check.Length)
                return false;

            for (int i = 0; i < self.Length; i++)
            {
                if (!self[i].Equals(check[i]))
                    return false;
            }
            return true;
        }
    }
    public static class TupleExt
    {
        public static IEnumerable ToEnumerable<Tuple>(this Tuple self)
        {
            // You can check if type of tuple is actually Tuple
            return self.GetType()
                .GetProperties()
                .Select(property => property.GetValue(self));
        }
    }
}
