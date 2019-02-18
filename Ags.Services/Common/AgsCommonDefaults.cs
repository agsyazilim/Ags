namespace Ags.Services.Common
{
    /// <summary>
    /// Represents default values related to common services
    /// </summary>
    public static partial class AgsCommonDefaults
    {
        #region Generic attributes
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : key group
        /// </remarks>
        public static string GenericAttributeCacheKey => "Ags.genericattribute.{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string GenericAttributePatternCacheKey => "Ags.genericattribute.";

        #endregion

    }
}