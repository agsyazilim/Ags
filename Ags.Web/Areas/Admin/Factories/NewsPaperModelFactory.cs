using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain.News;
using Ags.Services.NewsPapers;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Enews;

namespace Ags.Web.Areas.Admin.Factories
{
    public class NewsPaperModelFactory:INewsPaperModelFactory
    {
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly INewsPaperServices _newsPaperServices;
        private readonly IUrlRecordService _urlRecordService;

        public NewsPaperModelFactory(IBaseAdminModelFactory baseAdminModelFactory, INewsPaperServices newsPaperServices, IUrlRecordService urlRecordService)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _newsPaperServices = newsPaperServices;
            _urlRecordService = urlRecordService;
        }

        public ENewsContentModel PrepareENewsContentModel(ENewsContentModel enewsContentModel)
        {
            if (enewsContentModel == null)
            {
                throw new ArgumentNullException(nameof(enewsContentModel));
            }
            PrepareENewsItemSearchModel(enewsContentModel.ENewsItemSearchModel);
            PrepareENewsCategoriesSearchModel(enewsContentModel.ENewsCategoriesSearchModel);
            return enewsContentModel;
        }

        public ENewsItemSearchModel PrepareENewsItemSearchModel(ENewsItemSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }
            _baseAdminModelFactory.PrepareNewsPaperCategories(searchModel.AvailableCategories, true);
            searchModel.SetGridPageSize();
            return searchModel;
        }

        public ENewsItemListModel PrepareNewsItemListModel(ENewsItemSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            var newsItems = _newsPaperServices.GetAllNews(searchModel.SearchCategoryId, searchModel.CreatedOnTo,
                searchModel.Page -1, searchModel.PageSize);
            ENewsItemListModel model = new ENewsItemListModel
            {
                Data = newsItems.Select(x =>
                {
                    ENewsItemModel newsItemModel = x.ToModel<ENewsItemModel>();
                    newsItemModel.PictureId = x.PictureId;
                    newsItemModel.Categori = _newsPaperServices.GetCategoriById(x.NewsPaperCategoryId).Name;
                    return newsItemModel;
                }),
                Total = newsItems.Count
            };

            return model;
        }

        public ENewsItemModel PrepareENewsItemModel(ENewsItemModel model, EnewsPaper newsItem, bool excludeProperties = false)
        {
            if (newsItem != null)
            {
                model.PictureId = newsItem.PictureId;
                model.SelectedCategoryId = newsItem.NewsPaperCategoryId;
                model.Name = newsItem.Name;
                model.SeName = _urlRecordService.GetSeName(newsItem);
                 _baseAdminModelFactory.PrepareNewsPaperCategories(model.AvailableCategories, true);
            }

            if (newsItem == null)
            {

                _baseAdminModelFactory.PrepareNewsPaperCategories(model.AvailableCategories, true);

            }

            return model;
        }

        public ENewsCategoriesSearchModel PrepareENewsCategoriesSearchModel(ENewsCategoriesSearchModel searchModel)
        {

            searchModel.SetGridPageSize();
            return searchModel;
        }

        public ENewsCategoriesListModel PrepareCategoriesListModel(ENewsCategoriesSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

           var categoriList = _newsPaperServices.GetAllCategoriesNews(categoriName:searchModel.SearchText);
            ENewsCategoriesListModel model = new ENewsCategoriesListModel
            {
                Data = categoriList.Select(x =>
                {
                    ENewsCategoriesModel catModel = new ENewsCategoriesModel
                    {
                        Name = x.Name,
                        Id = x.Id
                    };
                    return catModel;
                }),
                Total = categoriList.Count
            };
            return model;
        }

        public ENewsCategoriesModel PrepareNewsCategoriesModel(ENewsCategoriesModel model, NewsPaperCategory newsPaperCategory,
            bool excludeProperties = false)
        {
            return model;
        }
    }
}