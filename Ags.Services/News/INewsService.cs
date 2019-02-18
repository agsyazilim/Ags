using System;
using System.Collections.Generic;
using Ags.Data.Core.Pages;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.News;

namespace Ags.Services.News
{
    /// <summary>
    /// News service interface
    /// </summary>
    public partial interface INewsService
    {
        #region News

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsItem">News item</param>
        void DeleteNews(NewsItem newsItem);

        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        NewsItem GetNewsById(int newsId);

        /// <summary>
        /// Gets news
        /// </summary>
        /// <param name="newsIds">The news identifiers</param>
        /// <returns>News</returns>
        IList<NewsItem> GetNewsByIds(int[] newsIds);

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="startTo"></param>
        /// <param name="endTo"></param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="customerId"></param>
        /// <param name="categoryId"></param>
        /// <param name="createTo"></param>
        /// <returns>News items</returns>
        IPagedList<NewsItem> GetAllNews(int customerId = 0, int categoryId = 0, DateTime? createTo = null, DateTime? startTo = null, DateTime? endTo = null, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false,bool approved =false);
        /// <summary>
        /// GetNewsListItems
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        List<NewsItem> GetNewsListItems(List<int> categoryIds);
        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="news">News item</param>
        void InsertNews(NewsItem news);

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="news">News item</param>
        void UpdateNews(NewsItem news);

        /// <summary>
        /// Get a value indicating whether a news item is available now (availability dates)
        /// </summary>
        /// <param name="newsItem">News item</param>
        /// <param name="dateTime">Datetime to check; pass null to use current date</param>
        /// <returns>Result</returns>
        bool IsNewsAvailable(NewsItem newsItem, DateTime? dateTime = null);
        /// <summary>
        /// GetAllNewsItems
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetAllNewsItems();
        /// <summary>
        /// GetAllNewsItemsIds
        /// </summary>
        /// <returns></returns>
        List<int> GetAllNewsItemsIds();
        /// <summary>
        /// GetBreakingNews
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetBreakingNews();
        /// <summary>
        /// GetTopNews
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetTopNews();
        /// <summary>
        /// GetPopulerNews
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetPopulerNews();
        /// <summary>
        /// GetNesNewsPictureMappings
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <returns></returns>
        List<NewsPictureMapping> GetNesNewsPictureMappings(int newsItemId);
        /// <summary>
        /// GetCategoryNewsList
        /// </summary>
        /// <returns></returns>
        List<CategoryNews> GetCategoryNewsList();
        /// <summary>
        /// GetUpBannerList
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetUpBannerList();
        /// <summary>
        /// GetMainBannerList
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetMainBannerList();
        /// <summary>
        /// GetTwoBannerList
        /// </summary>
        /// <returns></returns>
        List<NewsItem> GetTwoBannerList();
        #endregion

        #region News comments

        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="newsItemId">News item ID; 0 or null to load all records</param>
        /// <param name="approved">A value indicating whether to content is approved; null to load all records</param>
        /// <param name="fromUtc">Item creation from; null to load all records</param>
        /// <param name="toUtc">Item creation to; null to load all records</param>
        /// <param name="commentText">Search comment text; null to load all records</param>
        /// <returns>Comments</returns>
        IList<NewsComment> GetAllComments(int customerId = 0, int storeId = 0, int? newsItemId = null,
            bool? approved = null, DateTime? fromUtc = null, DateTime? toUtc = null, string commentText = null);

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifier</param>
        /// <returns>News comment</returns>
        NewsComment GetNewsCommentById(int newsCommentId);

        /// <summary>
        /// Get news comments by identifiers
        /// </summary>
        /// <param name="commentIds">News comment identifiers</param>
        /// <returns>News comments</returns>
        IList<NewsComment> GetNewsCommentsByIds(int[] commentIds);

        /// <summary>
        /// Get the count of news comments
        /// </summary>
        /// <param name="newsItem">News item</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="isApproved">A value indicating whether to count only approved or not approved comments; pass null to get number of all comments</param>
        /// <returns>Number of news comments</returns>
        int GetNewsCommentsCount(NewsItem newsItem, int storeId = 0, bool? isApproved = null);

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsComment">News comment</param>
        void DeleteNewsComment(NewsComment newsComment);

        /// <summary>
        /// Deletes a news comments
        /// </summary>
        /// <param name="newsComments">News comments</param>
        void DeleteNewsComments(IList<NewsComment> newsComments);

        #endregion
    }
}