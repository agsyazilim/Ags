﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Ags.Data.Core
{
    /// <summary>
    /// XML helper class
    /// </summary>
    public partial class XmlHelper
    {
        #region Methods

        /// <summary>
        /// XML Encode
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncode(string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", string.Empty, RegexOptions.Compiled);
            return XmlEncodeAsIs(str);
        }

        /// <summary>
        /// XML Encode as is
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncodeAsIs(string str)
        {
            if (str == null)
                return null;
            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xwr = new XmlTextWriter(sw))
            {
                xwr.WriteString(str);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Encodes an attribute
        /// </summary>
        /// <param name="str">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttribute(string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", string.Empty, RegexOptions.Compiled);
            return XmlEncodeAttributeAsIs(str);
        }

        /// <summary>
        /// Encodes an attribute as is
        /// </summary>
        /// <param name="str">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttributeAsIs(string str)
        {
            return XmlEncodeAsIs(str).Replace("\"", "&quot;");
        }

        /// <summary>
        /// Decodes an attribute
        /// </summary>
        /// <param name="str">Attribute</param>
        /// <returns>Decoded attribute</returns>
        public static string XmlDecode(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            return sb.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
        }

        /// <summary>
        /// Serializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Serialized datetime</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlS.Serialize(sw, dateTime);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Deserialized datetime</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            using (StringReader sr = new StringReader(dateTime))
            {
                object test = xmlS.Deserialize(sr);
                return (DateTime)test;
            }
        }

        #endregion
    }
}