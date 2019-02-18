namespace Ags.Services.Directory
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class AgsDirectoryDefaults
    {

        #region States and provinces

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// {1} : language ID
        /// {2} : show hidden records?
        /// </remarks>
        public static string StateProvincesAllCacheKey => "Ags.stateprovince.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StateProvincesPatternCacheKey => "Ags.stateprovince.";

        #endregion
    }
}