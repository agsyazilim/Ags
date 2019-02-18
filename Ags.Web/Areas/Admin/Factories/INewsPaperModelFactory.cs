using Ags.Data.Domain.News;
using Ags.Web.Areas.Admin.Models.Enews;

namespace Ags.Web.Areas.Admin.Factories
{
    public interface INewsPaperModelFactory
    {
        /// <summary>
        /// Prepare news content model
        /// </summary>
        /// <param name="enewsContentModel">News content model</param>
        /// <param name="filterByNewsItemId">Filter by news item ID</param>
        /// <returns>News content model</returns>
        ENewsContentModel PrepareENewsContentModel(ENewsContentModel enewsContentModel);

        /// <summary>
        /// Prepare news item search model
        /// </summary>
        /// <param name="searchModel">News item search model</param>
        /// <returns>News item search model</returns>
        ENewsItemSearchModel PrepareENewsItemSearchModel(ENewsItemSearchModel searchModel);

        /// <summary>
        /// Prepare paged news item list model
        /// </summary>
        /// <param name="searchModel">News item search model</param>
        /// <returns>News item list model</returns>
        ENewsItemListModel PrepareNewsItemListModel(ENewsItemSearchModel searchModel);

        /// <summary>
        /// Prepare news item model
        /// </summary>
        /// <param name="model">News item model</param>
        /// <param name="newsItem">News item</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>News item model</returns>
        ENewsItemModel PrepareENewsItemModel(ENewsItemModel model, EnewsPaper newsItem, bool excludeProperties = false);

        ENewsCategoriesSearchModel PrepareENewsCategoriesSearchModel(ENewsCategoriesSearchModel searchModel);

        ENewsCategoriesListModel PrepareCategoriesListModel(ENewsCategoriesSearchModel searchModel);

        ENewsCategoriesModel PrepareNewsCategoriesModel(ENewsCategoriesModel model, NewsPaperCategory newsPaperCategory,
            bool excludeProperties = false);



    }
}