using Ags.Data.Core.Caching;
using Ags.Data.Core.Events;
using Ags.Data.Domain.Catalog;
using Ags.Services.Events;

namespace Ags.Web.Areas.Admin.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer:
       //categories
        IConsumer<EntityInsertedEvent<Category>>,
        IConsumer<EntityUpdatedEvent<Category>>,
        IConsumer<EntityDeletedEvent<Category>>
        {


         /// <summary>
        /// Key for categories caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public const string CATEGORIES_LIST_KEY = "ags.pres.admin.categories.list-{0}";
        public const string CATEGORIES_LIST_PATTERN_KEY = "ags.pres.admin.categories.list";
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductCategoryIdsPatternCacheKey => "ags.totals.newsitem.categoryids";


        private readonly ICacheManager _cacheManager;

        public ModelCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }


        //categories
        public void HandleEvent(EntityInsertedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORIES_LIST_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORIES_LIST_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORIES_LIST_PATTERN_KEY);
        }

    }
}