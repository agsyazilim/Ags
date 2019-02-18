using System.ComponentModel;

namespace Ags.Web.Framework.Mvc.ModelBinding
{
    /// <summary>
    /// Represents model attribute that specifies the display name by passed key of the locale resource
    /// </summary>
    public class AgsDisplayNameAttribute : DisplayNameAttribute
    {
        #region Fields

        private string _resourceValue = string.Empty;
        private string _helpText = string.Empty;


        #endregion

        #region Ctor

        /// <summary>
        /// Create instance of the attribute
        /// </summary>
        /// <param name="resourceKey">Key of the locale resource</param>

        public AgsDisplayNameAttribute(string resourceKey) : base(resourceKey)
        {
            ResourceKey = resourceKey;


        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets key of the locale resource
        /// </summary>
        public string ResourceKey { get; set; }


        /// <summary>
        /// Getss the display name
        /// </summary>
        public override string DisplayName
        {
            get
            {
               //get locale resource value
                _resourceValue = ResourceKey;

                return _resourceValue;
            }
        }

        /// <summary>
        /// Gets name of the attribute
        /// </summary>
        public string Name
        {
            get { return nameof(AgsDisplayNameAttribute); }
        }

        #endregion
    }
}
