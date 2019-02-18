namespace Ags.Services.Topics
{
    /// <summary>
    /// Represents default values related to topic services
    /// </summary>
    public static partial class AgsTopicDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// {1} : ignore ACL?
        /// {2} : show hidden?
        /// </remarks>
        public static string TopicsAllCacheKey => "Ags.topics.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : topic ID
        /// </remarks>
        public static string TopicsByIdCacheKey => "Ags.topics.id-{0}";

        /// <summary>
        /// Gets a pattern to clear cache
        /// </summary>
        public static string TopicsPatternCacheKey => "Ags.topics.";
    }
}