using System;
using System.Collections.Generic;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Services.Blogs;
using Ags.Services.Common;
using Ags.Services.Customers;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents common models factory implementation
    /// </summary>
    public partial class CommonModelFactory : ICommonModelFactory
    {
        #region Constants



        #endregion

        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ISearchTermService _searchTermService;
        private readonly INewsCounterService _newsCounterService;
        private readonly IVisitorCounterService _visitorCounterService;
        private readonly ICustomerService _customerService;
        private readonly INewsService _newsService;
        private readonly IBlogService _blogService;

        #endregion

        #region Ctor

        public CommonModelFactory(
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory, IUrlRecordService urlRecordService, ISearchTermService searchTermService, INewsCounterService newsCounterService, IVisitorCounterService visitorCounterService, ICustomerService customerService, INewsService newsService, IBlogService blogService)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._httpContextAccessor = httpContextAccessor;
            this._urlHelperFactory = urlHelperFactory;
            _urlRecordService = urlRecordService;
            _searchTermService = searchTermService;
            _newsCounterService = newsCounterService;
            _visitorCounterService = visitorCounterService;
            _customerService = customerService;
            _newsService = newsService;
            _blogService = blogService;
        }

        #endregion
        #region Methods




        /// <summary>
        /// Prepare URL record search model
        /// </summary>
        /// <param name="searchModel">URL record search model</param>
        /// <returns>URL record search model</returns>
        public virtual UrlRecordSearchModel PrepareUrlRecordSearchModel(UrlRecordSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged URL record list model
        /// </summary>
        /// <param name="searchModel">URL record search model</param>
        /// <returns>URL record list model</returns>
        public virtual UrlRecordListModel PrepareUrlRecordListModel(UrlRecordSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get URL records
            var urlRecords = _urlRecordService.GetAllUrlRecords(slug: searchModel.SeName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //get URL helper
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            //prepare list model
            UrlRecordListModel model = new UrlRecordListModel
            {
                Data = urlRecords.Select(urlRecord =>
                {
                    //fill in model values from the entity
                    UrlRecordModel urlRecordModel = urlRecord.ToModel<UrlRecordModel>();
                    //details URL
                    string detailsUrl = string.Empty;
                    string entityName = urlRecord.EntityName?.ToLowerInvariant() ?? string.Empty;
                    switch (entityName)
                    {
                        case "blogpost":
                            detailsUrl = urlHelper.Action("BlogPostEdit", "Blog", new { id = urlRecord.EntityId });
                            break;
                        case "category":
                            detailsUrl = urlHelper.Action("Edit", "Category", new { id = urlRecord.EntityId });
                            break;
                        case "newsitem":
                            detailsUrl = urlHelper.Action("NewsItemEdit", "News", new { id = urlRecord.EntityId });
                            break;
                        case "topic":
                            detailsUrl = urlHelper.Action("Edit", "Topic", new { id = urlRecord.EntityId });
                            break;

                    }

                    urlRecordModel.DetailsUrl = detailsUrl;

                    return urlRecordModel;
                }),
                Total = urlRecords.TotalCount
            };

            return model;
        }



        /// <summary>
        /// Prepare popular search term search model
        /// </summary>
        /// <param name="searchModel">Popular search term search model</param>
        /// <returns>Popular search term search model</returns>
        public virtual PopularSearchTermSearchModel PreparePopularSearchTermSearchModel(PopularSearchTermSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.PageSize = 5;
            searchModel.AvailablePageSizes = "5";

            return searchModel;
        }

        /// <summary>
        /// Prepare paged popular search term list model
        /// </summary>
        /// <param name="searchModel">Popular search term search model</param>
        /// <returns>Popular search term list model</returns>
        public virtual PopularSearchTermListModel PreparePopularSearchTermListModel(PopularSearchTermSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get popular search terms
            var searchTermRecordLines = _searchTermService.GetStats(pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            PopularSearchTermListModel model = new PopularSearchTermListModel
            {
                //fill in model values from the entity
                Data = searchTermRecordLines.Select(searchTerm => new PopularSearchTermModel
                {
                    Keyword = searchTerm.Keyword,
                    Count = searchTerm.Count
                }),
                Total = searchTermRecordLines.TotalCount
            };

            return model;
        }

        public CommonStatisticsModel PrepareCommonStatisticsModel()
        {
            var model = new CommonStatisticsModel
            {
                NumberOfDayVisitors = _visitorCounterService.GetCounterCount(DateTime.Now),
                NumberOfCustomers = _customerService.GetAllCustomers().Count(),
                NumberOfTotalVisitCount = _visitorCounterService.GetByListCounter().Sum(x=>x.Count)

                
            };
            var blogIds = new List<int>();
            var newsIds = new List<int>();
            var newsCounterList = _newsCounterService.GetByListCounter(entityName: "NewsItem");
             model.NumberOfNewsRead = newsCounterList.Count;
            foreach (var counter in newsCounterList)
            { 
              newsIds.Add(counter.EntityId);   
            }
            var blogPostCounetrList = _newsCounterService.GetByListCounter(entityName: "BlogPost");
            model.NumberOfReadBlogPost = blogPostCounetrList.Count;
           foreach (var counter in blogPostCounetrList)
           {
               blogIds.Add(counter.EntityId);
           }
           var newsListIds = _newsService.GetAllNewsItemsIds().ToArray();
           var blogListIds = _blogService.GetBlogListIds().ToArray();
           model.NumberOfNotReadNews = newsListIds.Except(newsIds).Count();
           model.NumberOfNotReadBlogPost = blogListIds.Except(blogIds).Count();
           return model;
        }

        #endregion
    }
}