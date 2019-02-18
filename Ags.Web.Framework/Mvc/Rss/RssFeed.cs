using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Ags.Web.Framework.Mvc.Rss
{
    /// <summary>
    /// The class representing the RSS feed
    /// </summary>
    public class RssFeed
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="link">Link</param>
        /// <param name="lastBuildDate">Last build date</param>
        public RssFeed(string title, string description, Uri link, DateTimeOffset lastBuildDate)
        {
            Init(title, description, link, lastBuildDate);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="link">URL</param>
        public RssFeed(Uri link)
        {
            Init(string.Empty, string.Empty, link, DateTimeOffset.Now);
        }

        /// <summary>
        /// Initialize base filds of rss feed
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="link">Link</param>
        /// <param name="lastBuildDate">Last build date</param>
        private void Init(string title, string description, Uri link, DateTimeOffset lastBuildDate)
        {
            this.Title = new XElement("title", title);
            this.Description = new XElement("description", description);
            this.Link = new XElement("link", link);
            this.LastBuildDate = new XElement("lastBuildDate", lastBuildDate.ToString("r"));
        }

        /// <summary>
        /// Attribute extension
        /// </summary>
        public KeyValuePair<XmlQualifiedName, string> AttributeExtension { get; set; }

        /// <summary>
        /// Element extensions
        /// </summary>
        public List<XElement> ElementExtensions { get; } = new List<XElement>();

        /// <summary>
        /// List of rss items
        /// </summary>
        public List<RssItem> Items { get; set; } = new List<RssItem>();

        /// <summary>
        /// Title
        /// </summary>
        public XElement Title { get; private set; }

        /// <summary>
        /// Load rss feed from xml reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static RssFeed Load(XmlReader reader)
        {
            try
            {
                XDocument document = XDocument.Load(reader);

                XElement channel = document.Root?.Element("channel");

                if (channel == null)
                    return null;

                string title = channel.Element("title")?.Value ?? string.Empty;
                string description = channel.Element("description")?.Value ?? string.Empty;
                Uri link = new Uri(channel.Element("link")?.Value ?? string.Empty);
                string lastBuildDateValue = channel.Element("lastBuildDate")?.Value;
                DateTimeOffset lastBuildDate = lastBuildDateValue == null ? DateTimeOffset.Now : DateTimeOffset.ParseExact(lastBuildDateValue, "r", null);

                RssFeed feed = new RssFeed(title, description, link, lastBuildDate);

                foreach (XElement item in channel.Elements("item"))
                {
                    feed.Items.Add(new RssItem(item));
                }

                return feed;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public XElement Description { get; private set; }

        /// <summary>
        /// Link
        /// </summary>
        public XElement Link { get; private set; }

        /// <summary>
        /// Last build date
        /// </summary>
        public XElement LastBuildDate { get; private set; }

        /// <summary>
        /// Get content of RSS feed
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            XDocument document = new XDocument();
            XElement root = new XElement("rss", new XAttribute("version", "2.0"));
            XElement channel = new XElement("channel",
                new XAttribute(XName.Get(AttributeExtension.Key.Name, AttributeExtension.Key.Namespace), AttributeExtension.Value));

            channel.Add(Title, Description, Link, LastBuildDate);

            foreach (XElement element in ElementExtensions)
            {
                channel.Add(element);
            }

            foreach (RssItem item in Items)
            {
                channel.Add(item.ToXElement());
            }

            root.Add(channel);
            document.Add(root);

            return document.ToString();
        }
    }
}
