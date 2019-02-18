using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain.News;
using Ags.Data.Html;
using Ags.Services.Catalog;
using Ags.Services.Customers;
using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Stores;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.News;
using Ags.Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the news model factory implementation
    /// </summary>
    public partial class NewsModelFactory : INewsModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly INewsService _newsService;
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;

        #endregion

        #region Ctor

        public NewsModelFactory(IBaseAdminModelFactory baseAdminModelFactory,

            INewsService newsService,
            IStoreService storeService, IPictureService pictureService, ICustomerService customerService, ICategoryService categoryService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._newsService = newsService;
            this._storeService = storeService;
            _pictureService = pictureService;
            _customerService = customerService;
            _categoryService = categoryService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare news content model
        /// </summary>
        /// <param name="newsContentModel">News content model</param>
        /// <param name="filterByNewsItemId">Filter by news item ID</param>
        /// <returns>News content model</returns>
        public virtual NewsContentModel PrepareNewsContentModel(NewsContentModel newsContentModel, int? filterByNewsItemId)
        {
            if (newsContentModel == null)
            {
                throw new ArgumentNullException(nameof(newsContentModel));
            }

            //prepare nested search models
            PrepareNewsItemSearchModel(newsContentModel.NewsItems);
            NewsItem newsItem = _newsService.GetNewsById(filterByNewsItemId ?? 0);
            PrepareNewsCommentSearchModel(newsContentModel.NewsComments, newsItem);


            return newsContentModel;
        }

        /// <summary>
        /// Prepare news item search model
        /// </summary>
        /// <param name="searchModel">News item search model</param>
        /// <returns>News item search model</returns>
        public virtual NewsItemSearchModel PrepareNewsItemSearchModel(NewsItemSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            _baseAdminModelFactory.PrepareCategories(searchModel.AvailableCategories,true,"Kategori Seçiniz");
            _baseAdminModelFactory.PrepareEditorList(searchModel.AvailableEditor,true,"Editör Seçebilirsiniz");

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged news item list model
        /// </summary>
        /// <param name="searchModel">News item search model</param>
        /// <returns>News item list model</returns>
        public virtual NewsItemListModel PrepareNewsItemListModel(NewsItemSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            //get news items
            var newsItems = _newsService.GetAllNews(showHidden: true,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                categoryId: searchModel.SearchCategoryId,
                customerId: searchModel.SearchCustomerId,
                createTo: searchModel.CreatedOnTo,
                startTo: searchModel.StartDate,
                endTo: searchModel.EndDate);

            //prepare list model
            NewsItemListModel model = new NewsItemListModel
            {
                Data = newsItems.Select(newsItem =>
                {
                    //fill in model values from the entity
                    NewsItemModel newsItemModel = newsItem.ToModel<NewsItemModel>();

                    //little performance optimization: ensure that "Full" is not returned
                    newsItemModel.Full = string.Empty;

                    //convert dates to the user time
                    if (newsItem.StartDateUtc.HasValue)
                    {
                        newsItemModel.StartDate = newsItem.StartDateUtc.Value;
                    }

                    if (newsItem.EndDateUtc.HasValue)
                    {
                        newsItemModel.EndDate = newsItem.EndDateUtc.Value;
                    }

                    newsItemModel.CreatedOn = newsItem.CreatedOnUtc;

                    //fill in additional values (not existing in the entity)
                    newsItemModel.ApprovedComments = _newsService.GetNewsCommentsCount(newsItem, isApproved: true);
                    newsItemModel.NotApprovedComments = _newsService.GetNewsCommentsCount(newsItem, isApproved: false);

                    return newsItemModel;
                }),
                Total = newsItems.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare news item model
        /// </summary>
        /// <param name="model">News item model</param>
        /// <param name="newsItem">News item</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>News item model</returns>
        public virtual NewsItemModel PrepareNewsItemModel(NewsItemModel model, NewsItem newsItem, bool excludeProperties = false)
        {
            //fill in model values from the entity
            if (newsItem != null)
            {
                model = model ?? newsItem.ToModel<NewsItemModel>();

                model.StartDate = newsItem.StartDateUtc;
                model.EndDate = newsItem.EndDateUtc;
                _baseAdminModelFactory.PrepareEditorList(model.AvailableEditors,true,"Edior Seçebilirsiniz");
                _baseAdminModelFactory.PrepareCategories(model.AvailableCategories, defaultItemText: "Kategori Seçin");
                model.PictureId = newsItem.PictureId;
                model.SelectedCategoryIds = _categoryService.GetNewsItemCategoriesByNewsId(newsItem.Id, true)
                        .Select(newsCategory => newsCategory.CategoryId).ToList();
            }

            //set default values for the new model
            if (newsItem == null)
            {
                model.Published = true;
                model.AllowComments = true;
                _baseAdminModelFactory.PrepareEditorList(model.AvailableEditors,true,"Editör Seçebilirsiniz");
                _baseAdminModelFactory.PrepareCategories(model.AvailableCategories,true, defaultItemText: "Kategori Seçin");
            }
            return model;
        }

        /// <summary>
        /// Prepare news comment search model
        /// </summary>
        /// <param name="searchModel">News comment search model</param>
        /// <param name="newsItem">News item</param>
        /// <returns>News comment search model</returns>
        public virtual NewsCommentSearchModel PrepareNewsCommentSearchModel(NewsCommentSearchModel searchModel, NewsItem newsItem)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            //prepare "approved" property (0 - all; 1 - approved only; 2 - disapproved only)
            searchModel.AvailableApprovedOptions.Add(new SelectListItem
            {
                Text = "Hepsi",
                Value = "0"
            });
            searchModel.AvailableApprovedOptions.Add(new SelectListItem
            {
                Text = "Onaylı",
                Value = "1"
            });
            searchModel.AvailableApprovedOptions.Add(new SelectListItem
            {
                Text = "Onaysız",
                Value = "2"
            });

            searchModel.NewsItemId = newsItem?.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged news comment list model
        /// </summary>
        /// <param name="searchModel">News comment search model</param>
        /// <param name="newsItemId">News item Id; pass null to prepare comment models for all news items</param>
        /// <returns>News comment list model</returns>
        public virtual NewsCommentListModel PrepareNewsCommentListModel(NewsCommentSearchModel searchModel, int? newsItemId)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            //get parameters to filter comments
            DateTime? createdOnFromValue = searchModel.CreatedOnFrom == null ? null
                : (DateTime?)searchModel.CreatedOnFrom.Value;
            DateTime? createdOnToValue = searchModel.CreatedOnTo == null ? null
                : (DateTime?)searchModel.CreatedOnTo.Value.AddDays(1);
            bool? isApprovedOnly = searchModel.SearchApprovedId == 0 ? null : searchModel.SearchApprovedId == 1 ? true : (bool?)false;

            //get comments
            var comments = _newsService.GetAllComments(newsItemId: newsItemId,
                 approved: isApprovedOnly,
                 fromUtc: createdOnFromValue,
                 toUtc: createdOnToValue,
                 commentText: searchModel.SearchText);

            //prepare list model
            NewsCommentListModel model = new NewsCommentListModel
            {
                Data = comments.PaginationByRequestModel(searchModel).Select(newsComment =>
                {
                    //fill in model values from the entity
                    NewsCommentModel commentModel = new NewsCommentModel
                    {
                        Id = newsComment.Id,
                        NewsItemId = newsComment.NewsItemId,
                        CustomerId = newsComment.CustomerId == 0 ? 5 : newsComment.CustomerId,
                        IsApproved = newsComment.IsApproved,
                        CommentTitle = newsComment.CommentTitle,
                        CreatedOn = newsComment.CreatedOnUtc,

                    };
                    if (newsComment.NewsItemId != 0)
                    {
                        commentModel.NewsItemTitle = _newsService.GetNewsById(newsComment.NewsItemId).Title;
                    }
                    commentModel.CustomerInfo = _customerService.GetCustomerById(newsComment.Id).Email;
                    commentModel.CommentText = HtmlHelper.FormatText(newsComment.CommentText, false, true, false, false,
                        false,
                        false);

                    return commentModel;
                }).ToList(),
                Total = comments.Count
            };

            return model;
        }

        #endregion
    }
}