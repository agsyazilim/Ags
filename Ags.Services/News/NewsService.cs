using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Caching;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.News;
using Ags.Services.Catalog;
using Ags.Services.Events;

namespace Ags.Services.News
{
    /// <summary>
    /// News service
    /// </summary>
    public partial class NewsService : INewsService
    {
        #region Fields

        private readonly IRepository<NewsComment> _newsCommentRepository;
        private readonly IRepository<NewsItem> _newsItemRepository;
        private readonly IRepository<CategoryNews> _categoryNewsRepository;
        private readonly ICategoryService _categoryService;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<NewsPictureMapping> _newsPictuRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public NewsService(
            IRepository<NewsComment> newsCommentRepository,
            IRepository<NewsItem> newsItemRepository,
            IRepository<CategoryNews> categoryNewsRepository,
            ICategoryService categoryService,
            IRepository<Category> categoryRepository, IRepository<NewsPictureMapping> newsPictuRepository, IEventPublisher eventPublisher, ICacheManager cacheManager)
        {
            this._newsCommentRepository = newsCommentRepository;
            this._newsItemRepository = newsItemRepository;
            this._categoryNewsRepository = categoryNewsRepository;
            this._categoryService = categoryService;
            this._categoryRepository = categoryRepository;
            this._newsPictuRepository = newsPictuRepository;
            this._eventPublisher = eventPublisher;
            this._cacheManager = cacheManager;
            this._entityName = typeof(NewsItem).Name;
        }

        #endregion

        #region Methods

        #region News

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsItem">News item</param>
        public virtual void DeleteNews(NewsItem newsItem)
        {
            if (newsItem == null)
            {
                throw new ArgumentNullException(nameof(newsItem));
            }

            _newsItemRepository.Delete(newsItem);
            _eventPublisher.EntityDeleted(newsItem);

            //event notification
        }

        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        public virtual NewsItem GetNewsById(int newsId)
        {
            if (newsId == 0)
            {
                return null;
            }

            var key = string.Format(AgsCatalogDefaults.NewsItemsByIdCacheKey, newsId);
            return _cacheManager.Get(key,()=>_newsItemRepository.GetById(newsId));
        }

