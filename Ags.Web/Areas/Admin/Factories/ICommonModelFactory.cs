using Ags.Web.Areas.Admin.Models.Common;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents common models factory
    /// </summary>
    public partial interface ICommonModelFactory
    {

        /// <summary>
        /// Prepare URL record search model
        /// </summary>
        /// <param name="searchModel">URL record search model</param>
        /// <returns>URL record search model</returns>
        UrlRecordSearchModel PrepareUrlRecordSearchModel(UrlRecordSearchModel searchModel);

        /// <summary>
        /// Prepare paged URL record list model
        /// </summary>
        /// <param name="searchModel">URL record search model</param>
        /// <returns>URL record list model</returns>
        UrlRecordListModel PrepareUrlRecordListModel(UrlRecordSearchModel searchModel);



        /// <summary>
        /// Prepare popular search term search model
        /// </summary>
        /// <param name="searchModel">Popular search term search model</param>
        /// <returns>Popular search term search model</returns>
        PopularSearchTermSearchModel PreparePopularSearchTermSearchModel(PopularSearchTermSearchModel searchModel);

        /// <summary>
        /// Prepare paged popular search term list model
        /// </summary>
        /// <param name="searchModel">Popular search term search model</param>
        /// <returns>Popular search term list model</returns>
        PopularSearchTermListModel PreparePopularSearchTermListModel(PopularSearchTermSearchModel searchModel);

        /// <summary>
        /// Prepare common statistics model
        /// </summary>
        /// <returns>Common statistics model</returns>
        CommonStatisticsModel PrepareCommonStatisticsModel();

    }
}