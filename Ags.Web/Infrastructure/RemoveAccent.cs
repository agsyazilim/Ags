using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ags.Web.Infrastructure
{
    public static class RemoveAccent
    {
        public static string RemoveTurkce(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);

            return System.Text.Encoding.ASCII.GetString(bytes).ToUpper();
        }
        /// <summary>
        /// convert tr
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertTrCharToEnChar(string text)
        {
            return String.Join("", text.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
        }
        /// <summary>
        ///  replace
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StringReplace(string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            text = text.Replace(" ", "_");
            return text;
        }
    }
}