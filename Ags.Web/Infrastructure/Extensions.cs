using System;
using System.Text;

namespace Ags.Web.Infrastructure
{
    public static class Extensions
    {
        public static object OrDbNull(this string s)
        {
            return string.IsNullOrEmpty(s) ? DBNull.Value : (object) s;
        }

        /// <summary>
        /// true- false döner
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsOn(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Chop
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Chop(this string s, string length = "")
        {
            if (length != "")
            {
                var karakter = Int32.Parse(length);
                if (String.IsNullOrEmpty(s))
                    throw new ArgumentNullException(s);
                string[] words = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (words[0].Length > karakter)
                    return words[0];
                StringBuilder sb = new StringBuilder();

                foreach (string word in words)
                {
                    if ((sb + word).Length > karakter)
                        return string.Format("{0}...", sb.ToString().TrimEnd(' '));
                    sb.Append(word + " ");
                }
                return string.Format("{0}...", sb.ToString().TrimEnd(' '));
            }

            return s;
        }
    }
}