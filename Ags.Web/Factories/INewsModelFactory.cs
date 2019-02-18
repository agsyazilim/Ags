using System.Collections.Generic;
using Ags.Data.Domain.News;
using Ags.Web.Models.Media;
using Ags.Web.Models.News;

namespace Ags.Web.Factories
{
    /// <summary>
    /// Represents the interface of the news model factory
    /// </summary>
    public partial interface INewsModelFactory
    {
        /// <summary>
        /// Prepare the news comment model
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <returns>News comment model</returns>
        NewsCommentModel PrepareNewsCommentModel(NewsComment newsComment);

        /// <summary>
        /// Prepare the news item model
        /// </summary>
        /// <param name="model">News item model</param>
        /// <param name="newsItem">News item</param>
        /// <param name="prepareComments">Whether to prepare news comment models</param>
        /// <returns>News item model</returns>
        NewsItemModel PrepareNewsItemModel(NewsItemModel model, NewsItem newsItem, bool prepareComments);
        /// <summary>
        /// PrepareNewsOverviewModel
        /// </summary>
        /// <param name="newsItems"></param>
        /// <returns></returns>
        IEnumerable<NewsItemModel> PrepareNewsOverviewModel(IEnumerable<NewsItem> newsItems);
        /// <summary>
        /// PreParePictureListModel
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        List<PictureModel> PreParePictureListModel(int newsId);
        /// <summary>
        /// PreParePictureModel
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        PictureModel PreParePictureModel(int pictureId);
        /// <summary>
        /// PrePareNewsListModel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        NewsItemListModel PrePareNewsListModel(NewsPagingFilteringModel command);
        /// <summary>
        /// PrepareNewsSliderOverviewModel
        /// </summary>
        /// <param name="newsItems"></param>
        /// <returns></returns>
        IEnumerable<NewsItemModel> PrepareNewsSliderOverviewModel(IEnumerable<NewsItem> newsItems);
        /// <summary>
        /// PrepareNewsSliderItemModel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newsItem"></param>
        /// <param name="prepareComments"></param>
        /// <returns></returns>
        NewsItemModel PrepareNewsSliderItemModel(NewsItemModel model, NewsItem newsItem, bool prepareComments);





    }
}
