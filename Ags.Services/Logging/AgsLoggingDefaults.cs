namespace Ags.Services.Logging
{
    /// <summary>
    /// Represents default values related to logging services
    /// </summary>
    public static partial class AgsLoggingDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string ActivityTypeAllCacheKey => "Ags.activitytype.all";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ActivityTypePatternCacheKey => "Ags.activitytype.";
    }
}