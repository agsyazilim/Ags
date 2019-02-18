﻿using System.Collections.Generic;
using Ags.Data.Common;

namespace Ags.Data.Domain
{
    /// <summary>
    /// Common settings
    /// </summary>
    public class CommonSettings : ISettings
    {
        public CommonSettings()
        {
            SitemapCustomUrls = new List<string>();
            IgnoreLogWordlist = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contacts form should have "Subject"
        /// </summary>
        public bool SubjectFieldOnContactUsForm { get; set; }

        
       

        /// <summary>
        /// A list of custom URLs to be added to sitemap.xml (include page names only)
        /// </summary>
        public List<string> SitemapCustomUrls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display a warning if java-script is disabled
        /// </summary>
        public bool DisplayJavaScriptDisabledWarning { get; set; }

       

        

        /// <summary>
        /// Gets or sets a value indicating whether 404 errors (page or file not found) should be logged
        /// </summary>
        public bool Log404Errors { get; set; }

      

       

        /// <summary>
        /// Gets or sets ignore words (phrases) to be ignored when logging errors/messages
        /// </summary>
        public List<string> IgnoreLogWordlist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether links generated by BBCode Editor should be opened in a new window
        /// </summary>
        public bool BbcodeEditorOpenLinksInNewWindow { get; set; }

      

        /// <summary>
        /// Gets or sets a value indicating whether jQuery migrate script logging is active
        /// </summary>
        public bool JqueryMigrateScriptLoggingActive { get; set; }
        public string StaticFilesCacheControl { get; set; }
    }
}