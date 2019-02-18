using System.Linq;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class PopulerSectionCategoryViewComponent : AgsViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ICategoryService _categoryService;
        public PopulerSectionCategoryViewComponent(ICatalogModelFactory catalogModelFactory, ICategoryService categoryService)
        {
            _catalogModelFactory = catalogModelFactory;
            _categoryService = categoryService;
        }
        public IViewComponentResult Invoke()
        {
            var query = _categoryService.GetAllCategoriesDisplayedOnHomePage().Where(x => x.IncludeInManset).ToList();
            var model = _catalogModelFactory.PrepareCategoryModel(new CategoriModel(), query);
           
            return View(model);

        }
    }
}