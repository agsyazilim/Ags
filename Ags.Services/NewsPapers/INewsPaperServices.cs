using System;
using System.Collections.Generic;
using Ags.Data.Core.Pages;
using Ags.Data.Domain.News;

namespace Ags.Services.NewsPapers
{
    public interface INewsPaperServices
    {

        #region News

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsItem">News item</param>
        void DeleteNews(EnewsPaper newsItem);

        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        EnewsPaper GetNewsById(int newsId);

        EnewsPaper GetNewsByIdAs(int newsId);

        /// <summary>
        /// Gets news
        /// </summary>
        /// <param name="newsIds">The news identifiers</param>
        /// <returns>News</returns>
        IList<EnewsPaper> GetNewsByIds(int[] newsIds);

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryId"></param>
        /// <param name="createTo"></param>
        /// <returns>News items</returns>
        IPagedList<EnewsPaper> GetAllNews(int categoryId = 0, DateTime? createTo = null,int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="news">News item</param>
        void InsertNews(EnewsPaper news);

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="news">News item</param>
        void UpdateNews(EnewsPaper news);

        #endregion

        #region News categori

        /// <summary>
        /// Gets all comments
        /// </summary>
       /// <returns>Comments</returns>
        IList<NewsPaperCategory> GetAllCategories();

        /// <summary>
        /// Gets a news comment
        /// </summary>

        /// <returns>News comment</returns>
        EnewsPaper GetNewsCategoryById(int newsId);

        NewsPaperCategory GetCategoriById(int id);

        IPagedList<NewsPaperCategory> GetAllCategoriesNews(string categoriName, int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="newsCategory"></param>
        void DeleteNewsCategory(NewsPaperCategory newsCategory);

        void InsertCategory(NewsPaperCategory category);
        void UpdateCategory(NewsPaperCategory category);

        #endregion

    }
}