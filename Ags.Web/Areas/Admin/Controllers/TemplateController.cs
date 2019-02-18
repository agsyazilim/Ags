using System;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Topics;
using Ags.Services.Catalog;
using Ags.Services.Topics;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Templates;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class TemplateController : BaseAdminController
    {
        #region Fields

        private readonly ICategoryTemplateService _categoryTemplateService;
        private readonly ITemplateModelFactory _templateModelFactory;
        private readonly ITopicTemplateService _topicTemplateService;

        #endregion

        #region Ctor

        public TemplateController(ICategoryTemplateService categoryTemplateService,
            ITemplateModelFactory templateModelFactory,
            ITopicTemplateService topicTemplateService)
        {
            this._categoryTemplateService = categoryTemplateService;
            this._templateModelFactory = templateModelFactory;
            this._topicTemplateService = topicTemplateService;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {


            //prepare model
            TemplatesModel model = _templateModelFactory.PrepareTemplatesModel(new TemplatesModel());

            return View(model);
        }

        #region Category templates

        [HttpPost]
        public virtual IActionResult CategoryTemplates(CategoryTemplateSearchModel searchModel)
        {


            //prepare model
            CategoryTemplateListModel model = _templateModelFactory.PrepareCategoryTemplateListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult CategoryTemplateUpdate(CategoryTemplateModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a category template with the specified id
            CategoryTemplate template = _categoryTemplateService.GetCategoryTemplateById(model.Id)
                ?? throw new ArgumentException("No template found with the specified id");

            template = model.ToEntity(template);
            _categoryTemplateService.UpdateCategoryTemplate(template);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult CategoryTemplateAdd(CategoryTemplateModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            CategoryTemplate template = new CategoryTemplate();
            template = model.ToEntity(template);
            _categoryTemplateService.InsertCategoryTemplate(template);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult CategoryTemplateDelete(int id)
        {


            //try to get a category template with the specified id
            CategoryTemplate template = _categoryTemplateService.GetCategoryTemplateById(id)
                ?? throw new ArgumentException("No template found with the specified id");

            _categoryTemplateService.DeleteCategoryTemplate(template);

            return new NullJsonResult();
        }

        #endregion



        #region Topic templates

        [HttpPost]
        public virtual IActionResult TopicTemplates(TopicTemplateSearchModel searchModel)
        {


            //prepare model
            TopicTemplateListModel model = _templateModelFactory.PrepareTopicTemplateListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult TopicTemplateUpdate(TopicTemplateModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a topic template with the specified id
            TopicTemplate template = _topicTemplateService.GetTopicTemplateById(model.Id)
                ?? throw new ArgumentException("No template found with the specified id");

            template = model.ToEntity(template);
            _topicTemplateService.UpdateTopicTemplate(template);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult TopicTemplateAdd(TopicTemplateModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            TopicTemplate template = new TopicTemplate();
            template = model.ToEntity(template);
            _topicTemplateService.InsertTopicTemplate(template);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult TopicTemplateDelete(int id)
        {


            //try to get a topic template with the specified id
            TopicTemplate template = _topicTemplateService.GetTopicTemplateById(id)
                ?? throw new ArgumentException("No template found with the specified id");

            _topicTemplateService.DeleteTopicTemplate(template);

            return new NullJsonResult();
        }

        #endregion

        #endregion
    }
}