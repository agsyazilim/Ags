namespace Ags.Services.Seo
{
    /// <summary>
    /// Represents default values related to SEO services
    /// </summary>
    public static partial class AgsSeoDefaults
    {
        /// <summary>
        /// Gets a max length of forum topic slug name
        /// </summary>
        /// <remarks>For long URLs we can get the following error:
        /// "the specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters",
        /// that's why we limit it to 100</remarks>
        public static int ForumTopicLength => 100;

        /// <summary>
        /// Gets a max length of search engine name
        /// </summary>
        /// <remarks>For long URLs we can get the following error:
        /// "the specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters",
        /// that's why we limit it to 200</remarks>
        public static int SearchEngineNameLength => 200;

      

        #region URL records

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static string UrlRecordActiveByIdNameLanguageCacheKey => "Ags.urlrecord.active.id-name-{0}-{1}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string UrlRecordAllCacheKey => "Ags.urlrecord.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : slug
        /// </remarks>
        public static string UrlRecordBySlugCacheKey => "Ags.urlrecord.active.slug-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UrlRecordPatternCacheKey => "Ags.urlrecord.";

        #endregion
    }
}