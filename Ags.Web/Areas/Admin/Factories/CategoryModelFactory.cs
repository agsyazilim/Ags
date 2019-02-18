using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Services.Catalog;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Catalog;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the category model factory implementation
    /// </summary>
    public partial class CategoryModelFactory : ICategoryModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly StoreInformationSettings _storeInformationSettings;

        #endregion

        #region Ctor

        public CategoryModelFactory(
            IBaseAdminModelFactory baseAdminModelFactory,
            ICategoryService categoryService,
            IUrlRecordService urlRecordService, StoreInformationSettings storeInformationSettings)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._categoryService = categoryService;
            this._urlRecordService = urlRecordService;
            _storeInformationSettings = storeInformationSettings;
        }

        #endregion



        #region Methods

        /// <summary>
        /// Prepare category search model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>Category search model</returns>
        public virtual CategorySearchModel PrepareCategorySearchModel(CategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged category list model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>Category list model</returns>
        public virtual CategoryListModel PrepareCategoryListModel(CategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get categories
            var categories = _categoryService.GetAllCategories(categoryName: searchModel.SearchCategoryName,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model
            CategoryListModel model = new CategoryListModel
            {
                Data = categories.Select(category =>
                {
                    //fill in model values from the entity
                    CategoryModel categoryModel = category.ToModel<CategoryModel>();

                    //fill in additional values (not existing in the entity)
                    categoryModel.Breadcrumb = _categoryService.GetFormattedBreadCrumb(category);

                    return categoryModel;
                }),
                Total = categories.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare category model
        /// </summary>
        /// <param name="model">Category model</param>
        /// <param name="category">Category</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Category model</returns>
        public virtual CategoryModel PrepareCategoryModel(CategoryModel model, Category category, bool excludeProperties = false)
        {


            if (category != null)
            {
                //fill in model values from the entity
                model = model ?? category.ToModel<CategoryModel>();

            }

            //set default values for the new model
            if (category == null)
            {

                model.Published = true;
                model.IncludeInTopMenu = true;
                model.IncludeInFooterMenu = false;
                model.IncludeInManset = false;

            }



            //prepare available category templates
            _baseAdminModelFactory.PrepareCategoryTemplates(model.AvailableCategoryTemplates, false);

            //prepare available parent categories
            _baseAdminModelFactory.PrepareCategories(model.AvailableCategories,
                defaultItemText: "Kategori Seç");
            _baseAdminModelFactory.PrepareSliders(model.AvailableSliders, true, "Slider Seçebiliirsiniz");
            _baseAdminModelFactory.PreparePhotoGalleries(model.AvailablePhotoGallery, true, "Galeri Sçebilirsiniz");
            _baseAdminModelFactory.PrepareVideoGalleries(model.AvailableVideoGalleries, true, "Video Galeri Seçebilirsiniz");

            return model;
        }






        #endregion
    }
}