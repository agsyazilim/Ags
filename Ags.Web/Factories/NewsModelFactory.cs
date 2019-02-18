using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Domain;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.News;
using Ags.Services.Catalog;
using Ags.Services.Common;
using Ags.Services.Customers;
using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Web.Infrastructure;
using Ags.Web.Models.Media;
using Ags.Web.Models.News;

namespace Ags.Web.Factories
{
    /// <summary>
    /// Represents the news model factory
    /// </summary>
    public partial class NewsModelFactory : INewsModelFactory
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly INewsService _newsService;
        private readonly IPictureService _pictureService;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly NewsSettings _newsSettings;
        private readonly IVideoFactory _videoFactory;
        private readonly ICategoryService _categoryService;
        private readonly INewsCounterService _newsCounterService;



        #endregion

        #region Ctor

        public NewsModelFactory(

            ICustomerService customerService,
            INewsService newsService,
            IPictureService pictureService,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            NewsSettings newsSettings, IVideoFactory videoFactory, ICategoryService categoryService,
            INewsCounterService newsCounterService)
        {
            this._customerService = customerService;
            this._newsService = newsService;
            this._pictureService = pictureService;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._newsSettings = newsSettings;
            this._videoFactory = videoFactory;
            this._categoryService = categoryService;
            this._newsCounterService = newsCounterService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the news comment model
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <returns>News comment model</returns>
        public virtual NewsCommentModel PrepareNewsCommentModel(NewsComment newsComment)
        {
            if (newsComment == null)
            {
                throw new ArgumentNullException(nameof(newsComment));
            }

            NewsCommentModel model = new NewsCommentModel
            {
                Id = newsComment.Id,
                CustomerId = newsComment.CustomerId,
                CustomerName = _customerService.GetCustomerById(newsComment.CustomerId).Name,
                CommentTitle = newsComment.CommentTitle,
                CommentText = newsComment.CommentText,
                CreatedOn = newsComment.CreatedOnUtc,
            };
            return model;
        }

        /// <summary>
        /// Prepare the news item model
        /// </summary>
        /// <param name="model">News item model</param>
        /// <param name="newsItem">News item</param>
        /// <param name="prepareComments">Whether to prepare news comment models</param>
        /// <returns>News item model</returns>
        public virtual NewsItemModel PrepareNewsItemModel(NewsItemModel model, NewsItem newsItem, bool prepareComments)
        {
            if (model == null)
            {
                return null;
            }

            if (newsItem == null)
            {
                return null;
            }

            var category = _categoryService.GetNewsItemCategoriesByCategorysId(newsItem.Id);
            model.Id = newsItem.Id;
            model.MetaTitle = newsItem.MetaTitle;
            model.MetaDescription = newsItem.MetaDescription;
            model.MetaKeywords = newsItem.MetaKeywords;
            model.SeName = FriendlyUrlHelper.GetFriendlyTitle(_urlRecordService.GetSeName(newsItem), true);
            model.Title = newsItem.Title;
            model.EndDateUtc = newsItem.EndDateUtc;
            model.Short = newsItem.Short;
            model.Full = newsItem.Full;
            model.AllowComments = newsItem.AllowComments;
            model.CreatedOn = newsItem.StartDateUtc ?? newsItem.CreatedOnUtc;
            model.PictureUrl = _pictureService.GetPictureUrl(newsItem.PictureId);
            model.VideoId = newsItem.VideoId;
            model.PictureModels = PreParePictureListModel(newsItem.Id);
            model.Category = category;
            model.CategorySeName = _urlRecordService.GetSeName(category);
            model.CategoryName = _categoryService.GetNewsItemCategoriesByCategorysId(newsItem.Id).Name;
            if (newsItem.CustomerId > 0)
            {
                model.CustomerName = _customerService.GetCustomerById(newsItem.CustomerId).Name;
                model.AvatarUrl =
                    _pictureService.GetPictureUrl(
                        Convert.ToInt32(_customerService.GetCustomerById(newsItem.CustomerId).Zip), 120, defaultPictureType: PictureType.Avatar);
            }


            //number of news comments
            int storeId = _newsSettings.ShowNewsCommentsPerStore ? _storeContext.CurrentStore.Id : 0;
            model.NumberOfComments = _newsService.GetNewsCommentsCount(newsItem, storeId, true);
            model.NumberOfRead = _newsCounterService.GetByListCounter(newsItem.Id, "NewsItem", null).Sum(x => x.TotalVisitor);
            model.VideoGalleryModel = _videoFactory.PrepareVideoGalleryModel(model.VideoId);

            if (prepareComments)
            {
                var newsComments = newsItem.NewsComments.Where(comment => comment.IsApproved);
                foreach (NewsComment nc in newsComments.OrderBy(comment => comment.CreatedOnUtc))
                {
                    NewsCommentModel commentModel = PrepareNewsCommentModel(nc);
                    model.Comments.Add(commentModel);
                }
            }

            return model;
        }

        public IEnumerable<NewsItemModel> PrepareNewsOverviewModel(IEnumerable<NewsItem> newsItems)
        {
            if (newsItems == null)
            {
                return null;
            }

            var models = new List<NewsItemModel>();
            foreach (var item in newsItems)
            {
                var model = new NewsItemModel();
                PrepareNewsItemModel(model, item, true);
                models.Add(model);
            }
            return models;
        }

        public List<PictureModel> PreParePictureListModel(int newsId)
        {
            if (newsId == 0)
            {
                return null;
            }

            var query = _newsService.GetNesNewsPictureMappings(newsId);
            if (query == null)
            {
                return null;
            }

            var model = new List<PictureModel>();
            foreach (var mapping in query)
            {
                model.Add(PreParePictureModel(mapping.PictureId));
            }

            return model;

        }

        public PictureModel PreParePictureModel(int pictureId)
        {
            if (pictureId == 0)
            {
                return new PictureModel();
            }

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
            {
                return new PictureModel();
            }
            var model = new PictureModel
            {
                AlternateText = picture.AltAttribute,
                Title = picture.TitleAttribute,
                FullSizeImageUrl = _pictureService.GetPictureUrl(picture, 1920),
                ImageUrl = _pictureService.GetPictureUrl(picture, 1300),
                ThumbImageUrl = _pictureService.GetPictureUrl(picture, 400)

            };
            return model;
        }

        public NewsItemListModel PrePareNewsListModel(NewsPagingFilteringModel command)
        {
            var model = new NewsItemListModel();
            if (command.PageSize <= 0)
            {
                command.PageSize = _newsSettings.NewsArchivePageSize;
            }

            if (command.PageNumber <= 0)
            {
                command.PageNumber = 1;
            }

            var newsItems = _newsService.GetAllNews(pageIndex: command.PageNumber - 1, pageSize: command.PageSize);
            model.PagingFilteringContext.LoadPagedList(newsItems);
            model.NewsItems = newsItems
                .Select(x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel, x, false);
                    return newsModel;
                })
                .ToList();
            return model;
        }

        public IEnumerable<NewsItemModel> PrepareNewsSliderOverviewModel(IEnumerable<NewsItem> newsItems)
        {
            if (newsItems == null)
            {
                return null;
            }

            var models = new List<NewsItemModel>();
            foreach (var item in newsItems)
            {
                var model = new NewsItemModel();
                PrepareNewsSliderItemModel(model, item, true);
                models.Add(model);
            }

            return models;
        }

        public NewsItemModel PrepareNewsSliderItemModel(NewsItemModel model, NewsItem newsItem, bool prepareComments)
        {
            if (model == null)
            {
                return null;
            }

            if (newsItem == null)
            {
                return null;
            }
            model.Id = newsItem.Id;
            model.SeName = FriendlyUrlHelper.GetFriendlyTitle(_urlRecordService.GetSeName(newsItem), true);
            model.Title = newsItem.Title;
            model.PictureUrl = _pictureService.GetPictureUrl(newsItem.PictureId);
            return model;
        }

        #endregion
    }
}