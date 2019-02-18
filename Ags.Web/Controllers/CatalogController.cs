using System.Collections.Generic;
using System.Linq;
using Ags.Services.Catalog;
using Ags.Services.Common;
using Ags.Web.Factories;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class CatalogController : BasePublicController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ISectionService _sectionService;

        public CatalogController(ICategoryService categoryService, ICatalogModelFactory catalogModelFactory, ISectionService sectionService)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;
            _sectionService = sectionService;
        }
      [HttpGet("haber/{id}/{title}")]
        public virtual IActionResult Category(int id,string title, CatalogPagingFilteringModel command)
        {

            var category = _categoryService.GetCategoryById(id);
            if (category == null || category.Deleted)
                return InvokeHttp404();

            //model
            var model = _catalogModelFactory.PrepareCategoryModel(category, command);

            //template
            var templateViewPath = _catalogModelFactory.PrepareCategoryTemplateViewPath(category.CategoryTemplateId);
            return View(templateViewPath, model);


        }

        [HttpPost]
        public IActionResult PopulerSection()
        {
            var query = _categoryService.GetAllCategoriesInculdeMansetPage();
            var model = _catalogModelFactory.PrepareCategoryPopulerSectionModel(query);
            return Json(model);
        }

        [HttpPost]
        public IActionResult MainPageList()
        {
            var section = _sectionService.GetNewsSection("MainPage");
            var categorys = _categoryService.GetAllCategoriesDisplayedOnHomePage();
            var categoryList = categorys.Where(x => section.Contains(x.Name) & x.Published & !x.Deleted).ToList();
            var model = new List<CategoriModel>();
            foreach (var category in categoryList)
            {
                model.Add(_catalogModelFactory.PrepareCategoryModel(new CategoriModel(), category));
            }
            return Json(model);
        }

    }
}