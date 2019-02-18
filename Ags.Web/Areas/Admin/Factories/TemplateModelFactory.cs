using System;
using System.Linq;
using Ags.Services.Catalog;
using Ags.Services.Topics;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Templates;
using Ags.Web.Framework.Extensions;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the template model factory implementation
    /// </summary>
    public partial class TemplateModelFactory : ITemplateModelFactory
    {
        #region Fields

        private readonly ITopicTemplateService _topicTemplateService;
        private readonly ICategoryTemplateService _categoryTemplateService;

        #endregion

        #region Ctor

        public TemplateModelFactory(
            ITopicTemplateService topicTemplateService, ICategoryTemplateService categoryTemplateService)
        {
            this._topicTemplateService = topicTemplateService;
            _categoryTemplateService = categoryTemplateService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare templates model
        /// </summary>
        /// <param name="model">Templates model</param>
        /// <returns>Templates model</returns>
        public virtual TemplatesModel PrepareTemplatesModel(TemplatesModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //prepare nested search models
            PrepareCategoryTemplateSearchModel(model.TemplatesCategory);
            PrepareTopicTemplateSearchModel(model.TemplatesTopic);

            return model;
        }

        /// <summary>
        /// Prepare category template search model
        /// </summary>
        /// <param name="searchModel">Category template search model</param>
        /// <returns>Category template search model</returns>
        public virtual CategoryTemplateSearchModel PrepareCategoryTemplateSearchModel(CategoryTemplateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged category template list model
        /// </summary>
        /// <param name="searchModel">Category template search model</param>
        /// <returns>Category template list model</returns>
        public virtual CategoryTemplateListModel PrepareCategoryTemplateListModel(CategoryTemplateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get category templates
           var categoryTemplates = _categoryTemplateService.GetAllCategoryTemplates();

            //prepare grid model
            CategoryTemplateListModel model = new CategoryTemplateListModel
            {
                //fill in model values from the entity
                Data = categoryTemplates.PaginationByRequestModel(searchModel).Select(template => template.ToModel<CategoryTemplateModel>()),
                Total = categoryTemplates.Count
            };

            return model;
        }


        /// <summary>
        /// Prepare topic template search model
        /// </summary>
        /// <param name="searchModel">Topic template search model</param>
        /// <returns>Topic template search model</returns>
        public virtual TopicTemplateSearchModel PrepareTopicTemplateSearchModel(TopicTemplateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged topic template list model
        /// </summary>
        /// <param name="searchModel">Topic template search model</param>
        /// <returns>Topic template list model</returns>
        public virtual TopicTemplateListModel PrepareTopicTemplateListModel(TopicTemplateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get topic templates
            var topicTemplates = _topicTemplateService.GetAllTopicTemplates();

            //prepare grid model
            TopicTemplateListModel model = new TopicTemplateListModel
            {
                //fill in model values from the entity
                Data = topicTemplates.PaginationByRequestModel(searchModel).Select(template => template.ToModel<TopicTemplateModel>()),
                Total = topicTemplates.Count
            };

            return model;
        }

        #endregion
    }
}