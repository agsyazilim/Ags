using System.Linq;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class MainPageDownCenterViewComponent : AgsViewComponent
    {

        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;

        public MainPageDownCenterViewComponent(
            ICategoryService categoryService, ICatalogModelFactory catalogModelFactory)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke(string name)
        {
            var category = _categoryService.GetAllCategories(categoryName: name).FirstOrDefault();
            var model = _catalogModelFactory.PrepareCategoryModel(new CategoriModel(), category);
            return View(model);
        }


    }
}