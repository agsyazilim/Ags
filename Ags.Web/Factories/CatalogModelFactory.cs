using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Services.Catalog;
using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Web.Infrastructure;
using Ags.Web.Models.Catalog;
using Ags.Web.Models.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ags.Web.Factories
{
    public partial class CatalogModelFactory : ICatalogModelFactory
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryTemplateService _categoryTemplateService;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHelper _webHelper;
        private readonly INewsService _newsService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly IVideoFactory _videoFactory;
        private readonly IGalleryFactory _galleryFactory;
        private readonly ISliderFactory _sliderFactory;
        private readonly IPictureService _pictureService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public CatalogModelFactory(
            CatalogSettings catalogSettings,
            ICategoryService categoryService,
            ICategoryTemplateService categoryTemplateService,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            IWebHelper webHelper,
             INewsService newsService, INewsModelFactory newsModelFactory, IVideoFactory videoFactory, IGalleryFactory galleryFactory, ISliderFactory sliderFactory, IPictureService pictureService, IHttpContextAccessor httpContextAccessor)
        {
            this._catalogSettings = catalogSettings;
            this._categoryService = categoryService;
            this._categoryTemplateService = categoryTemplateService;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._webHelper = webHelper;
            _newsService = newsService;
            _newsModelFactory = newsModelFactory;
            _videoFactory = videoFactory;
            _galleryFactory = galleryFactory;
            _sliderFactory = sliderFactory;
            _pictureService = pictureService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion
        public virtual void PreparePageSizeOptions(CatalogPagingFilteringModel pagingFilteringModel, CatalogPagingFilteringModel command, string pageSizeOptions, int fixedPageSize)
        {
            if (pagingFilteringModel == null)
            {
                throw new ArgumentNullException(nameof(pagingFilteringModel));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (command.PageNumber <= 0)
            {
                command.PageNumber = 1;
            }
            if (pageSizeOptions != null)
            {
                var pageSizes = pageSizeOptions.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (pageSizes.Any())
                {
                    // get the first page size entry to use as the default (category page load) or if customer enters invalid value via query string
                    if (command.PageSize <= 0 || !pageSizes.Contains(command.PageSize.ToString()))
                    {
                        if (int.TryParse(pageSizes.FirstOrDefault(), out int temp))
                        {
                            if (temp > 0)
                            {
                                command.PageSize = temp;
                            }
                        }
                    }

                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.RemoveQueryString(currentPageUrl, "pagenumber");

                    foreach (var pageSize in pageSizes)
                    {
                        if (!int.TryParse(pageSize, out int temp))
                        {
                            continue;
                        }
                        if (temp <= 0)
                        {
                            continue;
                        }

                        pagingFilteringModel.PageSizeOptions.Add(new SelectListItem
                        {
                            Text = pageSize,
                            Value = _webHelper.ModifyQueryString(sortUrl, "pagesize", pageSize),
                            Selected = pageSize.Equals(command.PageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        });
                    }

                    if (pagingFilteringModel.PageSizeOptions.Any())
                    {
                        pagingFilteringModel.PageSizeOptions = pagingFilteringModel.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();

                        if (command.PageSize <= 0)
                        {
                            command.PageSize = int.Parse(pagingFilteringModel.PageSizeOptions.First().Text);
                        }
                    }
                }
            }
            else
            {
                //customer is not allowed to select a page size
                command.PageSize = fixedPageSize;
            }

            //ensure pge size is specified
            if (command.PageSize <= 0)
            {
                command.PageSize = fixedPageSize;
            }
        }



        #region Categories

        /// <summary>
        /// Prepare category model
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="command">Catalog paging filtering command</param>
        /// <returns>Category model</returns>
        public virtual CategoriModel PrepareCategoryModel(Category category, CatalogPagingFilteringModel command)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var model = new CategoriModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                MetaKeywords = category.MetaKeywords,
                MetaDescription = category.MetaDescription,
                MetaTitle = category.MetaTitle,
                SeName = _urlRecordService.GetSeName(category),
                VideoId = category.VideoGalleryId,
                SliderId = category.SliderId,
                PhotoId = category.PhotoGalleryId,
                BannerLitlePictureId = category.BannerLitlePictureId,
                BannerPictureId = category.BannerPictureId
            };
            //page size
            PreparePageSizeOptions(model.PagingFilteringContext, command, category.PageSizeOptions, category.PageSize);

            //subcategories
            model.SubCategories = _categoryService.GetAllCategoriesByParentCategoryId(category.Id)
                .Select(x =>
                {
                    var subCatModel = new CategoriModel.SubCategoryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SeName = _urlRecordService.GetSeName(x),
                        Description = x.Description

                    };
                    return subCatModel;
                }).ToList();
            var categoryIds = new List<int>();
            categoryIds.Add(category.Id);
            if (_catalogSettings.ShowProductsFromSubcategories)
            {
                //include subcategories
                categoryIds.AddRange(_categoryService.GetChildCategoryIds(category.Id, _storeContext.CurrentStore.Id));
            }

            var newses = _newsService.GetAllNews(categoryId: category.Id, showHidden: true, approved: true);
            model.LargeNewsModels = _newsModelFactory.PrepareNewsOverviewModel(newses).ToList();
            model.PagingFilteringContext.LoadPagedList(newses);
            model.VideoGalleryModel = _videoFactory.PrepareVideoGalleryModel(model.VideoId);
            model.GalleryModel = _galleryFactory.PrePareGalleryModel(model.PhotoId);
            model.SliderModel = _sliderFactory.PrePareSliderModel(model.SliderId);
            if (category.BannerPictureId > 0)
            {
                model.BannerPictureUrl = _pictureService.GetPictureUrl(category.BannerPictureId);

            }

            if (category.BannerLitlePictureId > 0)
            {
                model.BannerLittlePicturUrl = _pictureService.GetPictureUrl(category.BannerLitlePictureId);
            }


            return model;
        }

        public CategoriModel PrepareCategoryModel(CategoriModel model, Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            model.Id = category.Id;
            model.Name = category.Name;
            model.Description = category.Description;
            model.MetaKeywords = category.MetaKeywords;
            model.MetaDescription = category.MetaDescription;
            model.MetaTitle = category.MetaTitle;
            model.SeName = _urlRecordService.GetSeName(category);
            model.VideoId = category.VideoGalleryId;
            model.SliderId = category.SliderId;
            model.PhotoId = category.PhotoGalleryId;
            model.BannerLitlePictureId = category.BannerLitlePictureId;
            model.BannerPictureId = category.BannerPictureId;

            //subcategories
            model.SubCategories = _categoryService.GetAllCategoriesByParentCategoryId(category.Id)
                .Select(x =>
                {
                    var subCatModel = new CategoriModel.SubCategoryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SeName = _urlRecordService.GetSeName(x),
                        Description = x.Description
                    };
                    return subCatModel;
                }).ToList();
            var categoryIds = new List<int>();
            categoryIds.Add(category.Id);
            if (_catalogSettings.ShowProductsFromSubcategories)
            {
                //include subcategories
                categoryIds.AddRange(_categoryService.GetChildCategoryIds(category.Id, _storeContext.CurrentStore.Id));
            }

            var newses = _newsService.GetAllNews(categoryId: category.Id, showHidden: true, approved: true);
            var newsBig = newses.Where(x => x.BigNews);
            model.LargeNewsModels = _newsModelFactory.PrepareNewsOverviewModel(newsBig).ToList();
            model.VideoGalleryModel = _videoFactory.PrepareVideoGalleryModel(model.VideoId);
            model.GalleryModel = _galleryFactory.PrePareGalleryModel(model.PhotoId);
            model.SliderModel = _sliderFactory.PrePareSliderModel(model.SliderId);
            model.NewsModels = _newsModelFactory.PrepareNewsOverviewModel(newses).ToList();
            if (category.BannerPictureId > 0)
            {
                model.BannerPictureUrl = _pictureService.GetPictureUrl(category.BannerPictureId);

            }

            if (category.BannerLitlePictureId > 0)
            {
                model.BannerLittlePicturUrl = _pictureService.GetPictureUrl(category.BannerLitlePictureId);
            }
            return model;
        }

        public CategoriModel PrepareCategoryModel(CategoriModel model, List<Category> categorys)
        {
            if (categorys == null)
            {
                throw new ArgumentNullException(nameof(categorys));
            }

            model.SubCategories = categorys.Select(x => new CategoriModel.SubCategoryModel
            {
                Name = x.Name,
                Id = x.Id,
                SeName = _urlRecordService.GetSeName(x),
                Description = x.Description,
                VideoGalleryModel = _videoFactory.PrepareVideoGalleryModel(x.VideoGalleryId),
                GalleryModel = _galleryFactory.PrePareGalleryModel(x.PhotoGalleryId),
                SliderModel = _sliderFactory.PrePareSliderModel(x.SliderId)
            }).ToList();
            var categoryIds = new List<int>();

            foreach (var category in categorys)
            {
                categoryIds.Add(category.Id);
                if (_catalogSettings.ShowProductsFromSubcategories)
                {
                    //include subcategories
                    categoryIds.AddRange(_categoryService.GetChildCategoryIds(category.Id, _storeContext.CurrentStore.Id));
                }

                var newses = _newsService.GetAllNews(categoryId: category.Id, showHidden: true, approved: true);

                foreach (var newsItem in newses)
                {
                    model.NewsModels.Add(_newsModelFactory.PrepareNewsItemModel(new NewsItemModel(), newsItem, false));
                }
                var newsBig = newses.Where(x => x.BigNews);
                foreach (var newsItem in newsBig)
                {
                    model.LargeNewsModels.Add(_newsModelFactory.PrepareNewsItemModel(new NewsItemModel(), newsItem, false));
                }
            }

            return model;
        }

        public PopulerSectionModel PrepareCategoryPopulerSectionModel(List<Category> categorys)
        {
            if (categorys == null)
            {
                throw new ArgumentNullException(nameof(categorys));
            }
            var model = new PopulerSectionModel();
            var categoryIds = new List<int>();
            foreach (var category in categorys)
            {
                categoryIds.Add(category.Id);

            }
            var newses = _newsService.GetNewsListItems(categoryIds);
            var newsModel = newses.Select(x => new NewsPopulerModel
            {
                SeName = _urlRecordService.GetSeName(x),
                Title = x.Title,
                Id = x.Id,
                PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                Short = x.Short.Chop("20")

            }).ToList();
            model.NewsModels = newsModel;
            return model;
        }

        /// <summary>
        /// Prepare category template view path
        /// </summary>
        /// <param name="templateId">Template identifier</param>
        /// <returns>Category template view path</returns>
        public virtual string PrepareCategoryTemplateViewPath(int templateId)
        {
            var template = _categoryTemplateService.GetCategoryTemplateById(templateId);
            if (template == null)
            {
                template = _categoryTemplateService.GetAllCategoryTemplates().FirstOrDefault();
            }

            if (template == null)
            {
                throw new Exception("No default template could be loaded");
            }

            return template.ViewPath;
        }



        #endregion

    }
}