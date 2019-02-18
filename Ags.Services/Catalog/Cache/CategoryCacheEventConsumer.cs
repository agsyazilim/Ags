using Ags.Data.Core.Caching;
using Ags.Data.Core.Events;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Configuration;
using Ags.Services.Events;

namespace Ags.Services.Catalog.Cache
{
    /// <summary>
    /// Price cache event consumer (used for caching of prices)
    /// </summary>
    public partial class CategoryCacheEventConsumer :
        //settings
        IConsumer<EntityUpdatedEvent<Setting>>,
        //categories
        IConsumer<EntityInsertedEvent<Category>>,
        IConsumer<EntityUpdatedEvent<Category>>,
        IConsumer<EntityDeletedEvent<Category>>,

        //product categories
        IConsumer<EntityInsertedEvent<CategoryNews>>,
        IConsumer<EntityUpdatedEvent<CategoryNews>>,
        IConsumer<EntityDeletedEvent<CategoryNews>>
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public CategoryCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        public void HandleEvent(EntityUpdatedEvent<Setting> eventMessage)
        {
           _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoryIdsModelCacheKey);

        }

        #region Categories

        public void HandleEvent(EntityInsertedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoryIdsModelCacheKey);
        }

        public void HandleEvent(EntityUpdatedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoryIdsModelCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsCategoryIdsPatternCacheKey);
        }

        #endregion


        #region Product categories

        public void HandleEvent(EntityInsertedEvent<CategoryNews> eventMessage)
        {

            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsCategoryIdsPatternCacheKey);
        }

        public void HandleEvent(EntityUpdatedEvent<CategoryNews> eventMessage)
        {

            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsCategoryIdsPatternCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<CategoryNews> eventMessage)
        {

            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsCategoryIdsPatternCacheKey);
        }

        #endregion

        #endregion
    }
}