using System.Collections.Generic;
using Ags.Services.Catalog;
using Ags.Services.Seo;
using Ags.Web.Framework.Components;
using Ags.Web.Infrastructure;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class TopMenuViewComponent : AgsViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IUrlRecordService _urlRecordService;
        public TopMenuViewComponent(ICategoryService categoryService, IUrlRecordService urlRecordService)
        {
            _categoryService = categoryService;
            _urlRecordService = urlRecordService;
        }
        public IViewComponentResult Invoke()
        {
            var model = new TopMenuModel();
            var result = new List<CategorySimpleModel>();
            var allCategories = _categoryService.GetAllCategories();
            foreach (var category in allCategories)
            {
                var categoryModel = new CategorySimpleModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    SeName = FriendlyUrlHelper.GetFriendlyTitle(_urlRecordService.GetSeName(category),true),
                    IncludeInTopMenu = category.IncludeInTopMenu,
                    ParentCategoryId = category.ParentCategoryId
                };
                result.Add(categoryModel);
            }
            model.Categories = result;
            return View(model);
        }


    }

}