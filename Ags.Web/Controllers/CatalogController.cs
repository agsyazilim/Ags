using System.Linq;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class CatalogController : BasePublicController
    {
        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;

        public CatalogController(ICategoryService categoryService, ICatalogModelFactory catalogModelFactory)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;
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

    }
}