        /// <summary>
        /// Gets news
        /// </summary>
        /// <param name="newsIds">The news identifiers</param>
        /// <returns>News</returns>
        public virtual IList<NewsItem> GetNewsByIds(int[] newsIds)
        {
            IQueryable<NewsItem> query = _newsItemRepository.Table;
            return query.Where(p => newsIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="endTo"></param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="categoryId"></param>
        /// <param name="createTo"></param>
        /// <param name="startTo"></param>
        /// <param name="customerId"></param>
        /// <param name="approved"></param>
        /// <returns>News items</returns>
        public virtual IPagedList<NewsItem> GetAllNews(int customerId = 0, int categoryId = 0, DateTime? createTo = null, DateTime? startTo = null, DateTime? endTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false,bool approved = false)
        {
            IQueryable<NewsItem> query = _newsItemRepository.Table;
            if (!showHidden)
            {
                DateTime utcNow = DateTime.UtcNow;
                query = query.Where(n => n.Published);
                query = query.Where(n => !n.StartDateUtc.HasValue || n.StartDateUtc <= utcNow);
                query = query.Where(n => !n.EndDateUtc.HasValue || n.EndDateUtc >= utcNow);
            }

            if (customerId != 0)
            {
                query = query.Where(x => x.CustomerId == customerId);
            }

            if (startTo.HasValue)
            {
                query = query.Where(b => startTo.Value <= (b.StartDateUtc ?? b.CreatedOnUtc));
            }

            if (endTo.HasValue)
            {
                query = query.Where(b => endTo <= (b.EndDateUtc ?? b.CreatedOnUtc));
            }

            if (categoryId != 0)
            {
                query = from n in query
                        join cn in _categoryNewsRepository.Table on n.Id equals cn.NewsId
                        join c in _categoryRepository.Table on cn.CategoryId equals c.Id
                        where cn.CategoryId == categoryId
                        select n;
            }

            if (!approved)
            {
                query = query.Where(x => x.Approve);
            }


            query = query.OrderByDescending(n => n.StartDateUtc ?? n.CreatedOnUtc);
            PagedList<NewsItem> news = new PagedList<NewsItem>(query, pageIndex, pageSize);
            return news;
        }

        public List<NewsItem> GetNewsListItems(List<int> categoryIds)
        {
            if (categoryIds == null)
                return null;
            var query = from cn in _categoryNewsRepository.Table
                join n in _newsItemRepository.Table on cn.NewsId equals n.Id
                where categoryIds.Contains(cn.CategoryId)
                select n;
            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="news">News item</param>
        public virtual void InsertNews(NewsItem news)
        {
            if (news == null)
            {
                throw new ArgumentNullException(nameof(news));
            }

            _newsItemRepository.Insert(news);
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemsPatternCacheKey);
            //event notification
            _eventPublisher.EntityInserted(news);

        }

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="news">News item</param>
        public virtual void UpdateNews(NewsItem news)
        {
            if (news == null)
            {
                throw new ArgumentNullException(nameof(news));
            }
            _newsItemRepository.Update(news);
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemsPatternCacheKey);
            //event notification
            _eventPublisher.EntityUpdated(news);


        }

        /// <summary>
        /// Get a value indicating whether a news item is available now (availability dates)
        /// </summary>
        /// <param name="newsItem">News item</param>
        /// <param name="dateTime">Datetime to check; pass null to use current date</param>
        /// <returns>Result</returns>
        public virtual bool IsNewsAvailable(NewsItem newsItem, DateTime? dateTime = null)
        {
            if (newsItem == null)
            {
                throw new ArgumentNullException(nameof(newsItem));
            }

            if (newsItem.StartDateUtc.HasValue && newsItem.StartDateUtc.Value >= dateTime)
            {
                return false;
            }

            if (newsItem.EndDateUtc.HasValue && newsItem.EndDateUtc.Value <= dateTime)
            {
                return false;
            }

            return true;
        }

        public List<NewsItem> GetAllNewsItems()
        {
            IQueryable<NewsItem> query = _newsItemRepository.Table;
            var newsItems = query.ToList();
            return newsItems;
        }

        public List<int> GetAllNewsItemsIds()
        {
            var query = from item in _newsItemRepository.Table select item.Id;
            var list = query.ToList();
            return list;
        }

        public List<NewsItem> GetBreakingNews()
        {
            var query = _newsItemRepository.Table;
            var newsItem = query.Where(x => x.FlashNews & x.Published & x.Approve);
            return newsItem.ToList();
        }

        public List<NewsItem> GetTopNews()
        {
            IQueryable<NewsItem> query = (from news in _newsItemRepository.Table
                                          join cn in _categoryNewsRepository.Table on news.Id equals cn.NewsId
                                          join category in _categoryRepository.Table on cn.CategoryId equals category.Id
                                          where category.IncludeInFooterMenu & news.LastNews & news.Published & category.Published & news.Approve
                                          select news);
            query = query.Where(x => DateTime.Now <= (x.EndDateUtc)).Take(3);
            if (!query.Any())
            {
                return new List<NewsItem>();
            }

            List<NewsItem> newsItem = query.ToList();
            return newsItem;
        }
        public List<NewsItem> GetPopulerNews()
        {
            var query = (from news in _newsItemRepository.Table
                                          join cn in _categoryNewsRepository.Table on news.Id equals cn.NewsId
                                          join category in _categoryRepository.Table on cn.CategoryId equals category.Id
                                          where category.IncludeInFooterMenu & news.FlashNews & news.Published & category.Published & news.Approve
                                          select news);

            query = query.Where(x => DateTime.Now <= (x.EndDateUtc)).Take(3);
            if (!query.Any())
            {
                return new List<NewsItem>();
            }

            List<NewsItem> newsItem = query.ToList();
            return newsItem;
        }

        public List<NewsPictureMapping> GetNesNewsPictureMappings(int newsItemId)
        {
            if (newsItemId == 0)
            {
                return null;
            }
            var query = from pg in _newsPictuRepository.Table
                        where pg.NewsId == newsItemId
                        orderby pg.DisplayOrder, pg.Id
                        select pg;

            var gridData = query?.ToList();
            return gridData;

        }

        public List<CategoryNews> GetCategoryNewsList()
        {
            var query = _categoryNewsRepository.Table;
            var list = query.ToList();
            return list;
        }

        public List<NewsItem> GetUpBannerList()
        {
            var sorgu = _newsItemRepository.Table;
            sorgu = sorgu.Where(x => x.Published & x.Approve & x.UpBanner & DateTime.Now <= (x.EndDateUtc));
            var list = sorgu.OrderByDescending(x=>x.Id).Take(2);
            return list.ToList();
        }

        public List<NewsItem> GetMainBannerList()
        {
            var sorgu = _newsItemRepository.Table;
            sorgu = sorgu.Where(x => x.Published & x.Approve & x.MainBanner & DateTime.Now <= (x.EndDateUtc));
            var list = sorgu.OrderByDescending(x => x.Id).Take(15);
            return list.ToList();
        }

        public List<NewsItem> GetTwoBannerList()
        {
            var sorgu = _newsItemRepository.Table;
            sorgu = sorgu.Where(x => x.Published & x.Approve & x.SecondBanner & DateTime.Now <= (x.EndDateUtc));
            var list = sorgu.OrderByDescending(x => x.Id).Take(2);
            return list.ToList();
        }



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
        public virtual IList<NewsComment> GetAllComments(int customerId = 0, int storeId = 0, int? newsItemId = null,
            bool? approved = null, DateTime? fromUtc = null, DateTime? toUtc = null, string commentText = null)
        {
            IQueryable<NewsComment> query = _newsCommentRepository.Table;

            if (approved.HasValue)
            {
                query = query.Where(comment => comment.IsApproved == approved);
            }

            if (newsItemId > 0)
            {
                query = query.Where(comment => comment.NewsItemId == newsItemId);
            }

            if (customerId > 0)
            {
                query = query.Where(comment => comment.CustomerId == customerId);
            }

            if (fromUtc.HasValue)
            {
                query = query.Where(comment => fromUtc.Value <= comment.CreatedOnUtc);
            }

            if (toUtc.HasValue)
            {
                query = query.Where(comment => toUtc.Value >= comment.CreatedOnUtc);
            }

            if (!string.IsNullOrEmpty(commentText))
            {
                query = query.Where(c => c.CommentText.Contains(commentText) || c.CommentTitle.Contains(commentText));
            }

            query = query.OrderBy(nc => nc.CreatedOnUtc);

            return query.ToList();
        }

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifier</param>
        /// <returns>News comment</returns>
        public virtual NewsComment GetNewsCommentById(int newsCommentId)
        {
            if (newsCommentId == 0)
            {
                return null;
            }

            return _newsCommentRepository.GetById(newsCommentId);
        }

        /// <summary>
        /// Get news comments by identifiers
        /// </summary>
        /// <param name="commentIds">News comment identifiers</param>
        /// <returns>News comments</returns>
        public virtual IList<NewsComment> GetNewsCommentsByIds(int[] commentIds)
        {
            if (commentIds == null || commentIds.Length == 0)
            {
                return new List<NewsComment>();
            }

            IQueryable<NewsComment> query = from nc in _newsCommentRepository.Table
                                            where commentIds.Contains(nc.Id)
                                            select nc;
            List<NewsComment> comments = query.ToList();
            //sort by passed identifiers
            List<NewsComment> sortedComments = new List<NewsComment>();
            foreach (int id in commentIds)
            {
                NewsComment comment = comments.Find(x => x.Id == id);
                if (comment != null)
                {
                    sortedComments.Add(comment);
                }
            }

            return sortedComments;
        }

        /// <summary>
        /// Get the count of news comments
        /// </summary>
        /// <param name="newsItem">News item</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="isApproved">A value indicating whether to count only approved or not approved comments; pass null to get number of all comments</param>
        /// <returns>Number of news comments</returns>
        public virtual int GetNewsCommentsCount(NewsItem newsItem, int storeId = 0, bool? isApproved = null)
        {
            var query = _newsCommentRepository.Table.Where(comment => comment.NewsItemId == newsItem.Id);
            if (isApproved.HasValue)
            {
                query = query.Where(comment => comment.IsApproved == isApproved.Value);
            }

            return query.Count();
        }

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsComment">News comment</param>
        public virtual void DeleteNewsComment(NewsComment newsComment)
        {
            if (newsComment == null)
            {
                throw new ArgumentNullException(nameof(newsComment));
            }

            _newsCommentRepository.Delete(newsComment);

            //event notification
        }

        /// <summary>
        /// Deletes a news comments
        /// </summary>
        /// <param name="newsComments">News comments</param>
        public virtual void DeleteNewsComments(IList<NewsComment> newsComments)
        {
            if (newsComments == null)
            {
                throw new ArgumentNullException(nameof(newsComments));
            }

            foreach (NewsComment newsComment in newsComments)
            {
                DeleteNewsComment(newsComment);
            }
        }

        #endregion

        #endregion
    }
}