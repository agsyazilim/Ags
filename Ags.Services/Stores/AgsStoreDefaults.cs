namespace AdminLTE.Services.Stores
{
    /// <summary>
    /// Represents default values related to stores services
    /// </summary>
    public static partial class AgsStoreDefaults
    {
        #region Stores

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string StoresAllCacheKey => "Ags.stores.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string StoresByIdCacheKey => "Ags.stores.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StoresPatternCacheKey => "Ags.stores.";

        #endregion
    }
}